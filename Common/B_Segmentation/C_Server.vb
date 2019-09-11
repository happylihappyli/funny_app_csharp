

Imports System
Imports System.IO
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Web
Imports System.Web.Caching
Imports System.Net

Imports CommonTreapVB.TreapVB
Imports B_Data.Funny
Imports B_XML.Funny
Imports B_Segmentation.FunnyWeber
Imports B_File.Funny

Namespace FunnyWeber

    Public Class C_WebServer_Variable
        Public Key As String
        Public Value As String
    End Class


    Public Class C_Server
        '系统变量,只需要一个实例 代替原来的C_APP

        Public InitSystem As Boolean = False '是否初试化系统

        '-----------------String 变量
        Public Path_Home As String    '网站根目录
        Public Path_Pic_Tmp As String    '图片临时目录
        Public Path_Data As String  '数据所在的目录
        Public UploadPath As String
        Public Web_URL As String = "www.funnyweber.com"    '默认网址 不要http等
        Public Web_Name As String = "FunnyWeber"
        Public Web_Proxy As String = ""
        Public Admin_Email As String        '管理员email
        Public Admin_Password As String         '管理员Email密码

        Public AppPath As String
        '-----------------/String

        '-----------------Int 变量
        Public lng_Online As Int32        '在线人数
        '-----------------/Int

        'Public pFunnyService As FunnyService = New FunnyService
        'Public pCache_Search As C_Cache_Update_Tree = New C_Cache_Update_Tree

        Public pFunnySave As C_Queue = New C_Queue
        Public pQue_Refresh As C_Queue_File_Msg    '要刷新的集合 

        '------------------------------------
        'Public pFileKey As New Treap
        Public Tree_Que_Recycle As New Treap(Of C_Queue)
        'Public pRoomCollect As New Treap 'Room 集合


        Public bNoEmail As Boolean = False

        Public Tree_Room As New Treap(Of C_User) 'Room 集合

        'Public Tree_KeyWord As New Treap  '联想单词扩展
        'Public Tree_Online As New Treap '在线的人
        'Public Tree_Skin As New Treap 'SkinTree

        'Public Tree_Msg As New Treap 'TreeMsg

        'Public Tree_FunnyIndex As New Treap
        'Public Tree_FunnyIndex_Extend As New Treap

        'Public Tree_Index1 As New Treap '所有 C_Tree_VID 存储在这
        'Public Tree_Index2 As New Treap '所有 C_Tree_VVID 树存储在这

        Public Tree_Group_Array As New Treap(Of ArrayList)
        Public Tree_RecordCount As New Treap(Of Int32)

        'Public Tree_URL_ID As New Treap(Of C_DB_Treap_URL) 'URL ID 对应集



        Public DefaultTime As Date

        Public bIIS As Boolean = False

        Public pVars As New C_Treap_Funny(Of String)   'bIIS=Fasle 的时候用

        'Private Tree_Tables As New Treap(Of C_Treap_Funny)


        'Public pTree_Session As New C_Treap_Funny
        Public pResponse As C_Response
        Public strPath_Root As String = "C:\Funny\FunnyServer"
        Public strPath_Common As String = "C:\Funny\FunnyServer\Common"
        Public strPath_FTP As String

        Public pRequest As C_Request
        Public LogPath As String = "C:\Funny\FunnyServer"

        Public pCache As Caching.Cache

        Dim httpRT As HttpRuntime

        Public Sub New()

            pRequest = New C_Request
            pResponse = New C_Response

            If Right(strPath_Root, 1) = "\" Then
                strPath_Root = Left(strPath_Root, Len(strPath_Root) - 1)
            End If

            strPath_Common = strPath_Root & "\Common"

            pVars.insert(New C_K_Str("SYS.APP.PATH"), strPath_Root)


            httpRT = New HttpRuntime()

            pCache = HttpRuntime.Cache
        End Sub


        Public Sub InitPath( _
            ByVal strPath As String, _
            ByVal PathCommon As String, _
            ByVal PathFtp As String)

            strPath_Root = strPath
            strPath_Common = PathCommon
            strPath_FTP = PathFtp
            Init(bIIS)
        End Sub


        Public Sub Init(ByVal mIIS As Boolean)
            bIIS = mIIS
            If bIIS Then
                HttpContext.Current.Application.Lock()
            End If

            If bIIS Then
                AppPath = HttpContext.Current.Server.MapPath("/")
            Else
                Dim pObj As Object
                pObj = pVars.find(New C_K_Str("SYS.APP.PATH"))
                If pObj Is Nothing Then
                    AppPath = "C:\Funny\"
                Else
                    AppPath = pObj
                End If
            End If

            Path_Home = Me.GetWebSetting("Path.Home")
            If Path_Home = "" Then Path_Home = "/"

            If Me.GetWebSetting("NoEmail") = "yes" Then
                bNoEmail = True
            Else
                bNoEmail = False
            End If


            If Me.GetWebSetting("SYS.DefaultTime") = "" Then
                Me.DefaultTime = Convert.ToDateTime("2006-1-1")
            Else
                Me.DefaultTime = Convert.ToDateTime(Me.GetWebSetting("SYS.DefaultTime"))
            End If

            Path_Data = Me.GetWebSetting("Path.Data")
            If Path_Data = "" Then Path_Data = Me.MapPath(Path_Home & "Data/")


            Path_Pic_Tmp = Me.GetWebSetting("Path.PicTmp")
            If Path_Pic_Tmp = "" Then Path_Pic_Tmp = Path_Home & "Tmp/"

            InitSystem = (GetWebSetting("Init.Allow") = "1")

            UploadPath = Me.GetWebSetting("Down.URL")

            Web_Name = Me.GetWebSetting("Web.Name")
            Web_URL = Me.GetWebSetting("Web.URL")
            Admin_Email = Me.GetWebSetting("Admin.Email")
            If Admin_Email = "" Then Admin_Email = "admin@funnyweber.com"

            lngOnlineTime = Val(Me.GetWebSetting("OnLine.Time"))
            If lngOnlineTime <= 0 Then lngOnlineTime = 1
            lngOnlineAdd = Val(Me.GetWebSetting("OnLine.Add"))


            strFileRefresh = Me.MapPath("/Data/Tmp/Refresh/Queue.txt")

            pQue_Refresh = New C_Queue_File_Msg(1024)
            If File.Exists(strFileRefresh) Then
                pQue_Refresh.Read(strFileRefresh)
            End If
            If bIIS Then
                HttpContext.Current.Application.UnLock()
            End If
        End Sub


        Public Function ReadSub(ByVal strFile As String) As Int32

            If pCache(strFile) Is Nothing Then
                Dim onRemove As Caching.CacheItemRemovedCallback _
                    = New Caching.CacheItemRemovedCallback(AddressOf onRemoveCallback)

                Dim FileDepend As Caching.CacheDependency = New Caching.CacheDependency(strFile)
                pCache.Add(strFile, strFile, FileDepend, DateTime.Now.AddHours(3), TimeSpan.Zero, Caching.CacheItemPriority.Default, onRemove)
            End If


            Dim RecordCount As Int32
            If File.Exists(strFile) = False Then
                RecordCount = 0
                GoTo MyExit
            End If

            Dim FS As FileStream, BR As BinaryReader
            Try
                FS = New FileStream(strFile, FileMode.Open)
                BR = New BinaryReader(FS)
                FS.Seek(0, SeekOrigin.Begin)
                RecordCount = BR.ReadInt32()
                BR.Close()
                FS.Close()
            Catch ex As Exception

            End Try
            BR = Nothing
            FS = Nothing

MyExit:
            Tree_RecordCount.insert(New C_K_Str(strFile), RecordCount)
            Return RecordCount
        End Function

        Public Sub onRemoveCallback(ByVal key As String, _
                ByVal value As Object, _
                ByVal reason As Caching.CacheItemRemovedReason)

            ReadSub(value)
        End Sub


        Public Function Get_Recycle_Que( _
            ByRef pServer As C_Server, _
            ByVal strT As String, _
            ByVal strTable As String) As C_Queue

            Dim Que_Recycle As C_Queue
            Que_Recycle = Tree_Que_Recycle.find(New C_K_Str(strT & strTable))
            If Que_Recycle Is Nothing Then
                Que_Recycle = New C_Queue
                'Que_Recycle = S_FunnyIndex.Read_Recycle_FromDisk(pServer, strT, strTable)
                Tree_Que_Recycle.insert(New C_K_Str(strT & strTable), Que_Recycle)
            End If

            Return Que_Recycle
        End Function

        'Public Function Get_Tree_URL_ID(ByVal strT As String) As C_DB_Treap_URL
        '    'strT = S_FunnyWeber.Get_T(strT)

        '    Dim pTreapURL As C_DB_Treap_URL _
        '        = Tree_URL_ID.find(New C_K_Str(strT))

        '    If pTreapURL Is Nothing Then
        '        pTreapURL = New C_DB_Treap_URL
        '        Tree_URL_ID.insert(New C_K_Str(strT), pTreapURL)
        '        'If Not Left(UCase(strT), 4) = "TMP_" Then
        '        '    'pTreapURL.pTreap_String.strFile = Me.MapPath(Me.Path_Home & "/Data/Key/" & strT & "URL.funny")
        '        '    'Call pTreapURL.read_URL_Tree(Me)
        '        'End If
        '    End If
        '    pTreapURL.strT = strT

        '    Return pTreapURL
        'End Function

        Public Function GetVar(ByVal strKey As String) As String
            Dim strReturn As String
            Dim pObj As Object = Me.pVars.find(New C_K_Str(strKey))

            If Not pObj Is Nothing Then
                strReturn = pObj
            Else
                strReturn = ""
            End If

            Return strReturn
        End Function


        Public Function GetModifyTime(ByVal strFile As String) As Date
            Dim pFileInfo As System.IO.FileInfo = New System.IO.FileInfo(strFile)

            Return pFileInfo.LastWriteTime
        End Function

        Public Function GetStr(ByVal Item As VariantType) As String
            If IsDBNull(Item) Then
                GetStr = ""
            Else
                GetStr = CStr(Item)
            End If
        End Function



        'Public Function Get_TableTree( _
        '    ByVal strT As String, _
        '    ByVal strTable As String) As C_Treap_Funny

        '    '得到FunnyNode所存储的Treap
        '    Dim pData_Tree As C_Treap_Funny
        '    pData_Tree = Tree_Tables.find(New C_K_Str(strT & "_" & strTable))

        '    If pData_Tree Is Nothing Then
        '        pData_Tree = New C_Treap_Funny
        '        Tree_Tables.insert(New C_K_Str(strT & "_" & strTable), pData_Tree)
        '    End If

        '    Return pData_Tree
        'End Function

        'Public Function Clear_TableTree( _
        '    ByVal strT As String, _
        '    ByVal strTable As String) As C_Treap_Funny

        '    '得到FunnyNode所存储的Treap
        '    Dim pData_Tree As C_Treap_Funny = New C_Treap_Funny
        '    Tree_Tables.insert(New C_K_Str(strT & "_" & strTable), pData_Tree)

        '    Return pData_Tree
        'End Function



        Public Function GetWebSetting(ByVal strkey As String) As String
            Dim pXML_read As C_XML_Read = New C_XML_Read
            Dim strReturn As String = _
                pXML_read.Read_From_File( _
                    Me.MapPath("/Web.config"), _
                    "configuration/appSettings/add[@key='" & strkey & "']", _
                    C_XML_Read.Read_Type.C_Attribute, "value", False)

            Return strReturn    'ConfigurationSettings.AppSettings(Strkey)
        End Function

        Public strFileRefresh As String
        Public lngOnlineTime, lngOnlineAdd As Int32



        Public Function GetCookie( _
            ByRef pRequest As C_Request, _
            ByVal strKey As String) As String

            Dim strReturn As String = ""
            Dim pCookie As Cookie

            If Not pRequest.context Is Nothing Then
                pCookie = pRequest.context.Request.Cookies(strKey)
                If Not pCookie Is Nothing Then
                    strReturn = pCookie.Value
                End If
                If Not pCookie Is Nothing Then
                    strReturn = pCookie.Value
                End If
            Else
                strReturn = pRequest.GetValue(strKey)
            End If

            Return strReturn

            'If HttpContext.Current.Request.Cookies(Key) Is Nothing Then Return ""
            ''pRequest.Cookies(Key).Domain = Me.Web_URL
            'HttpContext.Current.Request.Cookies(Key).Path = "/"
            'Return Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies(Key).Value)
        End Function



        Public Sub Save_MaxUserID(ByVal intID As Int32)
            If bIIS Then
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application("Max_UserID") = intID
                HttpContext.Current.Application.UnLock()
            Else
                pVars.insert(New C_K_Str("Max_UserID"), intID)
            End If

            Dim strPath As String = Me.MapPath(Me.Path_Home + "Data/Setting/Max/UserID.funny")
            'S_FunnyWeber.InitDir_Sub(strPath)


            Dim pXML_Save As C_XML_Save = New C_XML_Save
            pXML_Save.Save(strPath, "Basic", "ID", intID)
            pXML_Save = Nothing

        End Sub



        Public Sub Group_Array_Add( _
            ByVal UserID As Int32, ByRef pArray As ArrayList)
            Tree_Group_Array.insert(New C_K_ID(UserID), pArray)
        End Sub

        Public Function Group_Array_Get(ByVal UserID As Int32) As ArrayList
            Dim pArray As ArrayList
            pArray = Tree_Group_Array.find(New C_K_ID(UserID))
            Return pArray
        End Function

        Public Sub Group_Array_Remove(ByVal UserID As Int32)
            Tree_Group_Array.Remove(New C_K_ID(UserID))
        End Sub

        Public Sub Save_MaxFileID(ByVal strT As String, ByVal intID As Int32)
            If bIIS Then
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application("Max_FileID_" & strT) = intID
                HttpContext.Current.Application.UnLock()
            Else
                pVars.insert(New C_K_Str("Max_FileID_" & strT), intID)
            End If

            If Left(LCase(strT), 4) = "tmp_" Then
                Exit Sub
            End If

            Dim strPath As String _
                = Me.MapPath(Me.Path_Home & "Data/Setting/Max/" & strT & "FileID.funny")

            'S_FunnyWeber.InitDir_Sub(strPath)

            Dim pXML_Save As C_XML_Save = New C_XML_Save
            pXML_Save.Save(strPath, "Basic", "ID", intID)
            pXML_Save = Nothing
        End Sub


        Public Function NewUserID() As Int32
            '得到下一个用户ID
            '回收的 暂时不在这处理
            Dim lngID As Int32

            If CInt(HttpContext.Current.Application("Max_UserID")) > 10000 Then
                lngID = Str(CInt(HttpContext.Current.Application("Max_UserID")))
            Else
                lngID = MaxUserID()
            End If

            lngID = lngID + 1
            Save_MaxUserID(lngID)

            Return lngID
        End Function


        Public Function MaxFileID( _
            ByVal strT As String) As Int32

            '得到最大的文件ID
            Dim lngID, lngID2 As Int32

            If bIIS Then
                If CInt(HttpContext.Current.Application("Max_FileID_" & strT)) > 0 Then
                    lngID = CInt(HttpContext.Current.Application("Max_FileID_" & strT))
                    Return lngID
                End If
            Else
                Dim pObj As Object = pVars.find(New C_K_Str("Max_FileID_" & strT))
                If Not pObj Is Nothing Then
                    lngID = pObj
                End If
            End If

            If Left(LCase(strT), 4) = "tmp_" Then
                Return 0
            End If

            Dim strPath As String = Me.MapPath(Me.Path_Home + "Data/Setting/Max/" & strT & "FileID.funny")
            'S_FunnyWeber.InitDir_Sub(strPath)

            Dim pXML_Read As C_XML_Read = New C_XML_Read
            lngID2 = Val(pXML_Read.Read_From_File(strPath, _
             "/Data/Basic/ID", C_XML_Read.Read_Type.C_InnerText, "", True))
            pXML_Read = Nothing

            If lngID2 > lngID Then
                lngID = lngID2
            End If

            Return lngID
        End Function


        Public Function MaxUserID() As Int32
            '得到最大的用户ID
            Dim lngID As Int32

            If bIIS Then
                If CInt(HttpContext.Current.Application("Max_UserID")) > 10000 Then
                    lngID = Str(CInt(HttpContext.Current.Application("Max_UserID")))
                    Return lngID
                End If
            Else
                Dim pObj As Object = pVars.find(New C_K_Str("Max_UserID"))
                If Not pObj Is Nothing Then
                    lngID = pObj
                End If
            End If

            Dim strPath As String = Me.MapPath(Me.Path_Home + "Data/Setting/Max/UserID.funny")
            'S_FunnyWeber.InitDir_Sub(strPath)

            Dim lngID2 As Int32
            Dim pXML_Read As C_XML_Read = New C_XML_Read
            lngID2 = Val(pXML_Read.Read_From_File(strPath, _
             "/Data/Basic/ID", C_XML_Read.Read_Type.C_InnerText, "", True))
            pXML_Read = Nothing

            If lngID2 > lngID Then
                lngID = lngID2
            End If

            Return lngID
        End Function

        'Public Sub saveKeyTree( _
        '    ByVal strFile As String, _
        '    ByRef pTreap As C_Treap_Funny) ' As Treap

        '    Dim pWriter As StreamWriter
        '    Dim fs As FileStream, strReturn As String = ""

        '    Try
        '        If strFile = "" Then Exit Sub
        '        '不要File.Delete,直接覆盖即可,
        '        '否则可能保存的时候失败会没有静态文件!()

        '        S_SYS.InitDir(strFile)    '创建目录等

        '        fs = New FileStream(strFile, FileMode.Create, FileAccess.Write)

        '        pWriter = New StreamWriter(fs, System.Text.Encoding.UTF8)

        '        Dim strKey As String
        '        Dim p As TreapEnumerator = pTreap.Elements(True)
        '        While (p.HasMoreElements())
        '            strKey = (CType(p.NextElement(), String))
        '            pWriter.WriteLine(strKey)
        '        End While

        '        pWriter.Close()
        '        fs.Close()
        '    Catch ex As Exception
        '        strReturn = ex.Message
        '    End Try
        '    pWriter = Nothing
        '    fs = Nothing
        'End Sub

        Public Function MapPath( _
                ByVal strPath As String, _
                Optional ByVal bVirtual As Boolean = False) As String

            If bVirtual Then
                strPath = HttpContext.Current.Server.MapPath(strPath)
            Else
                If Left(LCase(strPath), 13) = "/funnyserver/" Then
                    strPath = strPath_FTP + "\" + strPath.Substring(13).Replace("/", "\")
                ElseIf Left(LCase(strPath), 6) = "common" Or Left(LCase(strPath), 7) = "/common" Then
                    strPath = strPath_Common + "\" + strPath.Replace("/", "\")
                Else
                    strPath = strPath_Root + "\" + strPath.Replace("/", "\")
                End If
            End If
            strPath = Replace(strPath, "\\\", "\")
            strPath = Replace(strPath, "\\", "\")

            Return strPath
        End Function


        Public Sub MoveFile(ByVal strFile As String, ByVal strTag As String)
            Dim strFile2 As String

            Do
                strFile2 = strFile & strTag & Format(Now, "yyyy-MM-dd_(hh_mm_ss)") & CInt(Rnd() * 1000) & ".html"
            Loop While File.Exists(strFile2)

            File.Move(strFile, strFile2)
        End Sub
    End Class
End Namespace
