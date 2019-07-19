Imports Microsoft.Win32

Namespace Funny

    Public Module S_Reg
        Public Function QueryValue_LocalMachine(ByVal sKeyName As String, ByVal sValueName As String) As String

            Dim key As RegistryKey = Microsoft.Win32.Registry.LocalMachine ' .CurrentUser '如上
            Dim subkey As RegistryKey = key.OpenSubKey(sKeyName, True) 'subkey即为HKEY_USERS\software\vb.net键

            Return subkey.GetValue(sValueName) 'value


            'Dim lRetVal As Long         'result of the API functions
            ''Dim hkey As Long         'handle of opened key
            'Dim vValue As Object      'setting of queried value

            ''HKEY_LOCAL_MACHINE

            'lRetVal = RegOpenKeyEx(hKey, sKeyName, 0, KEY_ALL_ACCESS, hKey)
            'lRetVal = QueryValueEx(hKey, sValueName, vValue)
            'QueryValue = vValue
            'RegCloseKey(hKey)
        End Function

    End Module
End Namespace
