Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.ComponentModel.Design
Imports DevExpress.XtraGrid

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form

				Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Number", GetType(Integer))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
				End Function



		Public Sub New()
			InitializeComponent()
			gridControl1.DataSource = CreateTable(20)
		End Sub

		Private Sub checkButton1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles checkButton1.CheckedChanged
			customizator1.IsCustomization = checkButton1.Checked
		End Sub

		Private Sub radioGroup1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radioGroup1.SelectedIndexChanged
			checkButton1.Checked = False
			Select Case radioGroup1.SelectedIndex
				Case 0
					customizator1.SelectedControl = gridControl1
				Case 1
					customizator1.SelectedControl = radioGroup1
			End Select
		End Sub
	End Class
End Namespace