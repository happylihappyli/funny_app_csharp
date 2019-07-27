Imports System.Web
Imports B_File.Funny

Namespace Funny
    Public Module S_Debug

        Public Sub LogError(ByVal RootPath As String, ByVal strMsg As String)
            Try
                S_File_Text.WriteContent( _
                    RootPath & "/Error" & Format(Now(), "yyyy-MM-dd_(HH)") & ".html", _
                    Now() & vbCrLf & strMsg & vbCrLf, True)
            Catch ex As Exception

            End Try

        End Sub
    End Module
End Namespace
