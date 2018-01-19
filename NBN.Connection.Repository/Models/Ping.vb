Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("Ping")>
Public Class Ping

    Public Sub New()
        MyBase.New
        Me.PingDateTimeUTC = DateTime.UtcNow
    End Sub

    <Key, Column("IDPing")>
    Public Property PingId As Long

    <Column("PingDateTimeUTC", TypeName:="DateTime2")>
    Public Property PingDateTimeUTC As DateTime

    Public Property PingReceived As Boolean

    Public Property RoundTripTime As Long

End Class
