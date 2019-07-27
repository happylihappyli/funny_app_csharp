Imports System.Xml
Imports System.IO

Namespace Funny
    Public Module S_XML

        Public Function Read_FromRoot( _
            ByVal strFile As String, _
            ByVal strXPath As String, _
            Optional ByVal bXML As Boolean = False) As String

            Dim pNode As XmlNode
            Dim A As XmlDocument

            If File.Exists(strFile) Then
                A = New XmlDocument
                Call A.Load(strFile)
            Else
                Read_FromRoot = ""
                Exit Function
            End If

            pNode = A.SelectSingleNode(strXPath)


            Dim strReturn As String = ""
            If Not pNode Is Nothing Then
                If bXML Then
                    strReturn = pNode.InnerXml
                Else
                    strReturn = pNode.InnerText
                End If
            End If

            pNode = Nothing
            A = Nothing

            Return strReturn
        End Function

        Public Function Attribute( _
         ByRef pNode As Xml.XmlNode, _
         ByVal strName As String) As String

            Dim pNodeTmp As Xml.XmlNode, StrReturn As String

            If pNode.NodeType = Xml.XmlNodeType.Element Then
                pNodeTmp = pNode.Attributes(StrName)
                If Not pNodeTmp Is Nothing Then
                    StrReturn = pNodeTmp.Value
                Else
                    StrReturn = ""
                End If
            Else
                StrReturn = ""
            End If

            pNodeTmp = Nothing

            Return StrReturn
        End Function

        Public Function Read_Node_Text(ByRef pNode As XmlNode, ByVal strName As String)
            Dim pNode2 As XmlNode = pNode.SelectSingleNode(strName)
            If pNode2 Is Nothing Then
                Return ""
            Else
                Return pNode2.InnerText
            End If
        End Function
        Public Function CreateNode_IfNone( _
            ByRef p As Xml.XmlDocument, _
            ByRef pEditNode1 As Xml.XmlNode, _
            ByVal strName As String) As Xml.XmlNode

            Dim pEditNode2 As Xml.XmlNode

            pEditNode2 = pEditNode1.SelectSingleNode(strName)
            If pEditNode2 Is Nothing Then
                pEditNode2 = p.CreateNode(Xml.XmlNodeType.Element, strName, "")
                pEditNode1.AppendChild(pEditNode2)
            End If


            Return pEditNode2
        End Function
    End Module
End Namespace