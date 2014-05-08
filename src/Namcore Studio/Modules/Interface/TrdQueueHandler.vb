﻿'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
'* Copyright (C) 2013-2014 NamCore Studio <https://github.com/megasus/Namcore-Studio>
'*
'* This program is free software; you can redistribute it and/or modify it
'* under the terms of the GNU General Public License as published by the
'* Free Software Foundation; either version 3 of the License, or (at your
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
'*      /Filename:      TrdQueueHandler
'*      /Description:   Handles threadding queue items
'+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Imports System.Linq
Imports NamCore_Studio.Forms
Imports NCFramework.Framework.Extension
Imports NamCore_Studio.Forms.Character
Imports NCFramework.Framework.Modules

Namespace Modules.Interface
    Public Class TrdQueueHandler
        Public Function doOperate_av(ByVal sender As Object, ByVal cnt As Integer) As String
            For Each x As Form In _
               From mForm As Object In Application.OpenForms Where TryCast(mForm, Form).Name = "AchievementsInterface" Select mForm
                ThreadExtensions.QueueUserWorkItem(
                    New Func(Of Object, Integer, String)(
                        AddressOf DirectCast(x, AchievementsInterface).ContinueOperation),
                    sender, cnt)
            Next
            Return Nothing
        End Function

        Public Function doOperate_qst(ByVal cnt As Integer, ByVal qsts As List(Of Quest)) As String
            For Each x As Form In _
               From mForm As Object In Application.OpenForms Where TryCast(mForm, Form).Name = "QuestsInterface" Select mForm
                ThreadExtensions.QueueUserWorkItem(
                    New Func(Of Integer, List(Of Quest), String)(
                        AddressOf DirectCast(x, QuestsInterface).ContinueOperation), cnt, qsts)
            Next
            Return Nothing
        End Function

        Public Function doOperate_spellSkill(ByVal targetSetId As Integer) As String
            For Each x As Form In _
                 From mForm As Object In Application.OpenForms Where TryCast(mForm, Form).Name = "SpellSkillInterface" Select mForm
                ThreadExtensions.QueueUserWorkItem(
                    New Func(Of Integer, String)(AddressOf DirectCast(x, SpellSkillInterface).ContinueOperation),
                    targetSetId)
            Next
            Return Nothing
        End Function

        Public Function doOperate_templateExplorer() As String
            For Each x As Form In _
                 From mForm As Object In Application.OpenForms Where TryCast(mForm, Form).Name = "TemplateExplorer" Select mForm
                ThreadExtensions.QueueUserWorkItem(
                    New Func(Of String)(AddressOf DirectCast(x, TemplateExplorer).ContinueOperation))
            Next
            Return Nothing
        End Function
    End Class
End Namespace