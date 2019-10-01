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
        Try
            _Context.SaveChanges()
        Catch ex As Exception
            'Just ignore the exception I guess. We drop this ping and move on
        End Try
    End Sub


    Public Function RetrievePings(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of Ping)

        Dim pings = _Context.Pings.Where(Function(p) p.PingDateTimeUTC > startDateTime AndAlso p.PingDateTimeUTC < endDateTime).ToList()
        Dim pingsConverted = pings.Select(Of Ping)(Function(p) New Ping() With {.PingId = p.PingId,
                                                       .PingReceived = p.PingReceived,
                                                       .RoundTripTime = p.RoundTripTime,
                                                       .PingDateTimeUTC = p.PingDateTimeUTC.ToLocalTime})
        Return pings

    End Function

#End Region

#Region " Download Speed"
    Public Sub WriteDownloadSpeed(speed As DownloadSpeedTest)

        _Context.DownloadSpeedTests.Add(speed)
        Try
            _Context.SaveChanges()
        Catch ex As Exception
            'Just ignore the exception I guess. We drop this ping and move on
        End Try
    End Sub

    Public Function RetrieveDownloadSpeeds(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of DownloadSpeedTest)

        Dim dst = _Context.DownloadSpeedTests.Where(Function(p) p.SpeedTestDateTimeUTC > startDateTime AndAlso p.SpeedTestDateTimeUTC < endDateTime).ToList()
        Dim dstConverted = dst.Select(Of DownloadSpeedTest)(Function(d) New DownloadSpeedTest() With {.DownloadSpeedTestId = d.DownloadSpeedTestId,
                                                                .TransferSpeedKbps = d.TransferSpeedKbps,
                                                                .ResultReceived = d.ResultReceived,
                                                                .SpeedTestDateTimeUTC = d.SpeedTestDateTimeUTC.ToLocalTime})
        Return dstConverted
    End Function
#End Region

#Region " Upload Speed"
    Public Sub WriteUploadSpeed(speed As UploadSpeedTest)

        _Context.UploadSpeedTests.Add(speed)
        Try
            _Context.SaveChanges()
        Catch ex As Exception
            'Just ignore the exception I guess. We drop this ping and move on
        End Try
    End Sub

    Public Function RetrieveUploadSpeeds(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of UploadSpeedTest)

        Dim ust = _Context.UploadSpeedTests.Where(Function(p) p.SpeedTestDateTimeUTC > startDateTime AndAlso p.SpeedTestDateTimeUTC < endDateTime).ToList()
        Dim ustConverted = ust.Select(Of UploadSpeedTest)(Function(d) New UploadSpeedTest() With {.UploadSpeedTestId = d.UploadSpeedTestId,
                                                                .TransferSpeedKbps = d.TransferSpeedKbps,
                                                                .ResultReceived = d.ResultReceived,
                                                                .SpeedTestDateTimeUTC = d.SpeedTestDateTimeUTC.ToLocalTime})
        Return ustConverted
    End Function

#End Region

#Region " Recent Disconnections"

    Public Function RetrieveRecentDisconnections(startDateTime As DateTime, endDateTime As DateTime) As IEnumerable(Of DisconnectionInfo)

        Dim pings = _Context.Pings.Where(Function(p) p.PingDateTimeUTC > startDateTime AndAlso p.PingDateTimeUTC < endDateTime).ToList()
        Dim disconnections As New List(Of DisconnectionInfo)

        Dim currentDisconection As DisconnectionInfo = Nothing
        Dim lastWasDisconnected As Boolean = False
        For Each ping In pings

            If ping.RoundTripTime = 0 Then
                'This is a disconnect
                If Not lastWasDisconnected Then
                    currentDisconection = New DisconnectionInfo
                    currentDisconection.StartTime = ping.PingDateTimeUTC.ToLocalTime
                End If

                lastWasDisconnected = True
            Else
                'Received a response
                If lastWasDisconnected AndAlso currentDisconection IsNot Nothing Then
                    'Close this info out and add to list
                    currentDisconection.EndTime = ping.PingDateTimeUTC.ToLocalTime
                    Dim duration = currentDisconection.EndTime.Subtract(currentDisconection.StartTime)
                    currentDisconection.DurationInSeconds = duration.TotalSeconds
                    If currentDisconection.DurationInSeconds > 30 Then
                        disconnections.Add(currentDisconection)
                    End If
                End If
                lastWasDisconnected = False
            End If
        Next
        Return disconnections
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
