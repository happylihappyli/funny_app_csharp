
Namespace Funny

    Public Enum C_Var_Type
        String_Type
        Int_Type
        Array_Type
        Matrix_Type
    End Enum


    Public Class C_Var
        Inherits Object

        Public Name As String
        Public iType As C_Var_Type
        Public pValue As Object

        Public Sub New(ByVal mType As C_Var_Type,
                       ByVal strName As String,
                       ByVal mValue As Object)
            iType = mType
            Name = strName
            pValue = mValue
        End Sub

        Public Overrides Function ToString() As String
            If iType = C_Var_Type.String_Type Then
                Return pValue
            Else
                Return ""
            End If
        End Function
    End Class


End Namespace