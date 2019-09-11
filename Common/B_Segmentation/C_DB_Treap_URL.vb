Imports CommonTreapVB.TreapVB
Imports B_File.Funny

Imports System.IO
Imports B_Data.Funny

Namespace FunnyWeber
    'Treap_URL 是URL 组成的 和ID对应的 2棵树

    Public Class C_DB_Treap_URL(Of T)
        Public pTreap_String As C_Treap_Funny(Of T) = New C_Treap_Funny(Of T)
        Public pTreap_ID As Treap(Of T) = New Treap(Of T)
        Public strT As String

        'Public Sub insert( _
        '    ByRef pServer As C_Server, _
        '    ByRef pURL As C_FunnyNode)

        '    If pURL.ID <= 0 Then Exit Sub

        '    pTreap_String.Remove(New C_K_Str(pURL.URL))
        '    pTreap_ID.Remove(New C_K_ID(pURL.ID))

        '    pTreap_String.insert(New C_K_Str(pURL.URL), pURL)
        '    pTreap_ID.insert(New C_K_ID(pURL.ID), pURL)

        '    Select Case UCase(strT)
        '        Case "KEY_"
        '            Dim strSplit() As String, pTreap As Treap
        '            strSplit = Split(pURL.URL, "_")
        '            If UBound(strSplit) >= 2 Then
        '                pTreap = pServer.Tree_KeyWord.Add_Treap_IfNone(New C_K_Str(strSplit(0)))
        '                pTreap.insert(New C_K_Str(strSplit(2)), strSplit(2))
        '            End If
        '            'Case "NLR_"
        '            'pServer.NLP_Rule_Add(pURL.ID, pURL.URL)
        '            'Case "NLC_"
        '            '    pServer.NLP_RuleCompete_Add(pURL.URL)
        '    End Select
        'End Sub


        'Public Sub Remove( _
        '        ByRef pServer As C_Server, _
        '        ByVal ID As Int32)

        '    Dim pURL As C_FunnyNode
        '    pURL = pTreap_ID.find(New C_K_ID(ID))

        '    If Not pURL Is Nothing Then
        '        Select Case UCase(strT)
        '            Case "KEY_"
        '                Dim strSplit() As String, pTreap As Treap
        '                strSplit = Split(pURL.URL, "_")
        '                If UBound(strSplit) >= 2 Then
        '                    pTreap = pServer.Tree_KeyWord.Add_Treap_IfNone(New C_K_Str(strSplit(0)))
        '                    pTreap.Remove(New C_K_Str(strSplit(2)))
        '                End If
        '                'Case "NLR_"
        '                'pServer.NLP_Rule_Remove(pURL.URL)
        '        End Select

        '        pTreap_ID.Remove(New C_K_ID(ID))
        '        pTreap_String.Remove(New C_K_Str(pURL.URL))
        '    End If
        'End Sub


        'Public Function read_URL_Tree( _
        '    ByRef pServer As C_Server) As String

        '    Dim strID, strURL As String

        '    If File.Exists(pTreap_String.strFile) = True Then
        '        Dim SR As StreamReader, FS As FileStream
        '        FS = New FileStream(pTreap_String.strFile, FileMode.OpenOrCreate, FileAccess.Read)
        '        SR = New StreamReader(FS, System.Text.Encoding.UTF8)

        '        SR.BaseStream.Seek(0, SeekOrigin.Begin)

        '        Do While SR.Peek() > -1
        '            strID = SR.ReadLine
        '            strURL = SR.ReadLine
        '            Dim pURL As C_FunnyNode = _
        '                S_FunnyWeber.Get_FunnyNode(pServer, CInt(strID), strT, "文件")
        '            pURL.URL = strURL
        '            Me.Add_Item(pServer, pURL)
        '        Loop

        '        SR.Close()
        '        FS.Close()

        '        SR = Nothing
        '        FS = Nothing
        '    End If

        '    Return ""
        'End Function


        Public Function save_URL_Tree() As String

            Dim pWriter As StreamWriter
            Dim fs As FileStream, strReturn As String = ""

            Try
                If pTreap_String.strFile = "" Then Return ""
                '不要File.Delete,直接覆盖即可,
                '否则可能保存的时候失败会没有静态文件!()

                S_SYS.InitDir(pTreap_String.strFile)    '创建目录等

                fs = New FileStream(pTreap_String.strFile, FileMode.Create, FileAccess.Write)

                pWriter = New StreamWriter(fs, System.Text.Encoding.UTF8)

                Dim pURL As C_FunnyNode
                Dim p As TreapEnumerator = pTreap_ID.Elements(True)
                While (p.HasMoreElements())
                    pURL = (CType(p.NextElement(), C_FunnyNode))
                    pWriter.WriteLine(pURL.ID)
                    pWriter.WriteLine(pURL.URL)
                End While

                pWriter.Close()
                fs.Close()
            Catch ex As Exception
                strReturn = ex.Message
            End Try
            pWriter = Nothing
            fs = Nothing
            Return ""
        End Function

    End Class
End Namespace