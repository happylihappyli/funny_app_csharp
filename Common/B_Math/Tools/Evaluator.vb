Imports System
Imports System.Collections

Imports B_TreapVB.TreapVB
Imports B_Data.Funny
Imports B_Data

Namespace FunnyAI

    ''' <summary>
    ''' 表达式计算类:
    '''    表达式示例: 2+3*EXP(3.3*X, 5)/3-2*(3+2/2*X)/2-3
    ''' 说明:
    '''    为简化算法,表达式中无正、负号,要得到负号的作用请用 (1-2)*? 来表示
    '''    表达式中的元素(现区分大小写)有:
    '''    数值常数(包括小数点): 如: 3.2 8.3
    '''    运算符: 如: + - * / ^
    '''    括号: ( )
    '''    变量: 运算时将实际值代入其中,用一单词(字符串,原则上不应包括数字)来表示, 赋值时用类的索引器表示 如: X Y Delta
    '''    用户自定义函数: 函数的操作要用括号()括起来,里面的表达式用递归来求 如: EXP(2+3*X, 2), 但是函数里的表达式不能再有函数(即用户自定义函数不允许递归)
    '''    分隔符: 空格(不会放入符号栈中) 逗号(用于用户自定义函数中,分隔各参数,要放入栈中), 各运算符默认已是分隔符
    '''   .算法中要用到的:
    '''    单词解析函数: 将表达式中的各元素解析放到一字符串数组中(其中的每一字符串表示一个元素)
    '''    单词表(数组): 存放单词解析函数解析的元素
    '''    符号栈: 运算时存放符号
    '''    数值栈: 运算时存放数值(及中间值)
    '''    解析函数: 核心,对表达式作运算并求出最终结果
    ''' </summary>

    Public Class Evaluator
#Region "表达式计算需要的一些类"
        ''' <summary>
        ''' 数值栈
        ''' </summary>
        Public Class ValStack
            Private stack As New Stack()
            ''' <summary>
            ''' 弹出栈顶值
            ''' </summary>
            ''' <returns>栈为空则抛异常</returns>
            Public Function Pop() As Double
                If stack.Count = 0 Then
                    Return 0
                Else
                    Return CDbl(stack.Pop())
                End If
            End Function
            ''' <summary>
            ''' 向栈中压入值
            ''' </summary>
            ''' <param name="val"></param>
            Public Sub Push(ByVal val As Double)
                stack.Push(val)
            End Sub
            ''' <summary>
            ''' 取栈顶值,但不弹出
            ''' </summary>
            Public Function Peek() As Double
                If stack.Count = 0 Then
                    Return 0
                Else
                    Return CDbl(stack.Peek())
                End If
            End Function

            ''' <summary>
            ''' 栈中数值个数
            ''' </summary>
            Public ReadOnly Property Count() As Integer
                Get
                    Return stack.Count
                End Get
            End Property
            ''' <summary>
            ''' 清空栈
            ''' </summary>
            Public Sub Clear()
                stack.Clear()
            End Sub
        End Class

        ''' <summary>
        ''' 符号栈
        ''' </summary>
        Public Class SignStack
            Private stack As New Stack()
            ''' <summary>
            ''' 弹出栈顶值
            ''' </summary>
            ''' <returns>栈为空则抛异常</returns>
            Public Function Pop() As String
                If stack.Count = 0 Then
                    Return ""
                Else
                    Return DirectCast(stack.Pop(), String)
                End If
            End Function
            ''' <summary>
            ''' 向栈中压入值
            ''' </summary>
            ''' <param name="val"></param>
            Public Sub Push(ByVal val As String)
                stack.Push(val)
            End Sub
            ''' <summary>
            ''' 取栈顶值,但不弹出
            ''' </summary>
            ''' <returns></returns>
            Public Function Peek() As String
                If stack.Count = 0 Then
                    Return ""
                Else
                    Return DirectCast(stack.Peek(), String)
                End If
            End Function
            ''' <summary>
            ''' 栈中字符串个数
            ''' </summary>
            Public ReadOnly Property Count() As Integer
                Get
                    Return stack.Count
                End Get
            End Property
            ''' <summary>
            ''' 清空栈
            ''' </summary>
            Public Sub Clear()
                stack.Clear()
            End Sub
        End Class
        ''' <summary>
        ''' 单词解析
        ''' </summary>
        Public Class WordAnalysis
            ''' <summary>
            ''' 默认分隔符(有分隔单词功能的符号,因此包括一些运算符), 用户可以自定义这些分隔符
            ''' </summary>
            Public Shared Seperator As String() = New String() { _
                        "+", "-", "*", "/", "^", ",", "(", ")", "ABC"}
            ''' <summary>
            ''' 指定字符是否为分隔符
            ''' </summary>
            ''' <param name="s">字符</param>
            ''' <returns></returns>
            Public Shared Function IsSeperator(ByVal s As String) As Boolean
                For i As Integer = 0 To Seperator.Length - 1
                    If s = Seperator(i) Then
                        Return True
                    End If
                Next
                Return False
            End Function

            ''' <summary>
            ''' 单词分析
            ''' 空格, TAB, 回车 一定是分隔符
            ''' 如果分隔符由多个字符组成, 则分隔符的第一个字符前要有空格,TAB或回车符
            ''' </summary>
            ''' <param name="str">字符串</param>
            ''' <returns></returns>
            Public Shared Function Analysis(ByVal str As String) As String()
                If str Is Nothing OrElse str = "" Then
                    Throw New Exception("表达式不能为NULL 或 空 ")
                End If
                Dim list As New ArrayList()
                Dim cur As String = ""
                For i As Integer = 0 To str.Length - 1
                    '循环扫描字符串中的每个字符
                    Dim t As String = str(i).ToString()
                    If t = " " OrElse t = vbTab OrElse t = vbLf Then
                        If cur <> "" Then
                            list.Add(cur)
                            cur = ""
                        End If
                    ElseIf IsSeperator(t) Then
                        If cur <> "" Then
                            list.Add(cur)
                            cur = ""
                        End If
                        list.Add(t)
                    Else
                        cur += t
                        If IsSeperator(cur.Trim()) Then
                            list.Add(cur)
                            cur = ""
                        End If
                    End If
                Next
                If cur <> "" Then
                    list.Add(cur)
                End If
                Dim ret As String() = New String(list.Count - 1) {}
                For i As Integer = 0 To list.Count - 1
                    ret(i) = DirectCast(list(i), String)
                Next
                Return ret
            End Function
        End Class

        ''' <summary>
        ''' 变量操作结构
        ''' </summary>
        Public Structure Variable
            Private TKey As String()
            Private TValue As Double()
            Private index As Integer
            '已分配的变量个数
            ''' <summary>
            ''' 初始化
            ''' </summary>
            ''' <param name="size">允许的最大变量个数变量</param>
            Public Sub New(ByVal size As Integer)
                TKey = New String(size - 1) {}
                TValue = New Double(size - 1) {}
                For i As Integer = 0 To TKey.Length - 1
                    TKey(i) = ""
                    TValue(i) = 0
                Next
                index = 0
            End Sub

            ''' <summary>
            ''' 获取或设置指定变量的值
            ''' </summary>
            Default Public Property Item(ByVal key As String) As Double
                Get
                    Dim idx As Integer = IndexOf(key)
                    If idx = -1 Then
                        '      throw new Exception("变量"+key+"在数组中不存在");
                        Return 0
                        '考虑到算法的稳定性, 不抛异常
                    Else
                        Return TValue(idx)
                    End If
                End Get
                Set(ByVal value As Double)
                    Dim idx As Integer = IndexOf(key)
                    If idx = -1 Then
                        If index < Me.Length Then
                            '还有容量
                            TKey(index) = key
                            TValue(index) = value
                            index += 1
                        Else
                            Throw New Exception("新增变量出错, 变量数组已满")
                        End If
                        '现暂不抛异常
                    Else
                        TValue(idx) = value
                    End If
                End Set
            End Property


            ''' <summary>
            ''' 返回变量在数组中的索引(位置)
            ''' </summary>
            ''' <param name="key__1">变量名</param>
            ''' <returns>不存在返回-1, 否则返回>=0的整数</returns>
            ''' <remarks></remarks>
            Public Function IndexOf(ByVal key__1 As String) As Integer
                If key__1 Is Nothing OrElse key__1 = "" Then
                    Return -1
                Else
                    For i As Integer = 0 To index - 1
                        If TKey(i) = key__1 Then
                            Return i
                        End If
                    Next
                    Return -1
                End If
            End Function

            ''' <summary>
            ''' 返回已声明变量个数
            ''' </summary>
            Public ReadOnly Property Count() As Integer
                Get
                    Return index
                End Get
            End Property

            ''' <summary>
            ''' 变量数组最大容量
            ''' </summary>
            Public ReadOnly Property Length() As Integer
                Get
                    Return TKey.Length
                End Get
            End Property
        End Structure

        ''' <summary>
        ''' 用户自定义函数代理
        ''' </summary>
        Public Delegate Function MyFunction(ByRef param() As Double) As Double
        '   string functionName, //自定义函数在表达式中的别名
        '   Variable var // 参数所在变量结构数组
        '函数计算所需参数
        ''' <summary>
        ''' 用户自定义函数列表
        ''' </summary>
        Public Structure FunctionList
            Private TFunc As MyFunction()
            '函数执行体的代理
            Private TFuncName As String()
            '函数名
            Private ParamCount As Integer()
            '参数个数
            Private _Count As Integer
            '列表中已使用的代理的个数
            ''' <summary>
            ''' 获取自定义函数个数
            ''' </summary>
            Public ReadOnly Property Count() As Integer
                Get
                    Return _Count
                End Get
            End Property
            ''' <summary>
            '''  获取允许的自定义函数最大个数
            ''' </summary>
            Public ReadOnly Property Length() As Integer
                Get
                    Return TFuncName.Length
                End Get
            End Property
            ''' <summary>
            ''' 初始化
            ''' </summary>
            ''' <param name="size">自定义函数最大个数</param>
            Public Sub New(ByVal size As Integer)
                TFunc = New MyFunction(size - 1) {}
                TFuncName = New String(size - 1) {}
                ParamCount = New Integer(size - 1) {}
                For i As Integer = 0 To TFuncName.Length - 1
                    TFunc(i) = Nothing
                    TFuncName(i) = ""
                    ParamCount(i) = 0
                Next
                _Count = 0
            End Sub

            ''' <summary>
            ''' 获取指定函数别名在列表中的索引(位置)
            ''' </summary>
            ''' <param name="funcName__1">函数别名</param>
            ''' <returns>不存在返回-1, 否则返回>=0的整数</returns>
            Public Function IndexOf(ByVal funcName__1 As String) As Integer
                If funcName__1 Is Nothing OrElse funcName__1 = "" Then
                    Return -1
                End If
                For i As Integer = 0 To _Count - 1
                    If TFuncName(i) = funcName__1 Then
                        Return i
                    End If
                Next
                Return -1
            End Function

            ''' <summary>
            ''' 设置指定函数别名的执行体代理
            ''' </summary>
            ''' <param name="funcName__1">表达式中对应的函数名</param>
            ''' <param name="paramCount__2">参数个数</param>
            ''' <param name="func__3">函数体</param>
            Public Sub Add(ByVal funcName__1 As String, ByVal paramCount__2 As Integer, ByVal func__3 As MyFunction)
                funcName__1 = funcName__1.ToLower
                Dim index As Integer = IndexOf(funcName__1)
                If index = -1 Then
                    If _Count < Me.Length Then
                        TFuncName(_Count) = funcName__1
                        TFunc(_Count) = func__3
                        ParamCount(_Count) = paramCount__2
                        _Count += 1
                    Else
                        Throw New Exception("添加函数出错, 列表已满")
                    End If
                Else
                    TFunc(index) = func__3
                    ParamCount(index) = paramCount__2
                End If
            End Sub
            ''' <summary>
            ''' 获取指定函数的执行体代理
            ''' </summary>
            Default Public ReadOnly Property Item(ByVal func As String) As MyFunction
                Get
                    Dim index As Integer = IndexOf(func)
                    If index = -1 Then
                        Return Nothing
                    Else
                        Return TFunc(index)
                    End If
                End Get
            End Property
            ''' <summary>
            ''' 获取自定义函数的参数个数
            ''' </summary>
            ''' <param name="func">函数名</param>
            ''' <returns></returns>
            Public Function ParameterCount(ByVal func As String) As Integer
                Dim index As Integer = IndexOf(func)
                If index = -1 Then
                    Return -1
                Else
                    Return ParamCount(index)
                End If
            End Function
        End Structure
#End Region

        Public Expression As String
        '要计算的表在式
        Private Word As String()
        Private Val As ValStack
        Private Sign As SignStack
        ''' <summary>
        ''' 变量列表
        ''' </summary>
        Public Var As Variable
        ''' <summary>
        ''' 函数执行体列表
        ''' </summary>
        Public Func As FunctionList
        ''' <summary>
        ''' 以本实例表达式作为自定义函数
        ''' </summary>
        Public DefFunction As MyFunction

        ''' <summary>
        ''' 初始化
        ''' </summary>
        ''' <param name="strExpression">要计算的表达式</param>
        Public Sub New(ByVal strExpression As String)
            Val = New ValStack()
            Sign = New SignStack()
            Var = New Variable(20)
            Func = New FunctionList(50)
            FunctionLib.AssignFunctionLib(Func)

            DefFunction = New MyFunction(AddressOf Me.TFunction)

            Me.Expression = strExpression.ToLower
        End Sub

        Public Sub New()
            Me.New("")
        End Sub

        ''' <summary>
        ''' 获取表达式中的变量
        ''' </summary>
        ''' <returns></returns>
        Public Function GetVariables() As String()
            Dim wordList As New ArrayList()
            Dim words As String() = WordAnalysis.Analysis(Expression)
            For i As Integer = 0 To words.Length - 1
                If Char.IsLetter(words(i)(0)) AndAlso Not IsFunc(words(i)) Then
                    If wordList.Contains(words(i)) = False Then
                        wordList.Add(words(i))
                    End If
                End If
            Next
            Dim vars As String() = New String(wordList.Count - 1) {}
            For i As Integer = 0 To vars.Length - 1
                vars(i) = DirectCast(wordList(i), String)
            Next
            Return vars
        End Function
        ''' <summary>
        ''' 对每次更改计算表达式时进行类初始化
        ''' </summary>
        Private Sub InitExpression()
            Word = WordAnalysis.Analysis(Expression)
        End Sub

        '''' <summary>
        '''' DEBGU...???....delegate
        '''' </summary>
        '''' <param name="pa"></param>
        '''' <returns></returns>
        Public Function TFunction(ByRef pa() As Double) As Double
            Return Me.Calculate()
        End Function

        Private Function Priority(ByVal sign As String) As Integer
            '符号优先级
            If sign = "(" Then
                Return 32767
            ElseIf sign = ")" Then
                Return -32767
            ElseIf sign = "^" Then
                Return 16
            ElseIf sign = "*" OrElse sign = "/" Then
                Return 8
            ElseIf sign = "+" OrElse sign = "-" Then
                Return 4
            ElseIf sign = "," Then
                Return 2
            Else
                If IsFunc(sign) Then
                    Return 32
                Else
                    Return 0
                End If
            End If
        End Function

        Private Function IsNum(ByVal word As String) As Boolean
            '是数字
            For i As Integer = 0 To word.Length - 1
                If word(i) <> "."c AndAlso "0123456789".IndexOf(word(i)) = -1 Then
                    Return False
                End If
            Next
            Return True
        End Function

        Private Function IsVar(ByVal word As String) As Boolean
            '是变量
            Return Var.IndexOf(word) <> -1
        End Function

        Private Function IsFunc(ByVal word As String) As Boolean
            '是自定义函数
            Return Func.IndexOf(word) <> -1
        End Function

        Private Function IsSign(ByVal word As String) As Boolean
            '是符号
            Dim sign As String() = WordAnalysis.Seperator
            If word.Trim() = "" Then
                Return False
            End If
            For i As Integer = 0 To sign.Length - 1
                If word = sign(i) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Function IsOtherWord(ByVal word As String) As Boolean
            '不是数字, 变量, 函数名, 符号 的单词
            Return Not (IsNum(word) OrElse IsVar(word) OrElse IsFunc(word) OrElse IsSign(word))
        End Function
        ''' <summary>
        ''' 计算sign(+-*/等)的a,b值
        ''' </summary>
        ''' <param name="sign">符号</param>
        ''' <param name="a">每一个值(符号左边的值)</param>
        ''' <param name="b">每二个值(符号右边的值)</param>
        ''' <returns></returns>
        Private Function Compute(ByVal sign As String, ByVal a As Double, ByVal b As Double) As Double
            Dim ret As Double = 0
            Select Case sign
                Case "^"
                    ret = a ^ b
                Case "+"
                    ret = a + b
                Case "-"
                    ret = a - b
                Case "*"
                    ret = a * b
                    '可能会产生溢出错
                Case "/"
                    ret = a / b
                    '注意,a为0会产生0除错异常, 此处不处理
                Case Else
                    If IsFunc(sign) Then
                        Dim parameter As Double() = New Double(Func.ParameterCount(sign) - 1) {}
                        For i As Integer = parameter.Length - 1 To 0 Step -1
                            parameter(i) = Val.Pop()
                        Next
                        Dim func__1 As MyFunction = Func(sign)
                        ret = func__1(parameter)
                    Else
                        Throw New Exception("无法识别的符号: " & sign & vbCrLf & "表达式为：" & Me.Expression)
                    End If
            End Select
            Return ret
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
                Dim p As Evaluator = New Evaluator(pVar.pValue)
                p.pTreapVar = pTreapVar
                Return p.Calculate()
            End If
        End Function

        ''' <summary>
        ''' 计算表达式的值
        ''' UNDONE: 用户自定义函数有错...
        ''' </summary>
        ''' <returns></returns>
        Public Function Calculate() As Double
            InitExpression()

            Dim cur As Integer
            '当前计算到的单词在Word数组中的索引(位置)
            For cur = 0 To Word.Length - 1
                '循环读单词,计算表达式.
                If IsNum(Word(cur)) Then
                    Val.Push(Convert.ToSingle(Word(cur)))
                ElseIf TreapVar(Word(cur)) Then
                    Val.Push(CalculateVar(Word(cur)))
                ElseIf Word(cur) = "(" Then
                    Sign.Push(Word(cur))
                ElseIf Word(cur) = "," Then
                ElseIf IsFunc(Word(cur)) Then
                    Sign.Push(Word(cur))
                ElseIf IsVar(Word(cur)) Then
                    Val.Push(Var(Word(cur)))
                ElseIf IsSign(Word(cur)) Then
                    '可能是以下单词: "+-*/,)"
                    While Priority(Sign.Peek()) >= Priority(Word(cur))
                        If Sign.Peek() = "(" Then
                            If Word(cur) = ")" Then
                                Sign.Pop()
                            End If
                            Exit While
                        ElseIf IsFunc(Sign.Peek()) Then
                            '计算自定义函数
                            '       double[] parameter=new double[Func.ParameterCount(Sign.Peek())];
                            '       for(int i=parameter.Length-1;i>=0;i--)
                            '       {
                            '        parameter[i]=Val.Pop();
                            '       }
                            Val.Push(Compute(Sign.Peek(), 0, 0))
                        Else
                            '只计算有 "+-*/" 的情况
                            Dim b As Double = Val.Pop(), a As Double = Val.Pop()
                            Val.Push(Compute(Sign.Peek(), a, b))
                        End If
                        Sign.Pop()
                        '将当前栈顶已计算过的符号舍弃
                    End While
                    If Word(cur) <> ")" AndAlso Word(cur) <> "," Then
                        Sign.Push(Word(cur))
                    End If
                Else
                    Throw New Exception("表达式包含非法字符: " & Word(cur))
                End If
            Next
            While Sign.Count > 0
                '还有未计算完的符号,剩下的符号一定是栈底的优先级最低, 栈顶的最高
                Dim curSign As String = Sign.Pop()
                If curSign = "(" OrElse curSign = "," Then
                    Continue While
                ElseIf IsFunc(curSign) Then
                    Val.Push(Compute(curSign, 0, 0))
                ElseIf IsSign(curSign) Then
                    Dim b As Double = Val.Pop(), a As Double = Val.Pop()
                    Val.Push(Compute(curSign, a, b))
                End If
            End While
            Return Val.Pop()
            '最终数值栈只剩一个数值,这就是最终结果.
        End Function
    End Class

    Public Class FunctionLib
        Private Sub New()
        End Sub
        ''' <summary>
        ''' 表达式默认支持的函数
        ''' </summary>
        ''' <param name="list"></param>
        Public Shared Sub AssignFunctionLib(ByRef list As Evaluator.FunctionList)
            ' 不加ref 修饰 无法使得修改后的list被返回
            list.Add("Pow", 2, New Evaluator.MyFunction(AddressOf Pow))
            list.Add("Exp", 1, New Evaluator.MyFunction(AddressOf Exp))
            list.Add("Sqrt", 1, New Evaluator.MyFunction(AddressOf Sqrt))
            list.Add("Log", 2, New Evaluator.MyFunction(AddressOf Log))
            list.Add("Abs", 1, New Evaluator.MyFunction(AddressOf Abs))
            list.Add("Max", 2, New Evaluator.MyFunction(AddressOf Max))
            list.Add("Min", 2, New Evaluator.MyFunction(AddressOf Min))

            list.Add("CInt", 1, New Evaluator.MyFunction(AddressOf MyCInt))

            list.Add("Sign", 1, New Evaluator.MyFunction(AddressOf Sign))
            list.Add("Sin", 1, New Evaluator.MyFunction(AddressOf Sin))
            list.Add("Cos", 1, New Evaluator.MyFunction(AddressOf Cos))
            list.Add("Rnd", 0, New Evaluator.MyFunction(AddressOf MyRnd))
        End Sub

        Public Shared Function Sign(ByRef p As Double()) As Double
            Return Math.Sign(p(0))
        End Function

        Public Shared Function MyCInt(ByRef p As Double()) As Double
            Return CInt(p(0))
        End Function

        Public Shared Function MyRnd(ByRef p As Double()) As Double
            Randomize()
            Return Rnd()
        End Function

        Public Shared Function Sin(ByRef p As Double()) As Double
            Return Math.Sin(p(0))
        End Function

        Public Shared Function Cos(ByRef p As Double()) As Double
            Return Math.Cos(p(0))
        End Function

        Public Shared Function Pow(ByRef p As Double()) As Double
            Return Math.Pow(p(0), p(1))
        End Function

        Public Shared Function Sqrt(ByRef p As Double()) As Double
            Return Math.Sqrt(p(0))
        End Function

        Public Shared Function Exp(ByRef p As Double()) As Double
            Return Math.Exp(p(0))
        End Function

        Public Shared Function Log(ByRef p As Double()) As Double
            Return Math.Log(p(0), p(1))
        End Function

        Public Shared Function Abs(ByRef p As Double()) As Double
            Return Math.Abs(p(0))
        End Function

        Public Shared Function Max(ByRef p As Double()) As Double
            Return Math.Max(p(0), p(1))
        End Function

        Public Shared Function Min(ByRef p As Double()) As Double
            Return Math.Min(p(0), p(1))
        End Function
    End Class
End Namespace
