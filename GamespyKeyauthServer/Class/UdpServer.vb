Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Public Class UdpServer
    Public Property Address As ipaddress
    Public Property Port As Int32 = 27900

    Private listenThread As Thread
    Private listener As UdpClient
    Private running As Boolean

    Public Property Server As GamespyServer

    Public Event BindFailed(ByVal sender As UdpServer, ByVal ex As Exception)

    Sub New(ByVal server As GamespyServer)
        Me.Server = server
    End Sub

    Public Sub Open()
        If Not running Then
            Try
                listener = New UdpClient(New Net.IPEndPoint(Me.Address, Me.Port))
                'dpClien
            Catch ex As Exception
                Logger.Log("Bind failed [" & Me.Address.ToString & ":" & Me.Port & "]", LogLevel.Exception)
                RaiseEvent BindFailed(Me, ex)
            End Try
            running = True
            Me.listenThread = New Thread(AddressOf Me.Listen)
            Me.listenThread.Start()
            Logger.Log("Server Heartbeat Listener started [" & Me.Address.ToString & ":" & Me.Port & "]", LogLevel.Info)
        End If
    End Sub

    Public Sub Close()
        If running Then
            running = False
            Me.listener.Close()
        End If
    End Sub

    Private Sub Listen()
        While running
            Try
                Dim rIPEP As IPEndPoint = Nothing
                Dim data() As Byte = Me.listener.Receive(rIPEP)

                If data.Length > 0 Then
                    Me.OnDataInput(data, rIPEP)
                End If
            Catch ex As Exception
                Logger.Log(ex.ToString, LogLevel.Verbose)
            End Try
            Threading.Thread.Sleep(10)
        End While
    End Sub

    Friend Overridable Sub OnDataInput(ByVal data() As Byte, ByVal rIPEP As IPEndPoint)
        'Logger.Log("Server connected: " & rIPEP.Address.ToString, LogLevel.Verbose)
        Logger.Log("Fetched " & data.Count & " Bytes from " & rIPEP.Address.ToString & " [UDP]", LogLevel.Verbose)
        Dim p As GamespyUdpPacket
        p = New KeyCheckPacket(Me, rIPEP)
        p.data = data
        p.ManageData()
    End Sub

    Public Sub send(ByVal data() As Byte, ByVal rIPEP As Net.IPEndPoint)
        Dim c As New UdpClient
        Try
            Me.listener.Send(data, data.Length, rIPEP)
            'listener.Close()
        Catch ex As Exception
            Logger.Log("Couldn't send UDP-Packet to " & rIPEP.Address.ToString, LogLevel.Warning)
        End Try
        c = Nothing
    End Sub

End Class
