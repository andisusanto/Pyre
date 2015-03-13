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
Imports DevExpress.Xpo

' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
Partial Public Class PurchasePaymentViewController
    Inherits ViewController
    Public Sub New()
        InitializeComponent()
        RegisterActions(components)
        ' Target required Views (via the TargetXXX properties) and create their Actions.
    End Sub
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
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
    Public ReadOnly Property CurrentObject As PurchasePayment
        Get
            Return View.CurrentObject
        End Get
    End Property

    Private Sub btnWizardPurchaseInvoiceForPayment_CustomizePopupWindowParams(sender As Object, e As CustomizePopupWindowParamsEventArgs) Handles btnWizardPurchaseInvoiceForPayment.CustomizePopupWindowParams
        Dim objectSpace As IObjectSpace = Application.CreateObjectSpace()
        Dim collectionSource As New CollectionSource(objectSpace, GetType(PurchaseInvoice), False, CollectionSourceMode.Normal)
        collectionSource.Criteria("Default") = CriteriaOperator.And(New BinaryOperator("Supplier.Oid", CurrentObject.Supplier.Oid), New BinaryOperator("PaymentOutstandingAmount", 0, BinaryOperatorType.Greater), New BinaryOperator("Status", TransactionStatus.Submitted))
        Dim lv As ListView = Application.CreateListView("PurchaseInvoice_ForPurchasePaymentWizard", collectionSource, False)
        e.View = lv
    End Sub

    Private Sub btnWizardDebitNoteForPayment_CustomizePopupWindowParams(sender As Object, e As CustomizePopupWindowParamsEventArgs) Handles btnWizardDebitNoteForPayment.CustomizePopupWindowParams
        Dim objectSpace As IObjectSpace = Application.CreateObjectSpace()
        Dim collectionSource As New CollectionSource(objectSpace, GetType(DebitNote), False, CollectionSourceMode.Normal)
        collectionSource.Criteria("Default") = CriteriaOperator.And(New BinaryOperator("FromSupplier.Oid", CurrentObject.Supplier.Oid), New BinaryOperator("RemainingAmount", 0, BinaryOperatorType.Greater))
        Dim lv As ListView = Application.CreateListView("DebitNote_ForPurchasePaymentWizard", collectionSource, False)
        e.View = lv
    End Sub

    Private Sub btnWizardPurchaseInvoiceForPayment_Execute(sender As Object, e As PopupWindowShowActionExecuteEventArgs) Handles btnWizardPurchaseInvoiceForPayment.Execute
        For Each objPurchaseInvoice As PurchaseInvoice In e.PopupWindow.View.SelectedObjects
            Dim objPurchasePaymentDetail = View.ObjectSpace.CreateObject(Of PurchasePaymentDetail)()
            objPurchasePaymentDetail.PurchasePayment = CurrentObject
            objPurchasePaymentDetail.PurchaseInvoice = View.ObjectSpace.GetObjectByKey(Of PurchaseInvoice)(objPurchaseInvoice.Oid)
        Next
    End Sub

    Private Sub btnWizardDebitNoteForPayment_Execute(sender As Object, e As PopupWindowShowActionExecuteEventArgs) Handles btnWizardDebitNoteForPayment.Execute
        For Each objDebitNote As DebitNote In e.PopupWindow.View.SelectedObjects
            Dim objPurchasePaymentDetail = View.ObjectSpace.CreateObject(Of PurchasePaymentDebitNote)()
            objPurchasePaymentDetail.PurchasePayment = CurrentObject
            objPurchasePaymentDetail.DebitNote = View.ObjectSpace.GetObjectByKey(Of DebitNote)(objDebitNote.Oid)
        Next
    End Sub

End Class
