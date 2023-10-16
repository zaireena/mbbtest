Imports Microsoft.VisualBasic
Imports System.Web.Configuration.WebConfigurationManager

Public Class clsConnection

    Public Shared Function GetConnectionSetting() As String
        'Dim key As Microsoft.Win32.RegistryKey
        Try
            'key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\ReishiLabNet\Connection Settings", True)

            'GetConnectionSetting = "Data Source=" & key.GetValue("Server", "") & ";Initial Catalog=" & key.GetValue("Database Name", "") & _
            '                        ";User ID=" & key.GetValue("Username", "") & ";Password=" & key.GetValue("Password", "") & ";"
            GetConnectionSetting = "Data Source=" & AppSettings("DBSource") & ";Initial Catalog=" & AppSettings("DBName") & ";User ID=" & AppSettings("DBUser") & ";Password=" & AppSettings("DBPass") & "; Max Pool Size=1000; Min Pool Size = 0;"

        Catch ex As Exception
            GetConnectionSetting = -1
        End Try
    End Function
End Class
