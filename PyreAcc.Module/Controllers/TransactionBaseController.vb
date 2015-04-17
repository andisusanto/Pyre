Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Layout
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.ExpressApp.Model.NodeGenerators
Imports DevExpress.ExpressApp.Xpo

' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
Partial Public Class TransactionBaseController
    Inherits ViewController
    Public Sub New()
        InitializeComponent()
        RegisterActions(components)
        ' Target required Views (via the TargetXXX properties) and create their Actions.
    End Sub
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
        If TypeOf View Is ListView Then
            SetTransDateFilterAction()
            SetStatusFilterAction()
            TransactionTransDateFilter.SelectedIndex = 0
            TransactionStatusFilter.SelectedIndex = 0
            TransactionTransDateFilter.Caption = "Date:"
            TransactionStatusFilter.Caption = "Status:"
            ExecuteTransDateFilter()
            ExecuteStatusFilter()
        End If

        ' Perform various tasks depending on the target View.
    End Sub
    Protected Overrides Sub OnViewControlsCreated()
        MyBase.OnViewControlsCreated()
        ' Access and customize the target View control.
    End Sub
    Protected Overrides Sub OnDeactivated()
        ' Unsubscribe from previously subscribed events and release other references and resources.
        MyBase.OnDeactivated()
    End Sub

    Public ReadOnly Property MySubmitAction As PopupWindowShowAction
        Get
            Return SubmitAction
        End Get
    End Property
    Public ReadOnly Property MyCancelSubmitAction As PopupWindowShowAction
        Get
            Return CancelSubmitAction
        End Get
    End Property
   
    Private Sub CustomizePopupWindowParams(sender As Object, e As DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventArgs) Handles SubmitAction.CustomizePopupWindowParams, CancelSubmitAction.CustomizePopupWindowParams
        Dim objectSpace As IObjectSpace = Application.CreateObjectSpace()
        Dim popUpNote As PopUpNote = objectSpace.CreateObject(Of PopUpNote)()
        Dim dv As DetailView = Application.CreateDetailView(objectSpace, popUpNote)
        dv.ViewEditMode = ViewEditMode.Edit
        e.View = dv
    End Sub
    Private Sub SubmitAction_Execute(sender As System.Object, e As DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventArgs) Handles SubmitAction.Execute

        If View.SelectedObjects.Count > 0 Or View.CurrentObject IsNot Nothing Then
            Try
                View.ObjectSpace.CommitChanges()
                If TypeOf View Is DetailView Then
                    Dim TransactionBase As TransactionBase = View.CurrentObject
                    TransactionBase.SubmitNote = CType(e.PopupWindow.View.CurrentObject, PopUpNote).Note
                    TransactionBase.Submit()
                    View.ObjectSpace.CommitChanges()
                Else
                    For Each obj In View.SelectedObjects
                        Dim TransactionBase As TransactionBase = obj
                        TransactionBase.SubmitNote = CType(e.PopupWindow.View.CurrentObject, PopUpNote).Note
                        TransactionBase.Submit()
                    Next
                    View.ObjectSpace.CommitChanges()
                End If
             
            Finally
                View.ObjectSpace.Refresh()
            End Try

        End If
    End Sub

    Private Sub CancelSubmitAction_Execute(sender As System.Object, e As DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventArgs) Handles CancelSubmitAction.Execute
        View.ObjectSpace.CommitChanges()
        Try
            If TypeOf View Is DetailView Then
                Dim TransactionBase As TransactionBase = View.CurrentObject
                TransactionBase.CancelNote = CType(e.PopupWindow.View.CurrentObject, PopUpNote).Note
                TransactionBase.Cancel()
                View.ObjectSpace.CommitChanges()
            Else
                For Each obj In View.SelectedObjects
                    Dim TransactionBase As TransactionBase = obj
                    TransactionBase.CancelNote = CType(e.PopupWindow.View.CurrentObject, PopUpNote).Note
                    TransactionBase.Cancel()
                Next
                View.ObjectSpace.CommitChanges()
            End If
        Finally
            View.ObjectSpace.Refresh()
        End Try
        
    End Sub

#Region "Filtering"
    Private Sub SetTransDateFilterAction()
        With TransactionTransDateFilter.Items
            .Add(New ChoiceActionItem("Today", 0))
            .Add(New ChoiceActionItem("Last One Week", 1))
            .Add(New ChoiceActionItem("Last One Month", 2))
            .Add(New ChoiceActionItem("All", 3))
        End With
    End Sub

    Private Sub SetStatusFilterAction()
        With TransactionStatusFilter.Items
            .Add(New ChoiceActionItem("Submitted", 0))
            .Add(New ChoiceActionItem("Entry", 1))
            .Add(New ChoiceActionItem("All", 2))
        End With
    End Sub
    Private Sub TransactionBaseFilter_Execute(sender As Object, e As SingleChoiceActionExecuteEventArgs) Handles TransactionTransDateFilter.Execute
        ExecuteTransDateFilter()
    End Sub
    Private Sub ExecuteTransDateFilter()
        Select Case TransactionTransDateFilter.SelectedIndex
            Case 0
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BinaryOperator("TransDate", Today.Date)
            Case 1
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BinaryOperator("TransDate", DateAdd(DateInterval.Weekday, -1, Today.Date), BinaryOperatorType.GreaterOrEqual)
            Case 2
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = New BinaryOperator("TransDate", DateAdd(DateInterval.Month, -1, Today.Date), BinaryOperatorType.GreaterOrEqual)
            Case 3
                CType(View, ListView).CollectionSource.Criteria("TransDateFilter") = Nothing
        End Select
    End Sub

    Private Sub TransactionStatusFilter_Execute(sender As Object, e As SingleChoiceActionExecuteEventArgs) Handles TransactionStatusFilter.Execute
        ExecuteStatusFilter()
    End Sub
    Private Sub ExecuteStatusFilter()
        Select Case TransactionStatusFilter.SelectedIndex
            Case 0
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = New BinaryOperator("Status", TransactionStatus.Submitted)
            Case 1
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = New BinaryOperator("Status", TransactionStatus.Entered)
            Case 2
                CType(View, ListView).CollectionSource.Criteria("StatusFilter") = Nothing
        End Select
    End Sub
#End Region
End Class
