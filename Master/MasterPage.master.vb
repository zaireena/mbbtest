Imports clsConnection
Imports clsMain
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration.WebConfigurationManager

Partial Class Master_MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ShowMenu()

    End Sub

    Public Sub ShowMenu()
        Dim MenuText As New StringBuilder

        Dim cnn As SqlConnection
        cnn = New SqlConnection(GetConnectionSetting)
        cnn.Open()
        Try
            Dim rs As SqlDataReader
            Dim cmd As SqlCommand = cnn.CreateCommand

            lblProfileURL.Text = "#"

            cmd.Connection = cnn



            cmd.CommandText = "SELECT a.MenuID, a." & GetGlobalResourceObject("Master", "MenuName").ToString() & " AS MenuName, a." & GetGlobalResourceObject("Master", "MenuDescrip").ToString() & " AS MenuDescrip, a.bRoot, a.ParentID, a.MenuURL, a.ImgURL, a.SeperatorImgURL, a.PopUpImgURL "
            cmd.CommandText = cmd.CommandText & " FROM tblMenu a  WHERE a.bRoot = 'True' ORDER BY a.MenuID "
            ' Response.Write(cmd.CommandText)
            rs = cmd.ExecuteReader
            If rs.HasRows Then
                While rs.Read

                    MenuText.Append("<li>")
                    If rs("MenuURL").ToString = "" Then
                        MenuText.Append("<li>")
                        MenuText.Append("<a href='#'>" & "<img src=" & System.Web.Configuration.WebConfigurationManager.AppSettings("strURL") & rs("ImgURL").ToString() & ">" & "&nbsp;" & rs("MenuName").ToString() & "<span class='fa arrow'></span></a>")
                        MenuText.Append("<ul class='nav nav-second-level'>")
                        AddChildItems(rs("MenuID").ToString(), MenuText, 1)
                        MenuText.Append("</ul>")
                    Else
                        MenuText.Append("<a href='" & AppSettings("strURL") & rs("MenuURL").ToString() & "'>" & "<img src=" & System.Web.Configuration.WebConfigurationManager.AppSettings("strURL") & rs("ImgURL").ToString() & ">" & "&nbsp;" & rs("MenuName").ToString() & "</a>")
                        MenuText.Append("</li>")
                    End If

                    'get edit profile menu URL
                    If rs("MenuID").ToString = "12" Then
                        lblProfileURL.Text = AppSettings("strURL") & rs("MenuURL").ToString()
                    End If
                End While
                rs.Close()
            End If

        Catch ex As Exception
            ActivityLog(ConnectionLogLocation, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"), Session.SessionID.ToString, "F", "ADDF - Admin Default Page", ex.ToString)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopupScript", PopUp(GetGlobalResourceObject("Message", "ServerBusy").ToString()))
        Finally
            cnn.Close()
        End Try
        SideMenu.Text = MenuText.ToString
    End Sub

    Private Sub AddChildItems(ByVal menuID As String, ByVal sb As StringBuilder, ByVal level As Integer)
        Dim cnn As SqlConnection
        cnn = New SqlConnection(GetConnectionSetting)
        cnn.Open()

        Dim rs As SqlDataReader
        Dim cmd As SqlCommand = cnn.CreateCommand
        Try
            cmd.Connection = cnn

            cmd.CommandText = "SELECT a.MenuID, a." & GetGlobalResourceObject("Master", "MenuName").ToString() & " AS MenuName, a." & GetGlobalResourceObject("Master", "MenuDescrip").ToString() & " AS MenuDescrip, a.bRoot, a.ParentID, a.MenuURL, a.ImgURL, a.SeperatorImgURL, a.PopUpImgURL "


            cmd.CommandText = cmd.CommandText & " FROM tblmenu a WHERE a.ParentID = '" & menuID & "' ORDER BY a.MenuID "
            '  Response.Write(cmd.CommandText)
            rs = cmd.ExecuteReader
            If rs.HasRows Then
                While rs.Read
                    sb.Append("<li>")
                    If rs("MenuURL").ToString = "" Then
                        sb.Append("<a href='#'>" & rs("MenuName").ToString() & "<span class='fa arrow'></span></a>")

                        Select Case level + 1
                            Case 1
                                sb.Append("<ul class='nav nav-second-level'>")
                            Case 2
                                sb.Append("<ul class='nav nav-third-level'>")
                            Case 3
                                sb.Append("<ul class='nav nav-forth-level'>")
                            Case 4
                                sb.Append("<ul class='nav nav-fifth-level'>")
                            Case Else
                                sb.Append("<ul class='nav nav-fifth-level'>")
                        End Select

                        AddChildItems(rs("MenuID").ToString(), sb, level + 1)
                        sb.Append("</ul>")
                    Else
                        sb.Append("<a href='" & AppSettings("strURL") & rs("MenuURL").ToString() & "'>" & rs("MenuName").ToString() & "</a>")
                        sb.Append("</li>")
                    End If

                    'get edit profile menu URL
                    If rs("MenuID").ToString = "12" Then
                        lblProfileURL.Text = AppSettings("strURL") & rs("MenuURL").ToString()
                    End If
                End While
                rs.Close()
            End If

        Catch ex As Exception
            ActivityLog(ConnectionLogLocation, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"), Session.SessionID.ToString, "F", "ADDF - Admin Default Page", ex.ToString)

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopupScript", PopUp(GetGlobalResourceObject("Message", "ServerBusy").ToString()))
        Finally
            cnn.Close()
        End Try
    End Sub



End Class

