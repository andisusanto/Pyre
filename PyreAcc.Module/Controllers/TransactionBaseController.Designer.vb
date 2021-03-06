Partial Class TransactionBaseController

	<System.Diagnostics.DebuggerNonUserCode()> _
	Public Sub New(ByVal Container As System.ComponentModel.IContainer)
		MyClass.New()

		'Required for Windows.Forms Class Composition Designer support
		Container.Add(Me)

	End Sub

	'Component overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso components IsNot Nothing Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Component Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Component Designer
	'It can be modified using the Component Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SubmitAction = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        Me.CancelSubmitAction = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        Me.TransactionTransDateFilter = New DevExpress.ExpressApp.Actions.SingleChoiceAction(Me.components)
        Me.TransactionStatusFilter = New DevExpress.ExpressApp.Actions.SingleChoiceAction(Me.components)
        '
        'SubmitAction
        '
        Me.SubmitAction.AcceptButtonCaption = Nothing
        Me.SubmitAction.CancelButtonCaption = Nothing
        Me.SubmitAction.Caption = "Submit"
        Me.SubmitAction.Category = "RecordEdit"
        Me.SubmitAction.ConfirmationMessage = "Are you sure want to submit this transaction?"
        Me.SubmitAction.Id = "TransactionBaseSubmitAction"
        Me.SubmitAction.ImageName = "Submit"
        Me.SubmitAction.TargetObjectType = GetType(PyreAcc.[Module].TransactionBase)
        Me.SubmitAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.SubmitAction.ToolTip = Nothing
        '
        'CancelSubmitAction
        '
        Me.CancelSubmitAction.AcceptButtonCaption = Nothing
        Me.CancelSubmitAction.CancelButtonCaption = Nothing
        Me.CancelSubmitAction.Caption = "Cancel Submit"
        Me.CancelSubmitAction.Category = "RecordEdit"
        Me.CancelSubmitAction.ConfirmationMessage = "Are you sure want to cancel this transaction?"
        Me.CancelSubmitAction.Id = "TransactionBaseCancelSubmitAction"
        Me.CancelSubmitAction.ImageName = "Cancel"
        Me.CancelSubmitAction.TargetObjectType = GetType(PyreAcc.[Module].TransactionBase)
        Me.CancelSubmitAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.CancelSubmitAction.ToolTip = Nothing
        '
        'TransactionTransDateFilter
        '
        Me.TransactionTransDateFilter.Caption = "Date"
        Me.TransactionTransDateFilter.Category = "Filters"
        Me.TransactionTransDateFilter.ConfirmationMessage = Nothing
        Me.TransactionTransDateFilter.Id = "TransactionBaseFilter"
        Me.TransactionTransDateFilter.TargetObjectType = GetType(PyreAcc.[Module].TransactionBase)
        Me.TransactionTransDateFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.TransactionTransDateFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView
        Me.TransactionTransDateFilter.ToolTip = Nothing
        Me.TransactionTransDateFilter.TypeOfView = GetType(DevExpress.ExpressApp.ListView)
        '
        'TransactionStatusFilter
        '
        Me.TransactionStatusFilter.Caption = "Status"
        Me.TransactionStatusFilter.Category = "Filters"
        Me.TransactionStatusFilter.ConfirmationMessage = Nothing
        Me.TransactionStatusFilter.Id = "TransactionStatusFilter"
        Me.TransactionStatusFilter.TargetObjectType = GetType(PyreAcc.[Module].TransactionBase)
        Me.TransactionStatusFilter.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.TransactionStatusFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView
        Me.TransactionStatusFilter.ToolTip = Nothing
        Me.TransactionStatusFilter.TypeOfView = GetType(DevExpress.ExpressApp.ListView)
        '
        'TransactionBaseController
        '
        Me.TargetObjectType = GetType(PyreAcc.[Module].TransactionBase)
        Me.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.TypeOfView = GetType(DevExpress.ExpressApp.View)

    End Sub
    Friend WithEvents SubmitAction As DevExpress.ExpressApp.Actions.PopupWindowShowAction
    Friend WithEvents CancelSubmitAction As DevExpress.ExpressApp.Actions.PopupWindowShowAction
    Friend WithEvents TransactionTransDateFilter As DevExpress.ExpressApp.Actions.SingleChoiceAction
    Friend WithEvents TransactionStatusFilter As DevExpress.ExpressApp.Actions.SingleChoiceAction

End Class
