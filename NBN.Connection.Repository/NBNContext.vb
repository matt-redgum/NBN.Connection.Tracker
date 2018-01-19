Imports System.Data.Entity

Public Class NBNContext
    Inherits DbContext

    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Property Pings As DbSet(Of Ping)

End Class
