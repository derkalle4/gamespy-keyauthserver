Public Class KeyCheckPacket
    Inherits GamespyUdpPacket

    Sub New(ByVal server As UdpServer, ByVal remoteIPEP As Net.IPEndPoint)
        MyBase.New(server, remoteIPEP)
    End Sub

    Private keyhash As String = String.Empty
    Private skey As String = String.Empty
    Private clientAddress As String = String.Empty

    Public Overrides Sub ManageData()
        Dim decString As String = GetString(Me.XorBytes(Me.data, GetBytes(GS_XOR_KEYSTRING)))

        Dim params() As String = Split(decString, "\")
        '  1  2  3  4    5  6               7    8                                                                        9   10       11   12
        '\auth\\pid\1203\ch\39rI4Hekq4813TW\resp\1581dacb43e65f59dd23744ecdafc3bf35c8bea7d20bc9f3d361681430bd1f794f79bef3\ip\494564948\skey\2471   
        If params.Length <> 13 Then Return
        If params(8).Length < 32 Then Return
        If params(1) <> GS_CMD_KEYAUTH Then Return

        Me.keyhash = Mid(params(8), 1, 32)
        Me.skey = params(12)
        Me.clientAddress = params(10)
        Logger.Log("Got Key '" & Me.keyhash & "' from " & Me.RemoteIPEP.ToString, LogLevel.Verbose)
        Me.Server.send(Me)
    End Sub

    Public Overrides Function CompileResponse() As Byte()
        Dim responseString As String

        If Me.Server.Server.checkKey(Me.keyhash, Me.clientAddress) Then
            responseString = "\uok\\cd\" & Me.keyhash & "\skey\" & Me.skey
            Logger.Log("Acking Key '" & Me.keyhash & "'", LogLevel.Verbose)
        Else
            responseString = "\unok\\cd\" & Me.keyhash & "\skey\" & Me.skey & "\errmsg\" & GS_CDKEY_ERROR
            Logger.Log("Blocking Key '" & Me.keyhash & "'", LogLevel.Verbose)
        End If

        Dim buf() As Byte = Me.XorBytes(GetBytes(responseString), GetBytes(GS_XOR_KEYSTRING))
        Return buf
    End Function

    Private Function XorBytes(ByVal data() As Byte, ByVal key() As Byte) As Byte()
        For i = 0 To data.Length - 1
            data(i) = data(i) Xor key(i Mod key.Length)
        Next
        Return data
    End Function

End Class


