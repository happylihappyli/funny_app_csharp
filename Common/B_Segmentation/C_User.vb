


Imports System.Web.Security



Namespace FunnyWeber
	Public Class C_User

        Public Name As String
        ''' <summary>
        ''' 密码的MD5
        ''' </summary>
        ''' <remarks></remarks>
        Public MD5 As String = ""

        'Public roomID As String '房间关键字
        Public lastTime As DateTime

        Dim valueID As Int32
        Public pServer As C_Server

        '由于浏览器的提交模式不同,Session可能有多个,但每个人的User是一个
        '比如同一个用户在多个机器使用 User也只有一个

        Public MsgQue As C_Queue_File_Msg = New C_Queue_File_Msg(1000)

        Public Email As String = "" '最好是gtalk

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

