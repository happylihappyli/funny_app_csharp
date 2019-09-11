
Imports System.Xml
Imports System.Text

Imports B_Data.Funny
Imports CommonTreapVB.TreapVB
Imports B_Segmentation.FunnyWeber


Namespace FunnyAI
    Public Class C_Session_AI

        Public pTreapTip As New Treap(Of C_Tip)       '�ղŶԻ�����ʾ�ʻ�

        Public Shared UserSession As New Treap(Of Treap(Of String)) '���е�Session���洢�������

        '/////////////////////////////////////////
        Public pDebug As StringBuilder

        Public bShowTopic As Boolean = True '�Ƿ���ʾ��ͼ

        Public pKeyScale As New Treap(Of C_Queue_Key)   '˥�� �ؼ�����
        Public pNextKey As New Treap(Of C_Queue_Key) '����ؼ���

        Public Key As Long 'Key �������ֲ�ͬ��Socket���� 

        Public strReceive As String
        Public strHead As String    'ͷ�ļ����������ⶼ�ӵġ�

        Public strFrom_New As String    '���������� ʵ��Email

        Public strAppend As StringBuilder   '������Ϣ
        Public UserIP As String             'IP��ַ
        Public strProxy As String           '��ȡProxy=xxx

        Public pTreapXML As New Treap(Of XmlDocument)

        Public strTopicRoot As String = "" '����ͼ

        Public CurrentSentence As String = "" '��ǰ����

        Public Last_Sentence As String

        Public pNextSentence As C_Queue = New C_Queue
        Public Que_Msg As New ArrayList

        Public bRunAsAdmin As Boolean = False
        Public bNoResrict As Boolean = False
        Public pQueBat As C_Queue = New C_Queue         '������
        Public pQueAsk As C_Queue = New C_Queue         '����⣬��Ե�ǰ��

        Public pAppendAnswer As New ArrayList '����ش�  '��ѡ�ش����ǰ��Ļش�Ϊ�գ����ú�ѡ�Ļش�
        Public strMap As String

        Public bEnd As Boolean '�Ƿ��ж�
        Public bDebug As Boolean
        Public bRun As Boolean '�Ƿ���������
        Public bDebug_Learn As Boolean '�Ƿ���ʾAI.Learn��Ϣ

        Public pQueNextSentence As C_Queue '������Ҫ����ľ���

        Public pDebugLearn As StringBuilder

        Public MayRobot As Int32 = 0 '�ж��Ƿ��ǻ�����

        Public TimeStart As Date
        Public bLog As Boolean = True
        Public TreeConvertCallCount As Int32 = 0

        Public pQueTip As C_Queue = New C_Queue
        Public pTreapUserID As New Treap(Of String)

        ''' <summary>
        ''' ����� C_Convert_Word
        ''' </summary>
        ''' <remarks></remarks>
        Public pTreapConvert As New Treap(Of String)

        Public Tree_Vector As New Treap(Of ArrayList) '�����洢�ĵط�

        Public PowerTopic As Double

        Public pTreapQue As New Treap(Of C_Queue)       '���д洢�ĵط�

        Public AI_ITS As C_Intention = New C_Intention

        Public MapNext As String '��һ���������������

        Public strReturn_Just As String '�ղŻظ��ľ���

        Public bTest As Boolean = False '�Ƿ��ǲ���ģʽ

        'Public pTreap_Var As New Treap(Of C_Var) '����

        Public bScriptBotAdmin As Boolean = False '�������ػ����˽ű���������ʱ���Ƿ��ǹ���Ա��

        Public pScript As Object ' C_Script_Runner = Nothing
        Public BreakMsg As String = "" '��ͣ�ű���ʱ�����ʾ��Ϣ

        Public pOutPut As Object 'C_Return_CodeBlock ' String = "" 'sys.out������ʱ�洢���⣬����Input.Read��ʱ��һ�������


        Public intUserID As Int32 = -1


        Private pSession As C_Session
        Private strTalkEmail As String

        Private pQueMap As C_Queue = New C_Queue         'ӳ����С�


        '////////////////////////////////////////
        Public Shared pTreapSYS_Var As New Treap(Of String)

        Public bShowTip As Boolean = False
        Public Debug_Head As String = "==L:"


        Public Sub Session_ClearMe()
            '���Session 
            UserSession.insert(New C_K_Str(Me.Talk_Email), New Treap(Of String))
        End Sub


        Public Function Session_Load( _
                ByVal strKey As String) As String

            If strKey.ToUpper.StartsWith("SYS/") Then
                Dim strTmp As Object = pTreapSYS_Var.find(New C_K_Str(strTalkEmail & "/" & strKey))
                If strTmp IsNot Nothing Then
                    Return strTmp
                Else
                    Return ""
                End If
            Else
                If strKey.ToUpper.StartsWith("SYS.QUE/") Then
                    Dim pQue As C_Queue
                    pQue = pTreapQue.find(New C_K_Str(strKey))
                    If pQue IsNot Nothing AndAlso pQue.Count > 0 Then
                        Return pQue.DeQue()
                    Else
                        Return ""
                    End If
                Else
                    Return Session_Load_Sub(strKey)
                End If
            End If
        End Function


        Public Function Session_Load_Sub( _
                    ByVal strKey As String) As String

            Dim pTree_Session As Treap(Of String) = UserSession.find(New C_K_Str(Me.Talk_Email))
            If pTree_Session IsNot Nothing Then
                Dim pObj As Object = pTree_Session.find(New C_K_Str(strKey))
                If pObj Is Nothing Then
                    Return ""
                Else
                    Return CType(pObj, String)
                End If
            Else
                Return ""
            End If
        End Function

        Public Function Session_Save( _
                ByVal strKey As String, _
                ByVal strValue As String) As String

            If strKey.ToUpper.StartsWith("SYS/") Then
                pTreapSYS_Var.insert(New C_K_Str(strTalkEmail & "/" & strKey), strValue)
            Else
                If strKey.ToUpper = "SYS/MAP/OLD" Then
                    Me.Map_Add(New C_Map(strValue, False))
                Else
                    If strValue <> "" Then
                        If strValue = "{none}" Then
                            Session_Save_Sub(strKey, "")
                        Else
                            Session_Save_Sub(strKey, strValue)
                        End If
                    End If
                End If
            End If

            Return ""
        End Function

        Public Sub Session_Save_Sub( _
            ByVal strKey As String, _
            ByVal strValue As String)
            Dim pTree_Session As Treap(Of String) = UserSession.find(New C_K_Str(Me.Talk_Email))
            If pTree_Session Is Nothing Then
                pTree_Session = New Treap(Of String)
                UserSession.insert(New C_K_Str(Me.Talk_Email), pTree_Session)
            End If

            pTree_Session.insert(New C_K_Str(strKey), strValue)
            Debug.Print(strKey & "=" & strValue)
        End Sub


        Public ReadOnly Property iUserID() As Int32
            Get
                'If intUserID = -1 Then
                '    intUserID = AI_SQL.SQL_UserID(Me) ' AI_FW.GetUserID_FromEmail(Me)
                'End If
                Return intUserID
            End Get
        End Property



        Public Property Talk_Email() As String 'Gtalk ��email
            Get
                Dim strReturn As String = strTalkEmail
                If strReturn = "" Then strReturn = pSession.User_Email
                Return strReturn
            End Get
            Set(ByVal strEmail As String)
                strTalkEmail = strEmail
            End Set
        End Property

        Public Property User_Email() As String
            Get
                Return pSession.User_Email
            End Get
            Set(ByVal value As String)
                pSession.User_Email = value
            End Set
        End Property

        Public Property pRequest() As C_Request
            Get
                Return pSession.pRequest
            End Get
            Set(ByVal value As C_Request)
                pSession.pRequest = value
            End Set
        End Property

        Public Function User_ID() As Int32
            Return pSession.User_ID
        End Function

        Public Function User_Name() As String
            Return pSession.User_Name
        End Function


        '////////////////////////////////////////
        Public Sub Map_Add(ByRef pMap As C_Map)
            pQueMap.EnQue(pMap)
        End Sub

        Public Sub Map_Clear()
            pQueMap = New C_Queue
        End Sub

        Public Function Map_Get() As C_Map

            Dim strReturn As String

            If Me.MapNext <> "" Then
                strReturn = Me.MapNext
                Me.MapNext = ""
                Return New C_Map(strReturn, False)
            End If

            Dim pMap As C_Map
            Try
                pMap = pQueMap.DeQue

                If pMap Is Nothing Then
                    Return Nothing
                Else
                    Return pMap
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

            Return Nothing
        End Function


        Public Sub New( _
            ByRef TSession As C_Session, _
            ByVal UserEmail As String)

            'strUserEmail = UserEmail
            pSession = TSession
            pSession.User_Email = User_Email

            Me.Clear()
        End Sub



        Public Function AddMsg(ByVal strSentence As String) As Int32
            Dim Count As Int32
            For i As Int32 = Que_Msg.Count - 1 To 0 Step -1
                If strSentence = Que_Msg.Item(i) Then
                    Count += 1
                End If
            Next

            Que_Msg.Add(strSentence)
            If Que_Msg.Count > 10 Then Que_Msg.RemoveAt(0)

            Return Count
        End Function

        Public Sub ClearArray()
            Me.Que_Msg = New ArrayList
        End Sub

        Public Sub Clear()
            Me.strMap = ""
            Me.pAppendAnswer = New ArrayList()
            Me.pDebug = New StringBuilder
            Me.pDebugLearn = New StringBuilder

            Me.strAppend = New StringBuilder

            Me.bEnd = False
        End Sub




    End Class
End Namespace