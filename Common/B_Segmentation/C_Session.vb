
Imports CommonTreapVB.TreapVB

Imports B_File.Funny
Imports B_Data.Funny


Imports System
Imports System.IO
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Net
Imports System.Text
Namespace FunnyWeber
    Public Class C_Session
        '这个类保存在Session 里为了提高速度

        Public Const C_Normal As Int32 = 5

        Public User_ID As Int32     '成员.ID
        Public User_Name As String    '成员.名称
        Public User_Email As String    '成员.Email
        Public User_Passsword As String    '成员.密码
        Public User_Cookie As String    '成员的Cookie

        Public bChecked As Boolean    '是否通过密码验证
        Public bSearch As Boolean   '是否是搜索引擎
        Public bUseCookie As Boolean = False '是否已经用了Cookie
        Public bClear As Boolean = False '是否重新生成Session
        Public intClass As Int32 = 8 '是哪个组的成员
        Public lngWeight As Int32 = 1 '投票的权重 以后管理员为50 站长为10

        Public Ftp_Root As String ' FTP根目录
        Public FTP_Seek As Long 'FTP文件读取，写，位置定位


        Public pServer As C_Server
        Public pRequest As C_Request
        Public pResponse As C_Response

        Public LastContent As String '最后一次Post的帖子内容
        Public LastMsg As String '最后一次Post的Msg内容


        Public pUser As C_User

        'Public pTree_Session As New Treap '这里保存一些自定义的 Session信息
        Public pTree_Que As New Treap(Of C_Queue) '这里保存一些自定义的 Session信息

        Public Function GetTree_Que( _
            ByVal strKey As String) As C_Queue

            Dim pQue As C_Queue = _
                pTree_Que.find(New C_K_Str(strKey))

            If pQue Is Nothing Then
                pQue = New C_Queue
                pTree_Que.insert(New C_K_Str(strKey), pQue)
            End If

            Return pQue
        End Function


        Public Sub New( _
            ByRef TRequest As C_Request, _
            ByRef TResponse As C_Response, _
            ByRef TServer As C_Server)

            pRequest = TRequest
            pResponse = TResponse
            pServer = TServer

            User_Cookie = pServer.GetCookie(pRequest, "CookieUserCookies")
            User_Passsword = pServer.GetCookie(pRequest, "CookieUserPassWord")
            User_Email = pServer.GetCookie(pRequest, "CookieUserEmail")

            Init(User_Email, User_Passsword, User_Cookie)
        End Sub

        Public Sub Init( _
            ByVal User_Email As String, _
            ByVal User_Passsword As String, _
            ByVal User_Cookie As String)

            If pServer.InitSystem Then
                '如果是初始化,则不检查权限
                Me.bChecked = True
                User_ID = 10001
                User_Name = "Admin"
            Else
                'strBody = pServer.Check_ByEmail(Me, User_Passsword, User_Cookie, False, False)

                If User_ID <= 10000 Then User_ID = 8
                If Me.bChecked Then
                    If User_ID > 10000 Then
                        'If S_Right.IfInThisGroup(pServer, User_ID, S_Right.RightType.C_Admin_Super) Then
                        '    Me.intClass = 1
                        'ElseIf S_Right.IfInThisGroup(pServer, User_ID, S_Right.RightType.C_Admin) Then
                        '    Me.intClass = 2
                        'ElseIf S_Right.IfInThisGroup(pServer, User_ID, S_Right.RightType.C_Manager) Then
                        '    Me.intClass = 3
                        'ElseIf S_Right.IfInThisGroup(pServer, User_ID, S_Right.RightType.C_VIP) Then
                        '    Me.intClass = 4
                        'ElseIf S_Right.IfInThisGroup(pServer, User_ID, S_Right.RightType.C_Normal) Then
                        '    Me.intClass = 5
                        'End If
                        'If Val(S_FunnyWeber.FunnyNode_Read( _
                        '    pServer, "", "成员", User_ID, "User/Class")) <> Me.intClass Then
                        '    S_FunnyWeber.NewData_Save(pServer, "", "成员", _
                        '        "User/Class", User_ID, CStr(Me.intClass), "", 0)
                        '    Dim pCookie As C_Cookie = New C_Cookie
                        '    pCookie.SaveCookie_Sub(pResponse, "CookieGroup", Me.intClass, Now.AddYears(3))
                        'End If
                        'If pServer.GetCookie(pRequest, "CookieGroup") = "" Then
                        '    Dim pCookie As C_Cookie = New C_Cookie
                        '    pCookie.SaveCookie_Sub(pResponse, "CookieGroup", Me.intClass, Now.AddYears(3))
                        'End If

                        'Ftp_Root = S_FunnyWeber.FunnyNode_Read(pServer, "", "成员", User_ID, "Ftp/Path")

                        'S_Debug.LogError(pServer.LogPath, "User_ID:" & User_ID)
                        'S_Debug.LogError(pServer.LogPath, "Ftp_Root:" & Ftp_Root)
                    End If
                Else
                    User_ID = 8
                    User_Name = "Guest"
                End If
            End If

            '一个浏览器可能有多个Session 比如提交的是Flash程序等(有点奇怪)
            '不过所有的相同ID的User只有一个,即使在多个服务器操作
            pUser = pServer.Tree_Room.find(New C_K_ID(User_ID))

            If pUser Is Nothing Then
                pUser = New C_User(pServer, User_ID)
                pServer.Tree_Room.insert(New C_K_ID(User_ID), pUser)
            End If

            pUser.Name = Me.User_Name
        End Sub


        Private Sub Check_Update_H6(ByRef pRequest As C_Request)
            '当前这个用里H6
            Dim pData6 As C_Data = New C_Data(Me, pRequest, False)
            pData6.strF = pRequest.GetValue("F")
            pData6.Init(pServer, pRequest.GetValue("T"), pRequest.RawUrl)
            If Me.User_ID = 8 Then
                'S_FunnyWeber.Create_User(pServer, Me.User_ID, _
                '    Me.User_Name, "", Me.User_Email, _
                '    Me.User_Passsword, 5, False, False)
                pUser = Nothing
            End If
            pData6 = Nothing
        End Sub



    End Class
End Namespace

