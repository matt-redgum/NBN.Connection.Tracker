Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema


Public Class Ping

    Public Sub New()
        MyBase.New
        Me.PingDateTimeUTC = DateTime.UtcNow
    End Sub
    Public Property PingId As Long

    Public Property PingDateTimeUTC As DateTime

    Public Property PingReceived As Boolean

    Public Property RoundTripTime As Long

End Class
