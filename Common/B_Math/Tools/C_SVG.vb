Imports System.Xml
Imports System.Text
Imports System.Text.RegularExpressions


Namespace FunnyServer
    Public Class C_SVG
        Public Function SVG_Format(ByVal Str As String) As String

            Dim regExpression As New Regex("<[^<>]* ([^ <>]+=[^"" <>]*)( |\>)")
            Dim regMatchCollection As MatchCollection
            regMatchCollection = regExpression.Matches(Str)

            Dim i, Index As Long
            Dim Tmp, strSplit() As String
            'Dim reg As New Regex("")

            ReDim StrSplit(regMatchCollection.Count * 2)

            If regMatchCollection.Count > 0 Then

                StrSplit(0) = Left(Str, regMatchCollection.Item(0).Index)

                For i = 0 To regMatchCollection.Count - 2
                    StrSplit(i * 2 + 1) = Mid(Str, regMatchCollection.Item(i).Index + 1, regMatchCollection.Item(i).Length)
                    Index = regMatchCollection.Item(i).Index + 1 + regMatchCollection.Item(i).Length
                    StrSplit(i * 2 + 2) = Mid(Str, Index, regMatchCollection.Item(i + 1).Index + 1 - Index)
                Next

                StrSplit(i * 2 + 1) = Mid(Str, regMatchCollection.Item(i).Index + 1, regMatchCollection.Item(i).Length)
                Index = regMatchCollection.Item(i).Index + 1 + regMatchCollection.Item(i).Length
                StrSplit(i * 2 + 2) = Mid(Str, Index, Len(Str) + 1 - Index)

                For i = 0 To regMatchCollection.Count - 1
                    Tmp = StrSplit(i * 2 + 1)
                    Tmp = Regex.Replace(Tmp, "([^ <>]+)=([^"" <>]*)", "$1=""$2""")
                    StrSplit(i * 2 + 1) = Tmp
                Next

                Str = StrSplit(0)

                For i = 0 To regMatchCollection.Count - 1
                    Str += StrSplit(i * 2 + 1)
                    Str += StrSplit(i * 2 + 2)
                Next
            End If

            'reg = Nothing
            regExpression = Nothing
            regMatchCollection = Nothing

            Return Str
        End Function

        Public Function Funny_SVG_Decode(ByVal StrContent As String) As String
            '处理 svg:xxx

            Dim mIndex1, mIndex2 As Integer
            Dim StrLeft, StrRight, StrMid As String
            'Dim reg As Regex
            'reg = New Regex("", RegexOptions.IgnoreCase)

            mIndex2 = 1

            Do While mIndex2 > 0
                mIndex1 = InStr(StrContent, "<svg:svg ")
                If mIndex1 > 0 Then
                    mIndex2 = InStr(mIndex1, StrContent, "</svg:svg>")

                    If mIndex2 > mIndex1 Then                     'mindex2=0可以省略判断

                        mIndex2 = mIndex2 + Len("</svg:svg>")
                        StrLeft = Left(StrContent, mIndex1 - 1)
                        StrRight = Right(StrContent, Len(StrContent) - mIndex2 + 1)

                        StrMid = Mid(StrContent, mIndex1, mIndex2 - mIndex1)


                        StrMid = Regex.Replace(StrMid, "<br>", "", RegexOptions.IgnoreCase)
                        StrMid = Regex.Replace(StrMid, "<br\/>", "", RegexOptions.IgnoreCase)

                        StrMid = Regex.Replace(StrMid, "<svg:([A-Za-z]+?)([^>]*?)>", "<$1$2>", RegexOptions.IgnoreCase)
                        StrMid = Regex.Replace(StrMid, "</svg:([A-Za-z]+?)>", "</$1>", RegexOptions.IgnoreCase)

                        StrMid = Replace(StrMid, "&nbsp;", " ")

                        StrContent = StrLeft + StrMid + StrRight
                    End If
                Else
                    Exit Do
                End If
            Loop
            'reg = Nothing

            Return StrContent
        End Function

    End Class
End Namespace