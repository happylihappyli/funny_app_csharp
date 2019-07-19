Public Class C_MathCaculate

    Public Class C_Cal_Item
        Public mStr As String
        Public Order As Int32
        Public mValue As Double
    End Class

    Dim MyValue As Collection ' As String

    Private Function Substitute(Optional ByRef c As Collection = Nothing) As String
        'C:代入变量的值
        'if has error return Substitue<>""
        '代入计算
        'Substitute

        Dim i As Integer
        Dim pC_Cal_Item As C_Cal_Item
        Dim StrTmp As String
        Dim pItem As C_Cal_Item

        '找到a,b,...pi代入具体数字
        For i = 1 To MyValue.Count
            pC_Cal_Item = MyValue.Item(i)

            If pC_Cal_Item.Order = 0 Then   '0代表是数字或变量
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
                        Case UCase("π")
                            pC_Cal_Item.mValue = pC_Cal_Item.mValue * 4 * Math.Atan(1)
                    End Select

                End If
            End If
        Next

    End Function

    Private Function ReplaceOther(ByVal StrTmp As String) As String
        '其他进制的处理
        Dim StrTmp2 As String, j As Long
        Dim pC_Cal_Item As C_Cal_Item

        Select Case Right(StrTmp, 1)
            Case "b", "B"
                '二进制的转化
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
                '十进制的转化
                StrTmp = Left(StrTmp, Len(StrTmp) - 1)
                If IsNumeric(StrTmp) = False Then
                    'error
                    ReplaceOther = "D error"
                    Exit Function
                Else
                    pC_Cal_Item.mValue = pC_Cal_Item.mValue * Val(StrTmp)
                End If
            Case "h", "H"
                '十六进制的转化
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
        'Str是要转化为十进制的二进制字符串
        'convert bit to decimal
        '二进制的字符串转化为十进制的

        Dim lngPosDot As Long ' Position of "."
        Dim StrPre As String    '小数点前面部分
        Dim StrAfter As String  '小数点后面部分
        Dim i As Long   '循环变量

        '找到点的位置,然后分解成两部分
        lngPosDot = InStr(Str, ".")
        If lngPosDot > 0 Then
            StrPre = Left(Str, lngPosDot - 1)
            StrAfter = Right(Str, Len(Str) - lngPosDot)
        Else
            StrPre = Str
            StrAfter = ""
        End If

        FuncBin = 0
        '处理整数部分
        For i = 1 To Len(StrPre)
            If Mid(StrPre, i, 1) = "1" Then
                FuncBin = FuncBin + 2 ^ (Len(StrPre) - i)
            End If
        Next

        '处理小数部分
        For i = 1 To Len(StrAfter)
            If Mid(StrAfter, i, 1) = "1" Then
                FuncBin = FuncBin + 2 ^ (-i)
            End If
        Next
    End Function

    Function NotNum(ByVal Str As String) As Boolean
        '如果是运算符号,以及扩号,也就是非变量或数字 就是True
        '否则就是False
        If Str = "(" Or Str = ")" Or Str = "+" Or Str = "-" Or _
            Str = "*" Or Str = "/" Or Str = "^" Then
            NotNum = True
        Else
            NotNum = False
        End If
    End Function

    Function Calculate(ByVal NowStr As String, ByRef c As Collection, Optional ByVal ShowError As Boolean = True) As String
        '计算数学表达式,如果是正确的返回一个数字
        '否则返回一个非数字的字符串

        Dim i, j As Int16
        Dim StrTmp As String 'temperately use string
        Dim pC_Cal_Item As C_Cal_Item '计算集合里的元素指针
        Dim mC_Cal_Item(4) As C_Cal_Item '计算集合里的元素指针

        Dim mOldCount As Long
        '在计算集合里上一次计算完毕的元素个数
        '如果和这次一样,说明计算错误,也就是每次计算都要减少


        pC_Cal_Item = New C_Cal_Item       '生成一个计算元素
        MyValue = New Collection

        NowStr = Replace(NowStr, "＋", "+")
        NowStr = Replace(NowStr, "－", "-")
        NowStr = Replace(NowStr, vbCrLf, "")
        NowStr = Replace(NowStr, vbTab, "")
        NowStr = Replace(NowStr, " ", "")
        NowStr = Replace(NowStr, "+*", "+")
        NowStr = Replace(NowStr, "(+", "(")

        Do While Right(NowStr, 1) = "+"
            NowStr = Left(NowStr, Len(NowStr) - 1)
        Loop

        Dim StrInput As String  '输入   StrInput 
        StrInput = NowStr

        pC_Cal_Item.mStr = "(" '在最前面加一个左扩号
        MyValue.Add(pC_Cal_Item)
        NowStr = Trim(NowStr) + ")" '在表达式最右边加一个右扩号

        Do While Len(NowStr) > 0
            i = i + 1
            StrTmp = Mid(NowStr, i, 1) '一个一个字符检查,
            'i是开始数到符号位置的长度
            If i > 1 Then
				'如果前面有数字
				Select Case StrTmp
					Case "+", "-", "(", ")", "*", "/", "^", ","
						pC_Cal_Item = New C_Cal_Item
						pC_Cal_Item.mStr = Left(NowStr, i - 1)
						MyValue.Add(pC_Cal_Item)
						NowStr = Right(NowStr, Len(NowStr) - i + 1)
						i = 0
				End Select
			Else
				'如果没有数字
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

        '处理符号. 如++ +- --等

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


        '设置计算顺序
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
					pC_Cal_Item.Order = 0				'符号数字
			End Select
        Next


        '代入变量,常量,以及进制转化
        StrTmp = Substitute(c)

        If StrTmp <> "" Then
            Calculate = StrTmp
            Exit Function
        End If


        Do While MyValue.Count > 1
            If mOldCount = MyValue.Count Then
                '每次计算mOldCount都会越来越少,否则就是错误了
                If ShowError Then
                    Calculate = "无法计算:" & StrInput & "<br>"
                Else
                    Calculate = ""
                End If
                Exit Function
            End If

            mOldCount = MyValue.Count

            '二元计算
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
                    '0,2,4位置都不是数字,而且2的运算级别大于4的运算级别,大于等于0的运算级别
                    '且1,3位置是数字(即,运算级别为0)
                    '则代入二元运算函数

                    StrTmp = Cal_TwoElement(i + 1)
                    If StrTmp <> "" Then
                        'has error exit function
                        Calculate = StrTmp
                        Exit Function
                    End If
                End If
            Next

            '计算 "函数;(;数字;)" 的四个元素的形式
			i = 1
			Do While i <= MyValue.Count - 3
				'如果是"函数 ( 数字 )" 形式
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


			'计算 "函数;(;数字;,;数字;)" 的6个元素的形式
			i = 1
			Do While i <= MyValue.Count - 5
				'如果是"函数 ( 数字;,;数字;)" 形式

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


			'计算 "(;数字;)" =>"数字"
			For i = 1 To MyValue.Count - 3
				'如果"( 数字 )" 的形式
				If i + 3 > MyValue.Count Then Exit For
				If MyValue.Item(i).Order < 5 And MyValue.Item(i + 1).mStr = "(" _
				  And MyValue.Item(i + 3).mStr = ")" Then
					MyValue.Remove(i + 3)
					MyValue.Remove(i + 1)
				End If
			Next


			'去掉最后一层括号 "(;数字;)"=>"数字" 的形式
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
        'index : 计算的位置

        'if has error Cal_TwoElement<>""
        '二元运算

        On Error GoTo WrongExit

        Select Case MyValue.Item(Index + 1).mStr
            Case "+"    '加法运算
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue + MyValue.Item(Index + 2).mValue
            Case "-"    '减法运算
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue - MyValue.Item(Index + 2).mValue
            Case "*"    '乘法运算
                MyValue.Item(Index).mValue = MyValue.Item(Index).mValue * MyValue.Item(Index + 2).mValue
            Case "/"    '除法运算
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

        '计算完毕,删除算好的不再利用的元素
        MyValue.Remove(Index + 2)
        MyValue.Remove(Index + 1)
        MyValue.Item(Index).Order = 0

        Exit Function
WrongExit:
        Cal_TwoElement = Err.Description  '计算有错误
        Exit Function
    End Function

    Private Function CalculateFunction(ByVal Index As Integer) As String
        '如果有错误返回一个非空的字符串
        '计算函数
        'index : 计算的位置

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

		'计算完毕,删除算好的不再利用的元素

        MyValue.Remove(Index + 3)
        MyValue.Remove(Index + 1)
        MyValue.Remove(Index)

        Exit Function
WrongExit:
        CalculateFunction = Err.Description  '计算有错误
        Exit Function
    End Function

	Private Function CalculateFunction2(ByVal Index As Integer) As String
		'如果有错误返回一个非空的字符串
		'计算函数
		'index : 计算的位置

		On Error GoTo WrongExit

		Select Case MyValue.Item(Index).mStr
			Case "mod"
				MyValue.Item(Index + 2).mValue = MyValue.Item(Index + 2).mValue Mod MyValue.Item(Index + 4).mValue
			Case Else
				MyValue.Item(Index + 2).mValue = MyValue.Item(Index + 2).mValue
		End Select

		'计算完毕,删除算好的不再利用的元素
		MyValue.Remove(Index + 5)
		MyValue.Remove(Index + 4)
		MyValue.Remove(Index + 3)
		MyValue.Remove(Index + 1)
		MyValue.Remove(Index)

        Return ""
WrongExit:
        Return Err.Description     '计算有错误
	End Function

End Class

