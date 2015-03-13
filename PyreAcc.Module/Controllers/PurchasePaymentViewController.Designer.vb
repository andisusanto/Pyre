Partial Class PurchasePaymentViewController

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
        Me.btnWizardPurchaseInvoiceForPayment = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        Me.btnWizardDebitNoteForPayment = New DevExpress.ExpressApp.Actions.PopupWindowShowAction(Me.components)
        '
        'btnWizardPurchaseInvoiceForPayment
        '
        Me.btnWizardPurchaseInvoiceForPayment.AcceptButtonCaption = Nothing
        Me.btnWizardPurchaseInvoiceForPayment.CancelButtonCaption = Nothing
        Me.btnWizardPurchaseInvoiceForPayment.Caption = "Wizard Purchase Invoice"
        Me.btnWizardPurchaseInvoiceForPayment.Category = "RecordEdit"
        Me.btnWizardPurchaseInvoiceForPayment.ConfirmationMessage = Nothing
        Me.btnWizardPurchaseInvoiceForPayment.Id = "btnPurchasePaymentWizardPurchaseInvoiceForPayment"
        Me.btnWizardPurchaseInvoiceForPayment.ImageName = "Wizard"
        Me.btnWizardPurchaseInvoiceForPayment.TargetObjectType = GetType(PyreAcc.[Module].PurchasePayment)
        Me.btnWizardPurchaseInvoiceForPayment.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.btnWizardPurchaseInvoiceForPayment.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView
        Me.btnWizardPurchaseInvoiceForPayment.ToolTip = Nothing
        Me.btnWizardPurchaseInvoiceForPayment.TypeOfView = GetType(DevExpress.ExpressApp.DetailView)
        '
        'btnWizardDebitNoteForPayment
        '
        Me.btnWizardDebitNoteForPayment.AcceptButtonCaption = Nothing
        Me.btnWizardDebitNoteForPayment.CancelButtonCaption = Nothing
        Me.btnWizardDebitNoteForPayment.Caption = "Wizard Debit Note"
        Me.btnWizardDebitNoteForPayment.Category = "RecordEdit"
        Me.btnWizardDebitNoteForPayment.ConfirmationMessage = Nothing
        Me.btnWizardDebitNoteForPayment.Id = "btnPurchasePaymentWizardDebitNoteForPayment"
        Me.btnWizardDebitNoteForPayment.ImageName = "Wizard"
        Me.btnWizardDebitNoteForPayment.TargetObjectType = GetType(PyreAcc.[Module].PurchasePayment)
        Me.btnWizardDebitNoteForPayment.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.btnWizardDebitNoteForPayment.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView
        Me.btnWizardDebitNoteForPayment.ToolTip = Nothing
        Me.btnWizardDebitNoteForPayment.TypeOfView = GetType(DevExpress.ExpressApp.DetailView)
        '
        'PurchasePaymentViewController
        '
        Me.TargetObjectType = GetType(PyreAcc.[Module].PurchasePayment)

    End Sub
    Friend WithEvents btnWizardPurchaseInvoiceForPayment As DevExpress.ExpressApp.Actions.PopupWindowShowAction
    Friend WithEvents btnWizardDebitNoteForPayment As DevExpress.ExpressApp.Actions.PopupWindowShowAction

End Class
