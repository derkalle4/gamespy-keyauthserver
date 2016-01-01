'Gamespy KeyAuth Server Emulator v1.1
'Note: replaced local classfiles with reference to masterserver-project
'ensure to maintain compatibility
Module Main
    Private server As GamespyServer

    Sub Main()
        Console.WriteLine(PRODUCT_NAME)
        server = New GamespyServer
        server.Run()
        Console.ReadLine()
    End Sub

End Module
