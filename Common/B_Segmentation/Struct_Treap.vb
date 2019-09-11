Imports B_Data.Funny
Imports B_Segmentation
Imports CommonTreapVB.TreapVB

Public Class Struct_Treap
    Public pTreapStruct As New Treap(Of C_Seg_Struct)
    Public pTreapTreap As New Treap(Of Treap(Of C_Seg_Struct))

    Public Function Size() As String
        Return pTreapStruct.Size
    End Function

    Friend Function Find(pC_K_Str As C_K_Str) As C_Seg_Struct
        Return pTreapStruct.find(pC_K_Str)
    End Function

    Friend Sub insert(c_K_Str As C_K_Str, pStruct As C_Seg_Struct)
        pTreapStruct.insert(c_K_Str, pStruct)
    End Sub
End Class
