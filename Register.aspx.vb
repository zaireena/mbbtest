Imports clsConnection
Imports clsMain
Imports clsJson
Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Web.Configuration.WebConfigurationManager
Imports System.IO
Imports System.Collections.Generic
Imports System.Text
Imports System.Net

Partial Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtTelM.Attributes.Add("OnKeyPress", "return CheckInt();")
        If Request.QueryString("nRegType") = "F" Or Request.QueryString("nRegType") = "P" Then
            btnSubmit.Attributes.Add("onclick", "return confirm('" & GetGlobalResourceObject("Member", "SubmitConfirm").ToString() & "');")
        End If
    End Sub


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Session("nComplete") = "N"

        Dim cn As SqlConnection
        Dim rs As SqlDataReader
        Dim popUpScript As String

        cn = New SqlConnection(GetConnectionSetting)
        cn.Open()

        Dim cmd As SqlCommand = cn.CreateCommand

        cmd.Connection = cn

        Try
            labelError.Text = "True"
            labelMessage.Text = ""

            If txtUsername.Text = "" Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & "** " & GetGlobalResourceObject("Message", "InvalidUsername").ToString()

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                Exit Sub
            End If

            If Not RegularExpressions.Regex.IsMatch(Trim(txtUsername.Text), "^[a-zA-Z0-9]+$") Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & GetGlobalResourceObject("Message", "InvalidUsername").ToString()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))
                Exit Sub
            End If



            'check username duplicate
            cmd.CommandText = "SELECT UserName FROM tblFreelance WHERE UPPER(UserName) = @UserName "
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 80).Value = UCase(txtUsername.Text)
            rs = cmd.ExecuteReader
            If rs.Read() Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & "** " & GetGlobalResourceObject("Member", "UsernameExist").ToString()

                rs.Close()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                Exit Sub
            End If
            rs.Close()


            If txtEmail.Text = "" Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & "** " & GetGlobalResourceObject("Member", "PleaseEnterEmail").ToString()

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                Exit Sub
            End If

            If EmailValidation(txtEmail.Text) = False Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & "** " & GetGlobalResourceObject("Message", "InvalidEmailFormat").ToString()

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                Exit Sub
            End If

            If txtTelM.Text = "" Then
                labelError.Text = "False"
                labelMessage.Text = labelMessage.Text & "** " & GetGlobalResourceObject("Member", "PleaseEnterTelM").ToString()

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                Exit Sub
            End If

            cmd.CommandText = "Insert Into tblTempFreelance (Username, Email, "
            cmd.CommandText &= "TelM, Skill, Hobby, CreateDate, CreateBy) Values "
            cmd.CommandText &= "('" & Trim(Replace(txtUsername.Text, "'", "''")) & "', '" & Trim(Replace(txtEmail.Text, "'", "''")) & "', "
            cmd.CommandText &= "N'" & Trim(Replace(txtTelM.Text, "'", "''")) & "', N'" & Trim(txtSkillset.Text) & "', N'" & Trim(txtHobby.Text) & "', GetDate(), "
            cmd.CommandText &= "'" & Trim(Session("UserName")) & "')"
            cmd.ExecuteNonQuery()


            If labelMessage.Text = "" Then

                Dim ApiKey As String = AppSettings("APIKey")
                Dim strAPI_URL As String = AppSettings("strURL") & "/System/Process/ProcessRegister.aspx"

                Dim req As HttpWebRequest = CType(WebRequest.Create(strAPI_URL), HttpWebRequest)
                req.Method = "POST"
                req.ContentType = "application/json"

                Dim JSON_VVP As New JSON_result()

                With JSON_VVP
                    .api = ApiKey
                    .username = txtUsername.Text.ToString()
                End With

                Dim jsonString As String = JsonConvert.SerializeObject(JSON_VVP)
                ' jsonString = jsonString.Replace("[", "").Replace("]", "")
                'Response.Write(jsonString)

                Dim streamWriter As StreamWriter = New StreamWriter(req.GetRequestStream())
                streamWriter.Write(jsonString)
                streamWriter.Flush()
                streamWriter.Close()

                Dim webRes As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)

                Dim streamReader As StreamReader = New StreamReader(webRes.GetResponseStream())


                Dim resp_result As String = streamReader.ReadToEnd()
                Dim json As String = resp_result

                ' Response.Write(resp_result)
                'json = json.Replace("<", "").Replace("!", "").Replace("D", "").Replace("O", "").Replace("C", "").Replace("T", "").Replace("Y", "").Replace("P", "").Replace("E", "").Replace("h", "").Replace("t", "").Replace("m", "").Replace("l", "").Replace(">", "")
                Dim ser As JObject = JObject.Parse(json)

                Dim statusdesc As String = ser.SelectToken("Status")
                Dim Username As String = ser.SelectToken("Username")

                If statusdesc = "1" Then

                    labelMessage.Text = labelMessage.Text & "** " & Username & " SUCCESSFULLY REGISTERED"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopUpScript", PopUp(labelMessage.Text))

                    Exit Sub
                End If


            End If

        Catch ex As Exception
            If Session("nComplete") <> "Y" Then
                ActivityLog(ConnectionLogLocation, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"), Session.SessionID.ToString, "F", "ADDR - Distributor Registration", ex.ToString)
            End If

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "PopupScript", PopUp(GetGlobalResourceObject("Message", "TransactionFailed").ToString()))
        Finally
            cn.Close()
        End Try
    End Sub


End Class

