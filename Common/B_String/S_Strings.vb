
Imports System.Text
Imports System.Text.RegularExpressions
Imports B_Array.Funny

'Imports System.Web


Namespace Funny
    Public Module S_Strings

        Public Function Function_Strings(ByVal strMap As String, ByVal strNew As String) As String

            Dim strReturn As String = ""

            '<<<<<<<<<<<<<<字符串函数<<<<<<<<<<<<<<<<<
            Select Case LCase(strMap)
                Case LCase("S.Encrypt.Unicode")
                    strReturn = S_Strings.EnCrypt_Unicode(strNew, "")
                Case LCase("S.Decrypt.Unicode")
                    strReturn = S_Strings.DeCrypt_Unicode(strNew, "")
                Case LCase("S.URL.GB2312")
                    strReturn = S_Strings.UrlEncode_GB2312(strNew)
            End Select
            Return strReturn
        End Function

        Public Function Format_Time(ByVal value As Double) As String
            Dim hour As Long = value \ 3600
            value = value Mod 3600
            Dim minute As Long = value \ 60
            Dim second As Double = value Mod 60

            Return Format(hour, "00") & ":" & Format(minute, "00") & ":" & Format(second, "00.00")
        End Function

        Public Function Base64_Encode(ByVal strLine As String) As String
            Dim strReturn As String = Convert.ToBase64String( _
                System.Text.Encoding.Default.GetBytes(strLine))
            Return strReturn
        End Function


        ''' <summary>
        ''' 通过URL获取网站地址
        ''' </summary>
        Public Function Get_Site_FromURL(ByVal strURL As String) As String
            Dim pMatch As Match = Regex.Match(strURL, "(http://[\w\.]+)/")
            If pMatch.Success Then
                Return pMatch.Groups(0).Value
            Else
                Return ""
            End If
        End Function

        Public Function RegGet(ByVal strBody As String, _
                                      ByVal strPattern As String, _
                                      ByVal strTemplate As String, _
                                      ByVal iTimeCount As Long) As String

            Dim pMatchs As MatchCollection = Regex.Matches(strBody, strPattern, RegexOptions.IgnoreCase)

            Dim strValue As String = ""
            Dim i As Int32 = 0
            Dim pArrayVar As ArrayList = New ArrayList

            For Each pMatch In pMatchs
                If pMatch.Success Then
                    i += 1
                    strValue = strTemplate
                    For k As Int32 = 1 To pMatch.Groups.Count - 1
                        strValue = Replace(strValue, "$(" & k & ")", pMatch.Groups(k).Value)
                    Next
                    pArrayVar.Add(strValue)
                    If i >= iTimeCount Then
                        If pArrayVar.Count = 0 Then
                            Return "" 'New C_Var(C_Var_Type.String_Type , "", "")
                        ElseIf pArrayVar.Count = 1 Then
                            Return pArrayVar(0) 'New C_Var(C_Var_Type.String_Type , "", pArrayVar(0))
                        Else
                            Return pArrayVar(0) 'New C_Var(C_Var_Type.Array_Type, "", pArrayVar)
                        End If
                        Exit For
                    End If
                End If
            Next
            Return ""

        End Function

        Public Function String_Replace_Reg(ByVal strData As String, ByVal strFind As String, ByVal strReplace As String) As String
            If strFind <> "" Then
                strFind = Replace(strFind, "\r", vbCr)
                strFind = Replace(strFind, "\n", vbLf)
            End If
            If strReplace <> "" Then
                strReplace = Replace(strReplace, "\r", vbCr)
                strReplace = Replace(strReplace, "\n", vbLf)
            End If

            Return Regex.Replace(strData, strFind, strReplace)
        End Function


        Public Function ToBase64(ByVal data() As Byte) As String
            If data Is Nothing Then Throw New ArgumentNullException("data")
            Return Convert.ToBase64String(data)
        End Function

        Public Function FromBase64(ByVal base64 As String) As Byte()
            If base64 Is Nothing Then Throw New ArgumentNullException("base64")
            Return Convert.FromBase64String(base64)
        End Function

        Public Function Format_Array_ToString(ByVal strTemplate As String, ByRef pArray As ArrayList) As String
            Return S_Array.Array_ToString(Format_Array(strTemplate, pArray))
        End Function

        Public Function Format_Array(ByVal strTemplate As String, ByRef pArray As ArrayList) As ArrayList
            Dim pReturn As ArrayList = New ArrayList
            Dim strLine As String = ""
            Dim strSplit2() As String
            For i As Integer = 0 To pArray.Count - 1
                strLine = strTemplate
                strSplit2 = pArray(i).Split("|")
                For k As Integer = 0 To strSplit2.Length - 1
                    strLine = Replace(strLine, "{" & k & "}", strSplit2(k))
                Next
                pReturn.Add(strLine)
            Next

            Return pReturn
        End Function

        Public Function Format_Array(ByVal strTemplate As String, ByRef pArray() As String, ByVal strSeg As String) As ArrayList
            Dim pReturn As ArrayList = New ArrayList
            Dim strLine As String = ""
            Dim strSplit2() As String
            For i As Integer = 0 To pArray.Count - 1
                strLine = strTemplate
                strSplit2 = pArray(i).Split(strSeg)
                For k As Integer = 0 To strSplit2.Length - 1
                    strLine = Replace(strLine, "{" & k & "}", strSplit2(k))
                Next
                pReturn.Add(strLine)
            Next

            Return pReturn
        End Function

        Public Function Format_Array(ByVal strTemplate As String, ByRef pArray As ArrayList, ByVal strSeg As String) As ArrayList
            Dim pReturn As ArrayList = New ArrayList
            Dim strLine As String = ""
            Dim strSplit2() As String
            For i As Integer = 0 To pArray.Count - 1
                strLine = strTemplate
                strSplit2 = pArray(i).Split(strSeg)
                For k As Integer = 0 To strSplit2.Length - 1
                    strLine = Replace(strLine, "{" & k & "}", strSplit2(k))
                Next
                pReturn.Add(strLine)
            Next

            Return pReturn
        End Function

        Public Function Combine_CrLf(ByVal ParamArray strSplit() As String) As String

            Dim strReturn As String = ""
            For i As Integer = 0 To strSplit.Length - 1
                strReturn &= strSplit(i) & vbCrLf
            Next

            Return strReturn
        End Function

        Public Function MD5(ByVal strText As String) As String
            Return C_MD5.MD5(strText, 32, True)
        End Function

        Public Function getSiteFromURL(ByVal strURL As String) As String
            Dim strPattern As String = "http://(.*?)/"
            Dim pMatch As Match = Regex.Match(strURL, strPattern) ', RegexOptions.IgnoreCase)
            Dim strReturn As String = ""
            If pMatch.Success Then
                strReturn = pMatch.Groups(1).Value
            End If
            Return strReturn
        End Function

        Public Function RegGet_Old( _
            ByVal strPattern As String, _
            ByVal index As Integer, _
            ByVal strBody As String, _
            Optional ByVal iTimeCount As Integer = 0, _
            Optional ByVal strSplit As String = ",") As String

            Dim strReturn As String = ""
            Try
                Dim pMatch As Match
                Dim pMatchs As MatchCollection = Regex.Matches(strBody, strPattern, RegexOptions.IgnoreCase)

                Dim strValue As String = ""
                Dim i As Integer

                For Each pMatch In pMatchs
                    If pMatch.Success Then
                        i += 1
                        If index < pMatch.Groups.Count Then
                            strValue = pMatch.Groups(index).Value
                        End If

                        If strReturn = "" Then
                            strReturn = strValue
                        Else
                            strReturn = strReturn + strSplit + strValue
                        End If
                        If i > iTimeCount Then
                            Return strReturn
                        End If
                    End If
                Next
            Catch ex As Exception
                strReturn = ex.ToString
            End Try

            Return strReturn
        End Function

        Public Function IsIP(ByVal strURL As String) As String
            '判断是否是IP地址
            Dim strSplit() As String
            strSplit = Split(strURL, ".")
            Dim i As Long, bIp As Boolean : bIp = True
            For i = 0 To UBound(strSplit)
                If IsNumeric(strSplit(i)) = False Then
                    bIp = False
                End If
            Next

            Return bIp
        End Function


        Public Function isMath(ByVal Str As String) As Boolean
            If IsIP(Str) Then
                isMath = False
                Exit Function
            End If

            Dim Count As Long
            Count = Len(Str)

            Dim bBoolean As Boolean, i As Long
            bBoolean = True
            For i = 1 To Count
                Select Case Mid(Str, i, 1)
                    Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "."
                    Case "+", "-", "*", "/", "(", ")", "^", " "
                    Case Else
                        bBoolean = False
                End Select
            Next
            Return bBoolean
        End Function


        Public Function Get_WebSite_FromURL(ByVal strURL As String) As String
            Dim StrWeb As String, Index As Long

            If InStr(1, strURL, "http://", vbTextCompare) = 1 Then
                Index = InStr(9, strURL, "/", vbTextCompare)
                If Index > 0 Then
                    StrWeb = Mid(strURL, 8, Index - 8)
                Else
                    StrWeb = ""
                End If
            Else
                StrWeb = ""
            End If

            Return StrWeb
        End Function


        '除去右边的,号
        Public Function TrimRightComma(ByVal strReturn As String) As String
            If Right(strReturn, 1) = "," Then
                strReturn = Left(strReturn, Len(strReturn) - 1)
            End If
            Return strReturn
        End Function

        Function HTMLEncode(ByVal fString As String) As String
            If Not IsDBNull(fString) Then
                fString = Replace(fString, ">", "&gt;")
                fString = Replace(fString, "<", "&lt;")
                fString = Replace(fString, Chr(34), "&quot;")
                fString = Replace(fString, Chr(39), "&#39;")
                Return fString
            Else
                Return ""
            End If
        End Function


        Public Function CData_Encode(ByVal Str As String, _
            Optional ByVal bInvert As Boolean = False) As String
            If bInvert = False Then
                Str = Replace(Str, "&", "&amp;")
                Str = Replace(Str, "<", "&lt;")
                Str = Replace(Str, ">", "&gt;")
                Str = Replace(Str, "'", "&apos;")
                Str = Replace(Str, """", "&quot;")
            Else
                Str = Replace(Str, "&lt;", "<")
                Str = Replace(Str, "&gt;", ">")
                Str = Replace(Str, "&apos;", "'")
                Str = Replace(Str, "&quot;", """")
                Str = Replace(Str, "&amp;", "&")
            End If
            Return Str
        End Function


        Function UrlEncode_GB2312(ByVal strText As String) As String
            'Web.HttpUtility()
            Return Web.HttpUtility.UrlEncode(strText, System.Text.Encoding.GetEncoding("GB2312"))
        End Function


        ''' <summary>
        ''' 是否包含英语
        ''' </summary>
        Public Function ContainEnglish(ByVal strContent As String) As Boolean

            For i As Integer = 1 To Len(strContent)
                If (AscW(Mid(strContent, i, 1)) >= 65 AndAlso AscW(Mid(strContent, i, 1)) <= 90) _
                OrElse (AscW(Mid(strContent, i, 1)) >= 97 AndAlso AscW(Mid(strContent, i, 1)) <= 122) Then
                    Return True
                End If
            Next
            Return False

        End Function

        ''' <summary>
        ''' 是否包含数字
        ''' </summary>
        Public Function ContainNumber(ByVal strContent As String) As Boolean
            For i As Integer = 1 To Len(strContent)
                If (AscW(Mid(strContent, i, 1)) >= 48 AndAlso AscW(Mid(strContent, i, 1)) <= 57) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Function isEnglish(ByVal strContent As String) As Boolean
            For i As Integer = 1 To Len(strContent)
                If (AscW(Mid(strContent, i, 1)) >= 65 AndAlso AscW(Mid(strContent, i, 1)) <= 90) _
                OrElse (AscW(Mid(strContent, i, 1)) >= 97 AndAlso AscW(Mid(strContent, i, 1)) <= 122) Then

                Else
                    Return False
                End If
            Next
            Return True
        End Function

        Public Function UrlEncode(ByVal strText As String) As String
            Return Web.HttpUtility.UrlEncode(strText, System.Text.Encoding.UTF8)
        End Function

        Public Function URLEncode_Byte(ByRef bByte() As Byte) As String
            Dim strReturn As String = ""
            Dim strTmp As String
            For i As Integer = 0 To UBound(bByte)
                strTmp = Hex(bByte(i))
                If Len(strTmp) = 1 Then
                    strTmp = "0" & strTmp
                End If
                strReturn = strReturn + "%" & strTmp
            Next
            URLEncode_Byte = strReturn
        End Function

        '%E4%BD%A0%E5%A5%BD  你好
        Function URLDecode(ByVal strText As String) As String
            Return Web.HttpUtility.UrlDecode(strText, System.Text.Encoding.UTF8)
        End Function

        'URL Decode GB2312
        Function URLDecode_GB2312(ByVal strText As String) As String
            Return Web.HttpUtility.UrlDecode(strText, System.Text.Encoding.GetEncoding("GB2312"))
        End Function

        Public Function ReplaceNLP_Invert(ByVal strValue As String) As String
            Dim strReturn As String = strValue
            Dim strSplit() As String = { _
                    "{C.wen}", "?", "{C.mao}", ":", _
                    "{C.na}", "\", "{C.pie}", "/", _
                    "{C.at}", "@", "{C.baifen}", "%", _
                    "{C.xiaoyu}", "<", "{C.dayu}", ">", _
                    "{C.jian}", "-", "{C.zuodakuohu}", "(((", _
                    "{C.youdakuohu}", ")))", "{C.dou}", ",", _
                    "{C.fenhao}", ";", "{C.dian}", ".", _
                    "{C.sharp}", "#"}

            For i As Integer = 0 To strSplit.Length - 2 Step 2
                strReturn = Replace(strReturn, strSplit(i), strSplit(i + 1))
            Next

            Return strReturn
        End Function

        Public Function ReplaceNLP(ByVal strPath As String) As String

            strPath = Replace(strPath, ".", "C.dian")
            strPath = Replace(strPath, "{", "C.zuodakuohu")
            strPath = Replace(strPath, "}", "C.youdakuohu")

            strPath = Replace(strPath, "?", "C.wen")
            strPath = Replace(strPath, "%", "C.baifen")
            strPath = Replace(strPath, ":", "C.mao")
            strPath = Replace(strPath, "@", "C.at")

            strPath = Replace(strPath, "\", "C.na")
            strPath = Replace(strPath, "/", "C.pie")
            strPath = Replace(strPath, "*", "C.xing")
            strPath = Replace(strPath, """", "C.yin")

            strPath = Replace(strPath, "<", "C.xiaoyu")
            strPath = Replace(strPath, ">", "C.dayu")
            strPath = Replace(strPath, "|", "C.shu")
            strPath = Replace(strPath, ",", "C.dou")

            strPath = Replace(strPath, "-", "C.jian")
            strPath = Replace(strPath, "=", "C.deng")
            strPath = Replace(strPath, "&", "C.and")
            strPath = Replace(strPath, ";", "C.fenhao")
            strPath = Replace(strPath, "#", "C.sharp")

            Return strPath
        End Function


        Public Function Str_DealPre(ByRef mPhrase() As String, ByVal K As Long) As String
            '=======================================
            '描述:Str打头的函数代表是处理字符串的
            '预处理,
            '检测其中,动词名词的形态,变成标准形态

            'With rs
            Const C_Count As Integer = 7

            Dim mStr(C_Count) As String, mAdd(C_Count) As String
            Dim i As Long
            Dim mCount As Long

            mStr(1) = "ing" : mAdd(1) = "e"
            mStr(2) = "ing" : mAdd(2) = ""
            mStr(3) = "ies" : mAdd(3) = "y" '复数
            mStr(4) = "es" : mAdd(4) = ""
            mStr(5) = "s" : mAdd(5) = ""
            mStr(6) = "ed" : mAdd(6) = ""
            mStr(7) = "d" : mAdd(7) = ""


            For i = 1 To C_Count
                If Right(mPhrase(K), Len(mStr(i))) = mStr(i) Then
                    mPhrase(K) = Left(mPhrase(K), Len(mPhrase(K)) - Len(mStr(i))) '去掉最后一个mStr(i)

                    '补上mAdd(i),看有无此单词
                    mPhrase(K) = mPhrase(K) + mAdd(i)

                    If mCount > 0 Then
                        If i = 6 Or i = 7 Then
                            '过去式,过去分词
                            If i > 1 Then
                                If mPhrase(K) = "have" Or _
                                mPhrase(K) = "has" Then
                                    mPhrase(K - 1) = "" '前面的解释全部去掉
                                End If
                            End If
                        End If

                        If i = 1 Or i = 2 Then '{ing}变成一个单位
                            Str_DealPre = LCase(mPhrase(K)) + " {ing}"
                        ElseIf i = 3 Or i = 4 Or i = 5 Then '{s}变成一个单位
                            Str_DealPre = LCase(mPhrase(K)) + " {s}"
                        ElseIf i = 6 Or i = 7 Then '{ed}变成一个单位
                            Str_DealPre = LCase(mPhrase(K)) + " {ed}"
                        Else
                            Str_DealPre = LCase(mPhrase(K))
                        End If
                        GoTo 999
                    End If


                    mPhrase(K) = Left(mPhrase(K), Len(mPhrase(K)) - Len(mAdd(i))) + mStr(i) '恢复原来单词
                End If
            Next

            Str_DealPre = LCase(mPhrase(K))
999:

        End Function

        Public Function Str_ReplacePredeal(ByVal strWord As String) As String
            strWord = Replace(strWord, "：", ":")
            strWord = Replace(strWord, "）", ")")
            strWord = Replace(strWord, "（", "(")
            strWord = Replace(strWord, "？", "?")
            strWord = Replace(strWord, "，", ",")
            strWord = Replace(strWord, "！", "!")

            strWord = LCase(strWord)

            Return strWord
        End Function


        ''' <summary>
        ''' 在汉字前面后面加空格,以及在数字符号前后加空格
        ''' </summary>
        Public Function Blank_Chinese(ByVal strContext As String) As String
            Dim strReturn As StringBuilder = New StringBuilder

            Dim mStr As String

            '去掉两个空格，单个空格不能去掉，因为有english
            Do While Strings.InStr(strContext, vbCr) > 0 _
                    OrElse Strings.InStr(strContext, vbLf) > 0 _
                    OrElse Strings.InStr(strContext, vbTab) > 0
                strContext = Regex.Replace(strContext, "[\r|\n\t]{1,10}", " ")
            Loop
            Do While Strings.InStr(strContext, "  ") > 0
                strContext = Regex.Replace(strContext, "( ){2,10}", " ")
            Loop

            strContext = Str_ReplacePredeal(strContext)
            strContext = ReplaceFirst(strContext)

            For i As Integer = 1 To Len(strContext)
                mStr = Mid(strContext, i, 1)

                Select Case mStr
                    Case "+", "-", "*", "\", "^", "(", ")", ",", "/", "=", ":", "&", "#"
                        strReturn.Append(" {" + S_Strings.ReplaceNLP(mStr) + "} ")
                    Case """", "'", "?", ".", "@", "%", "<", ">", "{", "}", ";"
                        strReturn.Append(" {" + S_Strings.ReplaceNLP(mStr) + "} ")
                    Case ChrW(12288), ChrW(160)
                        strReturn.Append(" ")
                    Case "â", "Â", "$", "[", "]"
                        strReturn.Append(" " + mStr + " ")
                    Case Else
                        If Asc(mStr) >= 0 And Asc(mStr) <= 255 Then
                            strReturn.Append(mStr)
                        Else
                            strReturn.Append(" " + mStr + " ")
                        End If
                End Select
            Next

            strContext = strReturn.ToString.Trim


            Do While Strings.InStr(strContext, "  ") > 0
                strContext = Regex.Replace(strContext, "( ){2,10}", " ")
            Loop

            Return strContext
        End Function

        Public Function Split_ABC_Number(ByVal strInput As String) As String

            Dim CharSplit() As String = { _
                    "((\d|\.)+)([a-zA-z]+)===1,3", _
                    "([a-zA-z]+)((\d|\.)+)===1,2", _
                    "([a-zA-z]+)((\d|\.)+)([a-zA-z]+)===1,2,4", _
                    "((\d|\.)+)([a-zA-z]+)((\d|\.)+)===1,3,4", _
                    "((\d|\.)+)([a-zA-z]+)((\d|\.)+)([a-zA-z]+)===1,3,4,6", _
                    "([a-zA-z]+)((\d|\.)+)([a-zA-z]+)((\d|\.)+)===1,2,4,5" _
                    }


            Dim strSplit() As String = strInput.Split(" ")
            Dim strConvert() As String

            Dim strReturn As StringBuilder = New StringBuilder
            Dim pMatch As Match
            Dim index As Integer
            Dim iCount As Integer = 0

            For i As Integer = 0 To strSplit.Length - 1
                iCount = 0
                For j As Integer = 0 To CharSplit.Length - 1
                    strConvert = Split(CharSplit(j), "===")
                    If strConvert.Length > 1 Then
                        pMatch = Regex.Match(strSplit(i), strConvert(0))

                        If pMatch.Success AndAlso pMatch.Groups(0).Value = strSplit(i) Then
                            iCount += 1
                            strConvert = strConvert(1).Split(",")
                            For k As Integer = 0 To strConvert.Length - 1
                                If IsNumeric(strConvert(k)) Then
                                    index = Val(strConvert(k))
                                    If index < pMatch.Groups.Count Then
                                        strReturn.Append(pMatch.Groups(index).Value & " ")
                                    End If
                                Else
                                    strReturn.Append(strConvert(k) & " ")
                                End If
                            Next
                        End If
                    End If
                Next

                If iCount = 0 Then '只允许一次
                    If strSplit(i) <> "" Then
                        strReturn.Append(strSplit(i) & " ")
                    End If
                End If
            Next

            Return strReturn.ToString.Trim
        End Function

        Public Function Blank_Chinese_Invert(ByVal strInput As String) As String
            If strInput = "" Then Return ""

            Dim strSplit() As String = Split(strInput, " ")

            '汉字前面后面空格去掉
            Dim i As Integer
            Dim strResult As String = ""
            Dim bEnglish_JustNow As Boolean

            For i = 0 To UBound(strSplit)
                If Len(strSplit(i)) > 1 Then
                    If bEnglish_JustNow = True Then
                        strResult &= " " & strSplit(i)
                    Else
                        strResult &= strSplit(i)
                    End If

                    If AscW(Right(strSplit(i), 1)) > 255 Then
                        bEnglish_JustNow = False
                    Else
                        bEnglish_JustNow = True
                    End If
                Else
                    If strSplit(i) <> "" AndAlso Asc(strSplit(i)) >= 0 AndAlso Asc(strSplit(i)) <= 255 Then
                        If bEnglish_JustNow = True Then
                            strResult &= " " & strSplit(i)
                        Else
                            strResult &= strSplit(i)
                        End If
                        bEnglish_JustNow = True
                    Else
                        strResult &= strSplit(i)
                        bEnglish_JustNow = False
                    End If
                End If
            Next
            strResult = Trim(strResult)

            Return strResult
        End Function

        Public Function ReplaceFirst(ByVal mWord As String) As String
            '你被设计为多大？
            mWord = Replace(mWord, "？", "?")
            mWord = Replace(mWord, "?", " ? ")
            mWord = Replace(mWord, "!", " ! ")
            mWord = Replace(mWord, ",", " , ")
            mWord = Replace(mWord, "~", " ~ ")
            mWord = Replace(mWord, "。", ".")
            mWord = Replace(mWord, "'s", " 's")
            mWord = Replace(mWord, "'re", " 're")
            mWord = Replace(mWord, "'ll", " 'll")
            mWord = Replace(mWord, "I'm", "I 'm")
            mWord = Replace(mWord, "can't", "can 't")
            mWord = Replace(mWord, "isn't", "is not")


            mWord = Replace(mWord, "I'd", "I 'd")
            mWord = Replace(mWord, "wouldn't", "would 't")

            mWord = Replace(mWord, vbCrLf, "")

            mWord = Replace(mWord, "am not", "not am") 'ChinEnglish
            mWord = Replace(mWord, "can not", "not can") 'ChinEnglish
            mWord = Replace(mWord, "would not", "not would")

            mWord = Replace(mWord, vbCrLf, " ")

            ReplaceFirst = mWord
        End Function





        Public Function HTML2Text(ByVal strBody As String) As String
            If strBody = "" Then Return ""

            strBody = HTML2Text_Sub3(strBody, "<!--", "-->")
            strBody = HTML2Text_Sub2(strBody, "script")
            strBody = HTML2Text_Sub2(strBody, "style")


            If strBody = "" Then Return ""

            strBody = Regex.Replace(strBody, "<[^>]*?>", " ")

            strBody = Replace(strBody, "&nbsp;", " ")

            strBody = Replace(strBody, "&oslash;", " ")
            strBody = Replace(strBody, "·", " ")

            strBody = Replace(strBody, vbTab, "")

            Do While Strings.InStr(strBody, "  ") > 0
                strBody = Regex.Replace(strBody, "( ){2,10}", " ")
            Loop

            Do While Strings.InStr(strBody, vbCrLf & vbCrLf) > 0
                strBody = Regex.Replace(strBody, "[\r|\n]{4,10}", vbCrLf)
            Loop


            Return strBody
        End Function

        Function HTML2Text_Sub1(ByVal strContent As String, ByVal lcase_StrItem As String) As String
            'FilterHtmlItem()
            '过滤html item  AAAAAAAAAAAA<head>AAA</head>AAAAAAAA
            Dim Pos1, Pos2 As Integer

            Do
                Pos1 = InStr(strContent, "<" & lcase_StrItem, CompareMethod.Text)
                If Pos1 > 0 Then
                    Pos2 = InStr(Pos1, strContent, ">", CompareMethod.Text)
                    strContent = Mid(strContent, 1, Pos1 - 1) + Right(strContent, Len(strContent) - Pos2)
                End If
            Loop While Pos1 > 0 And Pos2 > 0

            Do
                Pos1 = InStr(strContent, "</" & lcase_StrItem, CompareMethod.Text)
                If Pos1 > 0 Then
                    Pos2 = InStr(Pos1, strContent, ">", CompareMethod.Text)
                    strContent = Mid(strContent, 1, Pos1 - 1) + Right(strContent, Len(strContent) - Pos2)
                End If
            Loop While Pos1 > 0 And Pos2 > 0

            Return strContent
        End Function


        Function HTML2Text_Sub2(ByVal strContent As String, ByVal strKey As String) As String
            strContent = Regex.Replace(strContent, _
              "(?is)<" & strKey & "([^>])*?></" & strKey & "([^>])*?>", _
              String.Empty, RegexOptions.IgnoreCase)

            strContent = Regex.Replace(strContent, _
              "(?is)<" & strKey & "([^>])*?>(\w|\W)*?</" & strKey & "([^>])*?>", _
              String.Empty, RegexOptions.IgnoreCase)

            Return strContent
        End Function



        Function HTML2Text_Sub3(ByVal strContent As String, _
            ByVal strLeft As String, ByVal strRight As String) As String
            'FilterHtmlItem()
            '过滤html 如 <!--  --> 
            Dim Pos1, Pos2 As Integer

            Do
                Pos1 = InStr(strContent, strLeft, CompareMethod.Text)
                If Pos1 > 0 Then
                    Pos2 = InStr(Pos1, strContent, strRight, CompareMethod.Text)
                    If Pos2 > 0 Then
                        strContent = Mid(strContent, 1, Pos1 - 1) + Right(strContent, Len(strContent) - Pos2 - Len(strRight) + 1)
                    End If
                End If
            Loop While Pos1 > 0 And Pos2 > 0


            Return strContent
        End Function


        ''' <summary>
        ''' 加密成unicode的
        ''' </summary>
        ''' <param name="mStr"></param>
        ''' <param name="MyKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeCrypt_Unicode(ByVal mStr As String, ByVal MyKey As String) As String
            mStr = UCase(mStr)
            Dim i, R_Key As Integer
            Dim KeyLen As Integer
            Dim strReturn As String, MyVal As Integer
            Dim mTmpStr As String

            If MyKey = "" Then
                MyKey = "FunnyWeber"
            End If

            KeyLen = Len(MyKey)


            strReturn = ""

            Dim Key_Index As Long

            Key_Index = 0
            For i = 1 To Len(mStr) Step 4
                Key_Index = Key_Index + 1
                R_Key = AscW(Mid(MyKey, ((Key_Index - 1) Mod KeyLen) + 1, 1))
                If R_Key < 0 Then R_Key = 65536 + R_Key

                '***********************************************
                MyVal = Val("&H" + Trim(Mid(mStr, i, 4)))
                MyVal = MyVal Xor R_Key
                If MyVal < 0 Then MyVal = 65536 + MyVal

                mTmpStr = ChrW(MyVal)
                strReturn = strReturn + mTmpStr
            Next

            Return strReturn
        End Function

        Public Function EnCrypt_Unicode(ByVal mStr As String, ByVal MyKey As String) As String
            Dim i As Long, R As Long, R_Key As Long
            Dim KeyLen As Long
            Dim strReturn As String, MyAsc As Long
            Dim mTmpStr As String

            If MyKey = "" Then
                MyKey = "FunnyWeber"
            End If

            KeyLen = Len(MyKey)

            strReturn = ""

            Dim Key_Index As Long
            Key_Index = 0

            For i = 1 To Len(mStr)

                R = AscW(Mid(mStr, i, 1))
                If R < 0 Then R = 65536 + R

                Key_Index = Key_Index + 1
                R_Key = AscW(Mid(MyKey, ((Key_Index - 1) Mod KeyLen) + 1, 1))
                If R_Key < 0 Then R_Key = 65536 + R_Key

                MyAsc = R Xor R_Key
                If MyAsc < 0 Then MyAsc = 65536 + MyAsc

                mTmpStr = Hex(MyAsc)

                If Len(mTmpStr) < 4 Then
                    mTmpStr = Strings.StrDup(4 - Len(mTmpStr), "0") + mTmpStr
                Else
                    mTmpStr = Right(mTmpStr, 4)
                End If

                strReturn = strReturn + mTmpStr
            Next

            Return strReturn
        End Function


        Public Function Filter_Tab_Space_Crlf(ByVal Str As String) As String
            Str = Replace(Str, vbTab, "")
            Str = Replace(Str, vbCrLf, "")
            Str = Replace(Str, " ", "")
            Return Str
        End Function

        Public Function If_FileName(ByVal StrFile As String) As Boolean
            Dim i As Long, Str() As String
            Dim bValid As Boolean = True

            If StrFile = "" Then
                bValid = False
            Else
                Str = Split("\ / : * ? "" < > |", " ")
                For i = 0 To UBound(Str)
                    If Str(i) <> "" Then
                        If InStr(StrFile, Str(i)) Then
                            bValid = False
                        End If
                    End If
                Next
            End If


            If_FileName = bValid
        End Function

        Public Function If_FileName_URL(ByVal StrFile As String) As Boolean
            Dim i As Long, Str() As String
            Dim bValid As Boolean = True

            If StrFile = "" Then
                bValid = False
            Else
                Str = Split(": * ? "" < > |", " ")
                For i = 0 To UBound(Str)
                    If Str(i) <> "" Then
                        If InStr(StrFile, Str(i)) Then
                            bValid = False
                        End If
                    End If
                Next
            End If


            If_FileName_URL = bValid
        End Function


        Public Function TrimLeft_Tab_Space(ByVal Str As String) As String
            '去掉左边 Tab 和 空格
            Do While (Left(Str, 1) = vbTab Or Left(Str, 1) = " ") And Len(Str) >= 1
                Str = Right(Str, Len(Str) - 1)
            Loop
            Return Str
        End Function

        Public Function TrimSql(ByVal Str As String) As String
            '去掉左边 Tab 和 空格
            Str = Replace(Str, vbCrLf, " ")

            Do While (Left(Str, 1) = vbTab Or Left(Str, 1) = " ") And Len(Str) >= 1
                Str = Right(Str, Len(Str) - 1)
            Loop

            Do While (Right(Str, 1) = vbTab Or Right(Str, 1) = " ") And Len(Str) >= 1
                Str = Left(Str, Len(Str) - 1)
            Loop

            Return Str
        End Function


        Public Function GetRightString(ByVal Str As String, ByVal StrSearch As String) As String
            '去掉左边 Tab 和 空格
            Dim index As Integer
            index = InStrRev(Str, StrSearch)
            If index > 0 Then
                If Len(Str) - index - Len(StrSearch) + 1 > 0 Then
                    Str = Right(Str, Len(Str) - index - Len(StrSearch) + 1)
                Else
                    Str = ""
                End If
            End If

            Return Str
        End Function

        Public Function Str_Balance( _
           ByVal StrContent As String, _
           ByVal StrLeft As String, _
           ByVal StrRight As String) As Boolean

            '看 StrLeft 和 StrRight 是否匹配

            Dim Count, PosTmp As Integer

            PosTmp = InStr(StrContent, StrLeft, CompareMethod.Text)
            Do While PosTmp > 0
                Count = Count + 1
                PosTmp = InStr(PosTmp + 1, StrContent, StrLeft, CompareMethod.Text)
            Loop

            PosTmp = InStr(StrContent, StrRight, CompareMethod.Text)
            Do While PosTmp > 0
                Count = Count - 1
                PosTmp = InStr(PosTmp + 1, StrContent, StrRight, CompareMethod.Text)
            Loop

            Return (Count = 0)
        End Function


        Public Function Filter_SQL_DangerChar(ByVal StrContent As String) As String

            '过滤所有可能有危险的
            StrContent = Replace(StrContent, "'", "")
            StrContent = Replace(StrContent, "-", "")
            StrContent = Replace(StrContent, ";", "")

            Return StrContent
        End Function

        Public Function Trim_XML_Head(ByVal Str As String) As String
            '去掉左边 Tab 和 空格
            Do While (Left(Str, 1) = vbTab Or Left(Str, 1) = " " Or Left(Str, 1) = Chr(10) Or Left(Str, 1) = Chr(13)) And Len(Str) >= 1
                Str = Right(Str, Len(Str) - 1)
            Loop
            Return Str
        End Function


        Public Function UTF8_Byte(ByVal str As String) As Byte()
            Dim utf8Encoding As Encoding = System.Text.Encoding.GetEncoding("utf-8")

            Dim Data() As Byte = utf8Encoding.GetBytes(str)

            Return Data '.ToString()
        End Function

        Public Function Get_UTF8_String(ByRef Data() As Byte) As String
            Return System.Text.UTF8Encoding.UTF8.GetString(Data, 0, Data.Length)
        End Function
    End Module
End Namespace