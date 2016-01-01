Public Class GamespyServer
    Public Property GSUdpServer As UdpServer
    'Public Property MySQL As MySQLHandler

    Public Property Config As CoreConfig
    Private ConfigMan As ConfigSerializer

#Region "Programm"
    Public Sub Run()
        Me.PreInit()
        Me.Execution()
        Me.PostInit()
    End Sub

    Private Sub PreInit()
        Me.ConfigMan = New ConfigSerializer(GetType(CoreConfig))
        Me.Config = Me.ConfigMan.loadFromFile(CFG_FILE, CurDir() & CFG_DIR)

        'Me.MySQL = New MySQLHandler
        Me.GSUdpServer = New UdpServer(Me)
        Me.GSUdpServer.Address = Net.IPAddress.Parse(Me.Config.UDPHeartbeatAddress)
        Me.GSUdpServer.Port = Me.Config.UDPHeartbeatPort

        'Me.MySQL.Hostname = Me.Config.MySQLHostname
        'Me.MySQL.Port = Me.Config.MySQLPort
        'Me.MySQL.DbName = Me.Config.MySQLDatabase
        'Me.MySQL.DbUser = Me.Config.MySQLUsername
        'Me.MySQL.DbPwd = Me.Config.MySQLPwd

        Logger.MinLogLevel = Me.Config.Loglevel
        Logger.LogToFile = Me.Config.LogToFile
        Logger.LogFileName = Me.Config.LogFileName

    End Sub

    Private Sub Execution()
        'Me.MySQL.Connect()
        Me.GSUdpServer.Open()
        Console.ReadLine()
        Logger.Log("Shutting down...", LogLevel.Info)
    End Sub
    Private Sub PostInit()
        Me.GSUdpServer.Close()
        'Me.MySQL.Close()
        Me.ConfigMan = Nothing
        Me.Config = Nothing
        Logger.Log("Server stopped.", LogLevel.Info)
    End Sub
#End Region


#Region "KeyHandling"
    Public Function checkKey(ByVal key As String, ByVal ip As String) As Boolean
        Return True
    End Function
#End Region



End Class
