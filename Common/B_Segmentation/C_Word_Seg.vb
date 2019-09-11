Namespace com.Funny.Segmentation

    ''' <summary>
    ''' 分词后的单词，比如 Word=发展中国, OutPutWord=发展 中国
    ''' 如果为空，则无分词
    ''' 也可以是泛化 苹果={水果.类}
    ''' </summary>
    ''' <remarks></remarks>
    Public Class C_Word_Seg
        Public Shared ID_Count As Int32 = 0

        Public ID As Int32 = 0

        ''' <summary>
        ''' 激活的单词，这个单词如果有，就把当前 C_Word_Convert 复制到 Session 中，如果没有，就不激活这个 C_Word_Convert
        ''' </summary>
        ''' <remarks></remarks>
        Public Active_Word As String

        ''' <summary>
        ''' 单词
        ''' </summary>
        ''' <remarks></remarks>
        Public Word As String

        Public OutPutWord As String


        Public Sub New(ByVal strLeft As String, ByVal strRight As String, ByVal strActive As String)
            C_Word_Convert.ID_Count += 1

            Me.ID = C_Word_Convert.ID_Count

            Me.Word = strLeft
            Me.OutPutWord = strRight
            Me.Active_Word = strActive
        End Sub
    End Class


End Namespace