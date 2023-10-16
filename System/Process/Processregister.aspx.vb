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

Partial Class System_Process_ProcessRegister
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        hide_body.Visible = False
        Dim ResponseJson As String = ""

        Try


            lblUsername.Text = ""


            Dim stream As Stream = Request.InputStream
            Dim streamReader As StreamReader = New StreamReader(stream)

            Dim resp_result As String = streamReader.ReadToEnd()
            Dim json As String = resp_result

            Dim ser As JObject = JObject.Parse(json)

            lblApi.Text = ser.SelectToken("api")
            lblUsername.Text = ser.SelectToken("username")

            If Trim(lblUsername.Text) = "" Then
                ResponseJson = BuildJson("0", "Applicant Username Cannot Empty.")

                Response.Write(ResponseJson)
                Exit Sub
            End If

            If Regex.IsMatch(lblUsername.Text, "^[a-zA-Z0-9]+$") Then
                ' -- Pass -- '
            Else
                ResponseJson = BuildJson("0", "Invalid Username.")

                Response.Write(ResponseJson)
                Exit Sub
            End If


            Dim API_ApiKey As String = AppSettings("APIKey")


            If Trim(lblApi.Text) = Trim(API_ApiKey) Then

                Registration_API(json)

            Else

                ResponseJson = BuildJson("0", "Invalid Signature Key.")

                Response.Write(ResponseJson)
                Exit Sub

            End If

        Catch ex As Exception


            Response.Write(BuildJson("0", "Invalid Request"))

        End Try

    End Sub

    Protected Sub Registration_API(ByVal RequestJSON As String)
        Dim cn As SqlConnection
        Dim rs As SqlDataReader

        cn = New SqlConnection(GetConnectionSetting)
        cn.Open()

        Dim cmd As SqlCommand = cn.CreateCommand
        cmd.Connection = cn

        Dim ResponseJson As String = ""

        Try


            ' -- Check Username Exist? -- '
            cmd.CommandText = "SELECT UserName FROM tblFreelance WHERE UPPER(UserName) = @UserName "
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UCase(lblUsername.Text)
            rs = cmd.ExecuteReader
            If rs.HasRows Then
                rs.Close()

                ResponseJson = BuildJson("0", "Username Already Exist.")

                Response.Write(ResponseJson)

                Exit Sub
            End If
            rs.Close()


            ' -- Check Username Exist? -- '

            '' -- Check Email Exist? -- '
            'cmd.CommandText = "SELECT Email FROM tblFreelance WHERE Email = @Email "
            'cmd.Parameters.Clear()
            'cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Trim(lblEmail.Text)
            'rs = cmd.ExecuteReader

            'If rs.HasRows Then
            '    rs.Close()

            '    ResponseJson = BuildJson("0", "Email Already Exist.")
            '    Response.Write(ResponseJson)

            '    Exit Sub
            'End If
            'rs.Close()
            ' -- Check Email Exist? -- '



            ' -- Insert New Registration [START] -- '

            ' -- Insert Member Profile -- '

            Dim txtRemark As String = "Registration via API"
            Session("RegKeyDate") = Now.ToString(ChangeDateTimeFormat("Database"))

            cmd.CommandText = "INSERT INTO tblFreelance SELECT UserName, "
            cmd.CommandText = cmd.CommandText & "Email,TelM,Skill,Hobby, "
            cmd.CommandText = cmd.CommandText & "getdate(), CreateBy, '1900-01-01','' "
            cmd.CommandText = cmd.CommandText & "FROM tblTempFreelance WHERE Username = @Username "
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = UCase(Trim(lblUsername.Text))
            cmd.ExecuteNonQuery()
            ' -- Insert Member Profile -- '

            ' -- Insert New Registration [END] -- '



            ' -- Return Success Response -- '
            Dim JSON_ReturnParam As New JSON_Return()

            With JSON_ReturnParam
                .Status = "1"
                .Username = lblUsername.Text

            End With

            ResponseJson = JsonConvert.SerializeObject(JSON_ReturnParam)

            Response.Write(ResponseJson)

            ' -- Return Success Response -- '

        Catch ex As Exception

            ResponseJson = BuildJson("0", "Registration Failed - Error Occurred.")

            Response.Write(ResponseJson)

            ActivityLog(ConnectionLogLocation, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"), Session.SessionID.ToString, "F", "Change Sponsor API", ex.ToString)

        Finally
            cn.Close()
        End Try

    End Sub



    Public Class JSON_Return
        Public Status As String
        Public Username As String


    End Class



    Private Function BuildJson(ByVal ReqStatus As String,
                               ByVal ReqStatusDesc As String) As String

        Dim Registration As New JSON_List()

        With Registration
            .Status = ReqStatus
            .StatusDesc = ReqStatusDesc
        End With

        'Call SeralizeObject to convert the object to JSON string
        Dim jsonStatus As String = JsonConvert.SerializeObject(Registration)

        Return jsonStatus

    End Function


End Class