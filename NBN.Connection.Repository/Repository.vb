Public Class NBNRepository
    Implements IDisposable

    Private _ConnectionString As String
    Private _Context As NBNContext

    Public Sub New(connectionString As String)
        MyBase.New
        Me._ConnectionString = connectionString
        Me._Context = New NBNContext(connectionString)

    End Sub

#Region " Ping"
    Public Sub WritePing(ping As Ping)

        _Context.Pings.Add(ping)
        _Context.SaveChanges()

    End Sub

    Public Function RetrievePings(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of Ping)

        Return _Context.Pings.Where(Function(p) p.PingDateTimeUTC > startDateTime AndAlso p.PingDateTimeUTC < endDateTime).ToList()

    End Function

#End Region

#Region " Download Speed"
    Public Sub WriteDownloadSpeed(speed As DownloadSpeedTest)

        _Context.DownloadSpeedTests.Add(speed)
        _Context.SaveChanges()

    End Sub

    Public Function RetrieveDownloadSpeeds(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of DownloadSpeedTest)

        Return _Context.DownloadSpeedTests.Where(Function(p) p.SpeedTestDateTimeUTC > startDateTime AndAlso p.SpeedTestDateTimeUTC < endDateTime).ToList()

    End Function
#End Region

#Region " Upload Speed"
    Public Sub WriteUploadSpeed(speed As UploadSpeedTest)

        _Context.UploadSpeedTests.Add(speed)
        _Context.SaveChanges()

    End Sub

    Public Function RetrieveUploadSpeeds(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of UploadSpeedTest)

        Return _Context.UploadSpeedTests.Where(Function(p) p.SpeedTestDateTimeUTC > startDateTime AndAlso p.SpeedTestDateTimeUTC < endDateTime).ToList()

    End Function

#End Region


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                _Context.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
