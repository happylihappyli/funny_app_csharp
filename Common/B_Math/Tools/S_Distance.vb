
Imports System.Drawing

Public Class S_Distance
    Public Shared Function Distance(ByVal p1 As Point, ByVal p2 As Point) As Double

        Return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y))
    End Function
End Class
