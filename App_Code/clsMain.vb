Imports Microsoft.VisualBasic
Imports System.Globalization
Imports clsConnection
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.net.Mail
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Security.Cryptography

Public Class clsMain
    Public Shared Title As String = AppSettings("Title")
    Public Shared TitleCN As String = AppSettings("TitleCN")
    Public Shared TitleID As String = AppSettings("Title")
    Public Shared CompanyName As String = AppSettings("CompanyName")
    Public Shared CompanyRegNo As String = AppSettings("CompanyRegNo")
    Public Shared CompanyAdd1 As String = AppSettings("CompanyAdd1")
    Public Shared CompanyAdd2 As String = AppSettings("CompanyAdd2")
    Public Shared CompanyTel As String = AppSettings("CompanyTel")
    Public Shared CompanyWebSite As String = AppSettings("CompanyWebSite")
    Public Shared CompanyEmail As String = AppSettings("CompanyEmail")
    Public Shared UploadFileURL As String = AppSettings("UploadFileURL")
    Public Shared fromEmail As String = AppSettings("fromEmail")

    'local host
    Public Shared DatabaseName As String = AppSettings("DBName")
    Public Shared emailDomain As String = AppSettings("emailDomain")
    Public Shared strURL As String = AppSettings("strURL")
    Public Shared strFilePath As String = AppSettings("strFilePath")
    Public Shared UploadSupportDocPath As String = AppSettings("strUploadFilePath")

    Public Shared AdminLoginURL As String = strURL & "System/Security/AMLogin.aspx"
    Public Shared StockistLoginURL As String = strURL & "System/Security/PHLogin.aspx"
    Public Shared DistributorLoginURL As String = strURL & "System/Security/HYLogin.aspx"
    Public Shared ServiceCenterLoginURL As String = strURL & "System/Security/ServiceCenterLogin.aspx"
    Public Shared defaultRedirect As String = strURL & "System/Security/DefaultRedirect.aspx"
    Public Shared ConnectionSettingURL As String = strURL & "System/ConnectionSetting.aspx"

    Public Shared errorTextLocation As String = strFilePath & "SystemLog\txtErrorLog.txt"
    Public Shared SecurityLogLocation As String = strFilePath & "SystemLog\txtSecurityLog.txt"
    Public Shared ConnectionLogLocation As String = strFilePath & "SystemLog\txtConnectionErrorLog.txt"

    Public Shared UploadSupportDocURL As String = strFilePath & "Img\UploadedSupportDocs\"
    Public Shared BackupURL As String = strFilePath & "DatabaseBackup\"
    Public Shared uploadFile As String = strFilePath & "UploadFile\"

    Public Shared downloadURL As String = strURL & "System/DownloadFile.aspx"
    Public Shared AttachmentURL As String = strURL & "System/Attachment/"
    Public Shared SystemURL As String = strURL & "System/Announcement/"

    Public Shared BtnImageURL As String = strURL & "Img/"
    Public Shared imgURLLanguage As String = strURL & "Img/"
    Public Shared ImageURL As String = strURL & "Img/"
    Public Shared ImageURLC As String = strURL & "ImgC/"
    Public Shared ImageURLCS As String = strURL & "ImgCS/"
    Public Shared ImageURLID As String = strURL & "ImgID/"

    Public Shared UploadImageURL As String = strURL & "Img/UploadedImg/"
    Public Shared UploadSlipURL As String = strURL & "Img/BankInSlip/"
    Public Shared UploadPaymmentURL As String = strURL & "Img/UploadedPaymentImg/"
    Public Shared WelcomeImage As String = strURL & "WelcomeImage/"

    Public Shared ConnectionErrorAlert As String = "Server is busy. Please try again."
    Public Shared PermissionAlert As String = "You have no permission to do this transaction."

    Public Enum UserState
        SERVICECENTER = 1
        DISTRIBUTOR = 2
        STOCKIST = 3
        ADMIN = 4
        'DISTRIBUTOR = 1
        'STOCKIST = 2
        'ADMIN = 3
    End Enum

    Public Shared Function ChangeDateDisplayFormat(ByVal nType As String, ByVal nLanguage As String) As String
        Select Case nType
            Case "Database"
                If nLanguage = "zh-CN" Then
                    ChangeDateDisplayFormat = "年/月/日"
                Else
                    ChangeDateDisplayFormat = "yyyy/MM/dd"
                End If
            Case "YrMth"
                If nLanguage = "zh-CN" Then
                    ChangeDateDisplayFormat = "年月月"
                Else
                    ChangeDateDisplayFormat = "yyyyMM"
                End If
            Case "Web"
                If nLanguage = "zh-CN" Then
                    ChangeDateDisplayFormat = "年/月/日"
                Else
                    ChangeDateDisplayFormat = "yyyy/MM/dd"
                End If
            Case Else
                If nLanguage = "zh-CN" Then
                    ChangeDateDisplayFormat = "年/月/日"
                Else
                    ChangeDateDisplayFormat = "yyyy/MM/dd"
                End If
        End Select
    End Function

    Public Shared Function ChangeDateFormat(ByVal nType As String) As String
        Select Case nType
            Case "Database"
                ChangeDateFormat = "yyyy/MM/dd"
            Case "YrMth"
                ChangeDateFormat = "yyyyMM"
            Case "Web"
                ChangeDateFormat = "yyyy/MM/dd"
            Case "Calender"
                ChangeDateFormat = "yy/mm/dd"
            Case Else
                ChangeDateFormat = "yyyy/MM/dd"
        End Select
    End Function

    Public Shared Function ChangeDateTimeFormat(ByVal nType As String) As String
        Select Case nType
            Case "Database"
                ChangeDateTimeFormat = "yyyy/MM/dd HH:mm:ss"
            Case "Web"
                ChangeDateTimeFormat = "yyyy/MM/dd hh:mm:ss tt"
            Case "Time"
                ChangeDateTimeFormat = "HH:mm:ss"
            Case Else
                ChangeDateTimeFormat = "yyyy/MM/dd hh:mm:ss tt"
        End Select
    End Function

    Public Shared Function ChageStrDateFormat(ByVal nDate As String, ByVal nType As String)
        Dim dfi As DateTimeFormatInfo = New DateTimeFormatInfo()
        Dim mdate As Date

        dfi.ShortDatePattern = "yyyy/MM/dd"
        mdate = Convert.ToDateTime(nDate, dfi)

        Select Case nType
            Case "Database"
                ChageStrDateFormat = mdate.ToString("yyyy/MM/dd")
            Case "Database2"
                ChageStrDateFormat = mdate.ToString("MM/dd/yyyy")
            Case "Web"
                ChageStrDateFormat = mdate.ToString("yyyy/MM/dd")
            Case Else
                ChageStrDateFormat = mdate.ToString("yyyy/MM/dd")
        End Select
    End Function

    Public Shared Function ChageStrDateTimeFormat(ByVal nDate As String, ByVal nType As String)
        Dim dfi As DateTimeFormatInfo = New DateTimeFormatInfo()
        Dim mdate As Date

        dfi.ShortDatePattern = "yyyy/MM/dd HH:mm:ss"
        mdate = Convert.ToDateTime(nDate, dfi)

        Select Case nType
            Case "Database"
                ChageStrDateTimeFormat = mdate.ToString("yyyy/MM/dd HH:mm:ss")
            Case "Database2"
                ChageStrDateTimeFormat = mdate.ToString("MM/dd/yyyy HH:mm:ss")
            Case "Web"
                ChageStrDateTimeFormat = mdate.ToString("yyyy/MM/dd hh:mm:ss tt")
            Case Else
                ChageStrDateTimeFormat = mdate.ToString("yyyy/MM/dd hh:mm:ss tt")
        End Select
    End Function

    Public Shared Function DateValidation(ByVal idate As String) As Boolean
        Dim dfi As DateTimeFormatInfo = New DateTimeFormatInfo()
        Dim mdate As Date

        'dfi.ShortDatePattern = "yyyy/MM/dd"
        dfi.ShortDatePattern = "yyyy/MM/dd"
        Try
            mdate = Convert.ToDateTime(idate, dfi)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function SQLCheck(ByVal strText As String) As Boolean
        Dim PassText = UCASE(strText)

        If PassText.Contains("SELECT") Then
            Return False
        ElseIf PassText.Contains("UPDATE") Then
            Return False
        ElseIf PassText.Contains("DELETE") Then
            Return False
        ElseIf PassText.Contains("TRUNCATE") Then
            Return False
        ElseIf PassText.Contains("CREATE") Then
            Return False
        ElseIf PassText.Contains("INSERT") Then
            Return False
        ElseIf PassText.Contains("ORDER BY") Then
            Return False
        ElseIf PassText.Contains("BACKUP") Then
            Return False
        ElseIf PassText.Contains("RESTORE") Then
            Return False
        ElseIf PassText.Contains("EXEC") Then
            Return False
        ElseIf PassText.Contains("EXECUTE") Then
            Return False
        ElseIf PassText.Contains("DROP") Then
            Return False
        ElseIf PassText.Contains("TABLE") Then
            Return False
        ElseIf PassText.Contains("DATABASE") Then
            Return False
        ElseIf PassText.Contains("ALTER") Then
            Return False
        ElseIf PassText.Contains("CONVERT") Then
            Return False
        ElseIf PassText.Contains("CAST") Then
            Return False
        ElseIf PassText.Contains("NVARCHAR") Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Shared Sub ActivityLog(ByVal FileLocation As String, ByVal UserIP As String, ByVal UserID As String, ByVal SessionID As String, ByVal nStatus As String, ByVal nActivity As String, ByVal LogRemark As String)
        'Write log to txt file
        'Creating stream writer type object 
        Dim SW As StreamWriter
        'Create filestream type object 
        Dim Fs As FileStream
        'Creating stream.txt using filestream object and returning reference to 
        'Filestream object 
        Fs = New FileStream(FileLocation, FileMode.Append)
        'Initalizing stream writer object using filestream object 
        SW = New StreamWriter(Fs)

        SW.WriteLine("")
        SW.WriteLine("Workstation IP : " & UserIP)
        SW.WriteLine("User ID : " & UserID)
        SW.WriteLine("Session ID : " & SessionID)
        SW.WriteLine("Date : " & Now())
        SW.WriteLine("Status : " & nStatus)
        SW.WriteLine("Activity : " & nActivity)
        SW.WriteLine("Remark : " & LogRemark)
        SW.WriteLine("")
        SW.WriteLine("************************************************************************************")

        'Closing stream writer 
        SW.Close()
        'Closing file stream 
        Fs.Close()
    End Sub

    Public Shared Function EmailValidation(ByVal strEmail As String) As Boolean
        Return Regex.IsMatch(strEmail, "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
    End Function

    'Public Shared Function EmailValidation(ByVal strEmail As String) As Boolean
    '    Dim lenWord As String
    '    Dim intCount1 As Integer
    '    Dim intCount2 As Integer
    '    Dim i As Integer

    '    'check appearance of '@' and '.'
    '    intCount1 = 0
    '    intCount2 = 0

    '    For i = 1 To Len(strEmail)
    '        lenWord = Mid(strEmail, i, 1)

    '        If lenWord = "@" Then
    '            intCount1 = intCount1 + 1
    '        ElseIf lenWord = " " Then
    '            intCount2 = intCount2 + 1
    '        End If
    '    Next

    '    If intCount1 <> 1 Or intCount2 > 0 Then
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function

    Public Shared Function RunningNumber(ByVal ActivityID As String, ByVal StockistCode As String, Optional ByVal CountryCode As String = "", Optional ByVal AreaCode As String = "") As String
        Dim rs As SqlDataReader
        Dim cn As SqlConnection

        cn = New SqlConnection(GetConnectionSetting)
        cn.Open()

        Dim cmd As SqlCommand = cn.CreateCommand

        cmd.Connection = cn

        Try
            cmd.CommandText = "EXECUTE sp_RunningNumber '" & ActivityID & "', '" & StockistCode & "', '" & CountryCode & "','" & AreaCode & "'"
            rs = cmd.ExecuteReader

            If rs.HasRows Then
                rs.Read()

                Dim NoStart As Integer
                Dim NoDigit As Integer
                Dim RunningHeader As String
                Dim RunningNo As Integer
                Dim strSQL As String

                strSQL = rs("SQLSTATEMENT")
                NoDigit = CType(rs("NDIGIT"), Integer)
                NoStart = CType(rs("STARTNO"), Integer)
                RunningHeader = rs("RHEADER")

                rs.Close()


                If ActivityID = "SS" Then
                    cmd.CommandText = "SELECT ISNULL(RunningCode, '') AS RunningCode FROM tblStockist WHERE StockistCode = '" & StockistCode & "' "
                    rs = cmd.ExecuteReader

                    If rs.HasRows Then
                        rs.Read()

                        If rs("RunningCode") <> "" Then
                            RunningHeader = rs("RunningCode")
                        End If
                    End If

                    rs.Close()
                End If


                cmd.CommandText = strSQL

                If cmd.CommandText = "" Then
                    RunningNo = NoStart
                Else
                    rs = cmd.ExecuteReader

                    If rs.HasRows Then
                        rs.Read()

                        If rs("RNO") = "" Then
                            RunningNo = NoStart
                        Else
                            Try
                                RunningNo = CType(Right(rs("RNO"), NoDigit), Integer) + 1
                            Catch ex As Exception
                                RunningNo = NoStart
                            End Try
                        End If
                    End If

                    rs.Close()
                End If

                Dim i As Integer
                Dim NoZero As String = ""

                For i = 1 To NoDigit - Len(RunningNo.ToString)
                    NoZero = NoZero & "0"
                Next

                Return RunningHeader & NoZero & RunningNo.ToString
            Else
                rs.Close()

                Return ""
            End If
        Catch ex As Exception
            Return ""
        Finally
            cn.Close()
        End Try
    End Function

    Public Shared Sub DownloadFile(ByVal FilePath As String, Optional ByVal ContentType As String = "")
        If File.Exists(FilePath) Then
            Dim myFileInfo As FileInfo
            Dim StartPos As Long = 0, FileSize As Long, EndPos As Long

            myFileInfo = New FileInfo(FilePath)
            FileSize = myFileInfo.Length
            EndPos = FileSize

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()

            Dim Range As String = HttpContext.Current.Request.Headers("Range")

            If Not ((Range Is Nothing) Or (Range = "")) Then
                Dim StartEnd As Array = Range.Substring(Range.LastIndexOf("=") + 1).Split("-")

                If Not StartEnd(0) = "" Then
                    StartPos = CType(StartEnd(0), Long)
                End If

                If StartEnd.GetUpperBound(0) >= 1 And Not StartEnd(1) = "" Then
                    EndPos = CType(StartEnd(1), Long)
                Else
                    EndPos = FileSize - StartPos
                End If

                If EndPos > FileSize Then
                    EndPos = FileSize - StartPos
                End If

                HttpContext.Current.Response.StatusCode = 206
                HttpContext.Current.Response.StatusDescription = "Partial Content"
                HttpContext.Current.Response.AppendHeader("Content-Range", "bytes " & StartPos & "-" & EndPos & "/" & FileSize)
            End If

            If Not (ContentType = "") And (StartPos = 0) Then
                HttpContext.Current.Response.ContentType = ContentType
            End If

            HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment; filename=" & myFileInfo.Name)
            HttpContext.Current.Response.WriteFile(FilePath, StartPos, EndPos)
            HttpContext.Current.Response.End()
        End If
    End Sub

    ' ******This function is for Rich Text Editor*****
    Public Shared Function RTESafe(ByVal strText As String) As String
        'returns safe code for preloading in the RTE
        Dim tmpString

        tmpString = Trim(strText)

        'convert all types of single quotes
        tmpString = Replace(tmpString, Chr(145), Chr(39))
        tmpString = Replace(tmpString, Chr(146), Chr(39))
        tmpString = Replace(tmpString, "'", "&#39;")

        'convert all types of double quotes
        tmpString = Replace(tmpString, Chr(147), Chr(34))
        tmpString = Replace(tmpString, Chr(148), Chr(34))
        '	tmpString = replace(tmpString, """", "\""")

        'replace carriage returns & line feeds
        tmpString = Replace(tmpString, Chr(10), " ")
        tmpString = Replace(tmpString, Chr(13), " ")

        RTESafe = tmpString
    End Function

    Public Shared Sub sendEmail(ByVal strMessage As String, ByVal strRecipient As String, ByVal strCC As String, ByVal strSubject As String)
        Dim MyEmail = New MailMessage
        Dim MyClient = New SmtpClient()
        Dim FromAddress = New MailAddress(fromEmail)
        Dim ToAddress = New MailAddress(strRecipient)

        Try

            Dim DisplayName As String = fromEmail
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
            MyClient.Host = System.Web.Configuration.WebConfigurationManager.AppSettings("emailDomain")
            MyClient.Port = 587
            MyClient.EnableSsl = True
            MyClient.Credentials = New System.Net.NetworkCredential(System.Web.Configuration.WebConfigurationManager.AppSettings("Email1"), System.Web.Configuration.WebConfigurationManager.AppSettings("Email2"))

            MyEmail.Body = strMessage
            MyEmail.IsBodyHtml = "True"

            MyEmail.From = FromAddress
            MyEmail.To.Add(ToAddress)

            If strCC <> "" Then
                MyEmail.CC.Add(strCC)
            End If

            MyEmail.Subject = strSubject
            MyClient.DeliveryMethod = SmtpDeliveryMethod.Network
            MyClient.Send(MyEmail)
        Catch ex As Exception
            ActivityLog(ConnectionLogLocation, "SYSTEM", "SYSTEM", "SYSTEM", "F", "Send Email", ex.ToString)

        End Try

        'Dim MyEmail = New MailMessage
        'Dim MyClient = New SmtpClient(System.Web.Configuration.WebConfigurationManager.AppSettings("emailDomain"))
        'Dim FromAddress = New MailAddress(System.Web.Configuration.WebConfigurationManager.AppSettings("fromEmail"))
        'Dim ToAddress = New MailAddress(strRecipient)

        'Try

        '    MyEmail.Body = strMessage
        '    MyEmail.IsBodyHtml = "True"

        '    MyEmail.From = FromAddress
        '    MyEmail.To.Add(ToAddress)

        '    If strCC <> "" Then
        '        'Dim CCAddress = New MailAddress(strCC)

        '        'MyEmail.CC.Add = CCAddress
        '        MyEmail.CC.Add(strCC)
        '    End If

        '    MyEmail.Subject = strSubject
        '    MyClient.DeliveryMethod = SmtpDeliveryMethod.Network
        '    'MyClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis
        '    MyClient.Send(MyEmail)
        'Catch ex As Exception
        '    ActivityLog(ConnectionLogLocation, "SYSTEM", "SYSTEM", "SYSTEM", "F", "Send Email", ex.ToString)

        'End Try
    End Sub

    Public Shared Sub sendEmailBySMTP(ByVal strMessage As String, ByVal strRecipient As String, ByVal strCC As String, ByVal strSubject As String)

        Dim MyEmail = New MailMessage
        Dim MyClient = New SmtpClient()
        Dim FromAddress = New MailAddress(fromEmail)
        Dim ToAddress = New MailAddress(strRecipient)

        Try

            Dim DisplayName As String = fromEmail
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
            MyClient.Host = System.Web.Configuration.WebConfigurationManager.AppSettings("emailDomain")
            MyClient.Port = 587
            MyClient.EnableSsl = True
            MyClient.Credentials = New System.Net.NetworkCredential(System.Web.Configuration.WebConfigurationManager.AppSettings("Email1"), System.Web.Configuration.WebConfigurationManager.AppSettings("Email2"))

            MyEmail.Body = strMessage
            MyEmail.IsBodyHtml = "True"

            MyEmail.From = FromAddress
            MyEmail.To.Add(ToAddress)

            If strCC <> "" Then
                MyEmail.CC.Add(strCC)
            End If

            MyEmail.Subject = strSubject
            MyClient.DeliveryMethod = SmtpDeliveryMethod.Network
            MyClient.Send(MyEmail)

        Catch ex As Exception
            ActivityLog(ConnectionLogLocation, "SYSTEM", "SYSTEM", "SYSTEM", "F", "Send Email", ex.ToString)

        End Try

    End Sub

    Public Shared Function unit(ByVal u) As String
        Dim temp As String = 0

        Select Case (u)
            Case 0 : temp = " "
            Case 1 : temp = " One"
            Case 2 : temp = " Two"
            Case 3 : temp = " Three"
            Case 4 : temp = " Four"
            Case 5 : temp = " Five"
            Case 6 : temp = " Six"
            Case 7 : temp = " Seven"
            Case 8 : temp = " Eight"
            Case 9 : temp = " Nine"
        End Select

        unit = temp
    End Function

    Public Shared Function tens1(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 10 : temp = " Ten"
            Case 11 : temp = " Eleven"
            Case 12 : temp = " Twelve"
            Case 13 : temp = " Thirteen"
            Case 14 : temp = " Fourteen"
            Case 15 : temp = " Fifteen"
            Case 16 : temp = " Sixteen"
            Case 17 : temp = " Seventeen"
            Case 18 : temp = " Eighteen"
            Case 19 : temp = " Nineteen"
        End Select

        tens1 = temp
    End Function

    Public Shared Function tens(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 2 : temp = " Twenty"
            Case 3 : temp = " Thirty"
            Case 4 : temp = " Forty"
            Case 5 : temp = " Fifty"
            Case 6 : temp = " Sixty"
            Case 7 : temp = " Seventy"
            Case 8 : temp = " Eighty"
            Case 9 : temp = " Ninty"
        End Select

        tens = temp
    End Function

    Public Shared Function hund(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 1 : temp = " One Hundred"
            Case 2 : temp = " Two Hundred"
            Case 3 : temp = " Three Hundred"
            Case 4 : temp = " Four Hundred"
            Case 5 : temp = " Five Hundred"
            Case 6 : temp = " Six Hundred"
            Case 7 : temp = " Seven Hundred"
            Case 8 : temp = " Eight Hundred"
            Case 9 : temp = " Nine Hundred"
        End Select

        hund = temp
    End Function

    Public Shared Function Thund(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 1 : temp = " One Thousand"
            Case 2 : temp = " Two Thousand"
            Case 3 : temp = " Three Thousand"
            Case 4 : temp = " Four Thousand"
            Case 5 : temp = " Five Thousand"
            Case 6 : temp = " Six Thousand"
            Case 7 : temp = " Seven Thousand"
            Case 8 : temp = " Eight Thousand"
            Case 9 : temp = " Nine Thousand"
                ' Case 10: temp = " Ten Thousand"
        End Select

        Thund = temp
    End Function

    Public Shared Function TThund1(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 10 : temp = " Ten Thousand"
            Case 11 : temp = " Eleven Thousand"
            Case 12 : temp = " Twelve Thousand"
            Case 13 : temp = " Thirteen Thousand"
            Case 14 : temp = " Fourteen Thousand"
            Case 15 : temp = " Fifteen Thousand"
            Case 16 : temp = " Sixteen Thousand"
            Case 17 : temp = " Seventeen Thousand"
            Case 18 : temp = " Eighteen Thousand"
            Case 19 : temp = " Ninteen Thousand"

            Case 20 : temp = " Twenty Thousand"
            Case 30 : temp = " Thirty Thousand"
            Case 40 : temp = " Forty Thousand"
            Case 50 : temp = " Fifty Thousand"
            Case 60 : temp = " Sixty Thousand"
            Case 70 : temp = " Seventy Thousand"
            Case 80 : temp = " Eighty Thousand"
            Case 90 : temp = " Ninty Thousand"
        End Select

        TThund1 = temp
    End Function

    Public Shared Function TThund(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 2 : temp = " Twenty"
            Case 3 : temp = " Thirty"
            Case 4 : temp = " Fourty"
            Case 5 : temp = " Fifty"
            Case 6 : temp = " Sixty"
            Case 7 : temp = " Seventy"
            Case 8 : temp = " Eighty"
            Case 9 : temp = " Ninety"
        End Select

        TThund = temp
    End Function

    Public Shared Function TLakh(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 1 : temp = " One Lakh"
            Case 2 : temp = " Two Lakh"
            Case 3 : temp = " Three Lakh"
            Case 4 : temp = " Four Lakh"
            Case 5 : temp = " Five Lakh"
            Case 6 : temp = " Six Lakh"
            Case 7 : temp = " Seven Lakh"
            Case 8 : temp = " Eight Lakh"
            Case 9 : temp = " Nine Lakh"
                ' Case 10: temp = " Ten Thousand"
        End Select

        TLakh = temp
    End Function

    Public Shared Function TTLakh1(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 10 : temp = " Ten Lakh"
            Case 11 : temp = " Eleven Lakh"
            Case 12 : temp = " Twelve Lakh"
            Case 13 : temp = " Thirteen Lakh"
            Case 14 : temp = " Fourteen Lakh"
            Case 15 : temp = " Fifteen Lakh"
            Case 16 : temp = " Sixteen Lakh"
            Case 17 : temp = " Seventeen Lakh"
            Case 18 : temp = " Eighteen Lakh"
            Case 19 : temp = " Ninteen Lakh"

            Case 20 : temp = " Twenty Lakh"
            Case 30 : temp = " Thirty Lakh"
            Case 40 : temp = " Forty Lakh"
            Case 50 : temp = " Fifty Lakh"
            Case 60 : temp = " Sixty Lakh"
            Case 70 : temp = " Seventy Lakh"
            Case 80 : temp = " Eighty Lakh"
            Case 90 : temp = " Ninty Lakh"
        End Select

        TTLakh1 = temp
    End Function

    Public Shared Function TTLakh(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 2 : temp = " Twenty"
            Case 3 : temp = " Thirty"
            Case 4 : temp = " Fourty"
            Case 5 : temp = " Fifty"
            Case 6 : temp = " Sixty"
            Case 7 : temp = " Seventy"
            Case 8 : temp = " Eighty"
            Case 9 : temp = " Ninety"
        End Select

        TTLakh = temp
    End Function

    Public Shared Function TCrore(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 1 : temp = " One Crore"
            Case 2 : temp = " Two Crore"
            Case 3 : temp = " Three Crore"
            Case 4 : temp = " Four Crore"
            Case 5 : temp = " Five Crore"
            Case 6 : temp = " Six Crore"
            Case 7 : temp = " Seven Crore"
            Case 8 : temp = " Eight Crore"
            Case 9 : temp = " Nine Crore"
                ' Case 10: temp = " Ten Thousand"
        End Select

        TCrore = temp
    End Function

    Public Shared Function TTCrore1(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 10 : temp = " Ten Crore"
            Case 11 : temp = " Eleven Crore"
            Case 12 : temp = " Twelve Crore"
            Case 13 : temp = " Thirteen Crore"
            Case 14 : temp = " Fourteen Crore"
            Case 15 : temp = " Fifteen Crore"
            Case 16 : temp = " Sixteen Crore"
            Case 17 : temp = " Seventeen Crore"
            Case 18 : temp = " Eighteen Crore"
            Case 19 : temp = " Ninteen Crore"

            Case 20 : temp = " Twenty Crore"
            Case 30 : temp = " Thirty Crore"
            Case 40 : temp = " Forty Crore"
            Case 50 : temp = " Fifty Crore"
            Case 60 : temp = " Sixty Crore"
            Case 70 : temp = " Seventy Crore"
            Case 80 : temp = " Eighty Crore"
            Case 90 : temp = " Ninty Crore"
        End Select

        TTCrore1 = temp
    End Function

    Public Shared Function TTCrore(ByVal t As Integer) As String
        Dim temp As String = 0

        Select Case (t)
            Case 2 : temp = " Twenty"
            Case 3 : temp = " Thirty"
            Case 4 : temp = " Fourty"
            Case 5 : temp = " Fifty"
            Case 6 : temp = " Sixty"
            Case 7 : temp = " Seventy"
            Case 8 : temp = " Eighty"
            Case 9 : temp = " Ninety"
        End Select

        TTCrore = temp
    End Function

    Public Shared Function digit2(ByVal Lg As String) As String
        Dim n As Integer
        Dim u As Integer
        Dim t As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 1)

        If (Lg1 = "1") Then
            sss = tens1(n)
        Else
            u = n Mod 10
            n = n \ 10
            t = n Mod 10
            sss = tens(t) & unit(u)
        End If

        digit2 = sss
    End Function

    Public Shared Function digit3(ByVal Lg As String) As String
        Dim tt As Integer
        Dim flag As Boolean

        flag = False

        Dim n As Integer
        Dim u As Integer
        Dim t As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 1)

        sss = hund(Val(Lg1))
        u = n Mod 10
        n = n \ 10
        t = n Mod 10

        If (t = 0) Then ' if middle digit is zero'
            Lg1 = Microsoft.VisualBasic.Right(Lg, 1)
            sss = sss + unit(Val(Lg1))
        Else
            If (t = 1) Then 'for middle digit'
                Lg1 = Microsoft.VisualBasic.Right(Lg, 1)
                tt = Val(Lg1) + 10
                sss = sss + tens1(tt)
            Else
                sss = sss + tens(t) + unit(u)
            End If
        End If

        digit3 = sss
    End Function

    Public Shared Function digit4(ByVal Lg As String) As String
        Dim n As Integer
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
        sss = Thund(Val(Lg1))

        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10

        If (h = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 2)
            sss = sss + digit2(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 3)
            sss = sss + digit3(Lg1)
        End If

        digit4 = sss
    End Function

    Public Shared Function digit5(ByVal Lg As String) As String
        Dim n As Double
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim thou As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 2)
        sss = TThund1(Val(Lg1))

        Dim F As String
        Dim N1 As String
        Dim res As Integer

        If (sss = "") Then
            F = Microsoft.VisualBasic.Left(Lg, 2)
            N1 = Microsoft.VisualBasic.Left(Lg, 1) + "0"
            res = Val(F) - Val(N1) 'second position from left'
            Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
            sss = TThund(Val(Lg1))
            sss = sss + Thund(res)
        End If

        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10
        n = n \ 10
        thou = n Mod 10


        If (h = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 2)
            sss = sss + digit2(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 3)
            sss = sss + digit3(Lg1)
        End If

        digit5 = sss
    End Function

    Public Shared Function digit6(ByVal Lg As String) As String
        Dim n As Double
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim thou As Integer
        Dim tthh As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
        sss = TLakh(Val(Lg1))

        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10
        n = n \ 10
        thou = n Mod 10
        n = n \ 10
        tthh = n Mod 10

        If (tthh = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 4)
            sss = sss + digit4(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 5)
            sss = sss + digit5(Lg1)
        End If

        digit6 = sss
    End Function

    Public Shared Function digit7(ByVal Lg As String) As String
        Dim n As Double
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim thou As Integer
        Dim tthh As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 2)
        sss = TTLakh1(Val(Lg1))

        Dim F As String
        Dim N1 As String
        Dim res As Integer

        If (sss = "") Then
            F = Microsoft.VisualBasic.Left(Lg, 2)
            N1 = Microsoft.VisualBasic.Left(Lg, 1) + "0"
            res = Val(F) - Val(N1) 'second position from left'
            Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
            sss = TTLakh(Val(Lg1))
            sss = sss + TLakh(res)
        End If

        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10
        n = n \ 10
        thou = n Mod 10
        n = n \ 10
        tthh = n Mod 10

        If (tthh = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 4)
            MsgBox(Lg1)
            sss = sss + digit4(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 5)
            sss = sss + digit5(Lg1)
        End If

        digit7 = sss
    End Function

    Public Shared Function digit8(ByVal Lg As String) As String
        Dim n As Double
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim thou As Integer
        Dim tthh As Integer
        Dim tthh6 As Integer
        Dim tthh7 As Integer

        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
        sss = TCrore(Val(Lg1))

        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10
        n = n \ 10
        thou = n Mod 10
        n = n \ 10
        tthh = n Mod 10
        n = n \ 10
        tthh6 = n Mod 10
        n = n \ 10
        tthh7 = n Mod 10

        MsgBox(tthh7)

        If (tthh7 = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 6)
            sss = sss + digit6(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 7)
            sss = sss + digit7(Lg1)
        End If

        digit8 = sss
    End Function

    Public Shared Function digit9(ByVal Lg As String) As String
        Dim n As Double
        Dim u As Integer
        Dim t As Integer
        Dim h As Integer
        Dim thou As Integer
        Dim tthh As Integer
        Dim tthh6 As Integer
        Dim tthh7 As Integer
        Dim tthh8 As Integer
        Dim sss As String
        Dim Lg1 As String

        n = Val(Lg)

        Lg1 = Microsoft.VisualBasic.Left(Lg, 2)
        sss = TTCrore1(Val(Lg1))

        Dim F As String
        Dim N1 As String
        Dim res As Integer

        If (sss = "") Then
            F = Microsoft.VisualBasic.Left(Lg, 2)
            N1 = Microsoft.VisualBasic.Left(Lg, 1) + "0"
            res = Val(F) - Val(N1) 'second position from left'
            Lg1 = Microsoft.VisualBasic.Left(Lg, 1)
            sss = TTCrore(Val(Lg1))
            sss = sss + TCrore(res)
        End If


        u = n Mod 10
        n = n \ 10
        t = n Mod 10
        n = n \ 10
        h = n Mod 10
        n = n \ 10
        thou = n Mod 10
        n = n \ 10
        tthh = n Mod 10
        n = n \ 10
        tthh6 = n Mod 10
        n = n \ 10
        tthh7 = n Mod 10
        n = n \ 10
        tthh8 = n Mod 10

        If (tthh7 = 0) Then
            Lg1 = Microsoft.VisualBasic.Right(Lg, 6)
            MsgBox(Lg1)
            sss = sss + digit6(Lg1)
        Else
            Lg1 = Microsoft.VisualBasic.Right(Lg, 7)
            sss = sss + digit7(Lg1)
        End If

        digit9 = sss
    End Function

    Public Shared Function GroupToWords(ByVal num As Integer) As _
        String
        Static one_to_nineteen() As String = {"zero", "one", _
            "two", "three", "four", "five", "six", "seven", _
            "eight", "nine", "ten", "eleven", "twelve", _
            "thirteen", "fourteen", "fifteen", "sixteen", _
            "seventeen", "eightteen", "nineteen"}
        Static multiples_of_ten() As String = {"twenty", _
            "thirty", "forty", "fifty", "sixty", "seventy", _
            "eighty", "ninety"}

        ' If the number is 0, return an empty string.
        If num = 0 Then Return ""

        ' Handle the hundreds digit.
        Dim digit As Integer
        Dim result As String = ""
        If num > 99 Then
            digit = num \ 100
            num = num Mod 100
            result = one_to_nineteen(digit) & " hundred"
        End If

        ' If num = 0, we have hundreds only.
        If num = 0 Then Return result.Trim()

        ' See if the rest is less than 20.
        If num < 20 Then
            ' Look up the correct name.
            result &= " " & one_to_nineteen(num)
        Else
            ' Handle the tens digit.
            digit = num \ 10
            num = num Mod 10
            result &= " " & multiples_of_ten(digit - 2)

            ' Handle the final digit.
            If num > 0 Then
                result &= " " & one_to_nineteen(num)
            End If
        End If

        Return result.Trim()
    End Function

    Public Shared Function NumberToString(ByVal num As Double) As _
    String
        ' Remove any fractional part.
        num = Int(num)

        ' If the number is 0, return zero.
        If num = 0 Then Return "zero"

        Static groups() As String = {"", "thousand", "million", _
            "billion", "trillion", "quadrillion", "?", "??", _
            "???", "????"}
        Dim result As String = ""

        ' Process the groups, smallest first.
        Dim quotient As Double
        Dim remainder As Integer
        Dim group_num As Integer = 0
        Do While num > 0
            ' Get the next group of three digits.
            quotient = Int(num / 1000)
            remainder = CInt(num - quotient * 1000)
            num = quotient

            ' Convert the group into words.
            result = GroupToWords(remainder) & _
                " " & groups(group_num) & _
                result

            ' Get ready for the next group.
            group_num += 1
        Loop

        ' Remove the trailing ", ".
        If result.EndsWith(", ") Then
            result = result.Substring(0, result.Length - 2)
        End If

        Return result.Trim().ToUpper
    End Function

    Public Shared Function PopUp(ByVal ErrorMsg As String) As String
        Dim popUpScript As String
        popUpScript = "<SCRIPT LANGUAGE=""JavaScript"">" & vbCrLf
        popUpScript = popUpScript & "alert(""" & ErrorMsg & """)" & vbCrLf
        popUpScript = popUpScript & "</SCRIPT>"
        Return popUpScript
    End Function

    Public Shared Sub ExportExcelGridView(ByVal GridView1 As GridView)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True

        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        HttpContext.Current.Response.Charset = ""
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        GridView1.AllowPaging = False
        GridView1.DataBind()

        GridView1.RenderControl(hw)

        'style to format numbers to string 
        Dim style As String = "<style> td { mso-number-format:\@; } </style>"
        HttpContext.Current.Response.Write(style)
        HttpContext.Current.Response.Output.Write(sw.ToString())
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()
    End Sub


    Public Shared Sub ExportExcel(ByVal GridView1 As DataGrid)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True

        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        HttpContext.Current.Response.Charset = ""
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())

        Dim sw As New StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        GridView1.AllowPaging = False
        GridView1.DataBind()


        GridView1.RenderControl(hw)

        'style to format numbers to string 
        Dim style As String = "<style> td { mso-number-format:\@; } </style>"
        HttpContext.Current.Response.Write(style)
        HttpContext.Current.Response.Output.Write(sw.ToString())
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()
    End Sub
End Class
