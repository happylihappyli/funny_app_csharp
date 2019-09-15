Namespace Funny

    Public Class C_K_Str
        Implements IComparable

        Private strMyKey As String

        Public Function getStr() As String
            Return strMyKey
        End Function

        Public Sub New(ByVal key As String)
            strMyKey = key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer Implements IComparable.CompareTo

            Dim iReturn As Integer = String.Compare(strMyKey, CType(key, C_K_Str).strMyKey, True)
            If iReturn > 0 Then
                Return 1
            ElseIf iReturn < 0 Then
                Return -1
            Else
                Return 0
            End If
        End Function

        Public Overrides Function ToString() As String
            Return strMyKey
        End Function
    End Class



    Public Class C_K_Str_CaseSensitive
        Implements IComparable

        Private strMyKey As String

        Public Function getStr() As String
            Return strMyKey
        End Function

        Public Sub New(ByVal key As String)
            strMyKey = key
        End Sub

        Public Overloads Function CompareTo(ByVal key As Object) As Integer Implements IComparable.CompareTo

            Dim iReturn As Integer = String.Compare(strMyKey, CType(key, C_K_Str_CaseSensitive).strMyKey, False)
            If iReturn > 0 Then
                Return 1
            ElseIf iReturn < 0 Then
                Return -1
            Else
                Return 0
            End If
        End Function

        Public Overrides Function ToString() As String
            Return strMyKey
        End Function
    End Class
End Namespace

