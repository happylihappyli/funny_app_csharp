Imports System
Imports System.IO
Imports System.Xml


Namespace Funny


    Public Class C_XML_Read

        Dim p As Xml.XmlDocument

        Public Enum Read_Type
            C_InnerXML = 1
            C_InnerText = 2
            C_Attribute = 3
        End Enum

        Public Function Read_From_File( _
            ByVal strFile As String, _
            ByVal strXPath As String, _
            ByVal pType As Read_Type, _
            ByVal strAttriBute As String, _
            ByVal NewXML As Boolean, _
            Optional ByRef bFileExist As Boolean = False) As String

            '这里的XPath从根目录开始
            Dim pNode As Xml.XmlNode
            Dim strReturn As String = ""


            If p Is Nothing OrElse NewXML Then
                p = New Xml.XmlDocument
                If File.Exists(strFile) = True Then
                    Try
                        p.Load(strFile)
                    Catch ex As Exception
                        p.LoadXml("<Data></Data>")
                    End Try
                Else
                    p.LoadXml("<Data></Data>")
                End If
            End If

            pNode = p.SelectSingleNode(strXPath)

            If Not pNode Is Nothing Then
                Select Case pType
                    Case Read_Type.C_InnerText
                        strReturn = pNode.InnerText
                    Case Read_Type.C_InnerXML
                        strReturn = pNode.InnerXml
                    Case Read_Type.C_Attribute
                        strReturn = pNode.Attributes(strAttriBute).Value
                End Select
            End If

            Return strReturn
        End Function


        Public Function Read_From_XML( _
            ByVal strXML As String) As XmlDocument

            '这里的XPath从根目录开始
            Dim strReturn As String = ""

            p = New XmlDocument
            Do While strXML.Substring(0, 1) <> "<"
                strXML = strXML.Substring(1)
            Loop

            Try
                p.LoadXml(strXML)
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            Return p
        End Function

        Public Function Get_Value_From_XPath( _
            ByVal strXPath As String, _
            ByVal pType As Read_Type, _
            ByVal strAttriBute As String) As String

            If p Is Nothing Then
                Return "先读取XML,用Read_From_XML"
            End If

            '这里的XPath从根目录开始
            Dim pNode As Xml.XmlNode
            Dim strReturn As String = ""

            pNode = p.SelectSingleNode(strXPath)

            If Not pNode Is Nothing Then
                Select Case pType
                    Case Read_Type.C_InnerText
                        strReturn = pNode.InnerText
                    Case Read_Type.C_InnerXML
                        strReturn = pNode.InnerXml
                    Case Read_Type.C_Attribute
                        strReturn = pNode.Attributes(strAttriBute).Value
                End Select
            End If

            Return strReturn
        End Function
    End Class
End Namespace