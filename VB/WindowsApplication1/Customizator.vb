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
Imports System.Collections

Namespace WindowsApplication1
	Public Class Customizator
		Inherits Component
		#Region "Fields"

		Private designerHost As IDesignerHost
		Private rootDesignControl As Control
		Private surface As DesignSurface
		Private parentControl As Control
		Private designPanel As Panel

		#End Region

		#Region "Propeties"
		Private _SelectedControl As Control
		Public Property SelectedControl() As Control
			Get
				Return _SelectedControl
			End Get
			Set(ByVal value As Control)
				IsCustomization = False
				_SelectedControl = value
				OnChanged()
			End Set
		End Property

		Private _DesignContainer As Control
		Public Property DesignContainer() As Control
			Get
				Return _DesignContainer
			End Get
			Set(ByVal value As Control)
				_DesignContainer = value
				OnChanged()
			End Set
		End Property

		Private _IsCustomization As Boolean
		Public Property IsCustomization() As Boolean
			Get
				Return _IsCustomization
			End Get
			Set(ByVal value As Boolean)
				If value AndAlso (Not CanCustomize(value)) Then
					Return
				End If
					_IsCustomization = value
					OnChanged()
			End Set
		End Property


		#End Region

		#Region "Methods"
		Private Sub CopyBounds(ByVal target As Control, ByVal source As Control)
			Dim p As Point = source.PointToScreen(New Point(0, 0))
			If target.Parent IsNot Nothing Then
				p = target.Parent.PointToClient(p)
			End If
			target.Location = p
			target.Width = source.Width
			target.Height = source.Height
		End Sub

		Private Shared Function IsNull(ByVal obj As Object) As Boolean
			Return obj Is Nothing
		End Function

		Private Sub AddToList(ByVal list As IList, ByVal c As Control)
			If c.Dock <> DockStyle.None Then
				Return
			End If
			If c.Parent Is Nothing Then
				Return
			End If
			If c.Name = String.Empty Then
				Return
			End If
			list.Add(c)
		End Sub

		Private Sub TraverseControls(ByVal list As IList, ByVal c As Control)
			AddToList(list, c)
			For Each control As Control In c.Controls
				TraverseControls(list, control)
			Next control
		End Sub

	   Public Function GetAvailableControls() As List(Of Control)
			Dim list As New List(Of Control)()
			TraverseControls(list, DesignContainer)
			Return list
	   End Function


		Private Function NeedDestroyCustomization() As Boolean
			Return (Not IsNull(rootDesignControl)) AndAlso Not IsNull(designPanel.Parent)
		End Function

		Private Function CanCustomize(ByVal isCutomization As Boolean) As Boolean
			Return Not(IsNull(DesignContainer) OrElse DesignMode OrElse IsNull(SelectedControl) OrElse ((Not isCutomization)) OrElse IsNull(SelectedControl.Parent))
		End Function

		Private Function CreateDesignPanel() As Panel
			Dim designPanel As Panel = TryCast(designerHost.CreateComponent(GetType(Panel)), Panel)
			designPanel.BackColor = Color.Black
			designPanel.Padding = New Padding(2, 2, 2, 2)
			Return designPanel
		End Function

		Private Sub CreateDesignSurface()
			surface = New DesignSurface()
			designerHost = TryCast(surface.GetService(GetType(IDesignerHost)), IDesignerHost)
			rootDesignControl = TryCast(designerHost.CreateComponent(GetType(UserControl)), Control)
			rootDesignControl.Dock = DockStyle.Fill
			rootDesignControl.BackColor = Color.AliceBlue
			Dim c As Control = TryCast(surface.View, Control)
			c.Dock = DockStyle.Fill
			c.BackColor = Color.White
			c.Location = New Point(15, 25)
			c.Parent = DesignContainer
		End Sub

		Private Sub StartControlCustomization()
			designPanel = CreateDesignPanel()
			rootDesignControl.Controls.Add(designPanel)
			CopyBounds(designPanel, SelectedControl)
			parentControl = SelectedControl.Parent
			SelectedControl.Parent = designPanel
			SelectedControl.Dock = DockStyle.Fill
		End Sub

		Private Sub StartCustomiztion()
			If (Not CanCustomize(IsCustomization)) Then
				Return
			End If
			CreateDesignSurface()
			StartControlCustomization()
		End Sub

		Private Sub FinishControlCustomization()
			SelectedControl.Parent = parentControl
			SelectedControl.Dock = DockStyle.None
			CopyBounds(SelectedControl, designPanel)
		End Sub

		Private Sub EndCustomization()
			If (Not NeedDestroyCustomization()) Then
				Return
			End If
			FinishControlCustomization()
			DestroyDesignSurface()
		End Sub


		Private Sub DestroyDesignSurface()
			rootDesignControl.Dispose()
		End Sub

		Private Sub OnChanged()
			If IsCustomization Then
				StartCustomiztion()
			Else
				EndCustomization()
			End If
		End Sub
		#End Region
	End Class
End Namespace
