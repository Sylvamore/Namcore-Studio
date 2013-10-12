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
'*      /Filename:      Basics
'*      /Description:   Includes basic and frequently used functions
'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Imports System.Drawing
Imports System.Net
Imports NCFramework.Framework.Logging
Imports NCFramework.Framework.Modules
Imports System.Windows.Forms
Imports NCFramework.Framework.Forms

Namespace Framework.Functions

    Public Module Basics

        '// Declaration
        Public Tmpset As Integer
        '// Declaration

        Public Sub InitializeDbc()
            LogAppend("Initializing DBC files", "Basics_InitializeDBC", True)
            libnc.Main.Initialize()
        End Sub

        Public Function GetCharacterSetBySetId(ByVal setId As Integer) As Character
            If Tmpset = setId Then
                Return GlobalVariables.TempCharacter
            End If
            If GlobalVariables.globChars.CharacterSetsIndex.Contains("setId:" & setId.ToString() & "|") Then
                'found
                Tmpset = setId
                GlobalVariables.TempCharacter =
                    GlobalVariables.globChars.CharacterSets(TryInt(SplitString(GlobalVariables.globChars.CharacterSetsIndex,
                                                                               "[setId:" & setId.ToString() & "|@", "]")))
                Return GlobalVariables.TempCharacter
            Else
                'not found
                Return Nothing
            End If
        End Function

        Public Sub AddCharacterSet(ByVal setId As Integer, ByVal player As Character)
            GlobalVariables.globChars.CharacterSets.Add(player)
            GlobalVariables.globChars.CharacterSetsIndex = GlobalVariables.globChars.CharacterSetsIndex & "[setId:" &
                                                           setId.ToString & "|@" &
                                                           (GlobalVariables.globChars.CharacterSets.Count - 1).ToString &
                                                           "]"
        End Sub

        Public Sub SetCharacterSet(ByVal setId As Integer, ByVal character As Character)
            If GlobalVariables.globChars.CharacterSetsIndex.Contains("setId:" & setId.ToString() & "|") Then
                'found
                GlobalVariables.globChars.CharacterSets(TryInt(SplitString(GlobalVariables.globChars.CharacterSetsIndex,
                                                                           "[setId:" & setId.ToString() & "|@", "]"))) =
                    character
            Else
                'not found
            End If
        End Sub

        Public Sub AddCharacterArmorItem(ByRef player As Character, ByVal itm As Item)
            If player.ArmorItems Is Nothing Then player.ArmorItems = New List(Of Item)
            player.ArmorItems.Add(itm)
            player.ArmorItemsIndex = player.ArmorItemsIndex & "[slot:" & itm.Slotname & "|@" &
                                     (player.ArmorItems.Count - 1).ToString & "]"
            player.ArmorItemsIndex = player.ArmorItemsIndex & "[slotnum:" & itm.Slot.ToString & "|@" &
                                     (player.ArmorItems.Count - 1).ToString & "]"
        End Sub

        Public Sub RemoveCharacterArmorItem(ByRef player As Character, ByVal itm As Item)
            If player.ArmorItems Is Nothing Then player.ArmorItems = New List(Of Item)
            Dim itmIndex As Integer = TryInt(SplitString(player.ArmorItemsIndex, "[slotnum:" & itm.Slot.ToString() & "|@",
                                                         "]"))
            player.ArmorItems.Item(itmIndex) = New Item With {.Id = 0, .Slot = itm.Slot, .Slotname = itm.Slotname}
            player.ArmorItemsIndex = player.ArmorItemsIndex.Replace("[slot:" & itm.Slotname & "|@" & itmIndex.ToString & "]",
                                                                    "")
            player.ArmorItemsIndex =
                player.ArmorItemsIndex.Replace("[slotnum:" & itm.Slot.ToString() & "|@" & itmIndex.ToString & "]", "")
        End Sub

        Public Sub SetCharacterArmorItem(ByRef player As Character, ByVal itm As Item)
            If _
                player.ArmorItemsIndex.Contains("[slot:" & itm.Slotname & "|@") Or
                player.ArmorItemsIndex.Contains("[slotnum:" & itm.Slot.ToString & "|@") Then
                player.ArmorItems(TryInt(SplitString(player.ArmorItemsIndex, "[slot:" & itm.Slotname & "|@", "]"))) = itm
                player.ArmorItems(TryInt(SplitString(player.ArmorItemsIndex, "[slotnum:" & itm.Slot.ToString & "|@", "]"))) _
                    = itm
            Else

            End If
        End Sub

        Public Function GetCharacterArmorItem(ByVal player As Character, ByVal slot As String,
                                              Optional isint As Boolean = False) As Item
            If _
                player.ArmorItemsIndex.Contains("[slot:" & slot & "|@") Or
                player.ArmorItemsIndex.Contains("[slotnum:" & slot & "|@") Then
                If isint = True Then
                    Dim result As Item = player.ArmorItems(TryInt(SplitString(player.ArmorItemsIndex, "[slotnum:" & slot & "|@", "]")))
                    If result.Id = 0 Then
                        Return Nothing
                    Else
                        Return result
                    End If
                Else
                    Dim result As Item = player.ArmorItems(TryInt(SplitString(player.ArmorItemsIndex, "[slot:" & slot & "|@", "]")))
                    If result.Id = 0 Then
                        Return Nothing
                    Else
                        Return result
                    End If
                End If

            Else
                Return Nothing
            End If
        End Function

        Public Sub AddCharacterGlyph(ByRef player As Character, ByVal gly As Glyph)
            If player.PlayerGlyphs Is Nothing Then player.PlayerGlyphs = New List(Of Glyph)
            player.PlayerGlyphs.Add(gly)
            player.PlayerGlyphsIndex = player.PlayerGlyphsIndex & "[slot:" & gly.Slotname & "|@" &
                                       (player.PlayerGlyphs.Count - 1).ToString & "]"
        End Sub

        Public Sub SetCharacterGlyph(ByRef player As Character, ByVal glph As Glyph)
            If player.PlayerGlyphsIndex.Contains("[slot:" & glph.Slotname & "|@") Then
                player.PlayerGlyphs(TryInt(SplitString(player.PlayerGlyphsIndex, "[slot:" & glph.Slotname & "|@", "]"))) =
                    glph
            Else

            End If
        End Sub

        Public Function GetCharacterGlyph(ByVal player As Character, ByVal slot As String) As Glyph
            If player.PlayerGlyphsIndex Is Nothing Then Return Nothing
            If player.PlayerGlyphsIndex.Contains("[slot:" & slot & "|@") Then
                Return player.PlayerGlyphs(TryInt(SplitString(player.PlayerGlyphsIndex, "[slot:" & slot & "|@", "]")))
            Else
                Return Nothing
            End If
        End Function

        Public Function SplitString(ByVal source As String, ByVal start As String, ByVal ending As String) As String
            If source Is Nothing Or start Is Nothing Or ending Is Nothing Then
                LogAppend("Failed to split a string: source might be nothing", "Basics_splitString", False, True)
                Return Nothing
            End If
            LogAppend(
                "Splitting a string. Sourcelength/-/Start/-/End: " & source.Length.ToString & "/-/" & start & "/-/" & ending,
                "Basics_splitString", False)
            Try
                Dim quellcode As String = source
                Dim mystart As String = start
                Dim myend As String = ending
                Dim quellcodeSplit As String
                quellcodeSplit = Split(quellcode, mystart, 5)(1)
                Return Split(quellcodeSplit, myend, 6)(0)
            Catch ex As Exception
                LogAppend(
                    "Error while splitting string! -> Returning nothing -> Exception is: ###START###" & ex.ToString() &
                    "###END###", "Basics_splitString", False, True)
                Return Nothing
            End Try
        End Function

        Public Function SplitList(ByVal source As String, ByVal category As String) As String
            LogAppend("Splitting a list. Sourcelength/-/Start/-/End: " & source.Length.ToString & "/-/" & category,
                      "Basics_splitList", False)
            Try
                Dim quellcode As String = source
                Dim mystart As String = "<" & category & ">"
                Dim myend As String = "</" & category & ">"
                Dim quellcodeSplit As String
                quellcodeSplit = Split(quellcode, mystart, 5)(1)
                quellcodeSplit = Split(quellcodeSplit, myend, 6)(0)
                Return quellcodeSplit
            Catch ex As Exception
                LogAppend(
                    "Error while splitting list! -> Returning nothing -> Exception is: ###START###" & ex.ToString() &
                    "###END###", "Basics_splitList", False, True)
                Return Nothing
            End Try
        End Function

        Public Function LoadImageFromUrl(ByRef url As String) As Image
            LogAppend("Loading image from url: " & url, "Basics_LoadImageFromUrl", False)
            Try
                Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)
                request.Proxy = GlobalVariables.GlobalWebClient.Proxy
                Dim response As HttpWebResponse = DirectCast(request.GetResponse, HttpWebResponse)
                Dim img As Image = Image.FromStream(response.GetResponseStream())
                response.Close()
                Return img
            Catch ex As Exception
                LogAppend("Error while loading image: " & ex.ToString(), "Basics_LoadImageFromUrl", False, True)
                Return Nothing
            End Try
        End Function

        Public Sub CloseProcessStatus()
            If GlobalVariables.DebugMode = False Then
                For Each myForm As Form In Application.OpenForms
                    If myForm.Name = "ProcessStatus" Then
                        Application.DoEvents()
                        Try
                            myForm.Close()
                        Catch ex As Exception : End Try
                    End If
                Next
            End If
        End Sub

        Public Sub NewProcessStatus()
            For Each myForm As Form In Application.OpenForms
                If myForm.Name = "ProcessStatus" Then
                    If GlobalVariables.DebugMode = True Then
                        Exit Sub
                    Else
                        Application.DoEvents()
                        Try
                            myForm.Close()
                        Catch ex As Exception : End Try
                    End If
                End If
            Next
            GlobalVariables.procStatus = New ProcessStatus
            GlobalVariables.procStatus.Show()
            Application.DoEvents()
        End Sub
    End Module
End Namespace