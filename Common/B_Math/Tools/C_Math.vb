Imports B_TreapVB.TreapVB
Imports B_Data.Funny


Public Class C_Math
    ' Evaluate the expression.

    'Private Shared m_Primatives As Hashtable

    Private Enum Precedence
        None = 11
        Unary = 10      ' Not actually used.
        Power = 9
        Times = 8
        Div = 7
        IntDiv = 6
        Modulus = 5
        Plus = 4
    End Enum

    ' Evaluate the expression.
    Public Function EvaluateExpression(ByVal expression As String) As Double
        Dim expr As String
        Dim is_unary As Boolean
        Dim next_unary As Boolean
        Dim parens As Integer
        Dim expr_len As Integer
        Dim ch As String
        Dim lexpr As String
        Dim rexpr As String
        'Dim status As Long
        Dim best_pos As Integer
        Dim best_prec As Precedence

        ' Remove all spaces.
        expr = expression.Replace(" ", "")
        expr_len = Len(expr)
        If expr_len = 0 Then Return 0

        ' If we find + or - now, it is a unary operator.
        is_unary = True

        ' So far we have nothing.
        best_prec = Precedence.None

        ' Find the operator with the lowest precedence.
        ' Look for places where there are no open
        ' parentheses.
        For pos As Integer = 0 To expr_len - 1
            ' Examine the next character.
            ch = expr.Substring(pos, 1)

            ' Assume we will not find an operator. In
            ' that case, the next operator will not
            ' be unary.
            next_unary = False

            If ch = " " Then
                ' Just skip spaces. We keep them here
                ' to make the error messages easier to
            ElseIf ch = "(" Then
                ' Increase the open parentheses count.
                parens += 1

                ' A + or - after "(" is unary.
                next_unary = True
            ElseIf ch = ")" Then
                ' Decrease the open parentheses count.
                parens -= 1

                ' An operator after ")" is not unary.
                next_unary = False

                ' If parens < 0, too many ')'s.
                If parens < 0 Then
                    Throw New FormatException( _
                        "Too many close parentheses in '" & _
                        expression & "'")
                End If
            ElseIf parens = 0 Then
                ' See if this is an operator.
                If ch = "^" Or ch = "*" Or _
                   ch = "/" Or ch = "\" Or _
                   ch = "%" Or ch = "+" Or _
                   ch = "-" _
                Then
                    ' An operator after an operator
                    ' is unary.
                    next_unary = True

                    ' See if this operator has higher
                    ' precedence than the current one.
                    Select Case ch
                        Case "^"
                            If best_prec >= Precedence.Power Then
                                best_prec = Precedence.Power
                                best_pos = pos
                            End If

                        Case "*", "/"
                            If best_prec >= Precedence.Times Then
                                best_prec = Precedence.Times
                                best_pos = pos
                            End If

                        Case "\"
                            If best_prec >= Precedence.IntDiv Then
                                best_prec = Precedence.IntDiv
                                best_pos = pos
                            End If

                        Case "%"
                            If best_prec >= Precedence.Modulus Then
                                best_prec = Precedence.Modulus
                                best_pos = pos
                            End If

                        Case "+", "-"
                            ' Ignore unary operators
                            ' for now.
                            If (Not is_unary) And _
                                best_prec >= Precedence.Plus _
                            Then
                                best_prec = Precedence.Plus
                                best_pos = pos
                            End If
                    End Select
                End If
            End If
            is_unary = next_unary
        Next pos

        ' If the parentheses count is not zero,
        ' there's a ')' missing.
        If parens <> 0 Then
            Throw New FormatException( _
                "Missing close parenthesis in '" & _
                expression & "'")
        End If

        ' Hopefully we have the operator.
        If best_prec < Precedence.None Then
            lexpr = expr.Substring(0, best_pos)
            rexpr = expr.Substring(best_pos + 1)
            Select Case expr.Substring(best_pos, 1)
                Case "^"
                    Return _
                        EvaluateExpression(lexpr) ^ _
                        EvaluateExpression(rexpr)
                Case "*"
                    Return _
                        EvaluateExpression(lexpr) * _
                        EvaluateExpression(rexpr)
                Case "/"
                    Return _
                        EvaluateExpression(lexpr) / _
                        EvaluateExpression(rexpr)
                Case "\"
                    Return _
                        CLng(EvaluateExpression(lexpr)) \ _
                        CLng(EvaluateExpression(rexpr))
                Case "%"
                    Return _
                        EvaluateExpression(lexpr) Mod _
                        EvaluateExpression(rexpr)
                Case "+"
                    Return _
                        EvaluateExpression(lexpr) + _
                        EvaluateExpression(rexpr)
                Case "-"
                    Return _
                        EvaluateExpression(lexpr) - _
                        EvaluateExpression(rexpr)
            End Select
        End If

        ' If we do not yet have an operator, there
        ' are several possibilities:
        '
        ' 1. expr is (expr2) for some expr2.
        ' 2. expr is -expr2 or +expr2 for some expr2.
        ' 3. expr is Fun(expr2) for a function Fun.
        ' 4. expr is a primitive.
        ' 5. It's a literal like "3.14159".

        ' Look for (expr2).
        If expr.StartsWith("(") And expr.EndsWith(")") Then
            ' Remove the parentheses.
            Return EvaluateExpression(expr.Substring(1, expr_len - 2))
            Exit Function
        End If

        ' Look for -expr2.
        If expr.StartsWith("-") Then
            Return -EvaluateExpression(expr.Substring(1))
        End If

        ' Look for +expr2.
        If expr.StartsWith("+") Then
            Return EvaluateExpression(expr.Substring(2))
        End If

        ' Look for Fun(expr2).
        If expr_len > 5 And expr.EndsWith(")") Then
            ' Find the first (.
            Dim paren_pos As Integer = expr.IndexOf("(")
            If paren_pos > 0 Then
                ' See what the function is.
                lexpr = expr.Substring(0, paren_pos)
                rexpr = expr.Substring(paren_pos + 1, expr_len - paren_pos - 2)
                Select Case lexpr.ToLower
                    Case "sin"
                        Return Math.Sin(EvaluateExpression(rexpr))
                    Case "cos"
                        Return Math.Cos(EvaluateExpression(rexpr))
                    Case "tan"
                        Return Math.Tan(EvaluateExpression(rexpr))
                    Case "sqrt"
                        Return Math.Sqrt(EvaluateExpression(rexpr))
                    Case "factorial"
                        Return Factorial(EvaluateExpression(rexpr))
                    Case "sign"
                        Return Math.Sign(EvaluateExpression(rexpr))
                End Select
            End If
        End If

        '' See if it's a primitive.
        'If m_Primatives.Contains(expr) Then
        '    ' Return the corresponding value,
        '    ' converted into a Double.
        '    Try
        '        ' Try to convert the expression into a value.
        '        Dim value As Double = _
        '            Double.Parse(m_Primatives.Item(expr).ToString)
        '        Return value
        '    Catch ex As Exception
        '        Throw New FormatException( _
        '            "Primative '" & expr & _
        '            "' has value '" & _
        '            m_Primatives.Item(expr).ToString & _
        '            "' which is not a Double.")
        '    End Try
        'End If

        ' It must be a literal like "2.71828".
        Try
            If TreapVar(expr) Then
                Return CalculateVar(expr)
            End If
            ' Try to convert the expression into a Double.
            Dim value As Double = Double.Parse(expr)
            Return value
        Catch ex As Exception
            Throw New FormatException( _
                "Error evaluating '" & expression & _
                "' as a constant.")
        End Try
    End Function



    Public pTreapVar As New Treap(Of C_Var)

    Public Function TreapVar(ByVal strName As String) As Boolean
        Dim pVar As C_Var = pTreapVar.find(strName)
        If pVar Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function CalculateVar(ByVal strName As String) As Double
        Dim pVar As C_Var = pTreapVar.find(strName)
        If pVar Is Nothing Then
            Return 0
        Else
            Dim pMath As C_Math = New C_Math
            Return pMath.EvaluateExpression(pVar.pValue)
            'Dim p As Evaluator = New Evaluator(pVar.pValue)
            'p.pTreapVar = pTreapVar
            'Return p.Calculate()
        End If
    End Function


    ' Return the factorial of the expression.
    Public Shared Function Factorial(ByVal value As Double) As Double
        Dim result As Double

        ' Make sure the value is an integer.
        If CLng(value) <> value Then
            Throw New ArgumentException( _
                "Parameter to Factorial function must be an integer in Factorial(" & _
                Format$(value) & ")")
        End If

        result = 1
        Do While value > 1
            result = result * value
            value = value - 1
        Loop
        Return result
    End Function
End Class
