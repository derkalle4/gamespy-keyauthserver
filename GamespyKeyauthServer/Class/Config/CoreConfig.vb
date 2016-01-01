Public Class CoreConfig
    Public Property UDPHeartbeatPort As Int32 = 29910
    Public Property UDPHeartbeatAddress As String = "0.0.0.0"

    Public Property LogToFile As Boolean = False
    Public Property LogFileName As String = "/log.txt"

    Public Property Loglevel As Byte = 2

    Public Property MySQLHostname As String = "localhost"
    Public Property MySQLPort As Int32 = 3306
    Public Property MySQLDatabase As String = "swbf2"
    Public Property MySQLUsername As String = "root"
    Public Property MySQLPwd As String = ""
End Class
