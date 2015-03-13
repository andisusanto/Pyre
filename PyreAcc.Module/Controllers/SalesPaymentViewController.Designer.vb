Partial Class SalesPaymentViewController

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
        Me.btnWizardSalesInvoiceForPayment = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        Me.btnWizardCreditNoteForPayment = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        '
        'btnWizardSalesInvoiceForPayment
        '
        Me.btnWizardSalesInvoiceForPayment.AcceptButtonCaption = Nothing
        Me.btnWizardSalesInvoiceForPayment.CancelButtonCaption = Nothing
        Me.btnWizardSalesInvoiceForPayment.Caption = "Wizard Sales Invoice"
        Me.btnWizardSalesInvoiceForPayment.Category = "RecordEdit"
        Me.btnWizardSalesInvoiceForPayment.ConfirmationMessage = Nothing
        Me.btnWizardSalesInvoiceForPayment.Id = "btnSalesPaymentWizardSalesInvoiceForPayment"
        Me.btnWizardSalesInvoiceForPayment.ImageName = "Wizard"
        Me.btnWizardSalesInvoiceForPayment.TargetObjectType = GetType(PyreAcc.[Module].SalesPayment)
        Me.btnWizardSalesInvoiceForPayment.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.btnWizardSalesInvoiceForPayment.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView
        Me.btnWizardSalesInvoiceForPayment.ToolTip = Nothing
        Me.btnWizardSalesInvoiceForPayment.TypeOfView = GetType(DevExpress.ExpressApp.DetailView)
        '
        'btnWizardCreditNoteForPayment
        '
        Me.btnWizardCreditNoteForPayment.AcceptButtonCaption = Nothing
        Me.btnWizardCreditNoteForPayment.CancelButtonCaption = Nothing
        Me.btnWizardCreditNoteForPayment.Caption = "Wizard Credit Note"
        Me.btnWizardCreditNoteForPayment.Category = "RecordEdit"
        Me.btnWizardCreditNoteForPayment.ConfirmationMessage = Nothing
        Me.btnWizardCreditNoteForPayment.Id = "btnSalesPaymentWizardCreditNoteForPayment"
        Me.btnWizardCreditNoteForPayment.ImageName = "Wizard"
        Me.btnWizardCreditNoteForPayment.TargetObjectType = GetType(PyreAcc.[Module].SalesPayment)
        Me.btnWizardCreditNoteForPayment.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.btnWizardCreditNoteForPayment.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView
        Me.btnWizardCreditNoteForPayment.ToolTip = Nothing
        Me.btnWizardCreditNoteForPayment.TypeOfView = GetType(DevExpress.ExpressApp.DetailView)
        '
        'SalesPaymentViewController
        '
        Me.TargetObjectType = GetType(PyreAcc.[Module].SalesPayment)

    End Sub
    Friend WithEvents btnWizardSalesInvoiceForPayment As DevExpress.ExpressApp.Actions.PopupWindowShowAction
    Friend WithEvents btnWizardCreditNoteForPayment As DevExpress.ExpressApp.Actions.PopupWindowShowAction

End Class
