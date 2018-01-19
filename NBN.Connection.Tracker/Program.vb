Imports NBN.Connection.Repository
Imports Topshelf

Module Program


    Sub Main()

        Dim serviceName As String = "NBN.Connection.Tracker"

        Dim host = HostFactory.Run(
                    Sub(x)
                        x.Service(Of TrackerService)(
                            Sub(s)
                                s.ConstructUsing(Function(name) New TrackerService())
                                s.WhenStarted(Sub(tc) tc.Start())
                                s.WhenStopped(Sub(tc) tc.Stop())
                            End Sub)
                        x.RunAsNetworkService()
                        x.StartAutomaticallyDelayed()
                        x.SetServiceName(serviceName)
                        x.SetDisplayName(serviceName)
                        x.SetDescription("NBN connection status and logging service")
                        x.DependsOnEventLog()
                        x.EnableServiceRecovery(Sub(rc)
                                                    rc.RestartService(5)
                                                    rc.SetResetPeriod(0)
                                                End Sub)
                    End Sub)




    End Sub

    Public Class TrackerService

        Private _Pinger As PingManager.Pinger
        Private _SpeedTester As SpeedTestManager.SpeedTester
        Private _Repo As NBNRepository

        Public Sub New()
            _Pinger = New PingManager.Pinger
            _SpeedTester = New SpeedTestManager.SpeedTester
            _Repo = New NBNRepository(My.Settings.DBConnectionString)
        End Sub

        Public Sub Start()
            _Pinger.Start(_Repo, My.Settings.PingTargetIP, My.Settings.PingIntervalMilliseconds)
            _SpeedTester.Start(_Repo, My.Settings.SpeedTestDefaultCountry, My.Settings.SpeedTestIntervalMilliseconds)
        End Sub

        Public Sub [Stop]()
            _Pinger.Stop()
            _SpeedTester.Stop()
        End Sub

    End Class

End Module
