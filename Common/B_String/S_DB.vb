Imports MySql.Data.MySqlClient

Namespace Funny

    Public Module S_DB
        Public Function Read(ByVal pReader As MySqlDataReader, ByVal strField As String) As String
            Return IIf(IsDBNull(pReader(strField)), "", pReader(strField))
        End Function
    End Module

End Namespace