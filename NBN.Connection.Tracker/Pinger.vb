Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Threading
Imports NBN.Connection.Repository

Namespace PingManager
    Public Class Pinger


        Private autoEvent As AutoResetEvent
        Private runner As PingRunner
        Private stateTimer As Timer

        Public Sub Start(repo As NBNRepository, targetIPAddress As String, pingInterval As Integer)
            autoEvent = New AutoResetEvent(False)
            runner = New PingRunner(repo, targetIPAddress)

            Dim timerDelegate As TimerCallback = New TimerCallback(AddressOf runner.Execute)

            stateTimer = New Timer(timerDelegate, autoEvent, pingInterval, pingInterval)

        End Sub

        Public Sub [Stop]()
            autoEvent.WaitOne(10, False)
            stateTimer.Dispose()
        End Sub

        Private Class PingRunner

            Private _Repository As NBNRepository
            Private _TargetIpAddress As String

            Public Sub New(repo As NBNRepository, targetIpAddress As String)
                Me._Repository = repo
                Me._TargetIpAddress = targetIpAddress
            End Sub

            Public Sub Execute(ByVal stateInfo As Object)
                Dim autoEvent As AutoResetEvent = CType(stateInfo, AutoResetEvent)
                'Console.WriteLine("{0} Checking status {1,2}.", DateTime.Now.ToString("h:mm:ss.fff"), (System.Threading.Interlocked.Increment(invokeCount)).ToString())

                Dim pingSender As New Net.NetworkInformation.Ping
                Dim options As New PingOptions

                options.DontFragment = True

                Dim data As String = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                Dim buffer As Byte() = Encoding.ASCII.GetBytes(data)

                Dim timeout As Integer = 120

                Dim pingResult As New Repository.Ping

                Dim reply As PingReply = pingSender.Send(_TargetIpAddress, timeout, buffer, options)
                If reply.Status = IPStatus.Success Then
                    pingResult.PingReceived = True
                    pingResult.RoundTripTime = reply.RoundtripTime
                Else
                    'Failed ping
                    pingResult.PingReceived = False
                End If
                Console.WriteLine(String.Format("Ping response: {0} {1}ms", pingResult.PingReceived, pingResult.RoundTripTime))
                _Repository.WritePing(pingResult)

            End Sub
        End Class

    End Class



End Namespace