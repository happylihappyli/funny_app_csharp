Namespace com.Funny.Segmentation

    ''' <summary>
    ''' �ִʺ�ĵ��ʣ����� Word=��չ�й�, OutPutWord=��չ �й�
    ''' ���Ϊ�գ����޷ִ�
    ''' Ҳ�����Ƿ��� ƻ��={ˮ��.��}
    ''' </summary>
    ''' <remarks></remarks>
    Public Class C_Word_Seg
        Public Shared ID_Count As Int32 = 0

        Public ID As Int32 = 0

        ''' <summary>
        ''' ����ĵ��ʣ������������У��Ͱѵ�ǰ C_Word_Convert ���Ƶ� Session �У����û�У��Ͳ�������� C_Word_Convert
        ''' </summary>
        ''' <remarks></remarks>
        Public Active_Word As String

        ''' <summary>
        ''' ����
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