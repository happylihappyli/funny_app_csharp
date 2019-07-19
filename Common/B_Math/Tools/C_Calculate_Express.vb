Public Class C_Calculate_Express
    Public Class CStack
        Private m_StackArray() As Object
        Private m_nStackSize As Short
        Private m_nCurrPos As Short

        'Granularity for growing stack
        Private Const INC_SIZE As Short = 10

        'Read-only property returns the number of items on the stack
        Public ReadOnly Property StackSize() As Short
            Get
                StackSize = m_nCurrPos
            End Get
        End Property

        'Pushes a value onto the stack.
        'The stack is made bigger if required.
        Public Sub Push(ByRef Value As Object)
            m_nCurrPos = m_nCurrPos + 1
            If m_nCurrPos > m_nStackSize Then
                m_nStackSize = m_nStackSize + INC_SIZE
                'UPGRADE_WARNING: 数组 m_StackArray 的下限已从 1 更改为 0。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1033"”
                ReDim Preserve m_StackArray(m_nStackSize)
            End If
            'UPGRADE_WARNING: 未能解析对象 Value 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
            'UPGRADE_WARNING: 未能解析对象 m_StackArray(m_nCurrPos) 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
            m_StackArray(m_nCurrPos) = Value
        End Sub

        'Returns the value on the top of the stack and removes it
        'from the stack. A run-time error is raised if the stack is empty.
        Public Function Pop() As Object
            If m_nCurrPos > 0 Then
                'UPGRADE_WARNING: 未能解析对象 m_StackArray(m_nCurrPos) 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
                Pop = m_StackArray(m_nCurrPos)
                m_nCurrPos = m_nCurrPos - 1
            Else
                Err.Raise(vbObjectError + 1011, , "Pop method invoked on empty stack")
            End If
        End Function

        'Returns the value on top of the stack without removing it
        'from the stack. A run-time error is raised if the stack is empty.
        Public Function GetPopValue() As Object
            If m_nCurrPos > 0 Then
                'UPGRADE_WARNING: 未能解析对象 m_StackArray(m_nCurrPos) 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
                Return m_StackArray(m_nCurrPos)
            Else
                Err.Raise(vbObjectError + 1012, , "GetPopValue method invoked on empty stack")
                Return Nothing
            End If
        End Function

        'Initialization routine
        'UPGRADE_NOTE: Class_Initialize 已升级到 Class_Initialize_Renamed。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1061"”
        Private Sub Class_Initialize_Renamed()
            m_nStackSize = 0
            m_nCurrPos = 0
        End Sub

    End Class


    Public Class CSymbolTable

        'CSymbolTable - Symbol table classe for Visual Basic 5
        'Copyright (c) 1995-97 SoftCircuits Programming (R)
        'Redistributed by Permission.
        '
        'This table is used by the CEval class. It's simply a wrapper for
        'a standard collection to make it easy to store a list of symbols
        'and associated values.
        '
        'This program may be distributed on the condition that it is
        'distributed in full and unchanged, and that no fee is charged for
        'such distribution with the exception of reasonable shipping and media
        'charged. In addition, the code in this program may be incorporated
        'into your own programs and the resulting programs may be distributed
        'without payment of royalties.
        '
        'This example program was provided by:
        ' SoftCircuits Programming
        ' http://www.softcircuits.com
        ' P.O. Box 16262
        ' Irvine, CA 92623

        'Private collection to hold symbol values
        Private m_Symbols As New Collection

        'Add a symbol to the symbol table
        'Raises run-time error if symbol already exists or if nValue is not numeric
        Public Sub Add(ByRef sName As String, ByRef nValue As Object)
            If IsSymbolDefined(sName) = False Then
                If IsNumeric(nValue) Then
                    m_Symbols.Add(nValue, sName)
                Else
                    Err.Raise(vbObjectError + 1020, , "Invalid symbol value")
                End If
            Else
                Err.Raise(vbObjectError + 1021, , "Symbol already defined")
            End If
        End Sub

        'Delete the specified symbol from the symbol table
        'If the symbol is not defined, the call is ignored
        Public Sub Delete(ByRef sName As String)
            If IsSymbolDefined(sName) Then
                m_Symbols.Remove(sName)
            End If
        End Sub

        'Indicates if the specified symbol name is currently defined
        Public Function IsSymbolDefined(ByRef sName As String) As Boolean
            Dim nValue As Object
            On Error Resume Next
            'UPGRADE_WARNING: 未能解析对象 m_Symbols() 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
            'UPGRADE_WARNING: 未能解析对象 nValue 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
            nValue = m_Symbols.Item(sName)
            If Err.Number Then
                IsSymbolDefined = False
            Else
                IsSymbolDefined = True
            End If
        End Function

        'Sets the value of the specified symbol
        'Raises a run-time error if the symbol is not currently defined or
        'if the value is non-numeric

        'Returns the value for the specified symbol
        'Raises a run-time error if the symbol is not currently defined
        Public Property Value(ByVal sName As String) As Object
            Get
                Dim strReturn As String = ""
                If IsSymbolDefined(sName) = True Then
                    'UPGRADE_WARNING: 未能解析对象 m_Symbols() 的默认属性。 单击以获得更多信息:“ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"”
                    strReturn = m_Symbols.Item(sName)
                Else
                    Err.Raise(vbObjectError + 1022, , "Symbol not defined")
                End If
                Return strReturn
            End Get
            Set(ByVal Value As Object)
                If IsSymbolDefined(sName) = True Then
                    If IsNumeric(Value) Then
                        Delete(sName)
                        Add(sName, Value)
                    Else
                        Err.Raise(vbObjectError + 1020, , "Invalid symbol value")
                    End If
                Else
                    Err.Raise(vbObjectError + 1022, , "Symbol not defined")
                End If
            End Set
        End Property

        'Returns the number of symbols in table
        Public ReadOnly Property Count() As Short
            Get
                Count = m_Symbols.Count()
            End Get
        End Property

		Protected Overrides Sub Finalize()
			m_Symbols = Nothing
			MyBase.Finalize()
		End Sub
	End Class

#Const USE_SYMBOLS = False

	'State constants
    Private Const STATE_NONE As Int32 = 0
	Private Const STATE_OPERAND as Int32  = 1
    Private Const STATE_OPERATOR As Int32 = 2
    Private Const STATE_UNARYOP As Int32 = 3

    Private Const UNARY_NEG As String = "(-)"

	Private m_sErrMsg As String

#If USE_SYMBOLS = True Then

    'Expose symbol table object
    Public m_SymbolTable As New CSymbolTable

#End If

	'Evaluates the expression and returns the result.
	Public Function Evaluate( _
	 ByVal sExpression As String, _
	 ByVal bShowError As Boolean, ByVal StrFormat As String) As String

		Dim sBuffer As String
		Dim nErrPosition As Integer
		Dim flValue As Double

		sExpression = Replace(sExpression, "＋", "+")
		sExpression = Replace(sExpression, "－", "-")
		sExpression = Replace(sExpression, vbCrLf, "")
		sExpression = Replace(sExpression, vbTab, "")
		sExpression = Replace(sExpression, " ", "")
		sExpression = Replace(sExpression, "+*", "+")

		Do While InStr(sExpression, "+-") > 0
			sExpression = Replace(sExpression, "+-", "-")
		Loop

		Do While InStr(sExpression, "++") > 0
			sExpression = Replace(sExpression, "++", "+")
		Loop

		sExpression = Replace(sExpression, "(+", "(")

		Do While Right(sExpression, 1) = "+"
			sExpression = Left(sExpression, Len(sExpression) - 1)
		Loop

		'Convert to postfix expression
		nErrPosition = InfixToPostfix(sExpression, sBuffer)
		'Raise trappable error if error in expression
		If nErrPosition Then
			If bShowError Then
				Return "计算错误！"
			Else
				Return ""
			End If
		End If
		'Evaluate postfix expression
		flValue = DoEvaluate(sBuffer)

		Dim StrReturn As String

		If StrFormat <> "" Then
			StrReturn = Format(flValue, StrFormat)
		Else
			StrReturn = CStr(flValue)
		End If

		'StrReturn = sExpression + "=" & StrReturn

		Return StrReturn
	End Function

	'Converts an infix expression to a postfix expression
	'that contains exactly one space following each token.
	Private Function InfixToPostfix(ByVal sExpression As String, ByRef sBuffer As String) As Integer
		Dim i As Integer, ch, sTemp As String
		Dim nCurrState, nParenCount As Integer
		Dim bDecPoint As Boolean
		Dim stkTokens As New CStack

		nCurrState = STATE_NONE
		nParenCount = 0
		i = 1
		Do Until i > Len(sExpression)
			'Get next character in expression
			ch = Mid$(sExpression, i, 1)
			'Respond to character type
			Select Case ch
				Case "("
					'Cannot follow operand
					If nCurrState = STATE_OPERAND Then
						m_sErrMsg = "Operator expected"
						GoTo EvalError
					End If
					'Allow additional unary operators after "("
					If nCurrState = STATE_UNARYOP Then
						nCurrState = STATE_OPERATOR
					End If
					'Push opening parenthesis onto stack
					stkTokens.Push(ch)
					'Keep count of parentheses on stack
					nParenCount = nParenCount + 1
				Case ")"
					'Must follow operand
					If nCurrState <> STATE_OPERAND Then
						m_sErrMsg = "Operand expected"
						GoTo EvalError
					End If
					'Must have matching open parenthesis
					If nParenCount = 0 Then
						m_sErrMsg = "Closing parenthesis without matching open parenthesis"
						GoTo EvalError
					End If
					'Pop all operators until matching "(" found
					sTemp = stkTokens.Pop
					Do Until sTemp = "("
						sBuffer = sBuffer & sTemp & " "
						sTemp = stkTokens.Pop
					Loop
					'Keep count of parentheses on stack
					nParenCount = nParenCount - 1
				Case "+", "-", "*", "/", "^"
					'Need a bit of extra code to handle unary operators
					If nCurrState = STATE_OPERAND Then
						'Pop operators with precedence >= operator in ch
						Do While stkTokens.StackSize > 0
							If GetPrecedence(stkTokens.GetPopValue) < GetPrecedence(ch) Then
								Exit Do
							End If
							sBuffer = sBuffer & stkTokens.Pop & " "
						Loop
						'Push new operand
						stkTokens.Push(ch)
						nCurrState = STATE_OPERATOR
					ElseIf nCurrState = STATE_UNARYOP Then
						'Don't allow two unary operators in a row
						m_sErrMsg = "Operand expected"
						GoTo EvalError
					Else
						'Test for unary operator
						If ch = "-" Then
							'Push unary minus
							stkTokens.Push(UNARY_NEG)
							nCurrState = STATE_UNARYOP
						ElseIf ch = "+" Then
							'Simply ignore positive unary operator
							nCurrState = STATE_UNARYOP
						Else
							m_sErrMsg = "Operand expected"
							GoTo EvalError
						End If
					End If
				Case "0" To "9", "."
					'Cannot follow other operand
					If nCurrState = STATE_OPERAND Then
						m_sErrMsg = "Operator expected"
						GoTo EvalError
					End If
					sTemp = ""
					bDecPoint = False
					Do While InStr("0123456789.", ch)
						If ch = "." Then
							If bDecPoint Then
								m_sErrMsg = "Operand contains multiple decimal points"
								GoTo EvalError
							Else
								bDecPoint = True
							End If
						End If
						sTemp = sTemp & ch
						i = i + 1
						If i > Len(sExpression) Then Exit Do
						ch = Mid$(sExpression, i, 1)
					Loop
					'i will be incremented at end of loop
					i = i - 1
					'Error if number contains decimal point only
					If sTemp = "." Then
						m_sErrMsg = "Invalid operand"
						GoTo EvalError
					End If
					sBuffer = sBuffer & sTemp & " "
					nCurrState = STATE_OPERAND
				Case Is <= " "				 'Ignore spaces, tabs, etc.
				Case Else

#If USE_SYMBOLS Then

                    'Symbol name cannot follow other operand
                    If nCurrState = STATE_OPERAND Then
                        m_sErrMsg = "Operator expected"
                        GoTo EvalError
                    End If
                    If IsSymbolCharFirst(ch) Then
                        sTemp = ch
                        i = i + 1
                        If i <= Len(sExpression) Then
                            ch = Mid$(sExpression, i, 1)
                            Do While IsSymbolChar(ch)
                                sTemp = sTemp & ch
                                i = i + 1
                                If i > Len(sExpression) Then Exit Do
                                ch = Mid$(sExpression, i, 1)
                            Loop
                        End If
                    Else
                        'Unexpected character
                        m_sErrMsg = "Unexpected character encountered"
                        GoTo EvalError
                    End If
                    'See if symbol is defined
                    If m_SymbolTable.IsSymbolDefined(sTemp) Then
                        sBuffer = sBuffer & CStr(m_SymbolTable.Value(sTemp)) & " "
                        nCurrState = STATE_OPERAND
                        'i will be incremented at end of loop
                        i = i - 1
                    Else
                        m_sErrMsg = "Undefined symbol : '" & sTemp & "'"
                        'Reset error position to start of symbol
                        i = i - Len(sTemp)
                        GoTo EvalError
                    End If

#Else

					'Unexpected character
					m_sErrMsg = "Unexpected character encountered"
					GoTo EvalError

#End If

			End Select
			i = i + 1
		Loop
		'Expression cannot end with operator
		If nCurrState = STATE_OPERATOR Or nCurrState = STATE_UNARYOP Then
			m_sErrMsg = "Operand expected"
			GoTo EvalError
		End If
		'Check for balanced parentheses
		If nParenCount > 0 Then
			m_sErrMsg = "Closing parenthesis expected"
			GoTo EvalError
		End If
		'Retrieve remaining operators from stack
		Do Until stkTokens.StackSize = 0
			sBuffer = sBuffer & stkTokens.Pop & " "
		Loop
		'Indicate no error
		InfixToPostfix = 0
		Exit Function
EvalError:
		'Report error postion
		InfixToPostfix = i
		Exit Function
	End Function

	'Returns a number that indicates the relative precedence of an operator.
	Private Function GetPrecedence(ByVal ch As String) As Integer
		Select Case ch
			Case "+", "-"
				GetPrecedence = 1
			Case "*", "/"
				GetPrecedence = 2
			Case "^"
				GetPrecedence = 3
			Case UNARY_NEG
				GetPrecedence = 10
			Case Else
				GetPrecedence = 0
		End Select
	End Function

	'Evaluates the given expression and returns the result.
	'It is assumed that the expression has been converted to
	'a postix expression and that a space follows each token.
	Private Function DoEvaluate(ByVal sExpression As String) As Double
		Dim i As Integer, j As Integer, stkTokens As New CStack
		Dim sTemp As String, Op1 As Object, Op2 As Object

		'Locate first token
		i = 1
		j = InStr(sExpression, " ")
		Do Until j = 0
			'Extract token from expression
			sTemp = Mid$(sExpression, i, j - i)
			If IsNumeric(sTemp) Then
				'If operand, push onto stack
				stkTokens.Push(CDbl(sTemp))
			Else
				'If operator, perform calculations
				Select Case sTemp
					Case "+"
						stkTokens.Push(stkTokens.Pop + stkTokens.Pop)
					Case "-"
						Op1 = stkTokens.Pop
						Op2 = stkTokens.Pop
						stkTokens.Push(Op2 - Op1)
					Case "*"
						stkTokens.Push(stkTokens.Pop * stkTokens.Pop)
					Case "/"
						Op1 = stkTokens.Pop
						Op2 = stkTokens.Pop
						stkTokens.Push(Op2 / Op1)
					Case "^"
						Op1 = stkTokens.Pop
						Op2 = stkTokens.Pop
						stkTokens.Push(Op2 ^ Op1)
					Case UNARY_NEG
						stkTokens.Push(-stkTokens.Pop)
					Case Else
						'This should never happen (bad tokens caught in InfixToPostfix)
						Err.Raise(vbObjectError + 1002, , "Bad token in Evaluate: " & sTemp)
				End Select
			End If
			'Find next token
			i = j + 1
			j = InStr(i, sExpression, " ")
		Loop
		'Remaining item on stack contains result
		If stkTokens.StackSize > 0 Then
			DoEvaluate = stkTokens.Pop
		Else
			'Null expression; return 0
			DoEvaluate = 0
		End If
	End Function

	'Returns a boolean value that indicates if sChar is a valid
	'character to be used as the first character in symbols names
	Private Function IsSymbolCharFirst(ByVal sChar As String) As Boolean
		Dim c As String

		c = UCase(Left(sChar, 1))
		If (c >= "A" And c <= "Z") Or InStr("_", c) Then
			IsSymbolCharFirst = True
		Else
			IsSymbolCharFirst = False
		End If
	End Function

	'Returns a boolean value that indicates if sChar is a valid
	'character to be used in symbols names
	Private Function IsSymbolChar(ByVal sChar As String) As Boolean
		Dim c As String

		c = UCase(Left(sChar, 1))
		If (c >= "A" And c <= "Z") Or InStr("0123456789_", c) Then
			IsSymbolChar = True
		Else
			IsSymbolChar = False
		End If
	End Function


End Class
