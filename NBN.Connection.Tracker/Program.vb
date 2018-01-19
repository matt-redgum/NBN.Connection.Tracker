Imports NBN.Connection.Repository

Module Program

    Private _Pinger As PingManager.Pinger
    Private _Repo As NBNRepository
    Sub Main()


        _Pinger = New PingManager.Pinger
        _Repo = New NBNRepository(My.Settings.DBConnectionString)

        _Pinger.Start(_Repo, My.Settings.PingTargetIP, My.Settings.PingIntervalMilliseconds)

        Console.ReadKey()
        _Pinger.Stop()


    End Sub



End Module
