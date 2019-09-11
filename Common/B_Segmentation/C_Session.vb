
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
        '����ౣ����Session ��Ϊ������ٶ�

        Public Const C_Normal As Int32 = 5

        Public User_ID As Int32     '��Ա.ID
        Public User_Name As String    '��Ա.����
        Public User_Email As String    '��Ա.Email
        Public User_Passsword As String    '��Ա.����
        Public User_Cookie As String    '��Ա��Cookie

        Public bChecked As Boolean    '�Ƿ�ͨ��������֤
        Public bSearch As Boolean   '�Ƿ�����������
        Public bUseCookie As Boolean = False '�Ƿ��Ѿ�����Cookie
        Public bClear As Boolean = False '�Ƿ���������Session
        Public intClass As Int32 = 8 '���ĸ���ĳ�Ա
        Public lngWeight As Int32 = 1 'ͶƱ��Ȩ�� �Ժ����ԱΪ50 վ��Ϊ10

        Public Ftp_Root As String ' FTP��Ŀ¼
        Public FTP_Seek As Long 'FTP�ļ���ȡ��д��λ�ö�λ


        Public pServer As C_Server
        Public pRequest As C_Request
        Public pResponse As C_Response

        Public LastContent As String '���һ��Post����������
        Public LastMsg As String '���һ��Post��Msg����


        Public pUser As C_User

        'Public pTree_Session As New Treap '���ﱣ��һЩ�Զ���� Session��Ϣ
        Public pTree_Que As New Treap(Of C_Queue) '���ﱣ��һЩ�Զ���� Session��Ϣ

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
                '����ǳ�ʼ��,�򲻼��Ȩ��
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
                        '    pServer, "", "��Ա", User_ID, "User/Class")) <> Me.intClass Then
                        '    S_FunnyWeber.NewData_Save(pServer, "", "��Ա", _
                        '        "User/Class", User_ID, CStr(Me.intClass), "", 0)
                        '    Dim pCookie As C_Cookie = New C_Cookie
                        '    pCookie.SaveCookie_Sub(pResponse, "CookieGroup", Me.intClass, Now.AddYears(3))
                        'End If
                        'If pServer.GetCookie(pRequest, "CookieGroup") = "" Then
                        '    Dim pCookie As C_Cookie = New C_Cookie
                        '    pCookie.SaveCookie_Sub(pResponse, "CookieGroup", Me.intClass, Now.AddYears(3))
                        'End If

                        'Ftp_Root = S_FunnyWeber.FunnyNode_Read(pServer, "", "��Ա", User_ID, "Ftp/Path")

                        'S_Debug.LogError(pServer.LogPath, "User_ID:" & User_ID)
                        'S_Debug.LogError(pServer.LogPath, "Ftp_Root:" & Ftp_Root)
                    End If
                Else
                    User_ID = 8
                    User_Name = "Guest"
                End If
            End If

            'һ������������ж��Session �����ύ����Flash�����(�е����)
            '�������е���ͬID��Userֻ��һ��,��ʹ�ڶ������������
            pUser = pServer.Tree_Room.find(New C_K_ID(User_ID))

            If pUser Is Nothing Then
                pUser = New C_User(pServer, User_ID)
                pServer.Tree_Room.insert(New C_K_ID(User_ID), pUser)
            End If

            pUser.Name = Me.User_Name
        End Sub


        Private Sub Check_Update_H6(ByRef pRequest As C_Request)
            '��ǰ�������H6
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

