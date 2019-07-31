Imports System.IO


Public Class C_IniFile
	Private rtn As String
	Private success As String

	Private StrFile As String

	Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
	Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32

	Private Declare Function GetPrivateProfileStringKeys& Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName$, ByVal lpszKey&, ByVal lpszDefault$, ByVal lpszReturnBuffer$, ByVal cchReturnBuffer&, ByVal lpszFile$)
	Private Declare Function GetPrivateProfileStringSections& Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName&, ByVal lpszKey&, ByVal lpszDefault$, ByVal lpszReturnBuffer$, ByVal cchReturnBuffer&, ByVal lpszFile$)
	Private Declare Function WritePrivateProfileStringByKeyName& Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lplFileName As String)
	Private Declare Function WritePrivateProfileStringToDeleteKey& Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As Int32, ByVal lplFileName As String)
	Private Declare Function WritePrivateProfileStringToDeleteSection& Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Int32, ByVal lpString As Int32, ByVal lplFileName As String)


	Function LoadIniString(ByVal Section$, ByVal Key$) As String
		'====================================================
		'描述 :从ini文件中读取设置的值.
		'参数:
		'Argument :
		'section     ----  ini文件中的段名
		'Key         ----  关键字名
		'得到由Section & Key 所确定的字符串
		'注意事项
		'====================================================

		If StrFile <> "" Then
			Dim KeyValue As String
			Dim characters, R As Int32

			KeyValue = Strings.StrDup(128, Chr(0))

			characters = GetPrivateProfileString(Section$, Key$, "", KeyValue, 127, StrFile)

			If characters > 1 Then
				KeyValue = Mid(KeyValue$, 1, characters)
			End If

			R = InStr(KeyValue, Chr(0))

			If R > 1 Then
				LoadIniString = Mid(KeyValue, 1, R - 1)
			ElseIf R = 1 Then
				LoadIniString = ""
			Else
				LoadIniString = KeyValue
			End If
		End If
	End Function


	Function LoadIniString_Extend(ByVal Section$, ByVal Key$, ByVal mLen As Int32) As String
		'====================================================
		''描述 :从ini文件中读取设置的值.
		'参数:
		''Argument :
		''section     ----  ini文件中的段名
		''Key         ----  关键字名
		''得到由section & Key 所确定的字符串
		'注意事项
		'
		'====================================================
		If StrFile <> "" Then
			Dim KeyValue$
			Dim characters As Long, R As Long

			KeyValue$ = Strings.StrDup(mLen + 1, Chr(0))

			characters = GetPrivateProfileString(Section$, Key$, "", KeyValue$, mLen, StrFile)

			If characters > 1 Then
				KeyValue$ = Mid(KeyValue$, 1, characters)
			End If

			R = InStr(KeyValue$, Chr(0))

			If R > 1 Then
				LoadIniString_Extend = Mid(KeyValue$, 1, R - 1)
			ElseIf R = 1 Then
				LoadIniString_Extend = ""
			Else
				LoadIniString_Extend = KeyValue$
			End If
		End If
	End Function

	Function SaveIniString(ByVal Section As String, ByVal Key As String, ByVal KeyValue As String) As String
		'=======================================
		''描述:存储设置值到ini文件中
		'
		''参数:section  段名 Key 关键字
		''KeyValue  要存储的值
		'=======================================
 
		'On Error GoTo 123
		If File.Exists(StrFile) Then

			If File.Exists(StrFile) Then
				If (File.GetAttributes(StrFile) And FileAttributes.ReadOnly) <> 0 Then
					File.SetAttributes(StrFile, FileAttributes.Normal)
				End If
				'去掉只读属性
			End If
			Call WritePrivateProfileString(Section, Key, KeyValue, StrFile)
		End If

		Exit Function
123:
		'Debug.Write("Saveini error ")
	End Function

	Public Sub SetFileName(ByVal myData As String)
		'=======================================
		''描述:设置ini文件
		'=======================================

		StrFile = myData
	End Sub
End Class

