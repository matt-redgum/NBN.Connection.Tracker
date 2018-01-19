Public MustInherit Class SpeedTestBase

    Public Sub New()
        MyBase.New
        Me.SpeedTestDateTimeUTC = DateTime.UtcNow
        Me.ResultReceived = True
    End Sub

    Public Property SpeedTestDateTimeUTC As DateTime
    Public Property TransferSpeedKbps As Integer

    Public Property ResultReceived As Boolean
End Class

Public Class DownloadSpeedTest
    Inherits SpeedTestBase
    Public Sub New()
        MyBase.New
    End Sub

    Public Property DownloadSpeedTestId As Long

End Class

Public Class UploadSpeedTest
    Inherits SpeedTestBase
    Public Sub New()
        MyBase.New
    End Sub

    Public Property UploadSpeedTestId As Long

End Class