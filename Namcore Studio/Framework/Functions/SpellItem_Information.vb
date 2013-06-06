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
'*      /Filename:      SpellItem_Information
'*      /Description:   Includes functions for locating certain item and spell information
'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


Imports Namcore_Studio.GlobalVariables
Imports Namcore_Studio.Basics
Imports Namcore_Studio.EventLogging
Imports Namcore_Studio.Conversions
Imports System.Net
Public Class SpellItem_Information
    Public Shared Function GetGlyphIdByItemId(ByVal itemid As Integer) As Integer
        LogAppend("Loading GlyphId by ItemId " & itemid.ToString, "SpellItem_Information_GetGlyphIdByItemId", False)
        Dim xpacressource As String
        Try
            Select Case expansion
                Case 3
                    xpacressource = My.Resources.GlyphProperties_335
                Case 4
                    xpacressource = My.Resources.GlyphProperties_434
                Case Else
                    xpacressource = My.Resources.GlyphProperties_335
            End Select
            Dim client As New WebClient
            Return tryint(splitString(client.DownloadString("http://www.wowhead.com/spell=" & splitString(xpacressource, "<entry>" & itemid.ToString & "</entry><spell>", "</spell>")), ",""id"":", ",""level"""))
        Catch ex As Exception
            LogAppend("Error while loading GlyphId! -> Returning 0 -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetGlyphIdByItemId", False, True)
            Return 0
        End Try
    End Function
    Public Shared Function GetIconByItemId(ByVal itemid As Integer) As Image
        If itemid = 0 Then Return Nothing
        Dim client As New WebClient
        Try
            LogAppend("Loading icon by ItemId " & itemid.ToString, "SpellItem_Information_GetIconByItemId", False)
            Dim itemContext As String = client.DownloadString("http://www.wowhead.com/item=" & itemid.ToString & "&xml")
            Return LoadImageFromUrl("http://wow.zamimg.com/images/wow/icons/large/" & (splitString(itemContext, "<icon displayId=""" & splitString(itemContext, "<icon displayId=""", """>") & """>", "</icon>")).ToLower() & ".jpg")
        Catch ex As Exception
            LogAppend("Error while loading icon! -> Returning nothing -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetIconByItemId", False, True)
            Return Nothing
        End Try
    End Function
    Public Shared Function GetRarityByItemId(ByVal itemid As Integer) As Integer
        If itemid = 0 Then Return Nothing
        Dim client As New WebClient
        Try
            LogAppend("Loading rarity by ItemId " & itemid.ToString, "SpellItem_Information_GetRarityByItemId", False)
            Dim itemContext As String = client.DownloadString("http://www.wowhead.com/item=" & itemid.ToString & "&xml")
            Return TryInt(splitString(itemContext, "<quality id=""", """>"))
        Catch ex As Exception
            LogAppend("Error while loading rarity! -> Returning nothing -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetRarityByItemId", False, True)
            Return Nothing
        End Try
    End Function
    Public Shared Function GetSlotByItemId(ByVal itemid As Integer) As Integer
        If itemid = 0 Then Return Nothing
        Dim client As New WebClient
        Try
            LogAppend("Loading inventorySlot by ItemId " & itemid.ToString, "SpellItem_Information_GetSlotByItemId", False)
            Dim itemContext As String = client.DownloadString("http://www.wowhead.com/item=" & itemid.ToString & "&xml")
            Return TryInt(splitString(itemContext, "<inventorySlot id=""", """>"))
        Catch ex As Exception
            LogAppend("Error while loading inventorySlot! -> Returning nothing -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetSlotByItemId", False, True)
            Return Nothing
        End Try
    End Function
    Public Shared Sub LoadWeaponType(ByVal itemid As Integer, ByVal tar_set As Integer)
        If Not itemid = 0 Then
            LogAppend("Loading weapon type of Item " & itemid.ToString, "SpellItem_Information_LoadWeaponType", False)
            Try
                Dim client As New WebClient
                Dim player As Character = GetCharacterSetBySetId(tar_set)
                Dim excerpt As String = splitString(client.DownloadString("http://www.wowhead.com/item=" & itemid.ToString & "&xml"), "<subclass id=", "</subclass>")
                Select Case True
                    Case excerpt.ToLower.Contains(" crossbow ") '5011
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 5011})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 226})
                    Case excerpt.ToLower.Contains(" bow ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 264})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 45})
                    Case excerpt.ToLower.Contains(" gun ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 266})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 46})
                    Case excerpt.ToLower.Contains(" thrown ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 2764})
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 2567})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 176})
                    Case excerpt.ToLower.Contains(" wands ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 5009})
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 5019})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 228})
                    Case excerpt.ToLower.Contains(" sword ")
                        If excerpt.ToLower.Contains(" one-handed ") Then
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 201})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 43})
                        Else
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 201})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 43})
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 202})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 55})
                        End If
                    Case excerpt.ToLower.Contains(" dagger ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 1180})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 173})
                    Case excerpt.ToLower.Contains(" axe ")
                        If excerpt.ToLower.Contains(" one-handed ") Then
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 196})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 44})
                        Else
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 197})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 44})
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 196})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 142})
                        End If
                    Case excerpt.ToLower.Contains(" mace ")
                        If excerpt.ToLower.Contains(" one-handed ") Then
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 198})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 54})
                        Else
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 54})
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 198})
                            player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 160})
                            player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 199})
                        End If
                    Case excerpt.ToLower.Contains(" polearm ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 200})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 229})
                    Case excerpt.ToLower.Contains(" staff ")
                        player.Spells.Add(New Spell With {.active = 1, .disabled = 0, .id = 227})
                        player.Skills.Add(New Skill With {.value = 100, .max = 100, .id = 136})
                    Case Else : End Select
            Catch ex As Exception
                LogAppend("Error while loading weapon type! -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_LoadWeaponType", False, True)
            End Try
        Else : End If
    End Sub
    Public Shared Function GetEffectNameByEffectId(ByVal effectid As Integer) As String
        LogAppend("Loading effectname by effectId: " & effectid.ToString, "SpellItem_Information_GetEffectNameByEffectId", False)
        If effectname_dt.Rows.Count = 0 Then
            Try
                effectname_dt.Clear()
                effectname_dt = New DataTable()
                Dim stext As String
                If My.Settings.language = "de" Then
                    stext = My.Resources.enchant_name_de
                Else
                    stext = My.Resources.enchant_name_en
                End If
                Dim a() As String
                Dim strArray As String()
                a = Split(stext, vbNewLine)
                For i = 0 To UBound(a)
                    strArray = a(i).Split(CChar(";"))
                    If i = 0 Then
                        For Each value As String In strArray
                            effectname_dt.Columns.Add(value.Trim())
                        Next
                    Else
                        effectname_dt.Rows.Add(strArray)
                    End If
                Next i
            Catch ex As Exception
                LogAppend("Error filling datatable! -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetEffectNameByEffectId", False, True)
                Return "Error loading effectname"
                Exit Function
            End Try
        End If
        Dim nameresult As String = Execute("effectid", effectid.ToString(), effectname_dt)
        If nameresult = "-" Then
            LogAppend("Entry not found -> Returning error message", "SpellItem_Information_GetEffectNameByEffectId", False, True)
            Return "Error loading effect name"
        Else
            Return nameresult
        End If
    End Function

    Public Shared Function getNameOfItem(ByVal itemid As String) As String
        LogAppend("Loading name of item: " & itemid.ToString, "SpellItem_Information_getNameOfItem", False)
        If Not itemid = Nothing Then
            If itemid.Length > 1 Then
                Dim client As New WebClient
                Try
                    If My.Settings.language = "de" Then
                        Dim clString As String = client.DownloadString("http://de.wowhead.com/item=" & itemid.ToString() & "&xml")
                        Return splitString(clString, "<name><![CDATA[", "]]></name>")
                    Else
                        Dim clString As String = client.DownloadString("http://wowhead.com/item=" & itemid.ToString() & "&xml")
                        Return splitString(clString, "<name><![CDATA[", "]]></name>")
                    End If
                Catch ex As Exception
                    LogAppend("Error while loading item name! -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_getNameOfItem", False, True)
                    Return "Error loading item name"
                End Try
            End If
        End If
        LogAppend("ItemId is nothing -> Returning error", "SpellItem_Information_getNameOfItem", False, True)
        Return "Error loading item name"
    End Function
    Public Shared Function GetGemEffectName(ByVal socketid As Integer) As String
        LogAppend("Loading effect name of gem: " & socketid.ToString, "SpellItem_Information_GetGemEffectName", False)
        Try
            Dim client As New WebClient
            Dim effectname As String
            If My.Settings.language = "de" Then
                effectname = client.DownloadString("http://de.wowhead.com/item=" & socketid.ToString & "&xml")
            Else
                effectname = client.DownloadString("http://www.wowhead.com/item=" & socketid.ToString & "&xml")
            End If
            effectname = splitString(effectname, "<span class=""q1"">", "</span>")

            If effectname.Contains("<a href") Then
                Try
                    effectname = effectname.Replace("<a href=""" & splitString(effectname, "<a href=""", """>") & """>", "")
                    effectname = effectname.Replace("</a>", "")
                    Return effectname
                Catch ex As Exception
                    Return effectname
                End Try
            Else
                Return effectname
            End If
        Catch ex As Exception
            LogAppend("Error while loading effect name! -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_GetGemEffectName", False, True)
            Return "Error loading effect name"
        End Try

    End Function
    Private Shared Function Execute(ByVal field As String, ByVal isvalue As String, ByVal tempdatatable As DataTable, Optional secfield As Integer = 1) As String
        LogAppend("Browsing datatale (field = " & field & " // value = " & isvalue & ")", "SpellItem_Information_Execute", False)
        Try
            Dim foundRows() As DataRow
            foundRows = tempdatatable.Select(field & " = '" & isvalue & "'")
            If foundRows.Length = 0 Then
                Return "-"
            Else
                Dim i As Integer
                Dim tmpreturn As String = "-"
                For i = 0 To foundRows.GetUpperBound(0)
                    tmpreturn = (foundRows(i)(secfield)).ToString
                Next i
                Return tmpreturn
            End If
        Catch ex As Exception
            LogAppend("Error while browsing datatable! -> Exception is: ###START###" & ex.ToString() & "###END###", "SpellItem_Information_Execute", False, True)
            Return "-"
        End Try
    End Function
End Class
