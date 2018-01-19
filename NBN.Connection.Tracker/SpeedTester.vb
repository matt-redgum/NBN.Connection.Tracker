Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Threading
Imports NBN.Connection.Repository
Imports NSpeedTest
Imports NSpeedTest.Models

Namespace SpeedTestManager
    Public Class SpeedTester


        Private autoEvent As AutoResetEvent
        Private runner As SpeedTestRunner
        Private stateTimer As Timer

        Public Sub Start(repo As NBNRepository, defaultCountry As String, pingInterval As Integer)
            autoEvent = New AutoResetEvent(False)
            runner = New SpeedTestRunner(repo, defaultCountry)

            Dim timerDelegate As TimerCallback = New TimerCallback(AddressOf runner.Execute)

            stateTimer = New Timer(timerDelegate, autoEvent, pingInterval, pingInterval)

        End Sub

        Public Sub [Stop]()
            autoEvent.WaitOne(10, False)
            stateTimer.Dispose()
        End Sub

        Private Class SpeedTestRunner

            Private _Repository As NBNRepository
            Private _DefaultCountry As String

            Public Sub New(repo As NBNRepository, defaultCountry As String)
                Me._Repository = repo
                Me._DefaultCountry = defaultCountry
            End Sub

            Public Sub Execute(ByVal stateInfo As Object)
                Dim autoEvent As AutoResetEvent = CType(stateInfo, AutoResetEvent)

                Dim downloadResult As New DownloadSpeedTest
                Dim uploadResult As New UploadSpeedTest

                Try
                    Dim client = New SpeedTestClient()
                    Dim settings = client.GetSettings()
                    Dim servers = SelectServers(client, settings, _DefaultCountry)
                    Dim bestServer = SelectBestServer(servers)

                    Console.WriteLine("Testing speed...")
                    Dim downloadSpeed = client.TestDownloadSpeed(bestServer, settings.Download.ThreadsPerUrl)

                    downloadResult.TransferSpeedKbps = Math.Round(downloadSpeed, 2)
                    PrintSpeed("Download", downloadSpeed)

                    Dim uploadSpeed = client.TestUploadSpeed(bestServer, settings.Upload.ThreadsPerUrl)

                    uploadResult.TransferSpeedKbps = Math.Round(uploadSpeed, 2)
                    PrintSpeed("Upload", uploadSpeed)

                Catch ex As Exception
                    'Probably not connected to the internetz
                    uploadResult.ResultReceived = False
                    downloadResult.ResultReceived = False
                End Try

                _Repository.WriteDownloadSpeed(downloadResult)
                _Repository.WriteUploadSpeed(uploadResult)
            End Sub


            Private Shared Function SelectBestServer(servers As IEnumerable(Of Server)) As Server
                Console.WriteLine()
                Console.WriteLine("Best server by latency:")
                Dim bestServer = servers.OrderBy(Function(x) x.Latency).First()
                PrintServerDetails(bestServer)
                Console.WriteLine()
                Return bestServer
            End Function

            Private Shared Function SelectServers(client As SpeedTestClient, settings As Settings, defaultCountry As String) As IEnumerable(Of Server)
                Console.WriteLine()
                Console.WriteLine("Selecting best server by distance...")
                Dim servers = settings.Servers.Where(Function(s) s.Country.Equals(defaultCountry)).Take(10).ToList()
                For Each server In servers
                    server.Latency = client.TestServerLatency(server)
                    PrintServerDetails(server)
                Next

                Return servers
            End Function

            Private Shared Sub PrintServerDetails(ByVal server As Server)
                Console.WriteLine("Hosted by {0} ({1}/{2}), distance: {3}km, latency: {4}ms", server.Sponsor, server.Name, server.Country, CInt(server.Distance) / 1000, server.Latency)
            End Sub

            Private Shared Sub PrintSpeed(ByVal type As String, ByVal speed As Double)
                If speed > 1024 Then
                    Console.WriteLine("{0} speed: {1} Mbps", type, Math.Round(speed / 1024, 2))
                Else
                    Console.WriteLine("{0} speed: {1} Kbps", type, Math.Round(speed, 2))
                End If
            End Sub
        End Class

    End Class



End Namespace