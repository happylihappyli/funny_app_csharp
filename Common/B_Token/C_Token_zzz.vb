Imports CommonTreapVB.TreapVB

Namespace Funny
    Public Class C_Token
        Public Shared MaxID As Int32 = 1

        Public ID As Int32

        Public iStart As Int32      '= 单词的开始位置
        Public iEnd As Int32        '= 单词的结束位置
        Public Child_Size As Int32 = 1

        '一个结构或分词的一个单位

        Public Tag As String '一些附属信息
        Public pTreap_Tag As Treap
        Public Name As String '最新的一个词汇

        Dim pList As New ArrayList  '存储多个类似并列结构

        Public pNextToken3 As C_Token = Nothing '检查的标记3，如果用过标记为右边相邻的Token
        Public pNextToken2 As C_Token = Nothing '检查的标记2，如果用过标记为右边相邻的Token

        Public pChild As New ArrayList
        'A_B_C 的pChild是指向A，B，C的指针
        'A B C

        ''' <summary>
        ''' 是否已经转化过
        ''' </summary>
        ''' <remarks></remarks>
        Public bCheckConvert As Boolean = False

        Public ReadOnly Property Item(ByVal index As Int32) As String
            Get
                Return pList.Item(index)
            End Get
        End Property

        Public Sub RemoveAt(ByVal index As Int32)
            pList.RemoveAt(index)
        End Sub

        Public Sub Add(ByVal strItem As String)
            If strItem = "" Then
                Return
            End If
            If Name = "" OrElse Name = "{any}" Then
                Name = strItem
            End If

            For i As Int32 = 0 To pList.Count - 1
                If strItem = pList.Item(i) Then
                    Return
                End If
            Next
            pList.Add(strItem)
        End Sub

        Public Function Count() As Int32
            Return pList.Count
        End Function

        Public Overrides Function ToString() As String
            Dim strReturn As String = ""
            For i As Int32 = 0 To pList.Count - 1
                strReturn += pList.Item(i) + "|"
            Next
            If Right(strReturn, 1) = "|" Then
                strReturn = strReturn.Substring(0, Len(strReturn) - 1)
            End If
            Return strReturn
        End Function

        Public Overloads Function ToString(ByVal showID As Boolean) As String
            Dim strReturn As String = ""
            For i As Int32 = 0 To pList.Count - 1
                strReturn += pList.Item(i) + "|"
            Next
            If Right(strReturn, 1) = "|" Then
                strReturn = strReturn.Substring(0, Len(strReturn) - 1)
            End If
            Return ID & "=" & strReturn
        End Function

        Public Sub New()
            ID = MaxID
            MaxID += 1
        End Sub

        Public Sub New(ByVal strInput As String, _
                       ByVal iStart As Int32, _
                       ByVal iEnd As Int32, ByVal iChild_Size As Int32)
            Me.New()

            Me.iStart = iStart
            Me.iEnd = iEnd
            Me.Child_Size = iChild_Size

            Dim strSplit() As String = Split(strInput, "|")
            Dim strTmp As String

            For i As Int32 = 0 To UBound(strSplit)
                strTmp = Trim(strSplit(i))
                If strTmp <> "" Then
                    Add(strTmp)
                End If
            Next
        End Sub

        Public Sub Copy(ByRef pTokenIn As C_Token, _
                            Optional ByVal iStart As Int32 = 0)
            '复制到后面
            Dim strTmp As String
            For i As Int32 = iStart To pTokenIn.pList.Count - 1
                strTmp = pTokenIn.pList.Item(i)
                If strTmp <> "" Then
                    Add(strTmp)
                End If
            Next
        End Sub

        Public Function Equal(ByRef pTokenIn As C_Token) As Boolean
            '比较是否相同
            If pTokenIn IsNot Nothing Then
                If pTokenIn.pList.Count <> pList.Count Then
                    Return False
                Else
                    For i As Int32 = 0 To pList.Count - 1
                        If pTokenIn.pList.Item(i) <> pList.Item(i) Then
                            Return False
                        End If
                    Next
                End If
            Else
                Return False
            End If
            Return True
        End Function

        Public Function checkExits(ByVal strWord As String) As Boolean
            For i As Int32 = 0 To pList.Count - 1
                '检查
                If pList(i).Equals(strWord) Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class

End Namespace
