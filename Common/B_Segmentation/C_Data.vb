Imports CommonTreapVB.TreapVB
Imports B_Data.Funny
Imports B_String.Funny
Imports B_XML.Funny
Imports B_File.Funny
Imports System
Imports System.IO
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions


Namespace FunnyWeber

    Public Class C_Data '_Skin_Var
        '一些系统变量

        Public bSave2Memory As Boolean = False   '是否把模板存储到内存,这样,下次就会比较快了
        Public bReload As Boolean = False    '是否重新读取模板
        Public Data_Value As String   '模板对应的key值,注意唯一

        Public StaticFile As String

        Public strBody As String

        Public Count_Include As Long   'Count_Include 最多6次
        Public Count_XML_Repeat As Long   'XML_Repeat 最多20次
        Public Count_Right As Long    '权限判断次数

        Public pSession As C_Session
        Public pServer As C_Server

        '====================================================

        Public strSaveTag, strFileType As String '文件保存类型和类型

        Public MAX_XML As Long = 3000
        Public strF As String     '模板ID 
        Public strT As String     'TableHead

        Public Path_SYS As String    '<Path> 系统变量

        Public strCacheFile As String = ""
        Public strClick As String
        Public strXMLHead As String = ""    'XML 头文件
        Public strParamBody As String    'Body
        Public strSkin As String    '皮肤的地址
        Public strNextURL As String = ""     '运行完毕转到的地址

        '=======================一些 Boolean 选项=========================
        Public b_Transform_In_Server As Boolean = False   '是否在服务器合成
        Public bRecordStatus As Boolean = False    '是否记录状态,默认不记录
        Public bShowCode As Boolean = False     '是否显示注册码

        Public bCache As Boolean = False
        Public bCache_Static As Boolean = False '静态缓存,如果有静态文件,则不更新,除非传参数 Cache=0
        Public bError As Boolean = False   '是否有运行错误

        Public bAddBR, bMath, bFilterVbCrlf, bHtmlEncode As Boolean

        '========================一些 整数变量================================
        Public minSize As Int32 = 40     'SYS.MIN.FILESIZE 文件的最小size
        Public intGroupID As Int32

        '========================一些浮点变量 ================================
        Public db_Time_AutoCreate As Double = 12    '超过12个小时自动创建新的静态页面

        Dim strCacheKey As String

        Public pRefresh As C_Queue     '要刷新的集合


        '========================一些文件变量 ================================
        Public file_Static As String    '静态文件 相对路径
        Public file_Static_Real As String    '静态文件 绝对路径


        '=======================================
        Public strError As String    '错误
        Public Glb_Error As Boolean = False    '全局错误变量
        '=======================================


        Public Var_User As New Treap(Of C_Var2)     '用户参数集合
        'Public Var_RegEx As New Treap    '正则表达式 集合
        Public Var_Request As New Treap(Of C_Var2) '如果这里有的变量则替换 Request 的


        Public Property CacheKey() As String  '关键字
            Get
                Return strCacheKey
            End Get
            Set(ByVal Value As String)
                Value = Replace(Value, ",", " ")
                Value = Trim(Value)
                strCacheKey = Value
            End Set
        End Property


        '====================================================
        Public Sub New(ByRef TSession As C_Session, _
                    ByRef pRequest As C_Request, _
                    ByVal bIIS As Boolean)

            pSession = TSession
            If pSession IsNot Nothing Then
                pServer = pSession.pServer
            End If

            If bIIS Then
                bReload = (pRequest.GetValue("R") = "1")

                If HttpContext.Current.Application("Init.Allow") Is Nothing Then
                    HttpContext.Current.Application("Init.Allow") = pServer.GetWebSetting("Init.Allow")
                End If
            End If

            strF = pRequest.GetValue("F")
        End Sub


        Public Function PowerReplace( _
         ByVal strTable As String, _
         ByRef strPower As String, _
         ByRef strReturn As String) As Boolean

            Dim bError As Boolean = False

            Select Case LCase(StrTable)
                Case "成员", "文件"
                    strPower = Replace(strPower, "*", Me.GetFunnyChar(StrTable))

                Case Else
                    strReturn = "表格名称错误:{" & StrTable & "}"
            End Select

            Return bError
        End Function


        Public Function GetFunnyChar(ByVal strTable As String) As String
            Dim strReturn As String = ""
            Select Case LCase(strTable)
                Case "成员"
                    StrReturn = "U"
                Case "文件"
                    StrReturn = "F"
            End Select

            Return StrReturn
        End Function


        Public Function GetTableFromPower(ByVal StrPower As String) As String
            Dim strTable As String = "文件"
            Select Case StrPower
                Case "FR"
                    strTable = "文件"
                Case "UR"
                    strTable = "成员"
            End Select
            Return strTable
        End Function


        'Public Sub InitDir_Sub_Relative(ByVal StrFile As String)
        '    S_SYS.InitDir(HttpContext.Current.Server.MapPath(StrFile))
        'End Sub


        Public Sub InitURL(ByVal strPath As String)

            Dim intPos As Int32 = InStr(strPath, "?")

            If intPos > 0 Then
                Dim strSplit() As String = Split(Right(strPath, Len(strPath) - intPos), "&")
                Dim i As Int32
                Dim pVar As C_Var2
                For i = 0 To UBound(strSplit)
                    intPos = InStr(strSplit(i), "=")
                    If intPos > 0 Then
                        pVar = New C_Var2(Left(strSplit(i), intPos - 1))
                        pVar.Value = Right(strSplit(i), Len(strSplit(i)) - intPos)
                        Me.Var_Request.insert(New C_K_Str(pVar.Key), CType(pVar, Object))
                    End If
                Next
            End If
        End Sub

        Public Function Request( _
            ByVal strKey As String, _
            ByRef pRequest As C_Request) As String

            'Request
            Dim pVar As C_Var2 = Var_Request.find(New C_K_Str(strKey))
            Dim strReturn As String

            If pVar Is Nothing Then
                If pServer.bIIS Or pRequest Is Nothing Then
                    strReturn = HttpContext.Current.Request(strKey)
                Else
                    strReturn = pRequest.GetValue(strKey)
                End If
            Else
                strReturn = pVar.Value
            End If
            Return strReturn
        End Function

        Public Function Get_Var_User(ByVal strKey As String) As String
            '用户 参数替换
            Dim pVar As C_Var2 = Me.Var_User.find(New C_K_Str(strKey))
            Dim strReturn As String = ""

            If Not pVar Is Nothing Then
                strReturn = pVar.Value
            End If

            Return strReturn
        End Function




        Public Function Var_Replace( _
            ByVal strContent As String, _
            Optional ByVal strLeft As String = "<V.U>", _
            Optional ByVal strRight As String = "</V.U>") As String
            '用户 参数替换

            Dim pVar As C_Var2
            Dim p As TreapEnumerator 'S_TreapEnumerator

            p = Me.Var_User.Elements(True)
            Do While (p.HasMoreElements())
                pVar = p.NextElement()
                strContent = Replace(strContent, _
                    strLeft + pVar.Key + strRight, _
                    pVar.Value, , , CompareMethod.Text)
            Loop

            If strContent = "" Then Return ""

            Dim pMatch As Match
            Dim pMatchs As MatchCollection
            'Dim Regex As Regex
            Dim i As Long

            'Regex = New Regex("", RegexOptions.IgnoreCase)

            Dim StrAttribute As String
            Dim StrSplit(), StrSplit2() As String


            Do While (p.HasMoreElements())
                pVar = p.NextElement()

                pMatchs = Regex.Matches(strContent, "<V\.U([^>]*)>" + pVar.Key + "<\/V\.U>")

                For Each pMatch In pMatchs
                    If pMatch.Success Then
                        StrAttribute = Trim(pMatch.Groups(1).Value)
                        StrSplit = Split(StrAttribute, " ")
                        For i = 0 To UBound(StrSplit)
                            StrSplit2 = Split(StrSplit(i), "=")
                            If UBound(StrSplit2) >= 1 Then
                                If StrSplit2(0) = "cdata" And StrSplit2(1) = """yes""" Then
                                    strContent = Replace(strContent, pMatch.Groups(0).Value, S_Strings.CData_Encode(pVar.Value))
                                End If
                            End If
                        Next
                    End If
                Next
            Loop

            '1zzzzzzzzzzzzz
            '   strContent = Replace(strContent, strLeft + "C" + strRight, HttpContext.Current.Request("C"), , , CompareMethod.Text)
            strContent = Replace(strContent, strLeft + "T" + strRight, Me.strT, , , CompareMethod.Text)

            Return strContent
        End Function


        Public Function Var_Replace_System( _
            ByVal strContent As String, _
            Optional ByVal StrLeft As String = "<V.S>", _
            Optional ByVal StrRight As String = "</V.S>") As String

            Dim strT As String = Me.strT

            strContent = Replace(strContent, "<T />", strT, , , CompareMethod.Text)
            strContent = Replace(strContent, "<T/>", strT, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "T" + StrRight, strT, , , CompareMethod.Text)

            Dim strTmp As String
            strTmp = Replace(Me.strF, "\", "/")

            strContent = Replace(strContent, StrLeft + "F" + StrRight, strTmp, , , CompareMethod.Text)

            'strContent = Replace(strContent, StrLeft + "RootID" + StrRight, Val(HttpContext.Current.Request("RootID")), , , CompareMethod.Text)

            strTmp = Replace(Me.Path_SYS, "\", "/")
            strContent = Replace(strContent, "<Path />", strTmp, , , CompareMethod.Text)
            strContent = Replace(strContent, "<Path/>", strTmp, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Path" + StrRight, strTmp, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Path.Home" + StrRight, pServer.Path_Home, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Web.URL" + StrRight, pServer.Web_URL, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Web.Name" + StrRight, pServer.Web_Name, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Admin.Email" + StrRight, pServer.Admin_Email, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Path.JavaScript" + StrRight, Replace(Me.Path_SYS, "\", "/"), , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Current.User.ID" + StrRight, pSession.User_ID, , , CompareMethod.Text)
            strContent = Replace(strContent, StrLeft + "Current.User.Name" + StrRight, pSession.User_Name, , , CompareMethod.Text)

            If pServer.lngOnlineTime > 1 Then
                strContent = Replace(strContent, StrLeft + "Web.Online" + StrRight, (pServer.lng_Online * pServer.lngOnlineTime + pServer.lngOnlineAdd + CInt(Rnd() * 90)))
            Else
                strContent = Replace(strContent, StrLeft + "Web.Online" + StrRight, (pServer.lng_Online * pServer.lngOnlineTime + pServer.lngOnlineAdd))
            End If


            strContent = Replace(strContent, StrLeft + "Now" + StrRight, Now())
            strContent = Replace(strContent, StrLeft + "Now.Date" + StrRight, Format(Now(), "yyyy-MM-dd"))
            strContent = Replace(strContent, StrLeft + "Now.Year" + StrRight, Format(Now(), "yyyy"))
            strContent = Replace(strContent, StrLeft + "Now.Month" + StrRight, Format(Now(), "MM"))
            strContent = Replace(strContent, StrLeft + "Now.Day" + StrRight, Format(Now(), "dd"))

            strContent = Replace(strContent, StrLeft + "Now.Day2" + StrRight, Microsoft.VisualBasic.DateAndTime.Day(Now()))

            strContent = Replace(strContent, StrLeft + "Now.Week" + StrRight, DateAndTime.WeekdayName(DateAndTime.Weekday(Now())))

            Return strContent
        End Function

        Public Sub Init( _
            ByRef pServer As C_Server, _
            ByVal mT As String, ByVal strRawURL As String)

            'strT = S_FunnyWeber.Get_T(mT)

            Dim strDir, StrURL As String
            strDir = Path.GetDirectoryName(pServer.MapPath(pServer.Path_Home & "Cache/Home/" & _
            Replace(Replace(Me.strF, "\", "/"), ":", "")) & "/")

            StrURL = Replace(strRawURL, Me.strF, "", , CompareMethod.Text)
            StrURL = Replace(StrURL, "/H6.aspx?", "", , , CompareMethod.Text)

            Me.StaticFile = strDir & "\" & Me.GetCacheName(StrURL)
        End Sub



        Public Function Attribute_Replace( _
                ByVal strValue As String) As String

            Return Me.Var_Replace_System( _
                Me.Var_Replace(strValue, "(V.U)", "(/V.U)"), _
                "(V.S)", "(/V.S)")
        End Function

        Public Function SessionValue(ByVal StrKey As String) As String
            Dim strReturn As String = ""
            If Not HttpContext.Current.Session(StrKey) Is Nothing Then
                strReturn = HttpContext.Current.Session(StrKey)
            End If
            Return strReturn
        End Function




        Public Function GetIP() As String
            With HttpContext.Current.Request
                Dim strIPAddr As String
                If .ServerVariables("HTTP_X_FORWARDED_FOR") = "" Or InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), "unknown") > 0 Then
                    strIPAddr = .ServerVariables("REMOTE_ADDR")
                ElseIf InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ",") > 0 Then
                    strIPAddr = Mid(.ServerVariables("HTTP_X_FORWARDED_FOR"), 1, InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ",") - 1)
                ElseIf InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ";") > 0 Then
                    strIPAddr = Mid(.ServerVariables("HTTP_X_FORWARDED_FOR"), 1, InStr(.ServerVariables("HTTP_X_FORWARDED_FOR"), ";") - 1)
                Else
                    strIPAddr = .ServerVariables("HTTP_X_FORWARDED_FOR")
                End If
                GetIP = Trim(Mid(strIPAddr, 1, 30))
            End With
        End Function


        'Public Sub Init_Browser6( _
        ' ByVal StrEmail As String, _
        ' ByVal StrPassword As String, _
        ' Optional ByVal bQuick As Boolean = False)

        '    strT = HttpContext.Current.Request("T")
        '    If Right(strT, 1) <> "_" Then strT = ""

        '    pSession.User_Email = StrEmail
        '    pSession.User_Passsword = FormsAuthentication.HashPasswordForStoringInConfigFile(StrPassword, "md5")

        '    strBody = pServer.Check_ByEmail(pSession, pSession.User_Passsword, pSession.User_Cookie, bQuick, False)

        '    If pSession.bChecked = True Then
        '        'pSession.User_ID = S_FunnyWeber.User_ID_fromEmail(pSession.pServer, pSession.User_Email, pSession.User_Name)
        '    Else
        '        pSession.User_ID = 8
        '        pSession.User_Name = "{Guest}"
        '    End If
        '    If pSession.User_ID <= 10000 Then pSession.User_ID = 8

        '    '==============================

        '    Dim StrDir, StrURL As String

        '    StrDir = Path.GetDirectoryName(HttpContext.Current.Server.MapPath(pServer.Path_Home & "Cache/Home/" & _
        '    Replace(Replace(HttpContext.Current.Request("F"), "\", "/"), ":", "")) & "/")

        '    If Directory.Exists(StrDir) = False Then
        '        Directory.CreateDirectory(StrDir)
        '    End If

        '    StrURL = Replace(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request("F"), "", , , CompareMethod.Text)
        '    StrURL = Replace(StrURL, "/H6.aspx?", "", , , CompareMethod.Text)

        '    Me.StaticFile = StrDir & "\" & Me.GetCacheName(StrURL)
        'End Sub



        Public Function File_ID_FromKey(ByVal strKey As String) As Long
            '获取权限ID
            Dim lngReturn As Long, strFile As String

            strFile = pServer.Path_Home & "Upload_File/" & strKey & ".funny"
            strFile = HttpContext.Current.Server.MapPath(strFile)

            Dim pXML_Read As C_XML_Read = New C_XML_Read
            lngReturn = Val(pXML_Read.Read_From_File(strFile, "/Data/Basic/ID", C_XML_Read.Read_Type.C_InnerText, "", True))
            pXML_Read = Nothing

            Return lngReturn
        End Function


        Public Function GetCacheName(ByVal StrFile As String) As String
            StrFile = Replace(StrFile, "/", "_1_")
            StrFile = Replace(StrFile, "\", "_2_")
            StrFile = Replace(StrFile, ":", "_3_")
            StrFile = Replace(StrFile, "*", "_4_")
            StrFile = Replace(StrFile, "?", "_5_")
            StrFile = Replace(StrFile, """", "_6_")
            StrFile = Replace(StrFile, "<", "_7_")
            StrFile = Replace(StrFile, ">", "_8_")
            StrFile = Replace(StrFile, "|", "_9_")

            Return StrFile & ".html"
        End Function


        Public Sub Copy(ByVal pData As C_Data)
            Me.strT = pData.strT
            'Me.CurrentSystem = pSysParam.CurrentSystem
        End Sub


        Public Sub Set_User_Var( _
            ByVal strKey As String, _
            ByVal strValue As String)

            '用户 参数替换
            Dim pVar As C_Var2

            pVar = Me.Var_User.find(New C_K_Str(strKey))

            If Not pVar Is Nothing Then
                pVar.Value = strValue
            Else
                pVar = New C_Var2(strKey)
                pVar.Value = strValue
                Me.Var_User.insert(New C_K_Str(strKey), CType(pVar, Object))
            End If


        End Sub


        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
