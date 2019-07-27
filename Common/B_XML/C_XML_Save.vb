Imports B_String.Funny
Imports B_File.Funny

Imports System
Imports System.IO
Imports System.Xml


Namespace Funny

    Public Class C_XML_Save 'FunnyNode 建议不要用这个,用 C_FunnyNode_Write

        Dim p As Xml.XmlDocument
        Public str_XML_File As String


        Public Function Save_To_Memory_CData( _
                ByVal strField As String, _
                ByVal strNode As String, _
                ByVal strInnerXML As String) As String
            strInnerXML = S_Strings.CData_Encode(strInnerXML)
            Save_To_Memory(strField, strNode, strInnerXML)
            Return ""
        End Function

        Public Function Save_To_Memory( _
        ByVal strField As String, _
        ByVal strNode As String, _
        ByVal strInnerXML As String) As String

            '外面要用 'pServer.CData_Encode() 先处理
            If str_XML_File = "" Then Return "没有设置文件名"

            Dim pEditNode1, pEditNode2 As Xml.XmlNode
            Dim strReturn As String = ""

            If p Is Nothing Then
                p = New Xml.XmlDocument

                If File.Exists(str_XML_File) = True Then
                    Try
                        p.Load(str_XML_File)
                    Catch ex As Exception
                        p.LoadXml("<Data></Data>")
                    End Try
                Else
                    p.LoadXml("<Data></Data>")
                End If
            End If

            pEditNode1 = p.SelectSingleNode("/Data/" & strField)
            If pEditNode1 Is Nothing Then
                pEditNode1 = p.CreateNode(Xml.XmlNodeType.Element, strField, "")
                p.SelectSingleNode("/Data").AppendChild(pEditNode1)
            End If

            pEditNode2 = pEditNode1.SelectSingleNode(strNode)
            If pEditNode2 Is Nothing Then
                pEditNode2 = p.CreateNode(Xml.XmlNodeType.Element, strNode, "")
                pEditNode1.AppendChild(pEditNode2)
            End If
            If strInnerXML <> "" Then
                pEditNode2.InnerXml = strInnerXML
            End If

            Return strReturn
        End Function

        Public Sub Flush_To_Disk()
            S_SYS.InitDir(str_XML_File)
            p.Save(str_XML_File)
        End Sub

        Public Function Save( _
            ByVal strFile As String, _
            ByVal strField As String, _
            ByVal strNode As String, _
            ByVal strInnerXML As String) As String

            Dim pEditNode1, pEditNode2 As Xml.XmlNode
            Dim strReturn As String = ""

            If p Is Nothing Then
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

            pEditNode1 = p.SelectSingleNode("/Data/" & strField)
            If pEditNode1 Is Nothing Then
                pEditNode1 = p.CreateNode(Xml.XmlNodeType.Element, strField, "")
                p.SelectSingleNode("/Data").AppendChild(pEditNode1)
            End If

            pEditNode2 = pEditNode1.SelectSingleNode(strNode)
            If pEditNode2 Is Nothing Then
                pEditNode2 = p.CreateNode(Xml.XmlNodeType.Element, strNode, "")
                pEditNode1.AppendChild(pEditNode2)
            End If
            If strInnerXML <> "" Then
                pEditNode2.InnerXml = strInnerXML
            End If

            Try
                S_SYS.InitDir(strFile)
                p.Save(strFile)
            Catch ex As Exception
                strReturn = "保存错误!"
            End Try

            Return strReturn
        End Function



        Protected Overrides Sub Finalize()
            p = Nothing
            MyBase.Finalize()
        End Sub
    End Class
End Namespace