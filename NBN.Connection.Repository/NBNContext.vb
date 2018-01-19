Imports System.Data.Entity

Public Class NBNContext
    Inherits DbContext

    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Property Pings As DbSet(Of Ping)
    Public Property DownloadSpeedTests As DbSet(Of DownloadSpeedTest)
    Public Property UploadSpeedTests As DbSet(Of UploadSpeedTest)

End Class
