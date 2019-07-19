

Imports System.IO
Imports B_String.Funny


Namespace Funny
    Public Class C_FunnyNode
        Public Class C_Extend
            Public Key As String
            'Public HTML As String
            Public Text As String  ' html2Text(HTML)
        End Class

        Public Class C_Map
            'NLM_ 映射存储需要用到的扩展
            Public strFrom, strMap As String ' , strTO=Basic/T
        End Class

        Public Class C_Static
            Public Static_VC, Static_CC, Static_RC As Integer
        End Class

        Const ReserveSpace As Integer = 1024 * 2

        '================String
        Public strT As String
        Public Name As String
        Public URL As String
        Public Type As String 'SG 用
        '================/String

        '================Int
        Public ID, RightBase, Creater, Updater As Integer
        Public intClass As Integer '当前用户属于哪个组 比如=1 超级管理员...
        '================/Int

        '================Long
        Public TimeCreate, TimeUpdate As Long
        '================/Long

        '================OBJ
        '#If CONFIG= then
        '#End If

        Public pExtend As C_Extend
        Public pExtend2 As Object

        'Public pMap As C_Map  'NLM_ 里需要用到
        Public pStatic As New C_Static
        '================/OBJ

        '=============Boolean
        Public bKill As Boolean
        Public bRefresh As Boolean    '是否需要刷新到XML文件里
        Public bReadData As Boolean '是否读取过硬盘数据
        '=============/Boolean

        Public Sub New( _
            ByVal T As String, _
            ByVal intID As Integer)
            strT = T
            ID = intID
        End Sub


        Public Sub New( _
            ByVal T As String, _
            ByVal intID As Integer, ByVal bExtend As Boolean)
            strT = T
            ID = intID

            Me.pExtend = New C_FunnyNode.C_Extend
        End Sub

        Public Function GetPathID(ByVal ID As Long) As String
            Dim strID As String = CStr(ID)
            Dim i As Integer
            Dim strReturn As String = ""

            For i = 1 To Len(strID)
                strReturn += Mid(strID, i, 1) & "\"
            Next

            Return strReturn
        End Function

        Public Sub setFunnyNode( _
            ByVal strXPath As String, _
            ByVal strValue As String)

            Select Case strXPath
                Case "Basic/N"
                    Me.Name = strValue
                Case "Basic/URL"
                    Me.URL = strValue
                Case "Basic/R"
                    Me.RightBase = Val(strValue)
                Case "Basic/K"
                    pExtend.Key = strValue
                Case "Basic/T"
                    'pExtend.HTML = strValue
                    pExtend.Text = S_Strings.HTML2Text(strValue)
                Case "Basic/C"
                    Me.Creater = Val(strValue)
                Case "Basic/U"
                    Me.Updater = Val(strValue)
                Case "Time/C"
                    Me.TimeCreate = Val(strValue)
                Case "Time/U"
                    Me.TimeUpdate = Val(strValue)
                Case "Static/VC"
                    If Me.pStatic Is Nothing Then
                        pStatic = New C_Static
                    End If
                    Me.pStatic.Static_VC = Val(strValue)
                Case "Static/CC"
                    If Me.pStatic Is Nothing Then
                        pStatic = New C_Static
                    End If
                    Me.pStatic.Static_CC = Val(strValue)
                Case "Static/RC"
                    If Me.pStatic Is Nothing Then
                        pStatic = New C_Static
                    End If
                    Me.pStatic.Static_RC = Val(strValue)
                    'Case "Map/From"
                    '    If Me.pMap Is Nothing Then
                    '        Me.pMap = New C_Map
                    '    End If
                    '    pMap.strFrom = strValue
                    'Case "Map/Name"
                    '    If Me.pMap Is Nothing Then
                    '        Me.pMap = New C_Map
                    '    End If
                    '    pMap.strMap = strValue
                Case Else
            End Select
        End Sub

    End Class



End Namespace
