


Imports System.Web.Security



Namespace FunnyWeber
	Public Class C_User

        Public Name As String
        ''' <summary>
        ''' �����MD5
        ''' </summary>
        ''' <remarks></remarks>
        Public MD5 As String = ""

        'Public roomID As String '����ؼ���
        Public lastTime As DateTime

        Dim valueID As Int32
        Public pServer As C_Server

        '������������ύģʽ��ͬ,Session�����ж��,��ÿ���˵�User��һ��
        '����ͬһ���û��ڶ������ʹ�� UserҲֻ��һ��

        Public MsgQue As C_Queue_File_Msg = New C_Queue_File_Msg(1000)

        Public Email As String = "" '�����gtalk

        Public ReadOnly Property ID() As Int32
            Get
                ID = valueID
            End Get
            '  Set(ByVal Value As Int32)
            '     valueID = Value
            'End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New( _
            ByRef TServer As C_Server, _
            ByVal intID As Int32)
            pServer = TServer
            valueID = intID

            Dim strFile As String
            strFile = pServer.MapPath( _
                 "/Data/MsgList/Talk/" & valueID & ".talk")
            MsgQue.Read(strFile)
        End Sub

        Public Sub SaveTalk(ByRef pServer As C_Server, _
            Optional ByVal bIIS As Boolean = True)
            Dim strFile As String
            strFile = pServer.MapPath( _
                 "/Data/MsgList/Talk/" & valueID & ".talk")
            MsgQue.Save(strFile, bIIS)
        End Sub

    End Class
End Namespace

