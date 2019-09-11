
Imports System.IO
Imports System.Web
Imports B_Data.Funny

Namespace FunnyWeber


    Public Class C_Queue_File_Msg
        Public pQue As C_Queue

        Dim lngSize As Int32

        Public Sub New(ByVal Size As Int32) 'ByVal strFile As String, 
            pQue = New C_Queue
            lngSize = Size
        End Sub

        Public Class C_Item_Msg
            Public ID As Long    'Time Or ID
            Public Msg As String
            Public SenderID As Int32
        End Class

        Public Sub Remove_ByID(ByVal lngID As Long)
            '通过ID比较,删除一个Msg

            Dim pItem As C_Item_Msg
            Dim i As Long

            Dim pQue_Item As C_Queue.C_Queue_Item = pQue.Peek_Queue()
            Dim pTmp As C_Queue.C_Queue_Item = Nothing

            For i = 1 To pQue.Count
                If Not pQue_Item Is Nothing Then
                    pItem = pQue_Item.pObject
                    If Not pItem Is Nothing Then
                        pTmp = pQue_Item.pNext
                        If pItem.ID = lngID Then
                            pQue.Remove(pQue_Item)
                        End If
                    End If
                    pQue_Item = pTmp
                End If
            Next
        End Sub

        Public Sub AddItem_NoRepeat_ByID( _
                 ByVal lngID As Int32, _
                 ByVal strMsg As String, _
                 ByVal Max As Long)

            Call Remove_ByID(lngID)

            '在添加
            AddItem(lngID, strMsg, Max, 0)

        End Sub

        Public Function DeQue() As C_Item_Msg
            Dim pItem As C_Item_Msg = Nothing
            If pQue.Count > 0 Then
                pItem = pQue.DeQue()
            End If
            Return pItem
        End Function

        Public Function DeQue_Msg() As String
            Dim pItem As C_Item_Msg
            If pQue.Count > 0 Then
                pItem = pQue.DeQue()
                If pItem Is Nothing Then
                    Return ""
                Else
                    Return pItem.Msg
                End If
            Else
                Return ""
            End If
        End Function

        Public Sub AddItem( _
         ByVal lngID As Long, _
         ByVal strMsg As String, _
         ByVal Max As Long, _
         ByVal SenderID As Int32)

            If strMsg = "" Then Exit Sub

            Dim pItem As C_Item_Msg = New C_Item_Msg
            pItem.ID = lngID
            pItem.Msg = strMsg
            pItem.SenderID = SenderID

            pQue.EnQue(pItem)

            If pQue.Count > Max Then
                pQue.DeQue()
            End If
        End Sub


        Public Sub Read(ByVal strFile As String)
            Dim buffer() As Byte
            Dim encoder As New System.Text.UnicodeEncoding
            Dim FS As FileStream
            Dim BR As BinaryReader
            Dim Pos As Long, lngID As Long
            Dim strMsg As String
            Dim lngLen, i, Count As Int32

            If File.Exists(strFile) = False Then Exit Sub

            FS = New FileStream(strFile, FileMode.Open)
            BR = New BinaryReader(FS)

            Try
                FS.Seek(0, SeekOrigin.Begin)
                Count = BR.ReadInt32()

                FS.Seek(1024 * 2, SeekOrigin.Begin)
            Catch ex As Exception
                GoTo MyExit
            End Try

            For i = 0 To Count - 1
                Pos = 1024 * 2 + i * (lngSize + 12 + 130) '130 为冗余
                BR.BaseStream.Seek(Pos, SeekOrigin.Begin)

                Try
                    lngID = BR.ReadInt64()
                    lngLen = BR.ReadInt32()
                    buffer = BR.ReadBytes(lngLen)
                    strMsg = encoder.GetString(buffer)
                    Me.AddItem(lngID, strMsg, 1000, 0)
                Catch ex As Exception
                    GoTo MyExit
                End Try
            Next
            '==================================

MyExit:
            BR.Close() : BR = Nothing
            FS.Close() : FS = Nothing
        End Sub


        Public Sub InitDir_Sub(ByVal StrFile As String)
            Dim StrDir As String, Index As Long
            Index = InStrRev(StrFile, "\")

            StrDir = Left(StrFile, Index)

            If StrDir = "" Then Exit Sub

            '如果这个目录不存在则创建一个目录

            Try
                If Directory.Exists(StrDir) = False Then
                    Directory.CreateDirectory(StrDir)
                End If
            Catch ex As Exception
                'HttpContext.Current.Response.Write("请设置服务器目录权限,创建目录错误:" & StrFile & "!")
                'HttpContext.Current.Response.End()
            End Try
        End Sub


        Public Sub Save( _
            ByVal strFile As String, _
            ByVal bIIS As Boolean)

            Dim buffer() As Byte
            Dim encoder As New System.Text.UnicodeEncoding
            Dim FS As FileStream
            Dim BW As BinaryWriter
            Dim Pos As Long
            Dim pItem As C_Item_Msg
            Dim i As Int32  'Length,
            Dim pQue_Item As C_Queue.C_Queue_Item = pQue.Peek_Queue()

            If bIIS Then
                HttpContext.Current.Application.Lock()
            End If

            InitDir_Sub(strFile)
            FS = New FileStream(strFile, FileMode.Create)
            BW = New BinaryWriter(FS)

            Dim j As Int32 = 0

            For i = 0 To pQue.Count - 1
                If Not pQue_Item Is Nothing Then
                    pItem = pQue_Item.pObject
                    If pItem Is Nothing Then
                    Else
                        If pItem.Msg Is Nothing Then
                            pItem.Msg = "none"
                        End If
                        buffer = encoder.GetBytes(pItem.Msg) ', 0, Length, buffer, 0)

                        Pos = 1024 * 2 + i * (lngSize + 12 + 130) '130 为冗余

                        BW.Seek(Pos, SeekOrigin.Begin)
                        BW.Write(pItem.ID)
                        'BW.Seek(Pos + 8, SeekOrigin.Begin) 可以省略
                        BW.Write(Math.Min(buffer.Length, lngSize))
                        'BW.Seek(Pos + 8+4, SeekOrigin.Begin) 可以省略
                        BW.Write(buffer, 0, Math.Min(buffer.Length, lngSize))
                    End If

                    pQue_Item = pQue_Item.pNext
                    j += 1
                End If
            Next

            BW.Seek(0, SeekOrigin.Begin)
            BW.Write(j)
            BW.Close() : BW = Nothing
            FS.Close() : FS = Nothing

            If bIIS Then
                HttpContext.Current.Application.UnLock()
            End If
        End Sub


    End Class


End Namespace