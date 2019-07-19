Imports System.IO

Namespace Funny

    Public Class S_Dir
        Public Shared Sub CopyDirectory( _
                    ByVal SourceDirectory As String, _
                    ByVal TargetDirectory As String)
            Dim source As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(SourceDirectory)
            Dim target As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(TargetDirectory)

            'Determine   whether   the   source   directory   exists.  
            If (source.Exists = False) Then
                Return
            End If
            If (target.Exists = False) Then
                target.Create()
            End If

            'Copy   files.  
            Dim sourceFiles As System.IO.FileInfo() = source.GetFiles()
            Dim i, j As Integer
            For i = 0 To sourceFiles.Length - 1
                File.Copy(sourceFiles(i).FullName, target.FullName + "\" + sourceFiles(i).Name, True)
            Next i

            'Copy   directories.  
            Dim sourceDirectories As System.IO.DirectoryInfo() = source.GetDirectories()
            For j = 0 To sourceDirectories.Length - 1
                CopyDirectory(sourceDirectories(j).FullName, target.FullName + "\" + sourceDirectories(j).Name)
            Next j
            source = Nothing
            target = Nothing
        End Sub

        Public Shared Function Exists(ByVal strDir As String) As Boolean
            Return Directory.Exists(strDir)
        End Function

        Public Shared Sub CreateDir_If_Not_Exists(ByVal strDir As String)
            If Directory.Exists(strDir) = False Then
                Directory.CreateDirectory(strDir)
            End If
        End Sub
        Public Shared Function ListDir(ByVal strDir As String) As ArrayList
            Dim pArrayList As New ArrayList
            If Directory.Exists(strDir) Then
                Dim source As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(strDir)
                Dim sourceFiles As System.IO.DirectoryInfo() = source.GetDirectories()
                For i As Integer = 0 To sourceFiles.Length - 1
                    pArrayList.Add(sourceFiles(i).Name)
                Next
            End If
            Return pArrayList
        End Function

        Public Shared Function ListFile(ByVal strDir As String) As ArrayList
            Dim pArrayList As New ArrayList
            If Directory.Exists(strDir) Then
                Dim source As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(strDir)
                Dim sourceFiles As System.IO.FileInfo() = source.GetFiles()
                For i As Integer = 0 To sourceFiles.Length - 1
                    pArrayList.Add(sourceFiles(i).Name)
                Next
            End If
            Return pArrayList
        End Function

    End Class

End Namespace