Imports System.Xml
Imports System.Text
Imports System.Text.RegularExpressions

Namespace FunnyServer

    Public Class C_MathML

        Public Class MathML_Struct_Data
            Public Input As String           '输入
            Public Tag As String             '节点名称
            Public Atname As String
            Public Output As String          '输出
            Public Tex As String

            Public b_Acc As Boolean
            Public b_Invisible As Boolean            '是否可见

            Public Codes As String
            Public Type As Math_Type


            Public Sub New()
                Input = ""
                Tag = ""
                Atname = ""
                Output = ""
                Tex = ""
                b_Acc = False
                b_Invisible = False
                Codes = ""
                Type = Math_Type.None
            End Sub
        End Class


        Public MathML_Head As String = "m:"
        Public Math_Color As String = "blue"
        Public Math_Font As String = "serif"
        Public Math_Style As Boolean = True

        '自定义变量
        Dim pStruct As New MathML_Structure


        '系统全局变量
        Dim b_MI As Boolean = True
        Dim lngDepth As Integer       '递归调用层次




        Public Enum Math_Type
            None = -1
            CON = 0
            UNARY = 1
            BINARY = 2
            INFIX = 3
            LEFTBRACKET = 4
            RIGHTBRACKET = 5
            SPACE = 6
            UNDEROVER = 7
            DEFINITION = 8
        End Enum

        Public Sub New()
        End Sub


        '返回C_Node_Data结构
        Function Set_Node_Data(ByVal node As String, ByVal str As String) As C_Node_Data
            Dim TMP As C_Node_Data
            TMP = New C_Node_Data

            TMP.Node = node
            TMP.Str = str

            Return TMP
        End Function

        '把<XXX></XXX>替换成<m:XXX></m:XXX>
        Function Node_AddM(ByVal Str As String)
            Dim M2() As String = { _
            "math", "semantics", _
            "mstyle", "mrow", "mspace", "munder", _
            "msub", "msup", "mtable", "mtr", "mtd", "mn", _
            "mi", "mo", "mtext", "msqrt", "mroot", "mfrac"}       ', "munderover"
            Dim i As Integer
            For i = 0 To M2.Length - 1
                Str = Replace(Str, M2(i), MathML_Head + M2(i), , , CompareMethod.Text)
            Next

            Str = Replace(Str, "munderOVER", "munderover", , , CompareMethod.Text)

            Return Str
        End Function

        '主程序接口
        Public Function Run(ByVal Str As String) As String
            Dim regExpression As New Regex("\$(.*?)\$")
            Dim regMatchCollection As MatchCollection
            regMatchCollection = regExpression.Matches(Str)

            Dim i As Integer, Index As Long
            Dim Tmp As String
            Dim StrSplit() As String

            ReDim StrSplit(regMatchCollection.Count * 2)

            If regMatchCollection.Count > 0 Then


                StrSplit(0) = Left(Str, regMatchCollection.Item(0).Index)

                For i = 0 To regMatchCollection.Count - 2
                    StrSplit(i * 2 + 1) = Mid(Str, regMatchCollection.Item(i).Index + 2, regMatchCollection.Item(i).Length - 2)
                    Index = regMatchCollection.Item(i).Index + 1 + regMatchCollection.Item(i).Length
                    StrSplit(i * 2 + 2) = Mid(Str, Index, regMatchCollection.Item(i + 1).Index + 1 - Index)
                Next

                StrSplit(i * 2 + 1) = Mid(Str, regMatchCollection.Item(i).Index + 2, regMatchCollection.Item(i).Length - 2)
                Index = regMatchCollection.Item(i).Index + 1 + regMatchCollection.Item(i).Length
                StrSplit(i * 2 + 2) = Mid(Str, Index, Len(Str) + 1 - Index)


                For i = 0 To regMatchCollection.Count - 1
                    Tmp = StrSplit(i * 2 + 1)

                    If Tmp <> "" Then
                        Tmp = Replace(Tmp, "&gt;", ">", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "&ge;", ">=", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "&lt;", "<", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "&le;", "<=", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "&nbsp;", " ", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "<br>", " ", , , CompareMethod.Text)
                        Tmp = Replace(Tmp, "<br/>", " ", , , CompareMethod.Text)

                        If Len(Tmp) < 200 Then
                            '不要设置太长,否则会死循环
                            StrSplit(i * 2 + 1) = Parse_Start(Tmp)
                        End If
                    Else
                        StrSplit(i * 2 + 1) = "$"
                    End If
                Next

                Str = StrSplit(0)

                For i = 0 To regMatchCollection.Count - 1
                    Str += StrSplit(i * 2 + 1)
                    Str += StrSplit(i * 2 + 2)
                Next
            End If

            regExpression = Nothing
            regMatchCollection = Nothing

            Return Str
        End Function


        '生成<XXX>YYY</XXX>
        Function Create_XML_Node(ByVal name As String, ByVal frag As String) As String
            Return "<" + name + ">" + frag + "</" + name + ">"
        End Function

        '去除str开头的"\"和空格
        Function String_Filter_CharsAndBlanks(ByVal str As String, ByVal n As Integer) As String
            Dim st As String = ""
            If str.Length > n + 1 Then
                If str.Chars(n) = "\" And str.Chars(n + 1) <> "\" And str.Chars(n + 1) <> " " Then
                    st = str.Substring(n + 1)
                Else
                    st = str.Substring(n)
                End If
            Else
                st = str.Substring(n)
            End If

            Dim i As Integer = 0
            For i = 0 To st.Length - 1
                If st.Chars(i) <> " " Then
                    Exit For
                End If
            Next

            Return st.Substring(i)
        End Function

        '返回Str在数据中的第几行

        Function Get_Index(ByVal str As String, ByVal n As Integer) As Int32
            Dim i As Integer
            For i = 0 To pStruct.pMax
                If pStruct.Data(i, 0) = str Then
                    Return i
                End If
            Next

            Return n
        End Function

        '查找可处理的符号
        Function Get_Struct(ByVal str As String) As MathML_Struct_Data
            Dim pData As MathML_Struct_Data

            If str = "" Then
                pData = New MathML_Struct_Data
                GoTo MyExit
            End If

            Dim k, j, mk As Integer
            Dim st, tagst, match As String
            Dim more As Boolean = True

            '开始查找str开头中符合库中input的部分
            Dim i As Integer
            For i = 1 To str.Length
                st = str.Substring(0, i)
                j = k
                k = Get_Index(st, j)
                If str.Substring(0, i) = pStruct.pData(k).Input Then
                    match = pStruct.pData(k).Input
                    mk = k
                    i = match.Length
                End If
                more = str.Substring(0, i) = pStruct.pData(k).Input
            Next

            If match <> "" Then
                pData = pStruct.Get_Struct_Data(mk)
                GoTo MyExit
            End If
            '结束查找诺找到,已经返回,不会运行下面的程序

            '没找到则返回第一个符号,如果是数字则返回开头所有的数字
            k = 1
            st = str.Substring(0, 1)
            Dim pos As Boolean = True
            Dim integ As Boolean = True

            If st = "-" Then
                pos = False
                If k + 1 <= str.Length Then
                    st = str.Substring(k, 1)
                Else
                    st = ""
                End If
                k += 1
            End If

            While "0" <= st And st <= "9" And k <= str.Length
                If k + 1 <= str.Length Then
                    st = str.Substring(k, 1)
                Else
                    st = ""
                End If
                k += 1
            End While

            If st = "." And k + 1 <= str.Length Then
                integ = False
                st = str.Substring(k, 1)
                k += 1
                While "0" <= st And st <= "9" And k <= str.Length
                    If k + 1 <= str.Length Then
                        st = str.Substring(k, 1)
                    Else
                        st = ""
                    End If
                    k += 1
                End While
            End If

            If (pos And integ And k > 1) Or ((pos Or integ) And k > 2) Or k > 3 Then
                st = str.Substring(0, k - 1)
                tagst = "mn"
                b_MI = True
            Else
                k = 2
                st = str.Substring(0, 1)
                b_MI = ("A" > st Or st > "Z") And ("a" > st Or st > "z")
                If b_MI Then
                    tagst = "mo"
                Else
                    tagst = "mi"
                End If
                If str.Length > 1 Then
                    b_MI = b_MI Or str.Chars(1) < "a" Or str.Chars(1) > "z"
                End If
            End If

            pData = New MathML_Struct_Data
            pData.Input = str.Substring(0, k - 1)

            pData.Tag = tagst
            pData.Atname = ""
            pData.Output = st
            pData.Tex = ""
            pData.b_Acc = False
            pData.b_Invisible = False
            pData.Codes = ""

            pData.Type = 0

MyExit:

            Return pData
        End Function

        '去除括弧,如:把(XXX)变成XXX
        Function String_Remove_Brackets(ByVal str As String) As String
            Dim doc As New XmlDocument
            Dim docFrag As XmlDocumentFragment = doc.CreateDocumentFragment()
            docFrag.InnerXml = str

            Dim st_Left, st_Right As String

            If docFrag.FirstChild.Name = "mrow" Then
                st_Left = docFrag.FirstChild.FirstChild.InnerXml
                st_Right = docFrag.FirstChild.LastChild.InnerXml
                If (st_Left = "(" And st_Right = ")") Or (st_Left = "[" And st_Right = "]") Or (st_Left = "{" And st_Right = "}") Then
                    docFrag.FirstChild.RemoveChild(docFrag.FirstChild.FirstChild)
                    docFrag.FirstChild.RemoveChild(docFrag.FirstChild.LastChild)
                End If
            End If

            str = docFrag.InnerXml

            doc = Nothing
            docFrag = Nothing
            Return str
        End Function

        '处理一个节点
        Function Parse_Node(ByVal Str As String) As C_Node_Data
            Dim doc As New XmlDocument
            Dim docFrag As XmlDocumentFragment = doc.CreateDocumentFragment()

            Dim pNode_Data, pNode_Data2 As C_Node_Data
            Dim pStruct_Data As MathML_Struct_Data
            Dim node, st, newfrag As String
            Dim i As Integer

            Str = String_Filter_CharsAndBlanks(Str, 0)           '过滤空格等

            pStruct_Data = Get_Struct(Str)

            'Type为 空 或者 右括号 时,返回空
            If pStruct_Data.Type = Math_Type.None Or _
            pStruct_Data.Type = Math_Type.RIGHTBRACKET And lngDepth > 0 Then
                pNode_Data = Set_Node_Data("", Str)
                GoTo MyExit
            End If

            'Type为 自定义 时,替换成output再往下处理
            If pStruct_Data.Type = Math_Type.DEFINITION Then
                Str = pStruct_Data.Output + String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                pStruct_Data = Get_Struct(Str)
            End If


            Select Case pStruct_Data.Type
                Case Math_Type.UNDEROVER                  '帽子下面的东西 比如Limt 下面的线 cap() 等 
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    If pStruct_Data.Tag = "mn" And pStruct_Data.Output.Chars(0) = "-" Then
                        node = Create_XML_Node("mo", "-")
                        node += Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output.Substring(1))
                        node = Create_XML_Node("mrow", node)
                        pNode_Data = Set_Node_Data(node, Str)
                    Else
                        pNode_Data = Set_Node_Data(Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output), Str)
                    End If
                Case Math_Type.CON                 '把latex字符替换为MathML字符
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    If pStruct_Data.Tag = "mn" And pStruct_Data.Output.Chars(0) = "-" Then
                        node = Create_XML_Node("mo", "-")
                        node += Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output.Substring(1))
                        node = Create_XML_Node("mrow", node)
                        pNode_Data = Set_Node_Data(node, Str)
                    Else
                        pNode_Data = Set_Node_Data(Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output), Str)
                    End If
                Case Math_Type.LEFTBRACKET                '左括号处理
                    lngDepth += 1
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)

                    pNode_Data2 = Parse_Main(Str)                     '递归调用

                    If pStruct_Data.b_Invisible Then
                        '如果节点是不可见的
                        node = Create_XML_Node("mrow", pNode_Data2.Node)
                    Else
                        node = Create_XML_Node("mo", pStruct_Data.Output)
                        node += pNode_Data2.Node
                        node = Create_XML_Node("mrow", node)
                    End If
                    pNode_Data = Set_Node_Data(node, pNode_Data2.Str)

                Case Math_Type.UNARY                  ' 于下一个节点一起处理 如 bbb A
                    'input是text,mbox,""时的处理
                    If pStruct_Data.Input = "text" Or pStruct_Data.Input = "mbox" Or pStruct_Data.Input = """" Then
                        If pStruct_Data.Input <> """" Then
                            Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                        End If
                        If Str = "" Then
                            newfrag += Create_XML_Node(pStruct_Data.Tag, "")
                            pNode_Data = Set_Node_Data(Create_XML_Node("mrow", newfrag), Str)
                            GoTo MyExit
                        End If

                        Select Case Str.Chars(0)
                            Case "("
                                i = Str.IndexOf(")")
                            Case "["
                                i = Str.IndexOf("]")
                            Case "{"
                                i = Str.IndexOf("}")
                            Case Else
                                If pStruct_Data.Input = """" Then
                                    i = Str.Substring(1).IndexOf("""") + 1
                                Else
                                    i = 0
                                End If
                        End Select
                        If i = 0 Then
                            st = Str.Substring(0, Str.Length)
                        Else
                            st = Str.Substring(1, i - 1)
                        End If
                        If st = "" Then
                            newfrag += Create_XML_Node(pStruct_Data.Tag, "")
                            pNode_Data = Set_Node_Data(Create_XML_Node("mrow", newfrag), Str)
                            GoTo MyExit
                        End If
                        If st.Chars(0) = " " Then
                            node = "<" + "mspace width=""1ex""></" + "mspace>"
                            newfrag += node
                        End If
                        newfrag += Create_XML_Node(pStruct_Data.Tag, st)
                        If st.Chars(st.Length - 1) = " " Then
                            node = "<" + "mspace width=""1ex""></" + "mspace>"
                            newfrag += node
                        End If
                        Str = String_Filter_CharsAndBlanks(Str, i + 1)
                        pNode_Data = Set_Node_Data(Create_XML_Node("mrow", newfrag), Str)
                    Else
                        Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                        pNode_Data2 = Parse_Node(Str)
                        If pNode_Data2.Node = "" Then
                            pNode_Data = Set_Node_Data(Create_XML_Node("mo", pStruct_Data.Input), Str)
                            GoTo MyExit
                        End If
                        pNode_Data2.Node = String_Remove_Brackets(pNode_Data2.Node)
                        'input是sqrt时的处理
                        If pStruct_Data.Input = "sqrt" Then
                            pNode_Data = Set_Node_Data(Create_XML_Node(pStruct_Data.Tag, pNode_Data2.Node), pNode_Data2.Str)
                        Else
                            'Acc = "True"时处理
                            If pStruct_Data.b_Acc Then
                                node = pNode_Data2.Node + Create_XML_Node("mo", pStruct_Data.Output)
                                node = Create_XML_Node(pStruct_Data.Tag, node)
                                pNode_Data = Set_Node_Data(node, pNode_Data2.Str)
                                GoTo MyExit
                            End If

                            '有属性时处理
                            If pStruct_Data.Atname <> "" Then
                                Dim atname, atval As String, n As Integer
                                For n = 0 To pStruct_Data.Atname.Length - 1
                                    If pStruct_Data.Atname.Substring(n, 1) = "=" Then
                                        atname = pStruct_Data.Atname.Substring(0, n)
                                        atval = pStruct_Data.Atname.Substring(n + 1)
                                    End If
                                Next
                                node = "<" + pStruct_Data.Tag + " " + atname + "=""" + atval + """>" + pNode_Data2.Node + "</" + pStruct_Data.Tag + ">"
                            Else
                                '没有属性时处理
                                node = Create_XML_Node(pStruct_Data.Tag, pNode_Data2.Node)
                            End If
                            pNode_Data = Set_Node_Data(node, pNode_Data2.Str)
                        End If
                    End If
                Case Math_Type.BINARY                '与下两个节点一起处理 root 3 a
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    '第一个节点
                    pNode_Data2 = Parse_Node(Str)
                    If pNode_Data2.Node = "" Then
                        pNode_Data = Set_Node_Data(Create_XML_Node("mo", pStruct_Data.Input), Str)
                        GoTo MyExit
                    End If
                    pNode_Data2.Node = String_Remove_Brackets(pNode_Data2.Node)

                    Dim result2 As C_Node_Data
                    '第二个节点
                    result2 = Parse_Node(pNode_Data2.Str)
                    If result2.Node = "" Then
                        pNode_Data = Set_Node_Data(Create_XML_Node("mo", pStruct_Data.Input), Str)
                        GoTo MyExit
                    End If
                    result2.Node = String_Remove_Brackets(result2.Node)
                    '处理第一和第二个节点
                    If pStruct_Data.Input = "root" Or pStruct_Data.Input = "stackrel" Then
                        newfrag += result2.Node
                    End If
                    newfrag += pNode_Data2.Node
                    If pStruct_Data.Input = "frac" Then newfrag += result2.Node
                    pNode_Data = Set_Node_Data(Create_XML_Node(pStruct_Data.Tag, newfrag), result2.Str)
                Case Math_Type.INFIX                '上标下标
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    pNode_Data = Set_Node_Data(Create_XML_Node("mo", pStruct_Data.Output), Str)
                Case Math_Type.SPACE                  '空格符号处理
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    node = "<" + "mspace width=""1ex"">" + "" + "</" + "mspace>"
                    newfrag += node
                    newfrag += Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output)
                    node = "<" + "mspace width=""1ex"">" + "" + "</" + "mspace>"
                    newfrag += node
                    pNode_Data = Set_Node_Data(Create_XML_Node("mrow", newfrag), Str)
                Case Else               '例外处理
                    Str = String_Filter_CharsAndBlanks(Str, pStruct_Data.Input.Length)
                    pNode_Data = Set_Node_Data(Create_XML_Node(pStruct_Data.Tag, pStruct_Data.Output), Str)
            End Select

MyExit:

            doc = Nothing
            docFrag = Nothing


            Return pNode_Data
        End Function

        '主体程序
        Function Parse_Main(ByVal Str As String) As C_Node_Data
            Dim doc As New XmlDocument
            Dim docFrag As XmlDocumentFragment = doc.CreateDocumentFragment()

            Dim Symbol, Sym1, Sym2 As MathML_Struct_Data
            Dim StrNode As String = ""
            Dim Str_New_Frag As String = ""
            Dim Result As C_Node_Data
            Dim underover As Boolean
            Dim i As Integer



            '循环处理str,直到遇见右括号或者结束位置
            Do
                Str = String_Filter_CharsAndBlanks(Str, 0)
                Sym1 = Get_Struct(Str)
                Result = Parse_Node(Str)                '处理一个节点

                StrNode = Result.Node
                Str = Result.Str

                Symbol = Get_Struct(Str)
                '当遇到 ^ 或者 _ 时处理
                If Symbol.Type = Math_Type.INFIX Then
                    Str = String_Filter_CharsAndBlanks(Str, Symbol.Input.Length)
                    Result = Parse_Node(Str)
                    If Result.Node = "" Then
                        Result.Node = Create_XML_Node("mo", "&squ;")
                    Else
                        Result.Node = String_Remove_Brackets(Result.Node)
                    End If
                    Str = Result.Str
                    If Symbol.Input = "/" And Symbol.Output = "/" Then
                        StrNode = String_Remove_Brackets(StrNode)
                    End If
                    If Symbol.Input = "_" And Symbol.Output = "_" Then
                        Sym2 = Get_Struct(Str)
                        underover = (Sym1.Type = Math_Type.UNDEROVER)
                        If Sym2.Input = "^" And Sym2.Output = "^" Then
                            Str = String_Filter_CharsAndBlanks(Str, Sym2.Input.Length)
                            Dim res2 As C_Node_Data
                            res2 = Parse_Node(Str)
                            String_Remove_Brackets(res2.Node)
                            Str = res2.Str
                            StrNode += Result.Node
                            StrNode += res2.Node
                            If underover Then
                                StrNode = Create_XML_Node("munderover", StrNode)
                            Else
                                StrNode = Create_XML_Node("msubsup", StrNode)
                            End If
                            StrNode = Create_XML_Node("mrow", StrNode)
                        Else
                            StrNode += Result.Node
                            If underover Then
                                StrNode = Create_XML_Node("munder", StrNode)
                            Else
                                StrNode = Create_XML_Node("msub", StrNode)
                            End If
                        End If
                    Else
                        StrNode += Result.Node
                        StrNode = Create_XML_Node(Symbol.Tag, StrNode)
                    End If
                    Str_New_Frag += StrNode
                Else
                    If StrNode <> "" Then
                        Str_New_Frag += StrNode
                    End If
                End If

            Loop While (Symbol.Type <> Math_Type.RIGHTBRACKET Or lngDepth = 0) _
            And Symbol.Type <> Math_Type.None And Symbol.Output <> ""

            If Symbol.Type = Math_Type.RIGHTBRACKET Then
                '处理右扩号
                If lngDepth > 0 Then
                    lngDepth -= 1
                End If

                '开始处理矩阵
                '判断是否矩阵
                If Not Str_New_Frag Is Nothing Then
                    docFrag.InnerXml() = Str_New_Frag
                End If

                Dim len As Integer = docFrag.ChildNodes.Count

                If len > 1 Then
                    If (len > 0 And docFrag.ChildNodes(len - 1).Name = "mrow" And len > 1 And docFrag.ChildNodes(len - 2).Name = "mo" And docFrag.ChildNodes(len - 2).InnerXml = ",") Then

                        Dim right As String = docFrag.ChildNodes(len - 1).LastChild.InnerXml
                        If right = ")" Or right = "]" Then
                            Dim left As String = docFrag.ChildNodes(len - 1).FirstChild.InnerXml
                            If left = "(" And right = ")" And Symbol.Output <> "}" Or left = "[" And right = "]" Then
                                '具体判断是否矩阵
                                Dim pos As New ArrayList                                   'var pos = []
                                Dim pos_i() As Integer = {}
                                Dim matrix As Boolean = True
                                Dim m As Integer = docFrag.ChildNodes.Count
                                For i = 0 To m - 1 And matrix Step 2
                                    If i = 0 Then                                    'pos[i] = []
                                        pos.Add(pos_i)
                                    Else
                                        pos.Add(pos_i)
                                        ReDim Preserve pos(i - 1)(0)
                                        pos(i - 1)(0) = -1
                                        pos.Add(pos_i)
                                    End If
                                    docFrag.InnerXml() = Str_New_Frag
                                    StrNode = docFrag.ChildNodes(i).OuterXml
                                    docFrag.InnerXml = docFrag.ChildNodes(i).OuterXml
                                    If matrix Then
                                        ' And (i = (m - 1) Or docFrag.ChildNodes(0).ChildNodes(2).Name = "mo" And docFrag.ChildNodes(0).ChildNodes(2).InnerXml = ",")无意义
                                        matrix = (docFrag.ChildNodes(0).Name = "mrow" And docFrag.ChildNodes(0).FirstChild.InnerXml = left And docFrag.ChildNodes(0).LastChild.InnerXml = right)
                                    End If
                                    If matrix Then
                                        Dim j As Integer
                                        For j = 0 To docFrag.ChildNodes(0).ChildNodes.Count - 1
                                            If docFrag.ChildNodes(0).ChildNodes(j).InnerXml = "," Then
                                                ReDim Preserve pos(i)(pos(i).Length)
                                                pos(i)(pos(i).Length - 1) = j
                                            End If
                                        Next
                                    End If
                                    If matrix And i > 1 Then
                                        matrix = (pos(i).Length = pos(i - 2).Length)
                                    End If
                                Next
                                '如果是矩阵,则开始处理
                                If matrix Then
                                    Dim row, frag, table As String
                                    Dim n, k As Integer

                                    For i = 0 To m - 1 Step 2
                                        row = ""
                                        frag = ""

                                        docFrag.InnerXml() = Str_New_Frag
                                        StrNode = docFrag.FirstChild.InnerXml
                                        docFrag.InnerXml = docFrag.FirstChild.InnerXml
                                        n = docFrag.ChildNodes.Count
                                        k = 0
                                        docFrag.RemoveChild(docFrag.FirstChild)

                                        Dim j As Integer
                                        For j = 1 To n - 2
                                            If k < pos(i).Length Then
                                                If j = pos(i)(k) Then
                                                    row += Create_XML_Node("mtd", frag)
                                                    k += 1
                                                    docFrag.RemoveChild(docFrag.FirstChild)
                                                    frag = ""
                                                Else
                                                    frag += docFrag.FirstChild.OuterXml
                                                    docFrag.RemoveChild(docFrag.FirstChild)
                                                End If
                                            Else
                                                frag += docFrag.FirstChild.OuterXml
                                                docFrag.RemoveChild(docFrag.FirstChild)
                                            End If
                                        Next
                                        row += Create_XML_Node("mtd", frag)

                                        docFrag.InnerXml() = Str_New_Frag
                                        If docFrag.ChildNodes.Count > 2 Then
                                            docFrag.RemoveChild(docFrag.FirstChild)
                                            docFrag.RemoveChild(docFrag.FirstChild)
                                        End If
                                        Str_New_Frag = docFrag.InnerXml()

                                        table += Create_XML_Node("mtr", row)
                                    Next

                                    StrNode = Create_XML_Node("mtable", table)
                                    If Symbol.b_Invisible = True Then
                                        docFrag.InnerXml() = StrNode
                                        Dim attr As XmlAttribute = doc.CreateAttribute("columnalign")
                                        attr.Value = "Left"
                                        docFrag.FirstChild.Attributes.SetNamedItem(attr)
                                        StrNode = docFrag.InnerXml
                                    End If

                                    Str_New_Frag = StrNode
                                End If
                                '结束判断和处理矩阵
                            End If
                        End If
                    End If
                End If
                '结束处理矩阵

                Str = String_Filter_CharsAndBlanks(Str, Symbol.Input.Length)

                If Symbol.b_Invisible = False Then
                    StrNode = Create_XML_Node("mo", Symbol.Output)
                    Str_New_Frag += StrNode
                End If
            End If

            Return Set_Node_Data(Str_New_Frag, Str)
        End Function


        Function Parse_Start(ByVal Str As String) As String
            '开始分析处理

            Dim node, at, TMP As String

            If Math_Color <> "" Then at += " " + "mathcolor=""" + Math_Color + """"
            If Math_Style Then at += " " + "displaystyle=""true"""
            If Math_Font <> "" Then at += " " + "fontfamily=""" + Math_Font + """"

            lngDepth = 0

            TMP = Node_AddM(Parse_Main(Str.Replace("^\s+", "")).Node)            '调用处理主过程

            TMP = TMP.Replace("fontfa" + MathML_Head + "mily", "fontfamily")

            node = "<" + MathML_Head + "mstyle" + at + ">" + TMP + "</" + MathML_Head + "mstyle>"

            node = "<" + MathML_Head + "math>" + node + "</" + MathML_Head + "math>"

            If Math_Font <> "" Then
                node = "<font face=""" + Math_Font + """>" + node + "</font>"
            End If

            Return node
        End Function
    End Class


    Public Class MathML_Structure
        Public pMax As Long = 194
        Public pData(194) As C_MathML.MathML_Struct_Data

        Public Data As String(,) = { _
        {"ZZ", "mo", "", "&integers;", "", "", "", "", "0"}, _
        {"zeta", "mi", "", "&zeta;", "", "", "", "", "0"}, _
        {"xx", "mo", "", "&times;", "times", "", "", "", "0"}, _
        {"xi", "mi", "", "&xi;", "", "", "", "", "0"}, _
        {"vvv", "mo", "", "&xvee;", "bigvee", "", "", "", "7"}, _
        {"vv", "mo", "", "&or;", "vee", "", "", "", "0"}, _
        {"vec", "mover", "", "&rarr;", "", "True", "", "", "1"}, _
        {"vdots", "mo", "", "&vellip;", "", "", "", "", "0"}, _
        {"vartheta", "mi", "", "&vartheta;", "", "", "", "", "0"}, _
        {"varphi", "mi", "", "&varphi;", "", "", "", "", "0"}, _
        {"varepsilon", "mi", "", "&varepsilon;", "", "", "", "", "0"}, _
        {"uuu", "mo", "", "&xcup;", "bigcup", "", "", "", "7"}, _
        {"uu", "mo", "", "&cup;", "cup", "", "", "", "0"}, _
        {"upsilon", "mi", "", "&upsilon;", "", "", "", "", "0"}, _
        {"ul", "munder", "", "&UnderBar;", "underline", "True", "", "", "1"}, _
        {"uarr", "mo", "", "&uarr;", "uparrow", "", "", "", "0"}, _
        {"TT", "mo", "", "&top;", "top", "", "", "", "0"}, _
        {"tt", "mstyle", "fontfamily=monospace", "tt", "", "", "", "", "1"}, _
        {"to", "mo", "", "&rarr;", "to", "", "", "", "0"}, _
        {"Theta", "mo", "", "&Theta;", "", "", "", "", "0"}, _
        {"theta", "mi", "", "&theta;", "", "", "", "", "0"}, _
        {"text", "mtext", "", "text", "", "", "", "", "1"}, _
        {"tau", "mi", "", "&tau;", "", "", "", "", "0"}, _
        {"tanh", "mo", "", "tanh", "", "", "", "", "0"}, _
        {"tan", "mo", "", "tan", "", "", "", "", "0"}, _
        {"supe", "mo", "", "&supE;", "supseteq", "", "", "", "0"}, _
        {"sup", "mo", "", "&sup;", "supset", "", "", "", "0"}, _
        {"sum", "mo", "", "&sum;", "", "", "", "", "7"}, _
        {"sube", "mo", "", "&subE;", "subseteq", "", "", "", "0"}, _
        {"sub", "mo", "", "&sub;", "subset", "", "", "", "0"}, _
        {"stackrel", "mover", "", "stackrel", "", "", "", "", "2"}, _
        {"square", "mo", "", "&squ;", "", "", "", "", "0"}, _
        {"sqrt", "msqrt", "", "sqrt", "", "", "", "", "1"}, _
        {"sinh", "mo", "", "sinh", "", "", "", "", "0"}, _
        {"sin", "mo", "", "sin", "", "", "", "", "0"}, _
        {"Sigma", "mo", "", "&Sigma;", "", "", "", "", "0"}, _
        {"sigma", "mi", "", "&sigma;", "", "", "", "", "0"}, _
        {"sf", "mstyle", "fontfamily=sans-serif", "sf", "", "", "", "", "1"}, _
        {"setminus", "mo", "", "\\", "", "", "", "", "0"}, _
        {"sec", "mo", "", "sec", "", "", "", "", "0"}, _
        {"RR", "mo", "", "&reals;", "", "", "", "", "0"}, _
        {"root", "mroot", "", "root", "", "", "", "", "2"}, _
        {"rho", "mi", "", "&rho;", "", "", "", "", "0"}, _
        {"rArr", "mo", "", "&harr;", "Rightarrow", "", "", "", "0"}, _
        {"rarr", "mo", "", "&rarr;", "rightarrow", "", "", "", "0"}, _
        {"quad", "mo", "", "&nbsp;&nbsp;", "", "", "", "", "0"}, _
        {"qquad", "mo", "", "&nbsp;&nbsp;&nbsp;&nbsp;", "", "", "", "", "0"}, _
        {"QQ", "mo", "", "&rationals;", "", "", "", "", "0"}, _
        {"psi", "mi", "", "&psi;", "", "", "", "", "0"}, _
        {"prop", "mo", "", "&prop;", "propto", "", "", "", "0"}, _
        {"pord", "mo", "", "&prod;", "", "", "", "", "7"}, _
        {"Pi", "mi", "", "&Pi;", "", "", "", "", "0"}, _
        {"pi", "mi", "", "&pi;", "", "", "", "", "0"}, _
        {"Phi", "mi", "", "&Phi;", "", "", "", "", "0"}, _
        {"phi", "mi", "", "&phi;", "", "", "", "", "0"}, _
        {"ox", "mo", "", "&otimes;", "otimes", "", "", "", "0"}, _
        {"or", "mtext", "", "or", "", "", "", "", "6"}, _
        {"oo", "mo", "", "&infin;", "infty", "", "", "", "0"}, _
        {"Omega", "mi", "", "&Omega;", "", "", "", "", "0"}, _
        {"omega", "mi", "", "&omega;", "", "", "", "", "0"}, _
        {"oint", "mo", "", "&conint;", "", "", "", "", "0"}, _
        {"o+", "mo", "", "&oplus;", "oplus", "", "", "", "0"}, _
        {"O/", "mo", "", "&empty;", "emptyset", "", "", "", "0"}, _
        {"o.", "mo", "", "&odot;", "odot", "", "", "", "0"}, _
        {"nu", "mi", "", "&nu;", "", "", "", "", "0"}, _
        {"not", "mo", "", "&not;", "neg", "", "", "", "0"}, _
        {"nnn", "mo", "", "&xcap;", "bigcap", "", "", "", "7"}, _
        {"NN", "mo", "", "&Nopf;", "", "", "", "", "0"}, _
        {"nn", "mo", "", "&cap;", "cap", "", "", "", "0"}, _
        {"mu", "mi", "", "&mu;", "", "", "", "", "0"}, _
        {"mod", "mo", "", "mod", "", "", "", "", "0"}, _
        {"min", "mo", "", "min", "", "", "", "", "7"}, _
        {"mbox", "mtext", "", "mbox", "", "", "", "", "1"}, _
        {"max", "mo", "", "max", "", "", "", "", "7"}, _
        {"mathtt", "mstyle", "fontfamily=monospace", "mathtt", "", "", "", "", "1"}, _
        {"mathsf", "mstyle", "fontfamily=sans-serif", "mathsf", "", "", "", "", "1"}, _
        {"mathfrak", "mstyle", "mathvariant=fraktur", "mathfrak", "", "", "", "AMfrk", "1"}, _
        {"mathcal", "mstyle", "mathvariant=script", "mathcal", "", "", "", "AMcal", "1"}, _
        {"mathbf", "mstyle", "fontweight=bold", "mathbf", "", "", "", "", "1"}, _
        {"mathbb", "mstyle", "mathvariant=double-struck", "mathbb", "", "", "", "AMbbb", "1"}, _
        {"lub", "mo", "", "lub", "", "", "", "", "0"}, _
        {"lt=", "mo", "", "&le;", "leq", "", "", "", "0"}, _
        {"-lt", "mo", "", "&pr;", "", "", "", "", "0"}, _
        {"lt", "mo", "", "&lt;", "", "", "", "", "0"}, _
        {"log", "mo", "", "log", "", "", "", "", "0"}, _
        {"ln", "mo", "", "ln", "", "", "", "", "0"}, _
        {"Lim", "mo", "", "Lim", "", "", "", "", "0"}, _
        {"lim", "mo", "", "lim", "", "", "", "", "7"}, _
        {"lcm", "mo", "", "lcm", "", "", "", "", "0"}, _
        {"lArr", "mo", "", "&rArr;", "Leftarrow", "", "", "", "0"}, _
        {"larr", "mo", "", "&larr;", "leftarrow", "", "", "", "0"}, _
        {"Lambda", "mi", "", "&Lambda;", "", "", "", "", "0"}, _
        {"lambda", "mi", "", "&lambda;", "", "", "", "", "0"}, _
        {"kappa", "mi", "", "&kappa;", "", "", "", "", "0"}, _
        {"iota", "mi", "", "&iota;", "", "", "", "", "0"}, _
        {"int", "mo", "", "&int;", "", "", "", "", "0"}, _
        {"infty", "mo", "", "&infin;", "infty", "", "", "", "0"}, _
        {"in", "mo", "", "&isin;", "", "", "", "", "0"}, _
        {"if", "mo", "", "if", "", "", "", "", "6"}, _
        {"hat", "mover", "", "&Hat;", "", "True", "", "", "1"}, _
        {"hArr", "mo", "", "&lArr;", "Leftrightarrow", "", "", "", "0"}, _
        {"harr", "mo", "", "&larr;", "leftrightarrow", "", "", "", "0"}, _
        {"gt", "mo", "", "&gt;", "", "", "", "", "0"}, _
        {"grad", "mo", "", "&nabla;", "nabla", "", "", "", "0"}, _
        {"glb", "mo", "", "glb", "", "", "", "", "0"}, _
        {"geq", "mo", "", "&ge;", "", "", "", "", "0"}, _
        {"gcd", "mo", "", "gcd", "", "", "", "", "0"}, _
        {"Gamma", "mi", "", "&Gamma;", "", "", "", "", "0"}, _
        {"gamma", "mi", "", "&gamma;", "", "", "", "", "0"}, _
        {"frac", "mfrac", "", "/", "", "", "", "", "2"}, _
        {"fr", "mstyle", "mathvariant=fraktur", "fr", "", "", "", "AMfrk", "1"}, _
        {"eta", "mi", "", "&eta;", "", "", "", "", "0"}, _
        {"epsi", "mi", "", "&epsi;", "", "", "", "", "0"}, _
        {"EE", "mo", "", "&exist;", "exists", "", "", "", "0"}, _
        {"dz", "mi", "", "{:d z:}", "", "", "", "", "8"}, _
        {"dy", "mi", "", "{:d y:}", "", "", "", "", "8"}, _
        {"dx", "mi", "", "{:d x:}", "", "", "", "", "8"}, _
        {"dt", "mi", "", "{:d t:}", "", "", "", "", "8"}, _
        {"dot", "mover", "", ".", "", "True", "", "", "1"}, _
        {"dim", "mo", "", "dim", "", "", "", "", "0"}, _
        {"diamond", "mo", "", "&diam;", "", "", "", "", "0"}, _
        {"det", "mo", "", "det", "", "", "", "", "0"}, _
        {"Delta", "mi", "", "&Delta;", "", "", "", "", "0"}, _
        {"delta", "mi", "", "&delta;", "", "", "", "", "0"}, _
        {"del", "mo", "", "&part;", "partial", "", "", "", "0"}, _
        {"ddots", "mo", "", "&dtdot;", "", "", "", "", "0"}, _
        {"ddot", "mover", "", "..", "", "True", "", "", "1"}, _
        {"darr", "mo", "", "&darr;", "downarrow", "", "", "", "0"}, _
        {"csc", "mo", "", "csc", "", "", "", "", "0"}, _
        {"cot", "mo", "", "cot", "", "", "", "", "0"}, _
        {"cosh", "mo", "", "cosh", "", "", "", "", "0"}, _
        {"cos", "mo", "", "cos", "", "", "", "", "0"}, _
        {"chi", "mi", "", "&chi;", "", "", "", "", "0"}, _
        {"cdots", "mo", "", "&ctdot;", "", "", "", "", "0"}, _
        {"cdot", "mo", "", "&sdot;", "", "", "", "", "0"}, _
        {"CC", "mo", "", "&Copf;", "", "", "", "", "0"}, _
        {"cc", "mstyle", "mathvariant=script", "cc", "", "", "", "AMcal", "1"}, _
        {"beta", "mi", "", "&beta;", "", "", "", "", "0"}, _
        {"bbb", "mstyle", "mathvariant=double-struck", "bbb", "", "", "", "AMbbb", "1"}, _
        {"bb", "mstyle", "fontweight=bold", "bb", "", "", "", "", "1"}, _
        {"bar", "mover", "", "&macr;", "overline", "True", "", "", "1"}, _
        {"and", "mtext", "", "and", "", "", "", "", "6"}, _
        {"alpha", "mi", "", "&alpha;", "", "", "", "", "0"}, _
        {"aleph", "mo", "", "&aleph;", "", "", "", "", "0"}, _
        {"AA", "mo", "", "&forall;", "forall", "", "", "", "0"}, _
        {">>", "mo", "", "&rang;", "", "", "", "", "5"}, _
        {">=", "mo", "", "&ge;", "ge", "", "", "", "0"}, _
        {">-", "mo", "", "&sc;", "succ", "", "", "", "0"}, _
        {"->", "mo", "", "&rarr;", "to", "", "", "", "0"}, _
        {">", "mo", "", "&gt;", "", "", "", "", "0"}, _
        {"=>", "mo", "", "&rArr;", "implies", "", "", "", "0"}, _
        {"-=", "mo", "", "&equiv;", "equiv", "", "", "", "0"}, _
        {"<=>", "mo", "", "&hArr;", "iff", "", "", "", "0"}, _
        {"<=", "mo", "", "&le;", "le", "", "", "", "0"}, _
        {"<<", "mo", "", "&lang;", "", "", "", "", "4"}, _
        {"-<", "mo", "", "&pr;", "prec", "", "", "", "0"}, _
        {"<", "mo", "", "&lt;", "", "", "", "", "0"}, _
        {"+-", "mo", "", "&plusmn;", "pm", "", "", "", "0"}, _
        {"~=", "mo", "", "&cong;", "cong", "", "", "", "0"}, _
        {"~~", "mo", "", "&ap;", "approx", "", "", "", "0"}, _
        {"~|", "mo", "", "&lceil;", "rceiling", "", "", "", "0"}, _
        {"}", "mo", "", "}", "", "", "", "", "5"}, _
        {"|->", "mo", "", "&map;", "mapsto", "", "", "", "0"}, _
        {"|=", "mo", "", "&vDash;", "models", "", "", "", "0"}, _
        {"|~", "mo", "", "&lceil;", "lceiling", "", "", "", "0"}, _
        {"|_", "mo", "", "&lfloor;", "lfloor", "", "", "", "0"}, _
        {"|-", "mo", "", "&vdash;", "vdash", "", "", "", "0"}, _
        {"{:", "mo", "", "{:", "", "", "True", "", "4"}, _
        {"{", "mo", "", "{", "", "", "", "", "4"}, _
        {"_|_", "mo", "", "&bottom;", "bot", "", "", "", "0"}, _
        {"_|", "mo", "", "&rfloor;", "rfloor", "", "", "", "0"}, _
        {"_", "msub", "", "_", "", "", "", "", "3"}, _
        {"^^^", "mo", "", "&xwedge;", "bigwedge", "", "", "", "7"}, _
        {"^^", "mo", "", "&and;", "wedge", "", "", "", "0"}, _
        {"^", "msup", "", "^", "", "", "", "", "3"}, _
        {"]", "mo", "", "]", "", "", "", "", "5"}, _
        {"\\", "mo", "", "\\", "backslash", "", "", "", "0"}, _
        {"\ ", "mo", "", "&nbsp;", "", "", "", "", "0"}, _
        {"[", "mo", "", "[", "", "", "", "", "4"}, _
        {"@", "mo", "", "&compfn;", "circ", "", "", "", "0"}, _
        {":=", "mo", "", ":=", "", "", "", "", "0"}, _
        {":}", "mo", "", ":}", "", "", "True", "", "5"}, _
        {":)", "mo", "", "&rang;", "rangle", "", "", "", "5"}, _
        {"-:", "mo", "", "&divide;", "divide", "", "", "", "0"}, _
        {"//", "mo", "", "/", "", "", "", "", "0"}, _
        {"/", "mfrac", "", "/", "", "", "", "", "3"}, _
        {"/", "mfrac", "", "/", "", "", "", "", "3"}, _
        {"...", "mo", "", "...", "ldots", "", "", "", "0"}, _
        {"**", "mo", "", "&sstarf;", "star", "", "", "", "0"}, _
        {"*", "mo", "", "&sdot;", "cdot", "", "", "", "0"}, _
        {")", "mo", "", ")", "", "", "", "", "5"}, _
        {"(:", "mo", "", "&lang;", "langle", "", "", "", "4"}, _
        {"(", "mo", "", "(", "", "", "", "", "4"}, _
        {"""", "mtext", "", "mbox", "", "", "", "", "1"}, _
        {"!in", "mo", "", "&notin;", "notin", "", "", "", "0"}, _
        {"!=", "mo", "", "&ne;", "ne", "", "", "", "0"}, _
        {"&", "mo", "", "&#x0026;", "x0026", "", "", "", "0"} _
        }



        Public Sub New()
            Dim i As Integer
            For i = 0 To Me.pMax
                Me.pData(i) = Get_Struct_Data(i)
            Next
        End Sub


        '返回symbol()结构
        Public Function Get_Struct_Data(ByVal n As Integer) As C_MathML.MathML_Struct_Data
            Dim TMP As New C_MathML.MathML_Struct_Data

            TMP.Input = Me.Data(n, 0)
            TMP.Tag = Me.Data(n, 1)
            TMP.Atname = Me.Data(n, 2)
            TMP.Output = Me.Data(n, 3)
            TMP.Tex = Me.Data(n, 4)
            TMP.b_Acc = (Me.Data(n, 5) = "True")
            TMP.b_Invisible = (Me.Data(n, 6) = "True")
            TMP.Codes = Me.Data(n, 7)

            If Me.Data(n, 8) = "" Then
                TMP.Type = C_MathML.Math_Type.None
            Else
                TMP.Type = Val(Me.Data(n, 8))
            End If


            Return TMP
        End Function
    End Class

    Public Class C_Node_Data
        Public Node As String
        Public Str As String
    End Class
End Namespace
