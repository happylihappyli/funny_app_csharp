
Imports B_TreapVB.TreapVB
Imports B_Data.Funny
Imports B_Token.Funny


Namespace Funny
    Public Class C_Token_Link
        Public Shared MaxID As Int32 = 1
        Public iStart As Int32 = 0 '//结构涵盖的开始和结尾。
        Public iEnd As Int32 = 0 '

        Public ID As Int32
        Public Tag As String = ""
        Public strFrom As String = "" '最后一步如何操作。 
        Public strSentence As String = "" '要分析的句子

        Dim pList As ArrayList = New ArrayList


        ''' <summary>
        ''' 清空pNextToken，比如这个链增加了东西
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub clearNextToken()
            Dim pToken As C_Token
            For i As Int32 = 0 To pList.Count - 1
                pToken = pList.Item(i)
                pToken.pNextToken2 = Nothing
                pToken.pNextToken3 = Nothing
            Next
        End Sub


        ''' <summary>
        ''' 激活的Token加入到Treap中
        ''' </summary>
        ''' <param name="pLink"></param>
        ''' <param name="pTreap"></param>
        ''' <remarks></remarks>
        Public Shared Sub AddKeyToTreap(
                ByRef pRobot As I_Robot,
                ByRef pLink As C_Token_Link,
                ByRef pTreap As Treap(Of C_Token_Key))

            Dim pToken As C_Token
            Dim strLine As String

            For i As Integer = 0 To pLink.Count() - 1
                ' Topic 激活 (单词激活) 
                ' 有时候整个句子识别了也要激活一下句子中的词汇
                ' 以方便后面的调用。
                pToken = pLink.Item(i)
                For j As Integer = 0 To pToken.Count() - 1
                    strLine = pToken.Item(j)
                    If strLine.Length > pRobot.Word_Min_Length Then
                        pTreap.insert(New C_K_Str(strLine), New C_Token_Key(strLine, pToken))

                        If pToken.Name.StartsWith("Word.L.") Then
                        ElseIf strLine = "{any}" Then
                        Else
                            If pToken.Name <> strLine Then
                                strLine = pToken.Name & "@" & strLine
                                pTreap.insert(New C_K_Str(strLine), New C_Token_Key(strLine, pToken))
                            End If
                        End If
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' pLink 添加到 pArray中 
        ''' </summary>
        ''' <param name="pRobot"></param>
        ''' <param name="pArray"></param>
        ''' <param name="pLink"></param>
        ''' <param name="pTreap"></param>
        ''' <remarks></remarks>
        Public Shared Sub AddToQue(
                ByRef pRobot As I_Robot,
                ByRef pArray As ArrayList,
                ByRef pLink As C_Token_Link,
                ByRef pTreap As Treap(Of C_Token_Key))

            If pLink Is Nothing Then Exit Sub
            If pArray.Count = 0 Then
                pArray.Add(pLink)
                AddKeyToTreap(pRobot, pLink, pTreap)
                pLink.clearNextToken()
            Else
                If pLink.ID <> pArray.Item(pArray.Count - 1).ID Then
                    pArray.Add(pLink)
                    AddKeyToTreap(pRobot, pLink, pTreap)
                    pLink.clearNextToken()
                End If
            End If
        End Sub


        Public ReadOnly Property Item(ByVal index As Int32) As C_Token
            Get
                If index < pList.Count Then
                    Return pList.Item(index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub Add(ByVal pToken As C_Token)
            pList.Add(pToken)
        End Sub

        Public Function Count() As Int32
            Return pList.Count
        End Function


        Public Sub New()
            ID = MaxID
            MaxID += 1
        End Sub

        Public Sub Init(ByVal strInput As String)
            strSentence = strInput
            Dim strSplit() As String = Split(strInput, " ")

            For i As Int32 = 0 To UBound(strSplit)
                Dim pToken As C_Token = New C_Token(strSplit(i), i, i + 1, 1)
                pList.Add(pToken)
            Next

            Me.iStart = pList(0).iStart
            Me.iEnd = pList(pList.Count - 1).iEnd
        End Sub

        Public Sub Copy(ByRef pTokenIn As C_Token_Link)
            '复制到后面
            For i As Int32 = 0 To pTokenIn.pList.Count - 1
                Dim pToken As C_Token = New C_Token()
                pToken.Copy(pTokenIn.pList.Item(i))
                pList.Add(pToken)
            Next

            Me.iStart = pList(0).iStart
            Me.iEnd = pList(pList.Count - 1).iEnd
        End Sub

        Public Overrides Function ToString() As String
            Dim pToken As C_Token
            Dim strReturn As String = ""
            For i As Int32 = 0 To pList.Count - 1
                pToken = pList.Item(i)
                strReturn += pToken.ToString(True) + " "
            Next

            Return ID & ": " & strReturn.Trim
        End Function

        Public Function Equal(ByRef pToken_Link2 As C_Token_Link) As Boolean
            '比较是否相同
            If pToken_Link2.pList.Count <> pList.Count Then
                Return False
            Else
                For i As Int32 = 0 To pList.Count - 1
                    If pToken_Link2.pList.Item(i).Equal(pList.Item(i)) = False Then
                        Return False
                    End If
                Next
            End If
            Return True
        End Function
    End Class

End Namespace
