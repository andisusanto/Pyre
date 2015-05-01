Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Win.Templates
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.XtraEditors
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.XtraBars.Docking2010
Imports System.Data.SqlClient
Imports DevExpress.Data.Filtering
Imports Report.Module
Imports Report.Module.Win

Public Class ShowCustomFormWindowController
    Inherits DevExpress.ExpressApp.WindowController

    Private navigationController As ShowNavigationItemController
    Private showCustomFormActionCore As SingleChoiceAction
    Private currentWindowCaption As String = String.Empty
    Private Shared nonPersistentObjectType As Type = GetType(UIContainerObject)
    Public Sub New()
        TargetWindowType = WindowType.Main
    End Sub
    Public ReadOnly Property ShowCustomFormAction() As SingleChoiceAction
        Get
            Return showCustomFormActionCore
        End Get
    End Property
    Protected Overrides Sub OnFrameAssigned()
        MyBase.OnFrameAssigned()
        AddHandler Application.CustomProcessShortcut, AddressOf Application_CustomProcessShortcut
        navigationController = Frame.GetController(Of ShowNavigationItemController)()
        If navigationController IsNot Nothing Then
            AddHandler navigationController.CustomShowNavigationItem, AddressOf navigationController_CustomShowNavigationItem
        End If
    End Sub
    Private Sub navigationController_CustomShowNavigationItem(ByVal sender As Object, ByVal e As CustomShowNavigationItemEventArgs)
        e.Handled = ShowFormCore(e.ActionArguments)
    End Sub

    Protected Overrides Sub OnDeactivated()
        MyBase.OnDeactivated()
        RemoveHandler Application.CustomProcessShortcut, AddressOf Application_CustomProcessShortcut
        If navigationController IsNot Nothing Then
            RemoveHandler navigationController.CustomShowNavigationItem, AddressOf navigationController_CustomShowNavigationItem
        End If
    End Sub

    Private Sub Application_CustomProcessShortcut(ByVal sender As Object, ByVal e As CustomProcessShortcutEventArgs)
        Dim app As XafApplication = CType(sender, XafApplication)
        If e.Shortcut IsNot Nothing AndAlso (Not String.IsNullOrEmpty(e.Shortcut.ViewId)) Then
            Dim modelView As IModelDetailView = TryCast(app.FindModelView(e.Shortcut.ViewId), IModelDetailView)
            If modelView IsNot Nothing Then
                Dim type As Type = ReflectionHelper.FindType(modelView.ModelClass.Name)
                If type Is nonPersistentObjectType Then
                    e.View = CreateDetailViewCore(app)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Function isOpened(ByVal manager As MainForm, ByVal prmType As Type) As Boolean
        For Each obj As Form In manager.MdiChildren
            If obj.GetType() Is prmType Then
                obj.Focus()
                Return True
            End If
        Next
        Return False
    End Function
    Private Function isOpened(ByVal manager As MainForm, ByVal prmType As Type, ByVal Caption As String) As Boolean
        For Each obj As Form In manager.MdiChildren
            If obj.GetType() Is prmType AndAlso obj.Text = Caption Then
                obj.Focus()
                Return True
            End If
        Next
        Return False
    End Function
    Private Function ShowFormCore(ByVal args As SingleChoiceActionExecuteEventArgs) As Boolean
        Dim id As String = args.SelectedChoiceActionItem.Id
        Dim handled As Boolean = False
        If (Not String.IsNullOrEmpty(id)) Then
            currentWindowCaption = args.SelectedChoiceActionItem.Caption
            Dim svp As ShowViewParameters = args.ShowViewParameters

            Dim mainForm As MainForm = CType(Window.Template, DevExpress.ExpressApp.Win.Templates.MainForm)

            Dim xpObjectSpace As XPObjectSpace = Application.CreateObjectSpace
            Dim session As Session = xpObjectSpace.Session
            Select Case id
                Case "Reports"
                    If Not isOpened(mainForm, GetType(frmReportBase), "Reports") Then
                        ShowCustomModalForm(New frmReportBase(session, New XPCollection(Of Report.Module.Report)(session, New BinaryOperator("Active", True)), New List(Of IReportParameterControl)))
                    End If
                    handled = True
            End Select
        End If

        Return handled
    End Function

    Private Function CreateDetailViewCore(ByVal app As XafApplication) As DetailView
        Dim os As XPObjectSpace = app.CreateObjectSpace()
        Dim obj As Object = CreateNonPersistentObject(nonPersistentObjectType, os)
        Dim dv As DetailView = app.CreateDetailView(os, obj, True)
        dv.ViewEditMode = ViewEditMode.View
        dv.Caption = GetWindowCaption()
        Return dv
    End Function
    Protected Function GetWindowCaption() As String
        Return currentWindowCaption
    End Function
    Private Shared Function CreateNonPersistentObject(ByVal type As Type, ByVal os As XPObjectSpace) As Object
        Dim obj As Object = Nothing
        Dim typeInfo As ITypeInfo = XafTypesInfo.Instance.FindTypeInfo(type)
        Dim dcAttribute As DomainComponentAttribute = typeInfo.FindAttribute(Of DomainComponentAttribute)(False)
        Dim npAttribute As NonPersistentAttribute = typeInfo.FindAttribute(Of NonPersistentAttribute)(False)
        If GetType(PersistentBase).IsAssignableFrom(type) Then
            If npAttribute IsNot Nothing Then
                obj = os.CreateObject(type)
            End If
        Else
            If dcAttribute IsNot Nothing OrElse npAttribute IsNot Nothing Then
                obj = Activator.CreateInstance(type)
            End If
        End If
        If obj Is Nothing Then
            Throw New InvalidOperationException("Cannot create an object of a non-persistent type.")
        End If
        Return obj
    End Function
    
    Protected Overridable Sub ShowStandardNonModalFormWithCustomControl(ByVal svp As ShowViewParameters)
        svp.CreatedView = CreateDetailViewCore(Application)
        svp.Context = TemplateContext.View
        svp.TargetWindow = TargetWindow.NewWindow
    End Sub
    Protected Overridable Sub ShowStandardModalFormWithCustomControl(ByVal svp As ShowViewParameters)
        svp.CreatedView = CreateDetailViewCore(Application)
        svp.Context = TemplateContext.PopupWindow
        svp.Controllers.Add(Application.CreateController(Of DialogController)())
        svp.TargetWindow = TargetWindow.NewModalWindow
    End Sub
    Protected Overridable Sub ShowStandardEmbeddedFormWithCustomControl(ByVal svp As ShowViewParameters)
        svp.CreatedView = CreateDetailViewCore(Application)
        svp.Context = TemplateContext.ApplicationWindow
        svp.TargetWindow = TargetWindow.Current
    End Sub
    Protected Sub ShowCustomNonModalForm(ByVal frm As XtraForm)
        frm.Text = GetWindowCaption()
        frm.Show()
    End Sub
    Protected Sub ShowCustomModalForm(ByVal frm As XtraForm)
        frm.Text = GetWindowCaption()
        frm.MdiParent = Application.MainWindow.Template
        'frm.ShowDialog()
        frm.Show()
        frm.Select()
    End Sub
End Class
