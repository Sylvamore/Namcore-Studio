﻿'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
'* Copyright (C) 2013 Namcore Studio <https://github.com/megasus/Namcore-Studio>
'*
'* This program is free software; you can redistribute it and/or modify it
'* under the terms of the GNU General Public License as published by the
'* Free Software Foundation; either version 2 of the License, or (at your
'* option) any later version.
'*
'* This program is distributed in the hope that it will be useful, but WITHOUT
'* ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
'* FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
'* more details.
'*
'* You should have received a copy of the GNU General Public License along
'* with this program. If not, see <http://www.gnu.org/licenses/>.
'*
'* Developed by Alcanmage/megasus
'*
'* //FileInfo//
'*      /Filename:      Interface_Operator
'*      /Description:   Includes operations for rendering user interfaces
'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Imports Namcore_Studio.GlobalVariables
Public Class Interface_Operator
    Public Shared Sub prepareLive_armory()
        Dim i As Integer
        While trd.IsAlive
            i += 1
            If i Mod 8 = 0 Then Application.DoEvents()
        End While
        Dim askjd As String = temporaryCharacterInformation.Item(1)
        MsgBox("adas")
    End Sub
    Public Shared Sub loadNamesAndPics()

    End Sub
End Class
