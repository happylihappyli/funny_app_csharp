Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization


Namespace Funny


    ''' <summary>
    ''' �й�������Ϣʵ����
    ''' cncxz����棩 2007-2-9
    ''' </summary>
    Public Class C_TimeChina
        Private m_SolarDate As DateTime
        Private m_LunarYear As Integer, m_LunarMonth As Integer, m_LunarDay As Integer
        Private m_IsLeapMonth As Boolean = False
        Private m_LunarYearSexagenary As String = Nothing, m_LunarYearAnimal As String = Nothing
        Private m_LunarYearText As String = Nothing, m_LunarMonthText As String = Nothing, m_LunarDayText As String = Nothing
        Private m_SolarWeekText As String = Nothing, m_SolarConstellation As String = Nothing, m_SolarBirthStone As String = Nothing

#Region "���캯��"

        Public Sub New()

            Me.New(DateTime.Now.[Date])
        End Sub

        ''' <summary>
        ''' ��ָ�����������ڴ����й�������Ϣʵ����
        ''' </summary>
        ''' <param name="date">ָ������������</param>
        Public Sub New([date] As DateTime)
            m_SolarDate = [date]
            LoadFromSolarDate()
        End Sub

        Private Sub LoadFromSolarDate()
            m_IsLeapMonth = False
            m_LunarYearSexagenary = Nothing
            m_LunarYearAnimal = Nothing
            m_LunarYearText = Nothing
            m_LunarMonthText = Nothing
            m_LunarDayText = Nothing
            m_SolarWeekText = Nothing
            m_SolarConstellation = Nothing
            m_SolarBirthStone = Nothing

            m_LunarYear = calendar.GetYear(m_SolarDate)
            m_LunarMonth = calendar.GetMonth(m_SolarDate)
            Dim leapMonth As Integer = calendar.GetLeapMonth(m_LunarYear)

            If leapMonth = m_LunarMonth Then
                m_IsLeapMonth = True
                m_LunarMonth -= 1
            ElseIf leapMonth > 0 AndAlso leapMonth < m_LunarMonth Then
                m_LunarMonth -= 1
            End If

            m_LunarDay = calendar.GetDayOfMonth(m_SolarDate)

            CalcConstellation(m_SolarDate, m_SolarConstellation, m_SolarBirthStone)
        End Sub

#End Region

#Region "��������"

        ''' <summary>
        ''' ��������
        ''' </summary>
        Public Property SolarDate() As DateTime
            Get
                Return m_SolarDate
            End Get
            Set(value As DateTime)
                If m_SolarDate.Equals(value) Then
                    Return
                End If
                m_SolarDate = value
                LoadFromSolarDate()
            End Set
        End Property
        ''' <summary>
        ''' ���ڼ�
        ''' </summary>
        Public ReadOnly Property SolarWeekText() As String
            Get
                If String.IsNullOrEmpty(m_SolarWeekText) Then
                    Dim i As Integer = CInt(m_SolarDate.DayOfWeek)
                    m_SolarWeekText = ChineseWeekName(i)
                End If
                Return m_SolarWeekText
            End Get
        End Property
        ''' <summary>
        ''' ��������
        ''' </summary>
        Public ReadOnly Property SolarConstellation() As String
            Get
                Return m_SolarConstellation
            End Get
        End Property
        ''' <summary>
        ''' ��������ʯ
        ''' </summary>
        Public ReadOnly Property SolarBirthStone() As String
            Get
                Return m_SolarBirthStone
            End Get
        End Property

        ''' <summary>
        ''' �������
        ''' </summary>
        Public ReadOnly Property LunarYear() As Integer
            Get
                Return m_LunarYear
            End Get
        End Property
        ''' <summary>
        ''' �����·�
        ''' </summary>
        Public ReadOnly Property LunarMonth() As Integer
            Get
                Return m_LunarMonth
            End Get
        End Property
        ''' <summary>
        ''' �Ƿ���������
        ''' </summary>
        Public ReadOnly Property IsLeapMonth() As Boolean
            Get
                Return m_IsLeapMonth
            End Get
        End Property
        ''' <summary>
        ''' ������������
        ''' </summary>
        Public ReadOnly Property LunarDay() As Integer
            Get
                Return m_LunarDay
            End Get
        End Property

        ''' <summary>
        ''' �������֧
        ''' </summary>
        Public ReadOnly Property LunarYearSexagenary() As String
            Get
                If String.IsNullOrEmpty(m_LunarYearSexagenary) Then
                    Dim y As Integer = calendar.GetSexagenaryYear(Me.SolarDate)
                    m_LunarYearSexagenary = CelestialStem.Substring((y - 1) Mod 10, 1) & TerrestrialBranch.Substring((y - 1) Mod 12, 1)
                End If
                Return m_LunarYearSexagenary
            End Get
        End Property
        ''' <summary>
        ''' ��������Ф
        ''' </summary>
        Public ReadOnly Property LunarYearAnimal() As String
            Get
                If String.IsNullOrEmpty(m_LunarYearAnimal) Then
                    Dim y As Integer = calendar.GetSexagenaryYear(Me.SolarDate)
                    m_LunarYearAnimal = Animals.Substring((y - 1) Mod 12, 1)
                End If
                Return m_LunarYearAnimal
            End Get
        End Property

        ''' <summary>
        ''' �������ı�
        ''' </summary>
        Public ReadOnly Property LunarYearText() As String
            Get
                If String.IsNullOrEmpty(m_LunarYearText) Then
                    m_LunarYearText = Animals.Substring(calendar.GetSexagenaryYear(New DateTime(m_LunarYear, 1, 1)) Mod 12 - 1, 1)
                    Dim sb As New StringBuilder()
                    Dim year As Integer = Me.LunarYear
                    Dim d As Integer
                    Do
                        d = year Mod 10
                        sb.Insert(0, ChineseNumber(d))
                        year = year \ 10
                    Loop While year > 0
                    m_LunarYearText = sb.ToString()
                End If
                Return m_LunarYearText
            End Get
        End Property
        ''' <summary>
        ''' �������ı�
        ''' </summary>
        Public ReadOnly Property LunarMonthText() As String
            Get
                If String.IsNullOrEmpty(m_LunarMonthText) Then
                    m_LunarMonthText = (If(Me.IsLeapMonth, "��", "")) & ChineseMonthName(Me.LunarMonth - 1)
                End If
                Return m_LunarMonthText
            End Get
        End Property

        ''' <summary>
        ''' �������������ı�
        ''' </summary>
        Public ReadOnly Property LunarDayText() As String
            Get
                If String.IsNullOrEmpty(m_LunarDayText) Then
                    m_LunarDayText = ChineseDayName(Me.LunarDay - 1)
                End If
                Return m_LunarDayText
            End Get
        End Property

#End Region



        'Public Function ConvertTime_FromChina( _
        '    ByVal iYear As Integer, _
        '    ByVal iMonth As Integer, _
        '    ByVal iDay As Integer, _
        '    Optional ByVal iHour As Integer = 12, _
        '    Optional ByVal iMinute As Integer = 0, _
        '    Optional ByVal iSecond As Integer = 0) As Date

        '    Dim p As New System.Globalization.ChineseLunisolarCalendar()
        '    Dim pTime As Date = p.ToDateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond, 0)
        '    Return pTime
        '    MsgBox(pTime.ToString())


        '    Dim conDate, setDate As Date
        '    Dim AddMonth, AddDay, AddYear As Integer
        '    Dim RunYue As Boolean
        '    If tYear > 2300 Or tYear < 1899 Then Return "" '���������Ч�����ڣ��˳� 
        '    AddYear = tYear
        '    RunYue = False
        '    Dim I1 As Integer

        '    '*********************************************
        '    'Ϊũ������ɹ�������

        '    AddMonth = Val(Mid(daList(AddYear - MinYear), 15, 2))
        '    AddDay = Val(Mid(daList(AddYear - MinYear), 17, 2))
        '    conDate = DateSerial(AddYear, AddMonth, AddDay)
        '    AddDay = tDay
        '    If Val("&H" & Mid(daList(AddYear - MinYear), 14, 1)) > 0 Then
        '        If tMonth > Val("&H" & Mid(daList(AddYear - MinYear), 14, 1)) Then
        '            AddDay += 29 ' + Mid(daList(AddYear - MinYear), 13, 1)
        '        End If
        '    End If
        '    For I1 = 1 To tMonth - 1
        '        AddDay = AddDay + 29 + Val(Mid(daList(tYear - MinYear), I1, 1))
        '    Next
        '    setDate = DateAdd("d", AddDay - 1, conDate)
        '    Return setDate  '���ع������� '------- 

        'End Function


        'Public Function ConvertTime_China(ByVal pTime As Date) As String

        '    Dim p As New System.Globalization.ChineseLunisolarCalendar()
        '    Dim iYear As Integer = p.GetYear(pTime)
        '    Dim iMonth As Integer = p.GetMonth(pTime)
        '    Dim iDay As Integer = p.GetDayOfMonth(pTime)
        '    Dim strReturn As String = iYear & "��"

        '    If p.IsLeapYear(iYear) Then
        '        If p.IsLeapMonth(iYear, iMonth) Then
        '            strReturn &= "��" & (iMonth - 1) & "��"
        '        Else
        '            If iMonth < p.GetLeapMonth(iYear) Then
        '                strReturn &= iMonth & "��"
        '            Else
        '                strReturn &= (iMonth - 1) & "��"
        '            End If
        '        End If
        '    Else
        '        strReturn &= iMonth & "��"
        '    End If

        '    strReturn &= iDay & "��"
        '    Return strReturn

        'End Function


        ''' <summary>
        ''' ����ָ���������ڼ�������������ʯ
        ''' </summary>
        ''' <param name="date">ָ����������</param>
        ''' <param name="constellation">����</param>
        ''' <param name="birthstone">����ʯ</param>
        Public Shared Sub CalcConstellation([date] As DateTime, ByRef constellation As String, ByRef birthstone As String)
            Dim i As Integer = Convert.ToInt32([date].ToString("MMdd"))
            Dim j As Integer
            If i >= 321 AndAlso i <= 419 Then
                j = 0
            ElseIf i >= 420 AndAlso i <= 520 Then
                j = 1
            ElseIf i >= 521 AndAlso i <= 621 Then
                j = 2
            ElseIf i >= 622 AndAlso i <= 722 Then
                j = 3
            ElseIf i >= 723 AndAlso i <= 822 Then
                j = 4
            ElseIf i >= 823 AndAlso i <= 922 Then
                j = 5
            ElseIf i >= 923 AndAlso i <= 1023 Then
                j = 6
            ElseIf i >= 1024 AndAlso i <= 1121 Then
                j = 7
            ElseIf i >= 1122 AndAlso i <= 1221 Then
                j = 8
            ElseIf i >= 1222 OrElse i <= 119 Then
                j = 9
            ElseIf i >= 120 AndAlso i <= 218 Then
                j = 10
            ElseIf i >= 219 AndAlso i <= 320 Then
                j = 11
            Else
                constellation = "δ֪����"
                birthstone = "δ֪����ʯ"
                Return
            End If
            constellation = Constellations(j)
            birthstone = BirthStones(j)
            '#Region "��������"
            '��������   3��21��------4��19��     ����ʯ��   ��ʯ   
            '��ţ����   4��20��------5��20��   ����ʯ��   ����ʯ   
            '˫������   5��21��------6��21��     ����ʯ��   ���   
            '��з����   6��22��------7��22��   ����ʯ��   ����   
            'ʨ������   7��23��------8��22��   ����ʯ��   �챦ʯ   
            '��Ů����   8��23��------9��22��   ����ʯ��   ���������   
            '�������   9��23��------10��23��     ����ʯ��   ����ʯ   
            '��Ы����   10��24��-----11��21��     ����ʯ��   è��ʯ   
            '��������   11��22��-----12��21��   ����ʯ��   �Ʊ�ʯ   
            'Ħ������   12��22��-----1��19��   ����ʯ��   ��������   
            'ˮƿ����   1��20��-----2��18��   ����ʯ��   ��ˮ��   
            '˫������   2��19��------3��20��   ����ʯ��   �³�ʯ��Ѫʯ  
            '#End Region
        End Sub

#Region "����ת����"

        ''' <summary>
        ''' ��ȡָ����ݴ��ڵ��գ����³�һ������������
        ''' </summary>
        ''' <param name="year">ָ�������</param>
        Private Shared Function GetLunarNewYearDate(year As Integer) As DateTime
            Dim dt As New DateTime(year, 1, 1)
            Dim cnYear As Integer = calendar.GetYear(dt)
            Dim cnMonth As Integer = calendar.GetMonth(dt)

            Dim num1 As Integer = 0
            Dim num2 As Integer = If(calendar.IsLeapYear(cnYear), 13, 12)

            While num2 >= cnMonth
                num1 += calendar.GetDaysInMonth(cnYear, System.Math.Max(System.Threading.Interlocked.Decrement(num2), num2 + 1))
            End While

            num1 = num1 - calendar.GetDayOfMonth(dt) + 1
            Return dt.AddDays(num1)
        End Function

        ''' <summary>
        ''' ����ת����
        ''' </summary>
        ''' <param name="year">������</param>
        ''' <param name="month">������</param>
        ''' <param name="day">������</param>
        ''' <param name="IsLeapMonth">�Ƿ�����</param>
        Public Shared Function ConvertTime_FromChina(year As Integer, month As Integer, day As Integer, IsLeapMonth As Boolean) As DateTime
            If year < 1902 OrElse year > 2100 Then
                Throw New Exception("ֻ֧��1902��2100�ڼ��ũ����")
            End If
            If month < 1 OrElse month > 12 Then
                Throw New Exception("��ʾ�·ݵ����ֱ�����1��12֮��")
            End If

            If day < 1 OrElse day > calendar.GetDaysInMonth(year, month) Then
                Throw New Exception("ũ��������������")
            End If

            Dim num1 As Integer = 0, num2 As Integer = 0
            Dim leapMonth As Integer = calendar.GetLeapMonth(year)

            If ((leapMonth = month + 1) AndAlso IsLeapMonth) OrElse (leapMonth > 0 AndAlso leapMonth <= month) Then
                num2 = month
            Else
                num2 = month - 1
            End If

            While num2 > 0
                num1 += calendar.GetDaysInMonth(year, System.Math.Max(System.Threading.Interlocked.Decrement(num2), num2 + 1))
            End While

            Dim dt As DateTime = GetLunarNewYearDate(year)
            Return dt.AddDays(num1 + day - 1)
        End Function

        ''' <summary>
        ''' ����ת����
        ''' </summary>
        ''' <param name="date">��������</param>
        ''' <param name="IsLeapMonth">�Ƿ�����</param>
        Public Shared Function ConvertTime_FromChina([date] As DateTime, IsLeapMonth As Boolean) As DateTime
            Return ConvertTime_FromChina([date].Year, [date].Month, [date].Day, IsLeapMonth)
        End Function

        Public Shared Function ConvertTime_China(ByVal strDate As String)
            Dim strReturn As String = ""
            Try
                Dim pChinaDay As C_TimeChina = New C_TimeChina(Convert.ToDateTime(strDate)) ' C_TimeChina = New C_TimeChina
                'pChinaDay.DatDate = Convert.ToDateTime(strNew)
                strReturn = pChinaDay.SolarDate '.ConvertTime_China(Convert.ToDateTime(strNew))
                pChinaDay = Nothing
            Catch ex As Exception

            End Try
            Return strReturn
        End Function
#End Region
#Region "��������������"

        ''' <summary>
        ''' ��������������ʵ��
        ''' </summary>
        ''' <param name="year">������</param>
        ''' <param name="month">������</param>
        ''' <param name="day">������</param>
        ''' <param name="IsLeapMonth">�Ƿ�����</param>
        Public Shared Function FromLunarDate(year As Integer, month As Integer, day As Integer, IsLeapMonth As Boolean) As C_TimeChina
            Dim dt As DateTime = ConvertTime_FromChina(year, month, day, IsLeapMonth)
            Return New C_TimeChina(dt)
        End Function
        ''' <summary>
        ''' ��������������ʵ��
        ''' </summary>
        ''' <param name="date">��������</param>
        ''' <param name="IsLeapMonth">�Ƿ�����</param>
        Public Shared Function FromLunarDate([date] As DateTime, IsLeapMonth As Boolean) As C_TimeChina
            Return FromLunarDate([date].Year, [date].Month, [date].Day, IsLeapMonth)
        End Function

        ''' <summary>
        ''' ��������������ʵ��
        ''' </summary>
        ''' <param name="date">��ʾ�������ڵ�8λ���֣����磺20070209</param>
        ''' <param name="IsLeapMonth">�Ƿ�����</param>
        Public Shared Function FromLunarDate([date] As String, IsLeapMonth As Boolean) As C_TimeChina
            Dim rg As Regex = New System.Text.RegularExpressions.Regex("^/d{7}(/d)$")
            Dim mc As Match = rg.Match([date])
            If Not mc.Success Then
                Throw New Exception("�����ַ�����������")
            End If
            Dim dt As DateTime = DateTime.Parse(String.Format("{0}-{1}-{2}", [date].Substring(0, 4), [date].Substring(4, 2), [date].Substring(6, 2)))
            Return FromLunarDate(dt, IsLeapMonth)
        End Function

#End Region

        Private Shared calendar As New ChineseLunisolarCalendar()
        Public Const ChineseNumber As String = "��һ�����������߰˾�"
        Public Const CelestialStem As String = "���ұ����켺�����ɹ�"
        Public Const TerrestrialBranch As String = "�ӳ���î������δ�����纥"
        Public Const Animals As String = "��ţ������������Ｆ����"
        Public Shared ReadOnly ChineseWeekName As String() = New String() {"������", "����һ", "���ڶ�", "������", "������", "������", _
         "������"}
        Public Shared ReadOnly ChineseDayName As String() = New String() {"��һ", "����", "����", "����", "����", "����", _
         "����", "����", "����", "��ʮ", "ʮһ", "ʮ��", _
         "ʮ��", "ʮ��", "ʮ��", "ʮ��", "ʮ��", "ʮ��", _
         "ʮ��", "��ʮ", "إһ", "إ��", "إ��", "إ��", _
         "إ��", "إ��", "إ��", "إ��", "إ��", "��ʮ"}
        Public Shared ReadOnly ChineseMonthName As String() = New String() {"��", "��", "��", "��", "��", "��", _
         "��", "��", "��", "ʮ", "ʮһ", "ʮ��"}
        Public Shared ReadOnly Constellations As String() = New String() {"������", "��ţ��", "˫����", "��з��", "ʨ����", "��Ů��", _
         "�����", "��Ы��", "������", "Ħ����", "ˮƿ��", "˫����"}
        Public Shared ReadOnly BirthStones As String() = New String() {"��ʯ", "����ʯ", "���", "����", "�챦ʯ", "���������", _
         "����ʯ", "è��ʯ", "�Ʊ�ʯ", "��������", "��ˮ��", "�³�ʯ��Ѫʯ"}

    End Class



    'Public Class C_TimeChina
    '    Public DatDate As Date = Today '��Ψһ���������������ڵģ�������ũ����Ҳ�����ǹ���,��ʼ��Ϊϵͳ��������

    '    '#######################################################
    '    '���ڼ�¼�������յ��Զ�����������
    '    Private Structure SolarHolidayStruct
    '        Dim Month As Integer
    '        Dim Day As Integer
    '        Dim Recess As Integer
    '        Dim HolidayName As String
    '    End Structure

    '    '�����������Զ����������͵ı���
    '    Private sHolidayInfo() As SolarHolidayStruct
    '    Private sFtv() As Object = {1, 1, 1, "Ԫ��", 2, 14, 0, "���˽�", 2, 10, 0, "�����", 3, 8, 0, "��Ů��", 3, 12, 0, "ֲ����", _
    '    3, 15, 0, "Ȩ����", 4, 1, 0, "���˽�", 5, 1, 1, "�Ͷ���", 5, 4, 0, "�����", 5, 12, 0, "��ʿ��", _
    '    5, 31, 0, "������", 6, 1, 0, "��ͯ��", 7, 1, 0, "������", 8, 1, 0, "������", 8, 8, 0, _
    '    "���׽�", 9, 10, 0, "��ʦ��", 10, 1, 0, "�����", 10, 6, 0, "���˽�", 12, 24, 0, "ƽ��ҹ", 12, 25, 0, "ʥ����"}

    '    '��������
    '    Public ReadOnly Property sHoliday(Optional ByVal InActax As Boolean = False) As String
    '        Get
    '            Dim TempDate As Date
    '            Dim i As Long
    '            Dim b As Integer
    '            Dim tempstr As String = ""
    '            If InActax Then
    '                TempDate = ConvertTime_FromChina(DatDate.Year, DatDate.Month, DatDate.Day)
    '            Else
    '                TempDate = DatDate
    '            End If
    '            b = UBound(sHolidayInfo)
    '            For i = 0 To b
    '                If (sHolidayInfo(i).Month = TempDate.Month) And (sHolidayInfo(i).Day = TempDate.Day) Then
    '                    tempstr = sHolidayInfo(i).HolidayName
    '                    Exit For
    '                End If
    '            Next
    '            Return tempstr
    '        End Get
    '    End Property
    '    '************************************************
    '    '���ڼ�¼ũ���Ľ��յ��Զ�����������
    '    Private Structure LunarHolidayStruct
    '        Dim Month As Integer
    '        Dim Day As Integer
    '        Dim Recess As Integer
    '        Dim HlidayName As String
    '    End Structure
    '    '����ũ�����Զ����������͵ı���
    '    Private lHolidayInfo() As LunarHolidayStruct
    '    Private lFtv() As Object = {1, 1, 1, "����", 1, 15, 0, "Ԫ����", 5, 5, 0, "�����", 7, 7, 0, "��Ϧ���˽�", 7, 15, 0, "��Ԫ�ڡ��������", _
    '    8, 15, 0, "�����", 9, 9, 0, "������", 12, 8, 0, "���˽�", 12, 24, 0, "С��", 12, 31, 0, "��Ϧ"}

    '    '��ũ������
    '    'Public ReadOnly Property lHoliday(Optional ByVal InActax As Boolean = False) As String '���в�������True�����������ũ��
    '    '    Get '��ΪFalse����������ǹ�����Ҫ�����ũ��
    '    '        Dim i As Long
    '    '        Dim b As Integer
    '    '        Dim strTemp As String = ""
    '    '        Dim month As Integer
    '    '        Dim day As Integer
    '    '        b = UBound(lHolidayInfo)
    '    '        If InActax = False Then '���������ǹ�����Ҫͨ��GetYLDate���������ũ����
    '    '            mGetChinaDateInfo = ConvertTime_China(Me.DatDate)
    '    '            month = mGetChinaDateInfo.Month
    '    '            day = mGetChinaDateInfo.Day
    '    '        Else
    '    '            month = DatDate.Month
    '    '            day = DatDate.Day
    '    '        End If
    '    '        If month = 12 And (day = 29 Or day = 30) Then
    '    '            If day = 29 And daList((DatDate.Year - 1) - MinYear).Substring(11, 1) = "0" Then
    '    '                strTemp = "��Ϧ"
    '    '            End If
    '    '            If day = 30 Then strTemp = "��Ϧ"
    '    '        Else
    '    '            For i = 0 To b
    '    '                If (lHolidayInfo(i).Month = month) And (lHolidayInfo(i).Day = day) Then
    '    '                    strTemp = lHolidayInfo(i).HlidayName
    '    '                    Exit For
    '    '                End If
    '    '            Next
    '    '        End If
    '    '        Return strTemp
    '    '    End Get
    '    'End Property
    '    '***********************************************
    '    '���ڼ�¼ĳ�µĵڼ������ڼ����յ��Զ�����������
    '    Private Structure WeekHolidayStruct
    '        Dim Month As Integer
    '        Dim WeekAtMonth As Integer
    '        Dim WeekDay As Integer
    '        Dim HolidayName As String
    '    End Structure
    '    '����ĳ�µĵڼ������ڼ��Ľ����Զ����������͵ı���
    '    Private wHolidayInfo() As WeekHolidayStruct
    '    Private wFtv() As Object = {5, 2, 0, "����ĸ�׽�", 5, 3, 0, "ȫ��������", 6, 3, 0, "���ʸ��׽�", 9, 3, 2, "���ʺ�ƽ��", _
    '    9, 3, 6, "ȫ������������", 9, 4, 0, "�������˽�", 10, 1, 1, "����ס����", 10, 2, 3, "���ʼ�����Ȼ�ֺ���", _
    '    10, 2, 4, "���簮����", 11, 4, 4, "�ж���"}
    '    '��ĳ�µĵڼ������ڵĽ���
    '    Public ReadOnly Property wHoliday(Optional ByVal InActax As Boolean = False) As String
    '        Get
    '            Dim TempDate As Date
    '            Dim intw As Integer
    '            Dim inti As Integer
    '            Dim intb As Integer
    '            Dim datFirstDay As Date
    '            Dim strTemp As String = ""
    '            If InActax Then
    '                TempDate = ConvertTime_FromChina(DatDate.Year, DatDate.Month, DatDate.Day) '���InActaxΪTrue����˵���������ũ����Ҫ����ɹ���
    '            Else
    '                TempDate = DatDate
    '            End If
    '            intb = UBound(wHolidayInfo)
    '            For inti = 0 To intb
    '                If wHolidayInfo(inti).Month = TempDate.Month Then
    '                    intw = TempDate.DayOfWeek
    '                    If wHolidayInfo(inti).WeekDay = intw Then
    '                        datFirstDay = DateSerial(TempDate.Year, TempDate.Month, 1)
    '                        If (DateDiff(DateInterval.WeekOfYear, datFirstDay, TempDate)) = wHolidayInfo(inti).WeekAtMonth Then
    '                            strTemp = wHolidayInfo(inti).HolidayName
    '                        End If
    '                    End If
    '                End If
    '            Next
    '            Return strTemp
    '        End Get
    '    End Property
    '    '***********************************************
    '    '����������
    '    Public Function Constellation(Optional ByVal InActax As Boolean = False) As String
    '        Dim datTemp As Date = DatDate
    '        If InActax Then
    '            datTemp = ConvertTime_FromChina(DatDate.Year, DatDate.Month, DatDate.Day)
    '        End If
    '        Dim intYear As Integer = datTemp.Year
    '        If datTemp >= DateSerial(intYear, 1, 20) And datTemp <= DateSerial(intYear, 2, 18) Then
    '            Return "ˮ������ƿ��"
    '        ElseIf datTemp >= DateSerial(intYear, 2, 19) And datTemp <= DateSerial(intYear, 3, 20) Then
    '            Return "˫����"
    '        ElseIf datTemp >= DateSerial(intYear, 3, 21) And datTemp <= DateSerial(intYear, 4, 19) Then
    '            Return "������"
    '        ElseIf datTemp >= DateSerial(intYear, 4, 20) And datTemp <= DateSerial(intYear, 5, 20) Then
    '            Return "��ţ��"
    '        ElseIf datTemp >= DateSerial(intYear, 5, 21) And datTemp <= DateSerial(intYear, 6, 20) Then
    '            Return "˫����"
    '        ElseIf datTemp >= DateSerial(intYear, 6, 21) And datTemp <= DateSerial(intYear, 7, 22) Then
    '            Return "��з��"
    '        ElseIf datTemp >= DateSerial(intYear, 7, 23) And datTemp <= DateSerial(intYear, 8, 22) Then
    '            Return "ʨ����"
    '        ElseIf datTemp >= DateSerial(intYear, 8, 23) And datTemp <= DateSerial(intYear, 9, 22) Then
    '            Return "��Ů��"
    '        ElseIf datTemp >= DateSerial(intYear, 9, 23) And datTemp <= DateSerial(intYear, 10, 22) Then
    '            Return "�����"
    '        ElseIf datTemp >= DateSerial(intYear, 10, 23) And datTemp <= DateSerial(intYear, 11, 21) Then
    '            Return "��Ы��"
    '        ElseIf datTemp >= DateSerial(intYear, 11, 22) And datTemp <= DateSerial(intYear, 12, 21) Then
    '            Return "������"
    '        ElseIf (datTemp >= DateSerial(intYear, 12, 22) And datTemp <= DateSerial(intYear, 12, DateTime.DaysInMonth(intYear, 12))) _
    '        Or (datTemp >= DateSerial(intYear, 1, 1) And datTemp <= DateSerial(intYear, 1, 19)) Then
    '            Return "Ħ����"
    '        Else
    '            Return "���󣡣�����"
    '        End If
    '    End Function

    '    '*****************************************************
    '    '���ص��µ�24����
    '    Private SolarTerm() As String = {"С��", "����", "����", "��ˮ", "����", "����", "����", "����", "����", "С��", _
    '    "â��", "����", "С��", "����", "����", "����", "��¶", "���", "��¶", "˪��", "����", "Сѩ", "��ѩ", "����"}
    '    Private STermInfo() As String = {0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, _
    '    331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758}

    '    Public Function lSolarTermMonth(Optional ByVal InActax As Boolean = False) As String '���µļ���
    '        Dim TempDate As Date
    '        Dim BASEDATEANDTIME As Date
    '        Dim newdate As Date
    '        Dim num As Double
    '        Dim y As Long
    '        Dim tempstr As String = ""
    '        If InActax Then
    '            TempDate = ConvertTime_FromChina(DatDate.Year, DatDate.Month, DatDate.Day)
    '        Else
    '            TempDate = DatDate
    '        End If
    '        BASEDATEANDTIME = #1/6/1900 2:05:00 AM#
    '        y = TempDate.Year
    '        Dim j As Integer
    '        For j = 1 To Date.DaysInMonth(TempDate.Year, TempDate.Month)
    '            Dim i As Integer
    '            For i = 1 To 24
    '                num = 525948.76 * (y - 1900) + STermInfo(i - 1)
    '                newdate = DateAdd("n", num, BASEDATEANDTIME)
    '                If Math.Abs(DateDiff("d", newdate, DateSerial(TempDate.Year, TempDate.Month, j))) = 0 Then
    '                    tempstr = tempstr & TempDate.Month & "��" & j & "�գ�" & SolarTerm(i - 1) & Chr(13) & Chr(10)
    '                    j += 1
    '                End If
    '            Next
    '        Next
    '        Return tempstr
    '    End Function

    '    '���յļ���
    '    Public Function lSolarTermDay(Optional ByVal d As Date = #12:00:00 AM#, Optional ByVal InActax As Boolean = False) As String
    '        Dim TempDate As Date
    '        Dim BASEDATEANDTIME As Date
    '        Dim newdate As Date
    '        Dim num As Double
    '        Dim y As Long
    '        Dim tempstr As String = ""
    '        If d = #12:00:00 AM# Then
    '            If InActax Then
    '                TempDate = ConvertTime_FromChina(DatDate.Year, DatDate.Month, DatDate.Day)
    '            Else
    '                TempDate = DatDate
    '            End If
    '        Else
    '            TempDate = d
    '        End If
    '        BASEDATEANDTIME = #1/6/1900 2:05:00 AM#
    '        y = TempDate.Year
    '        Dim i As Integer
    '        For i = 1 To 24
    '            num = 525948.76 * (y - 1900) + STermInfo(i - 1)
    '            newdate = DateAdd("n", num, BASEDATEANDTIME)
    '            If Math.Abs(DateDiff("d", newdate, TempDate)) = 0 Then
    '                tempstr = SolarTerm(i - 1)
    '            End If
    '        Next
    '        Return tempstr
    '    End Function

    '    '*******************************************************
    '    'VB����ũ�����㷨 
    '    '������һ������VB��ũ���㷨 
    '    '�������ݶ��巽������ 
    '    'ǰ12���ֽڴ���1-12��Ϊ���»���С�£�1Ϊ����30�죬0ΪС��29�죬 
    '    '��13λΪ���µ������1Ϊ����30�죬0ΪС��29�죬��14λΪ���µ��� 
    '    '�ݣ������������Ϊ0����������·ݣ�10��11��12�ֱ���A��B��C���� 
    '    'ʾ����ʹ��16���ơ����4λΪ�����ũ������-��ũ��1��1�����ڹ��� 
    '    '�����ڣ���0131����1��31�ա� 
    '    'GetYLDate����ʹ�÷�ʽ����tYearΪҪ������꣬tMonthΪ�£�tDayΪ 
    '    '���ڣ�YLyear�Ƿ���ֵ������ũ������ݣ�������꣬YLShuXing���� 
    '    '������������IsGetGl�������ǲ���ͨ��ũ��ȡ����ֵ������ǣ� 
    '    'ǰ����������Ӧ�Ĺ������ڣ����ҷ���ֵ��һ���������ڡ� 
    '    'Private datTemp As Date
    '    Private Const MinYear As Integer = 1899
    '    Private daList(2300 - MinYear) As String

    '    '���ũ���գ���������GetChinaDateInfo.ChinaDay��һ���ģ���������Ե�Ŀ�����ܹ�����Ļ�ȡũ����
    '    Private mLLYDay As String
    '    Public ReadOnly Property LLYDay() As String
    '        Get
    '            Return mLLYDay
    '        End Get
    '    End Property


    '    '************************************************
    '    Public Structure udtChinaDateInfo '��¼ũ����Ϣ
    '        Dim ChinaDate As String '��¼ũ������
    '        Dim ChinaYear As String '��¼ũ����
    '        Dim ChinaMonth As String '��¼ũ����
    '        Dim ChinaDay As String '��¼ũ����
    '        Dim Month As Integer '��¼�����͵�ũ����
    '        Dim Day As Integer '��¼�����͵�ũ����
    '        Dim WeekDate As String ' ��¼����
    '        Dim Animal As String '��¼����
    '        Dim Constellation As String '��¼����
    '        Dim TermMonth As String '��¼���½���
    '        Dim TermDay As String '��¼���յĽ���
    '        Dim ChinaRestDay As String '��¼ũ������
    '        Dim RestDay As String '��¼��������
    '    End Structure
    '    Private mGetChinaDateInfo As udtChinaDateInfo
    '    '�����û�����ũ����Ϣ��ֻ������
    '    'Public ReadOnly Property GetChinaDateInfo() As udtChinaDateInfo
    '    '    Get
    '    '        mGetChinaDateInfo.WeekDate = Microsoft.VisualBasic.Format(DatDate, "dddd")
    '    '        mGetChinaDateInfo.Constellation = Constellation()
    '    '        mGetChinaDateInfo.TermMonth = lSolarTermMonth()
    '    '        If (lSolarTermDay() = "" Or DatDate.Day = 1) Then
    '    '            mGetChinaDateInfo.TermDay = lSolarTermDay()
    '    '        Else
    '    '            If lSolarTermDay(DateSerial(DatDate.Year, DatDate.Month, DatDate.Day - 1)) = "" Then
    '    '                mGetChinaDateInfo.TermDay = lSolarTermDay()
    '    '            Else
    '    '                mGetChinaDateInfo.TermDay = ""
    '    '            End If
    '    '        End If
    '    '        mGetChinaDateInfo.ChinaRestDay = lHoliday
    '    '        mGetChinaDateInfo.RestDay = sHoliday

    '    '        Return mGetChinaDateInfo
    '    '    End Get
    '    'End Property
    '    '***************************************************
    '    'һЩ��ʼ������
    '    Public Sub New()
    '        '---------------------------------------------------
    '        '��ʼ����������
    '        Dim b As Long
    '        Dim i As Integer
    '        b = UBound(sFtv) + 1
    '        ReDim sHolidayInfo(b / 4)
    '        For i = 0 To (b / 4) - 1
    '            sHolidayInfo(i).Month = sFtv(i * 4)
    '            sHolidayInfo(i).Day = sFtv(i * 4 + 1)
    '            sHolidayInfo(i).Recess = sFtv(i * 4 + 2)
    '            sHolidayInfo(i).HolidayName = sFtv(i * 4 + 3)
    '        Next
    '        '---------------------------------------------------
    '        '��ʼ��ũ������
    '        b = UBound(lFtv) + 1
    '        ReDim lHolidayInfo(b / 4)
    '        For i = 0 To (b / 4) - 1
    '            lHolidayInfo(i).Month = lFtv(i * 4)
    '            lHolidayInfo(i).Day = lFtv(i * 4 + 1)
    '            lHolidayInfo(i).Recess = lFtv(i * 4 + 2)
    '            lHolidayInfo(i).HlidayName = lFtv(i * 4 + 3)
    '        Next
    '        '---------------------------------------------------------
    '        '��ʼ��ĳ�µĵڼ������ڼ��Ľ���
    '        b = UBound(wFtv) + 1
    '        ReDim wHolidayInfo(b / 4)
    '        For i = 0 To (b / 4) - 1
    '            wHolidayInfo(i).Month = wFtv(i * 4)
    '            wHolidayInfo(i).WeekAtMonth = wFtv(i * 4 + 1)
    '            wHolidayInfo(i).WeekDay = wFtv(i * 4 + 2)
    '            wHolidayInfo(i).HolidayName = wFtv(i * 4 + 3)
    '        Next
    '        '---------------------------------------------------------
    '        '1899 to 2300
    '        daList(1899 - MinYear) = "101010110101000210"
    '        daList(1900 - MinYear) = "010010110110180131"
    '        daList(1901 - MinYear) = "010010101110000219"
    '        daList(1902 - MinYear) = "101001010111000208"
    '        daList(1903 - MinYear) = "010100100110150129"
    '        daList(1904 - MinYear) = "110100100110000216"
    '        daList(1905 - MinYear) = "110110010101000204"
    '        daList(1906 - MinYear) = "011010101010140125"
    '        daList(1907 - MinYear) = "010101101010000213"
    '        daList(1908 - MinYear) = "100110101101000202"
    '        daList(1909 - MinYear) = "010010101110120122"
    '        daList(1910 - MinYear) = "010010101110000210"
    '        daList(1911 - MinYear) = "101001001101160130"
    '        daList(1912 - MinYear) = "101001001101000218"
    '        daList(1913 - MinYear) = "110100100101000206"
    '        daList(1914 - MinYear) = "110101010100150126"
    '        daList(1915 - MinYear) = "101101010101000214"
    '        daList(1916 - MinYear) = "010101101010000204"
    '        daList(1917 - MinYear) = "100101101101020123"
    '        daList(1918 - MinYear) = "100101011011000211"
    '        daList(1919 - MinYear) = "010010011011170201"
    '        daList(1920 - MinYear) = "010010011011000220"
    '        daList(1921 - MinYear) = "101001001011000208"
    '        daList(1922 - MinYear) = "101100100101150128"
    '        daList(1923 - MinYear) = "011010100101000216"
    '        daList(1924 - MinYear) = "011011010100000205"
    '        daList(1925 - MinYear) = "101011011010140124"
    '        daList(1926 - MinYear) = "001010110110000213"
    '        daList(1927 - MinYear) = "100101010111000202"
    '        daList(1928 - MinYear) = "010010010111120123"
    '        daList(1929 - MinYear) = "010010010111000210"
    '        daList(1930 - MinYear) = "011001001011060130"
    '        daList(1931 - MinYear) = "110101001010000217"
    '        daList(1932 - MinYear) = "111010100101000206"
    '        daList(1933 - MinYear) = "011011010100150126"
    '        daList(1934 - MinYear) = "010110101101000214"
    '        daList(1935 - MinYear) = "001010110110000204"
    '        daList(1936 - MinYear) = "100100110111030124"
    '        daList(1937 - MinYear) = "100100101110000211"
    '        daList(1938 - MinYear) = "110010010110170131"
    '        daList(1939 - MinYear) = "110010010101000219"
    '        daList(1940 - MinYear) = "110101001010000208"
    '        daList(1941 - MinYear) = "110110100101060127"
    '        daList(1942 - MinYear) = "101101010101000215"
    '        daList(1943 - MinYear) = "010101101010000205"
    '        daList(1944 - MinYear) = "101010101101140125"
    '        daList(1945 - MinYear) = "001001011101000213"
    '        daList(1946 - MinYear) = "100100101101000202"
    '        daList(1947 - MinYear) = "110010010101120122"
    '        daList(1948 - MinYear) = "101010010101000210"
    '        daList(1949 - MinYear) = "101101001010170129"
    '        daList(1950 - MinYear) = "011011001010000217"
    '        daList(1951 - MinYear) = "101101010101000206"
    '        daList(1952 - MinYear) = "010101011010150127"
    '        daList(1953 - MinYear) = "010011011010000214"
    '        daList(1954 - MinYear) = "101001011011000203"
    '        daList(1955 - MinYear) = "010100101011130124"
    '        daList(1956 - MinYear) = "010100101011000212"
    '        daList(1957 - MinYear) = "101010010101080131"
    '        daList(1958 - MinYear) = "111010010101000218"
    '        daList(1959 - MinYear) = "011010101010000208"
    '        daList(1960 - MinYear) = "101011010101060128"
    '        daList(1961 - MinYear) = "101010110101000215"
    '        daList(1962 - MinYear) = "010010110110000205"
    '        daList(1963 - MinYear) = "101001010111040125"
    '        daList(1964 - MinYear) = "101001010111000213"
    '        daList(1965 - MinYear) = "010100100110000202"
    '        daList(1966 - MinYear) = "111010010011030121"
    '        daList(1967 - MinYear) = "110110010101000209"
    '        daList(1968 - MinYear) = "010110101010170130"
    '        daList(1969 - MinYear) = "010101101010000217"
    '        daList(1970 - MinYear) = "100101101101000206"
    '        daList(1971 - MinYear) = "010010101110150127"
    '        daList(1972 - MinYear) = "010010101101000215"
    '        daList(1973 - MinYear) = "101001001101000203"
    '        daList(1974 - MinYear) = "110100100110140123"
    '        daList(1975 - MinYear) = "110100100101000211"
    '        daList(1976 - MinYear) = "110101010010180131"
    '        daList(1977 - MinYear) = "101101010100000218"
    '        daList(1978 - MinYear) = "101101101010000207"
    '        daList(1979 - MinYear) = "100101101101060128"
    '        daList(1980 - MinYear) = "100101011011000216"
    '        daList(1981 - MinYear) = "010010011011000205"
    '        daList(1982 - MinYear) = "101001001011140125"
    '        daList(1983 - MinYear) = "101001001011000213"
    '        daList(1984 - MinYear) = "1011001001011A0202"
    '        daList(1985 - MinYear) = "011010100101000220"
    '        daList(1986 - MinYear) = "011011010100000209"
    '        daList(1987 - MinYear) = "101011011010060129"
    '        daList(1988 - MinYear) = "101010110110000217"
    '        daList(1989 - MinYear) = "100100110111000206"
    '        daList(1990 - MinYear) = "010010010111150127"
    '        daList(1991 - MinYear) = "010010010111000215"
    '        daList(1992 - MinYear) = "011001001011000204"
    '        daList(1993 - MinYear) = "011010100101030123"
    '        daList(1994 - MinYear) = "111010100101000210"
    '        daList(1995 - MinYear) = "011010110010180131"
    '        daList(1996 - MinYear) = "010110101100000219"
    '        daList(1997 - MinYear) = "101010110110000207"
    '        daList(1998 - MinYear) = "100101101101050128"
    '        daList(1999 - MinYear) = "100100101110000216"
    '        daList(2000 - MinYear) = "110010010110000205"
    '        daList(2001 - MinYear) = "110101001010140124"
    '        daList(2002 - MinYear) = "110101001010000212"
    '        daList(2003 - MinYear) = "110110100101000201"
    '        daList(2004 - MinYear) = "011101010101020122"
    '        daList(2005 - MinYear) = "010101101010000209"
    '        daList(2006 - MinYear) = "101010101101170129"
    '        daList(2007 - MinYear) = "001001011101000218"
    '        daList(2008 - MinYear) = "100100101101000207"
    '        daList(2009 - MinYear) = "110010010101150126"
    '        daList(2010 - MinYear) = "101010010101000214"
    '        daList(2011 - MinYear) = "101101001010000214"
    '        daList(2012 - MinYear) = "101110101010040123"
    '        daList(2013 - MinYear) = "101011010101000210"
    '        daList(2014 - MinYear) = "010101011101090131"
    '        daList(2015 - MinYear) = "010010111010000219"
    '        daList(2016 - MinYear) = "101001011011000208"
    '        daList(2017 - MinYear) = "010100010111160128"
    '        daList(2018 - MinYear) = "010100101011000216"
    '        daList(2019 - MinYear) = "101010010011000205"
    '        daList(2020 - MinYear) = "011110010101040125"
    '        daList(2021 - MinYear) = "011010101010000212"
    '        daList(2022 - MinYear) = "101011010101000201"
    '        daList(2023 - MinYear) = "010110110101020122"
    '        daList(2024 - MinYear) = "010010110110000210"
    '        daList(2025 - MinYear) = "101001101110060129"
    '        daList(2026 - MinYear) = "101001001110000217"
    '        daList(2027 - MinYear) = "110100100110000206"
    '        daList(2028 - MinYear) = "111010100110050126"
    '        daList(2029 - MinYear) = "110101010011000213"
    '        daList(2030 - MinYear) = "010110101010000203"
    '        daList(2031 - MinYear) = "011101101010030123"
    '        daList(2032 - MinYear) = "100101101101000211"
    '        daList(2033 - MinYear) = "010010111101070131"
    '        daList(2034 - MinYear) = "010010101101000219"
    '        daList(2035 - MinYear) = "101001001101000208"
    '        daList(2036 - MinYear) = "110100001011160128"
    '        daList(2037 - MinYear) = "110100100101000215"
    '        daList(2038 - MinYear) = "110101010010000204"
    '        daList(2039 - MinYear) = "110111010100050124"
    '        daList(2040 - MinYear) = "101101011010000212"
    '        daList(2041 - MinYear) = "010101101101000201"
    '        daList(2042 - MinYear) = "010101011011020122"
    '        daList(2043 - MinYear) = "010010011011000210"
    '        daList(2044 - MinYear) = "101001010111070130"
    '        daList(2045 - MinYear) = "101001001011000217"
    '        daList(2046 - MinYear) = "101010100101000206"
    '        daList(2047 - MinYear) = "101100100101150126"
    '        daList(2048 - MinYear) = "011011010010000214"
    '        daList(2049 - MinYear) = "101011011010000202"
    '        daList(2050 - MinYear) = "010010110110130123"
    '        daList(2051 - MinYear) = "100100110111000211"
    '        daList(2052 - MinYear) = "010010011111080201"
    '        daList(2053 - MinYear) = "010010010111000219"
    '        daList(2054 - MinYear) = "011001001011000208"
    '        daList(2055 - MinYear) = "011010001010160128"
    '        daList(2056 - MinYear) = "111010100101000215"
    '        daList(2057 - MinYear) = "011010110010000204"
    '        daList(2058 - MinYear) = "101001101100140124"
    '        daList(2059 - MinYear) = "101010101110000212"
    '        daList(2060 - MinYear) = "100100101110000202"
    '        daList(2061 - MinYear) = "110100101110030121"
    '        daList(2062 - MinYear) = "110010010110000209"
    '        daList(2063 - MinYear) = "110101010101070129"
    '        daList(2064 - MinYear) = "110101001010000217"
    '        daList(2065 - MinYear) = "110110100101000205"
    '        daList(2066 - MinYear) = "010111010101050126"
    '        daList(2067 - MinYear) = "010101101010000214"
    '        daList(2068 - MinYear) = "101001101101000203"
    '        daList(2069 - MinYear) = "010101011101040123"
    '        daList(2070 - MinYear) = "010100101101000211"
    '        daList(2071 - MinYear) = "101010011011080131"
    '        daList(2072 - MinYear) = "101010010101000219"
    '        daList(2073 - MinYear) = "101101001010000207"
    '        daList(2074 - MinYear) = "101101101010060127"
    '        daList(2075 - MinYear) = "101011010101000215"
    '        daList(2076 - MinYear) = "010101011010000205"
    '        daList(2077 - MinYear) = "101010111010040124"
    '        daList(2078 - MinYear) = "101001011011000212"
    '        daList(2079 - MinYear) = "010100101011000202"
    '        daList(2080 - MinYear) = "101100100111030122"
    '        daList(2081 - MinYear) = "011010010011000209"
    '        daList(2082 - MinYear) = "011100110011070129"
    '        daList(2083 - MinYear) = "011010100110000217"
    '        daList(2084 - MinYear) = "101011010101000206"
    '        daList(2085 - MinYear) = "011010110101050126"
    '        daList(2086 - MinYear) = "010010110110000214"
    '        daList(2087 - MinYear) = "101001010111000203"
    '        daList(2088 - MinYear) = "010101001110040124"
    '        daList(2089 - MinYear) = "110100010110000210"
    '        daList(2090 - MinYear) = "111010010110080130"
    '        daList(2091 - MinYear) = "110101010010000218"
    '        daList(2092 - MinYear) = "110110101010000207"
    '        daList(2093 - MinYear) = "011010101010160127"
    '        daList(2094 - MinYear) = "010101101101000215"
    '        daList(2095 - MinYear) = "010010101110000205"
    '        daList(2096 - MinYear) = "101010011101040125"
    '        daList(2097 - MinYear) = "101000101101000212"
    '        daList(2098 - MinYear) = "110100010101000201"
    '        daList(2099 - MinYear) = "111100100101020121"
    '        daList(2100 - MinYear) = "110101010010000209"
    '        daList(2101 - MinYear) = "110110110010070129"
    '        daList(2102 - MinYear) = "101101011010000217"
    '        daList(2103 - MinYear) = "010101011101000207"
    '        daList(2104 - MinYear) = "010011011011050128"
    '        daList(2105 - MinYear) = "010010011011000215"
    '        daList(2106 - MinYear) = "101001001011000204"
    '        daList(2107 - MinYear) = "110101001011040124"
    '        daList(2108 - MinYear) = "101010100101000212"
    '        daList(2109 - MinYear) = "101101010101090131"
    '        daList(2110 - MinYear) = "011011010010000219"
    '        daList(2111 - MinYear) = "101011010110000208"
    '        daList(2112 - MinYear) = "010101110110060129"
    '        daList(2113 - MinYear) = "100100110111000216"
    '        daList(2114 - MinYear) = "010010010111000206"
    '        daList(2115 - MinYear) = "011010010111040126"
    '        daList(2116 - MinYear) = "010101001011000214"
    '        daList(2117 - MinYear) = "011010100101000202"
    '        daList(2118 - MinYear) = "011110100101030122"
    '        daList(2119 - MinYear) = "011010101010000210"
    '        daList(2120 - MinYear) = "101010101010170130"
    '        daList(2121 - MinYear) = "101010101101000217"
    '        daList(2122 - MinYear) = "010100101110000207"
    '        daList(2123 - MinYear) = "110010101110050127"
    '        daList(2124 - MinYear) = "101010010110000215"
    '        daList(2125 - MinYear) = "110101001010000203"
    '        daList(2126 - MinYear) = "111001001010140123"
    '        daList(2127 - MinYear) = "110110010101000211"
    '        daList(2128 - MinYear) = "010110101101090201"
    '        daList(2129 - MinYear) = "010101101010000219"
    '        daList(2130 - MinYear) = "101001101101000208"
    '        daList(2131 - MinYear) = "010100011101160129"
    '        daList(2132 - MinYear) = "010100101101000217"
    '        daList(2133 - MinYear) = "101010001101000205"
    '        daList(2134 - MinYear) = "110100010101150125"
    '        daList(2135 - MinYear) = "101100101010000213"
    '        daList(2136 - MinYear) = "101101010101000202"
    '        daList(2137 - MinYear) = "011011010101020100"
    '        daList(2138 - MinYear) = "010101011010000210"
    '        daList(2139 - MinYear) = "101001011010170130"
    '        daList(2140 - MinYear) = "101001011011000218"
    '        daList(2141 - MinYear) = "010100101011000207"
    '        daList(2142 - MinYear) = "101010010111050127"
    '        daList(2143 - MinYear) = "011010001011000215"
    '        daList(2144 - MinYear) = "011100101001000204"
    '        daList(2145 - MinYear) = "101110101010040123"
    '        daList(2146 - MinYear) = "011010110101000211"
    '        daList(2147 - MinYear) = "0010110110110B0201"
    '        daList(2148 - MinYear) = "010010110110000220"
    '        daList(2149 - MinYear) = "101001010111000208"
    '        daList(2150 - MinYear) = "010100101110060129"
    '        daList(2151 - MinYear) = "110100010110000216"
    '        daList(2152 - MinYear) = "111010001011000205"
    '        daList(2153 - MinYear) = "011011010010050125"
    '        daList(2154 - MinYear) = "110110101001000212"
    '        daList(2155 - MinYear) = "010110110101000202"
    '        daList(2156 - MinYear) = "001101101101030123"
    '        daList(2157 - MinYear) = "001010101110000210"
    '        daList(2158 - MinYear) = "101000111101070130"
    '        daList(2159 - MinYear) = "101000101101000218"
    '        daList(2160 - MinYear) = "110100010101000207"
    '        daList(2161 - MinYear) = "110101010101060126"
    '        daList(2162 - MinYear) = "101101010010000214"
    '        daList(2163 - MinYear) = "110101101001000203"
    '        daList(2164 - MinYear) = "010101011010140124"
    '        daList(2165 - MinYear) = "010101011011000211"
    '        daList(2166 - MinYear) = "0010101011000A0201"
    '        daList(2167 - MinYear) = "010001011011000220"
    '        daList(2168 - MinYear) = "101000101011060128"
    '        daList(2169 - MinYear) = "101010101011060128"
    '        daList(2170 - MinYear) = "101010010101000216"
    '        daList(2171 - MinYear) = "101101001010000205"
    '        daList(2172 - MinYear) = "101100101010150125"
    '        daList(2173 - MinYear) = "101011010101000212"
    '        daList(2174 - MinYear) = "010101011011000202"
    '        daList(2175 - MinYear) = "001010110111030123"
    '        daList(2176 - MinYear) = "010001010111000211"
    '        daList(2177 - MinYear) = "011000110111070130"
    '        daList(2178 - MinYear) = "010100101011000218"
    '        daList(2179 - MinYear) = "011010010101000207"
    '        daList(2180 - MinYear) = "011011010101060127"
    '        daList(2181 - MinYear) = "010110101010000214"
    '        daList(2182 - MinYear) = "101010110101000203"
    '        daList(2183 - MinYear) = "010101101101040124"
    '        daList(2184 - MinYear) = "010010101110000212"
    '        daList(2185 - MinYear) = "101001011110080131"
    '        daList(2186 - MinYear) = "101001010110000219"
    '        daList(2187 - MinYear) = "110100101010000208"
    '        daList(2188 - MinYear) = "111010101010060128"
    '        daList(2189 - MinYear) = "110101010101000215"
    '        daList(2190 - MinYear) = "010110101010000205"
    '        daList(2191 - MinYear) = "101011101010050125"
    '        daList(2192 - MinYear) = "101001101101000213"
    '        daList(2193 - MinYear) = "010010101110000202"
    '        daList(2194 - MinYear) = "101010101011030121"
    '        daList(2195 - MinYear) = "101001001101000210"
    '        daList(2196 - MinYear) = "110100101011070130"
    '        daList(2197 - MinYear) = "101100101001000217"
    '        daList(2198 - MinYear) = "101101010101000206"
    '        daList(2199 - MinYear) = "010101010101160127"
    '        daList(2200 - MinYear) = "001011011010000215"
    '        daList(2201 - MinYear) = "100101011101000204"
    '        daList(2202 - MinYear) = "010001011011140125"
    '        daList(2203 - MinYear) = "010010011011000213"
    '        daList(2204 - MinYear) = "101001001111090202"
    '        daList(2205 - MinYear) = "011001001011000220"
    '        daList(2206 - MinYear) = "011010101001000209"
    '        daList(2207 - MinYear) = "101101101001060129"
    '        daList(2208 - MinYear) = "011010110101000217"
    '        daList(2209 - MinYear) = "001010110110000206"
    '        daList(2210 - MinYear) = "100110110110040126"
    '        daList(2211 - MinYear) = "100100110111000214"
    '        daList(2212 - MinYear) = "010010010111000204"
    '        daList(2213 - MinYear) = "011010010110030123"
    '        daList(2214 - MinYear) = "111001001010000210"
    '        daList(2215 - MinYear) = "111010101010070130"
    '        daList(2216 - MinYear) = "110110101001000218"
    '        daList(2217 - MinYear) = "010110110111000207"
    '        daList(2218 - MinYear) = "001011101101050128"
    '        daList(2219 - MinYear) = "001010101110000216"
    '        daList(2220 - MinYear) = "100100101110000205"
    '        daList(2221 - MinYear) = "110000101101140124"
    '        daList(2222 - MinYear) = "110010010101000212"
    '        daList(2223 - MinYear) = "110101001101090201"
    '        daList(2224 - MinYear) = "101101001010000220"
    '        daList(2225 - MinYear) = "101101101001000208"
    '        daList(2226 - MinYear) = "010101111010070129"
    '        daList(2227 - MinYear) = "010101011011000217"
    '        daList(2228 - MinYear) = "001001011101000207"
    '        daList(2229 - MinYear) = "100101011011050126"
    '        daList(2230 - MinYear) = "100100101011000214"
    '        daList(2231 - MinYear) = "101010010101000203"
    '        daList(2232 - MinYear) = "110010010101130123"
    '        daList(2233 - MinYear) = "101101001010000210"
    '        daList(2234 - MinYear) = "101101011010080130"
    '        daList(2235 - MinYear) = "101011010101000218"
    '        daList(2236 - MinYear) = "010101011011000208"
    '        daList(2237 - MinYear) = "001000110111150128"
    '        daList(2238 - MinYear) = "001001010111000216"
    '        daList(2239 - MinYear) = "010100101011000205"
    '        daList(2240 - MinYear) = "101000101011140125"
    '        daList(2241 - MinYear) = "011010010101000212"
    '        daList(2242 - MinYear) = "011011001101090201"
    '        daList(2243 - MinYear) = "010110101010000220"
    '        daList(2244 - MinYear) = "101010110101000209"
    '        daList(2245 - MinYear) = "010010101101160129"
    '        daList(2246 - MinYear) = "010010101110000217"
    '        daList(2247 - MinYear) = "101001010111000206"
    '        daList(2248 - MinYear) = "010101001101050127"
    '        daList(2249 - MinYear) = "110100100110000213"
    '        daList(2250 - MinYear) = "111010010101000202"
    '        daList(2251 - MinYear) = "011101010101030123"
    '        daList(2252 - MinYear) = "010110101010000211"
    '        daList(2253 - MinYear) = "101010111010070130"
    '        daList(2254 - MinYear) = "100101011101000218"
    '        daList(2255 - MinYear) = "010010101110000208"
    '        daList(2256 - MinYear) = "101001011011060128"
    '        daList(2257 - MinYear) = "101001001101000215"
    '        daList(2258 - MinYear) = "110100100101000204"
    '        daList(2259 - MinYear) = "110110100101050124"
    '        daList(2260 - MinYear) = "101101010101000212"
    '        daList(2261 - MinYear) = "010101101010000201"
    '        daList(2262 - MinYear) = "101011011010010121"
    '        daList(2263 - MinYear) = "100101011011000209"
    '        daList(2264 - MinYear) = "010010110111070130"
    '        daList(2265 - MinYear) = "010010011011000217"
    '        daList(2266 - MinYear) = "101001001011000206"
    '        daList(2267 - MinYear) = "101101001011050126"
    '        daList(2268 - MinYear) = "011010100101000214"
    '        daList(2269 - MinYear) = "101011010100000202"
    '        daList(2270 - MinYear) = "101010110101130122"
    '        daList(2271 - MinYear) = "001010110110000211"
    '        daList(2272 - MinYear) = "100101010110180131"
    '        daList(2273 - MinYear) = "100100110111000218"
    '        daList(2274 - MinYear) = "010010010111000208"
    '        daList(2275 - MinYear) = "011001010110060128"
    '        daList(2276 - MinYear) = "111001001010000215"
    '        daList(2277 - MinYear) = "111010100101000203"
    '        daList(2278 - MinYear) = "011010101001140124"
    '        daList(2279 - MinYear) = "010110101101000212"
    '        daList(2280 - MinYear) = "001010110110000202"
    '        daList(2281 - MinYear) = "101010101110020121"
    '        daList(2282 - MinYear) = "100100101110000209"
    '        daList(2283 - MinYear) = "110010101101060129"
    '        daList(2284 - MinYear) = "110010010101000217"
    '        daList(2285 - MinYear) = "110101001010000205"
    '        daList(2286 - MinYear) = "110111001010050125"
    '        daList(2287 - MinYear) = "101101100101000213"
    '        daList(2288 - MinYear) = "010101101010000203"
    '        daList(2289 - MinYear) = "101101011011030122"
    '        daList(2290 - MinYear) = "001001011101000211"
    '        daList(2291 - MinYear) = "100100111011070131"
    '        daList(2292 - MinYear) = "100100101011000219"
    '        daList(2293 - MinYear) = "101010010101000207"
    '        daList(2294 - MinYear) = "101101010101060127"
    '        daList(2295 - MinYear) = "011101001010000215"
    '        daList(2296 - MinYear) = "101101010101000204"
    '        daList(2297 - MinYear) = "010111010101040124"
    '        daList(2298 - MinYear) = "010011011010000212"
    '        daList(2299 - MinYear) = "101001011011000201"
    '        daList(2300 - MinYear) = "011001010111020122"
    '    End Sub
    'End Class

    Public Module S_Time
        Public Function GetWeekTime(ByVal index As Integer) As String
            Dim strReturn As String = ""
            Select Case index
                Case 0
                    strReturn = "��"
                Case 1
                    strReturn = "һ"
                Case 2
                    strReturn = "��"
                Case 3
                    strReturn = "��"
                Case 4
                    strReturn = "��"
                Case 5
                    strReturn = "��"
                Case 6
                    strReturn = "��"
            End Select

            Return strReturn
        End Function
    End Module



End Namespace
