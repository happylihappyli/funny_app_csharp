Public Class C_MathCaculate

    Public Class C_Cal_Item
        Public mStr As String
        Public Order As Int32
        Public mValue As Double
    End Class

    Dim MyValue As Collection ' As String

    Private Function Substitute(Optional ByRef c As Collection = Nothing) As String
        'C:���������ֵ
        'if has error return Substitue<>""
        '�������
        'Substitute

        Dim i As Integer
        Dim pC_Cal_Item As C_Cal_Item
        Dim StrTmp As String
        Dim pItem As C_Cal_Item

        '�ҵ�a,b,...pi�����������
        For i = 1 To MyValue.Count
            pC_Cal_Item = MyValue.Item(i)

            If pC_Cal_Item.Order = 0 Then   '0���������ֻ����
                If IsNumeric(pC_Cal_Item.mStr) Then
                    pC_Cal_Item.mValue = Val(pC_Cal_Item.mStr)
                Else
                    If Left(pC_Cal_Item.mStr, 1) = "-" Then

                        StrTmp = Right(pC_Cal_Item.mStr, Len(pC_Cal_Item.mStr) - 1)
                        pC_Cal_Item.mValue = -1
                    Else
                        StrTmp = pC_Cal_Item.mStr
                        pC_Cal_Item.mValue = 1
                    End If

                    Select Case UCase(StrTmp)
                        Case "A" To "Z"
                            'lngIndex = Asc(UCase(StrTmp)) - Asc("A") + 1
                            If c Is Nothing Then
                                pC_Cal_Item.mValue = 0
                            Else
                                For Each pItem In c
                                    If pItem.mStr = pC_Cal_Item.mStr Then
                                        pC_Cal_Item.mValue = pC_Cal_Item.mValue * pItem.mValue
                                    End If

                                    'If lngIndex <= c.Count And lngIndex >= 1 Then
                                    '    pC_Cal_Item.mValue = pC_Cal_Item.mValue * c.Item(lngIndex).mValue
                                    'Else
                                    '    pC_Cal_Item.mValue = 0
                                    'End If
                                Next
                            End If
                        Case UCase("��")
                            pC_Cal_Item.mValue = pC_Cal_Item.mValue * 4 * Math.Atan(1)
                    End Select

                End If
            End If
        Next

    End Function

    Private Function ReplaceOther(ByVal StrTmp As String) As String
        '�������ƵĴ���
        Dim StrTmp2 As String, j As Long
        Dim pC_Cal_Item As C_Cal_Item

        Select Case Right(StrTmp, 1)
            Case "b", "B"
                '�����Ƶ�ת��
                StrTmp = Left(StrTmp, Len(StrTmp) - 1)
                StrTmp2 = Replace(Replace(StrTmp, "1", ""), "0", "")

                If StrTmp2 <> "" And StrTmp2 <> "." Then
                    'error
                    ReplaceOther = "bin error"
                    Exit Function
                Else
                    pC_Cal_Item.mValue = pC_Cal_Item.mValue * FuncBin(StrTmp)
                End If
            Case "d", "D"
                'ʮ���Ƶ�ת��
                StrTmp = Left(StrTmp, Len(StrTmp) - 1)
                If IsNumeric(StrTmp) = False Then
                    'error
                    ReplaceOther = "D error"
                    Exit Function
                Else
                    pC_Cal_Item.mValue = pC_Cal_Item.mValue * Val(StrTmp)
                End If
            Case "h", "H"
                'ʮ�����Ƶ�ת��
                StrTmp = Left(StrTmp, Len(StrTmp) - 1)
                StrTmp2 = StrTmp
                For j = 0 To 9
                    StrTmp2 = Replace(StrTmp2, CStr(j), "")
                Next
                For j = 0 To 5
                    StrTmp2 = Replace(UCase(StrTmp2), Chr(Asc("A") + j), "")
                Next

                If StrTmp2 <> "" And StrTmp2 <> "." Then
                    'error
                    ReplaceOther = "HEX error"
                    Exit Function
                Else
                    pC_Cal_Item.mValue = pC_Cal_Item.mValue * Val("&H" + StrTmp)
                End If
            Case Else
                Select Case UCase(StrTmp)
                    Case "PI"
                        pC_Cal_Item.mValue = pC_Cal_Item.mValue * 4 * Math.Atan(1)
                    Case Else
                        ReplaceOther = "error:" + StrTmp
                        Exit Function
                End Select
        End Select
    End Function
    Function FuncBin(ByVal Str As String) As Double
        'Str��Ҫת��Ϊʮ���ƵĶ������ַ���
        'convert bit to decimal
        '�����Ƶ��ַ���ת��Ϊʮ���Ƶ�

        Dim lngPosDot As Long ' Position of "."
        Dim StrPre As String    'С����ǰ�沿��
        Dim StrAfter As String  'С������沿��
        Dim i As Long   'ѭ������

        '�ҵ����λ��,Ȼ��ֽ��������
        lngPosDot = InStr(Str, ".")
        If lngPosDot > 0 Then
            StrPre = Left(Str, lngPosDot - 1)
            StrAfter = Right(Str, Len(Str) - lngPosDot)
        Else
            StrPre = Str
            StrAfter = ""
        End If

        FuncBin = 0
        '������������
        For i = 1 To Len(StrPre)
            If Mid(StrPre, i, 1) = "1" Then
                FuncBin = FuncBin + 2 ^ (Len(StrPre) - i)
            End If
        Next

        '����С������
        For i = 1 To Len(StrAfter)
            If Mid(StrAfter, i, 1) = "1" Then
                FuncBin = FuncBin + 2 ^ (-i)
            End If
        Next
    End Function

    Function NotNum(ByVal Str As String) As Boolean
        '������������,�Լ�����,Ҳ���ǷǱ��������� ����True
        '�������False
        If Str = "(" Or Str = ")" Or Str = "+" Or Str = "-" Or _
            Str = "*" Or Str = "/" Or Str = "^" Then
            NotNum = True
        Else
            NotNum = False
        End If
    End Function

    Function Calculate(ByVal NowStr As String, ByRef c As Collection, Optional ByVal ShowError As Boolean = True) As String
        '������ѧ���ʽ,�������ȷ�ķ���һ������
        '���򷵻�һ�������ֵ��ַ���

        Dim i, j As Int16
        Dim StrTmp As String 'temperately use string
        Dim pC_Cal_Item As C_Cal_Item '���㼯�����Ԫ��ָ��
        Dim mC_Cal_Item(4) As C_Cal_Item '���㼯�����Ԫ��ָ��

        Dim mOldCount As Long
        '�ڼ��㼯������һ�μ�����ϵ�Ԫ�ظ���
        '��������һ��,˵���������,Ҳ����ÿ�μ��㶼Ҫ����


        pC_Cal_Item = New C_Cal_Item       '����һ������Ԫ��
        MyValue = New Collection

        NowStr = Replace(NowStr, "��", "+")
        NowStr = Replace(NowStr, "��", "-")
        NowStr = Replace(NowStr, vbCrLf, "")
        NowStr = Replace(NowStr, vbTab, "")
        NowStr = Replace(NowStr, " ", "")
        NowStr = Replace(NowStr, "+*", "+")
        NowStr = Replace(NowStr, "(+", "(")

        Do While Right(NowStr, 1) = "+"
            NowStr = Left(NowStr, Len(NowStr) - 1)
        Loop

        Dim StrInput As String  '����   StrInput 
        StrInput = NowStr

        pC_Cal_Item.mStr = "(" '����ǰ���һ��������
        MyValue.Add(pC_Cal_Item)
        NowStr = Trim(NowStr) + ")" '�ڱ��ʽ���ұ߼�һ��������

        Do While Len(NowStr) > 0
            i = i + 1
            StrTmp = Mid(NowStr, i, 1) 'һ��һ���ַ����,
            'i�ǿ�ʼ��������λ�õĳ���
            If i > 1 Then
				'���ǰ��������
				Select Case StrTmp
					Case "+", "-", "(", ")", "*", "/", "^", ","
						pC_Cal_Item = New C_Cal_Item
						pC_Cal_Item.mStr = Left(NowStr, i - 1)
						MyValue.Add(pC_Cal_Item)
						NowStr = Right(NowStr, Len(NowStr) - i + 1)
						i = 0
				End Select
			Else
				'���û������
				Select Case StrTmp
					Case "+", "-"
						'no StrTmp = "-" for -A=-1*A
						Select Case MyValue.Item(MyValue.Count).mStr							' (j - 1)
							Case "+", "-", "*", "/", "^", "("							   ', ")"
							Case Else
								pC_Cal_Item = New C_Cal_Item
								pC_Cal_Item.mStr = Left(NowStr, 1)								  ': j = j + 1
								MyValue.Add(pC_Cal_Item)
								NowStr = Right(NowStr, Len(NowStr) - 1)

								i = 0
						End Select
					Case "(", ")", "*", "/", "^", ","
						pC_Cal_Item = New C_Cal_Item
						pC_Cal_Item.mStr = Left(NowStr, 1)
						MyValue.Add(pC_Cal_Item)
						NowStr = Right(NowStr, Len(NowStr) - 1)
						i = 0
					Case Else
				End Select

            End If
        Loop

        '++ +- -+ --
flagfirst:

        '�������. ��++ +- --��

        For i = 1 To MyValue.Count - 1
            If MyValue.Item(i).mStr = "+" And _
                (MyValue.Item(i + 1).mStr = "-" Or MyValue.Item(i + 1).mStr = "+") Then
                MyValue.Item(i).mStr = MyValue(i + 1).mStr
            ElseIf MyValue(i).mStr = "-" And MyValue(i + 1).mStr = "-" Then
                MyValue.Item(i).mStr = "+"
            ElseIf MyValue(i).mStr = "-" And MyValue(i + 1).mStr = "+" Then
            ElseIf MyValue(i).mStr = " " Then
                MyValue.Item(i).mStr = MyValue.Item(i + 1).mStr
            Else
                GoTo FlagNext
            End If

            MyValue.Remove(i + 1)

            GoTo flagfirst
FlagNext:
        Next


        '���ü���˳��
        For i = 1 To MyValue.Count
            pC_Cal_Item = MyValue.Item(i)
            Select Case LCase(pC_Cal_Item.mStr)
                Case "(", ")"
                    pC_Cal_Item.Order = 1
                Case "+", "-"
                    pC_Cal_Item.Order = 2
                Case "*", "/"
                    pC_Cal_Item.Order = 3
                Case "^"
                    pC_Cal_Item.Order = 4
				Case "sin", "cos", "tg", "ctg", _
						"ln", "exp", "atn", "oct", "hex", "rnd", _
						"int", "mod"
					pC_Cal_Item.Order = 5
				Case Else
					pC_Cal_Item.Order = 0				'��������
			End Select
        Next


        '�������,����,�Լ�����ת��
        StrTmp = Substitute(c)

        If StrTmp <> "" Then
            Calculate = StrTmp
            Exit Function
        End If


        Do While MyValue.Count > 1
            If mOldCount = MyValue.Count Then
                'ÿ�μ���mOldCount����Խ��Խ��,������Ǵ�����
                If ShowError Then
                    Calculate = "�޷�����:" & StrInput & "<br>"
                Else
                    Calculate = ""
                End If
                Exit Function
            End If

            mOldCount = MyValue.Count

            '��Ԫ����
            For i = 1 To MyValue.Count - 4
                If i + 4 > MyValue.Count Then
                    Exit For
                End If

                For j = 0 To 4
                    mC_Cal_Item(j) = MyValue.Item(i + j)
                Next

                'caculate  +a*b+ or '*a*b/' or '*a*b+ ...
                If NotNum(mC_Cal_Item(0).mStr) And NotNum(mC_Cal_Item(2).mStr) And NotNum(mC_Cal_Item(4).mStr) _
                    And mC_Cal_Item(2).Order > mC_Cal_Item(0).Order _
                    And mC_Cal_Item(2).Order >= mC_Cal_Item(4).Order And _
                    mC_Cal_Item(1).Order = 0 And mC_Cal_Item(3).Order = 0 Then
                    '0,2,4λ�ö���������,����2�����㼶�����4�����㼶��,���ڵ���0�����㼶��
                    '��1,3λ��������(��,���㼶��Ϊ0)
                    '������Ԫ���㺯��

                    StrTmp = Cal_TwoElement(i + 1)
                    If StrTmp <> "" Then
                        'has error exit function
                        Calculate = StrTmp
                        Exit Function
                    End If
                End If
            Next

            '���� "����;(;����;)" ���ĸ�Ԫ�ص���ʽ
			i = 1
			Do While i <= MyValue.Count - 3
				'�����"���� ( ���� )" ��ʽ
				If MyValue.Item(i).Order = 5 And MyValue.Item(i + 1).mStr = "(" _
				  And MyValue.Item(i + 3).mStr = ")" Then
					StrTmp = CalculateFunction(i)
					If StrTmp <> "" Then				'has error exit function
						Calculate = StrTmp
						Exit Function
					End If
				End If
				i += 1
			Loop


			'���� "����;(;����;,;����;)" ��6��Ԫ�ص���ʽ
			i = 1
			Do While i <= MyValue.Count - 5
				'�����"���� ( ����;,;����;)" ��ʽ

				If MyValue.Item(i).Order = 5 And MyValue.Item(i + 1).mStr = "(" _
				 And MyValue.Item(i + 3).mStr = "," And MyValue.Item(i + 5).mStr = ")" Then
					StrTmp = CalculateFunction2(i)
					If StrTmp <> "" Then				'has error exit function
						Calculate = StrTmp
						Exit Function
					End If
				End If
				i += 1
			Loop


			'���� "(;����;)" =>"����"
			For i = 1 To MyValue.Count - 3
				'���"( ���� )" ����ʽ
				If i + 3 > MyValue.Count Then Exit For
				If MyValue.Item(i).Order < 5 And MyValue.Item(i + 1).mStr = "(" _
				  And MyValue.Item(i + 3).mStr = ")" Then
					MyValue.Remove(i + 3)
					MyValue.Remove(i + 1)
				End If
			Next


			'ȥ�����һ������ "(;����;)"=>"����" ����ʽ
			If MyValue.Count = 3 Then
				If MyValue.Item(1).mStr = "(" _
				  And MyValue.Item(3).mStr = ")" Then
					MyValue.Remove(3)
					MyValue.Remove(1)
				End If
			End If
		Loop

		Calculate = CStr(MyValue.Item(1).mValue)


		Exit Function

calculateexit:
		Calculate = "0"
    End Function

    Private Function Cal_TwoElement(ByVal Index As Integer) As String
        'index : �����λ��

        'if has error Cal_TwoElement<>""
        '��Ԫ����

        On Error GoTo WrongExit

        Select Case MyValue.Item(Index + 1).mStr
            Case "+"    '�ӷ�����
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue + MyValue.Item(Index + 2).mValue
            Case "-"    '��������
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue - MyValue.Item(Index + 2).mValue
            Case "*"    '�˷�����
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue * MyValue.Item(Index + 2).mValue
            Case "/"    '��������
                If MyValue.Item(Index + 2).mValue = 0 Then
                    Cal_TwoElement = "Divide by Zero"
                    Exit Function
                Else
                    MyValue.Item(Index).mValue = MyValue.Item(Index).mValue / MyValue.Item(Index + 2).mValue
                End If

            Case "^"
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue ^ MyValue.Item(Index + 2).mValue
            Case Else
                GoTo WrongExit
        End Select

        '�������,ɾ����õĲ������õ�Ԫ��
        MyValue.Remove(Index + 2)
        MyValue.Remove(Index + 1)
        MyValue.Item(Index).Order = 0

        Exit Function
WrongExit:
        Cal_TwoElement = Err.Description  '�����д���
        Exit Function
    End Function

    Private Function CalculateFunction(ByVal Index As Integer) As String
        '����д��󷵻�һ���ǿյ��ַ���
        '���㺯��
        'index : �����λ��

        On Error GoTo WrongExit

        Select Case MyValue.Item(Index).mStr
            Case "sin"
                MyValue.Item(Index + 2).mValue = Math.Sin(MyValue.Item(Index + 2).mValue)
            Case "cos"
                MyValue.Item(Index + 2).mValue = Math.Cos(MyValue.Item(Index + 2).mValue)
            Case "tg", "tan"
                MyValue.Item(Index + 2).mValue = Math.Tan(MyValue.Item(Index + 2).mValue)
            Case "ctg"
                MyValue.Item(Index + 2).mValue = 1 / Math.Tan(MyValue.Item(Index + 2).mValue)
            Case "exp"
                MyValue.Item(Index + 2).mValue = Math.Exp(MyValue.Item(Index + 2).mValue)
            Case "atn"
                MyValue.Item(Index + 2).mValue = Math.Atan(MyValue.Item(Index + 2).mValue)
            Case "hex"
                MyValue.Item(Index + 2).mValue = Hex(MyValue.Item(Index + 2).mValue)
            Case "oct"
                MyValue.Item(Index + 2).mValue = Oct(MyValue.Item(Index + 2).mValue)
            Case "rnd"
                MyValue.Item(Index + 2).mValue = Rnd() * (MyValue.Item(Index + 2).mValue)
            Case "int"
				MyValue.Item(Index + 2).mValue = Int(MyValue.Item(Index + 2).mValue)
			Case Else
				MyValue.Item(Index + 2).mValue = MyValue.Item(Index + 2).mValue
		End Select

		'�������,ɾ����õĲ������õ�Ԫ��

        MyValue.Remove(Index + 3)
        MyValue.Remove(Index + 1)
        MyValue.Remove(Index)

        Exit Function
WrongExit:
        CalculateFunction = Err.Description  '�����д���
        Exit Function
    End Function

	Private Function CalculateFunction2(ByVal Index As Integer) As String
		'����д��󷵻�һ���ǿյ��ַ���
		'���㺯��
		'index : �����λ��

		On Error GoTo WrongExit

		Select Case MyValue.Item(Index).mStr
			Case "mod"
				MyValue.Item(Index + 2).mValue = MyValue.Item(Index + 2).mValue Mod MyValue.Item(Index + 4).mValue
			Case Else
				MyValue.Item(Index + 2).mValue = MyValue.Item(Index + 2).mValue
		End Select

		'�������,ɾ����õĲ������õ�Ԫ��
		MyValue.Remove(Index + 5)
		MyValue.Remove(Index + 4)
		MyValue.Remove(Index + 3)
		MyValue.Remove(Index + 1)
		MyValue.Remove(Index)

        Return ""
WrongExit:
        Return Err.Description     '�����д���
	End Function

End Class

