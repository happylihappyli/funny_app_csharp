Imports System.Windows.Forms
Imports System.Drawing
Imports B_File.Funny
Imports UILibrary

Namespace Funny

    Public Class C_Debug

        Public html_Content As String = ""
        Public OutputTextBox As WebBrowser

        Public bSaveFile As Boolean = True
        Public bShowTime As Boolean = True

        Public Delegate Sub ClearMsg_CallBack()

        Public strFile As String = Application.StartupPath & "\Debug_Log.txt"

        Public Sub ClearFile()
            S_File_Text.Write(strFile, Format(Now, "yyyy-MM-dd hh:mm:ss") & vbCrLf & "清空" & vbCrLf, False)
        End Sub

        Public Sub Clear()
            If Me.OutputTextBox.Parent.InvokeRequired Then
                Dim d As New ClearMsg_CallBack(AddressOf Clear)
                Me.OutputTextBox.Parent.Invoke(d)
            Else
                OutputTextBox.DocumentText = ""
            End If
        End Sub



        Public Delegate Sub WriteLine_CallBack(ByVal strText As String)

        Public Sub WriteLine(ByVal strText As String, Optional bSave As Boolean = False)

            If strText Is Nothing Then Return
            If Me.OutputTextBox Is Nothing Then Return

            If Me.OutputTextBox.InvokeRequired Then 'Parent.
                Dim d As New WriteLine_CallBack(AddressOf WriteLine)
                Me.OutputTextBox.Invoke(d, New Object() {strText})
            Else
                If bShowTime Then
                    Dim strTime As String = Format(Now, "yyyy-MM-dd HH:mm:ss") & ":" & Format(Now.Millisecond, "000") & " "

                    Me.WriteText(strTime & strText & vbCrLf)
                Else
                    Me.WriteText(strText & vbCrLf)
                End If

                'If bSaveFile OrElse bSave Then
                '    S_File_Text.Write(strFile, Format(Now, "yyyy-MM-dd HH:mm:ss") & vbCrLf & strText & vbCrLf, True)
                'End If

            End If
        End Sub

        Public Sub WriteText(output As String)

            Me.html_Content = output + "<br>" + Me.html_Content
            OutputTextBox.DocumentText = Me.html_Content
        End Sub
    End Class

End Namespace