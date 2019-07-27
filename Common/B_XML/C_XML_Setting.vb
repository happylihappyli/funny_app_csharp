Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports B_Debug.Funny

Namespace Funny

    Public Class C_XML_Setting
        Dim doc As XmlDocument = New XmlDocument
        Private strFile As String
        Private bDirty As Boolean = False '是否被修改
        Private bNeedFlush As Boolean = False

        Public Sub setFile(ByVal strFile As String, _
                           Optional ByVal bNeedFlush As Boolean = False)
            Me.bNeedFlush = bNeedFlush

            doc = Nothing
            doc = New XmlDocument
            Me.strFile = strFile
            If System.IO.File.Exists(strFile) Then
                Try
                    doc.Load(strFile)
                Catch ex As Exception
                    doc.LoadXml("<Data></Data>")
                    S_Debug.LogError(strFile & "log", strFile & ex.ToString())
                End Try
            Else
                doc.LoadXml("<Data></Data>")
            End If
        End Sub

        Public Function Read_Setting( _
            ByVal strSection As String, _
            ByVal strKey As String) As String

            Dim strReturn As String = ""
            Try
                Dim pSection As XmlNode = doc.DocumentElement.SelectSingleNode(strSection)
                If Not pSection Is Nothing Then
                    Dim pKey As XmlNode = pSection.SelectSingleNode(strKey)
                    If Not pKey Is Nothing Then
                        strReturn = pKey.InnerText
                    End If
                End If
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
            Return strReturn
        End Function



        Public Function Read_NodeList( _
                    ByVal strSection As String) As XmlNodeList

            Dim pNodeList As XmlNodeList = Nothing
            Dim strReturn As String = ""
            Try
                pNodeList = doc.DocumentElement.SelectNodes(strSection)
                'If Not pSection Is Nothing Then
                '    Dim pKey As XmlNode = pSection.SelectSingleNode(strKey)
                '    If Not pKey Is Nothing Then
                '        strReturn = pKey.InnerText
                '    End If
                'End If
            Catch ex As Exception

            End Try
            Return pNodeList
        End Function

        Public Function LoadAttribute( _
            ByVal strSection As String, _
            ByVal strKey As String, _
            ByVal strAttribute As String) As String

            Dim strReturn As String = ""
            Try

                Dim pSection As XmlNode = doc.DocumentElement.SelectSingleNode(strSection)
                If Not pSection Is Nothing Then
                    Dim pKey As XmlNode = pSection.SelectSingleNode(strKey)
                    If Not pKey Is Nothing Then
                        If pKey.Attributes(strAttribute) IsNot Nothing Then
                            strReturn = pKey.Attributes(strAttribute).Value
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            Return strReturn
        End Function


        Public Sub Save_Setting( _
            ByVal strSection As String, _
            ByVal strKey As String, _
            ByVal strValue As String)

            bDirty = True

            Dim pSection As XmlNode = GetOrCreateNode(strSection)

            Dim pKey As XmlNode = pSection.SelectSingleNode(strKey)
            If pKey Is Nothing Then
                pKey = doc.CreateNode(XmlNodeType.Element, "", strKey, "")
                pSection.AppendChild(pKey)
            End If
            pKey.InnerText = strValue

            If Me.bNeedFlush = False Then
                doc.Save(Me.strFile)
            End If
        End Sub

        ''' <summary>
        ''' 查找或创建XML节点，支持XPath，如 strXPath="Name/Table/Field/A"
        ''' </summary>
        Public Function GetOrCreateNode(ByVal strXPath As String) As XmlNode
            Dim pSection As XmlNode = doc.DocumentElement.SelectSingleNode(strXPath)
            If pSection Is Nothing Then
                Dim strSplit() As String = Split(strXPath, "/")
                pSection = doc.DocumentElement.SelectSingleNode(strSplit(0))
                If pSection Is Nothing Then
                    pSection = doc.CreateNode(XmlNodeType.Element, "", strSplit(0), "")
                    doc.DocumentElement.AppendChild(pSection)
                End If

                Dim pSection2 As XmlNode = pSection
                For i As Integer = 1 To strSplit.Length - 1
                    pSection = pSection.SelectSingleNode(strSplit(i))
                    If pSection Is Nothing Then
                        pSection = doc.CreateNode(XmlNodeType.Element, "", strSplit(i), "")
                        doc.DocumentElement.AppendChild(pSection)
                    End If
                    pSection2.AppendChild(pSection)
                    pSection2 = pSection
                Next
            End If

            Return pSection
        End Function

        Public Sub SaveAttribute( _
            ByVal strSection As String, _
            ByVal strKey As String, _
            ByVal strAttribute As String, _
            ByVal strValue As String)

            bDirty = True
            Dim pSection As XmlNode = GetOrCreateNode(strSection)

            Dim pKey As XmlNode = pSection.SelectSingleNode(strKey)
            If pKey Is Nothing Then
                pKey = doc.CreateNode(XmlNodeType.Element, "", strKey, "")
                pSection.AppendChild(pKey)
            End If
            If pKey.Attributes(strAttribute) Is Nothing Then
                Dim attr As XmlNode = doc.CreateNode(XmlNodeType.Attribute, "", strAttribute, "")
                attr.Value = strValue
                pKey.Attributes.Append(attr)
            Else
                pKey.Attributes(strAttribute).InnerText = strValue
            End If

            If Me.bNeedFlush = False Then
                doc.Save(Me.strFile)
            End If

        End Sub

        Public Sub FlushToDisk()
            If bDirty = True Then
                doc.Save(strFile)
            End If
        End Sub

        Sub Finished()
            doc = Nothing
        End Sub

    End Class

End Namespace