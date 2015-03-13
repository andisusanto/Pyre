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
Partial Public Class SalesPaymentViewController
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
    Public ReadOnly Property CurrentObject As SalesPayment
        Get
            Return View.CurrentObject
        End Get
    End Property

    Private Sub btnWizardSalesInvoiceForPayment_CustomizePopupWindowParams(sender As Object, e As CustomizePopupWindowParamsEventArgs) Handles btnWizardSalesInvoiceForPayment.CustomizePopupWindowParams
        Dim objectSpace As IObjectSpace = Application.CreateObjectSpace()
        Dim collectionSource As New CollectionSource(objectSpace, GetType(SalesInvoice), False, CollectionSourceMode.Normal)
        collectionSource.Criteria("Default") = CriteriaOperator.And(New BinaryOperator("Customer.Oid", CurrentObject.Customer.Oid), New BinaryOperator("PaymentOutstandingAmount", 0, BinaryOperatorType.Greater), New BinaryOperator("Status", TransactionStatus.Submitted))
        Dim lv As ListView = Application.CreateListView("SalesInvoice_ForSalesPaymentWizard", collectionSource, False)
        e.View = lv
    End Sub

    Private Sub btnWizardCreditNoteForPayment_CustomizePopupWindowParams(sender As Object, e As CustomizePopupWindowParamsEventArgs) Handles btnWizardCreditNoteForPayment.CustomizePopupWindowParams
        Dim objectSpace As IObjectSpace = Application.CreateObjectSpace()
        Dim collectionSource As New CollectionSource(objectSpace, GetType(CreditNote), False, CollectionSourceMode.Normal)
        collectionSource.Criteria("Default") = CriteriaOperator.And(New BinaryOperator("ForCustomer.Oid", CurrentObject.Customer.Oid), New BinaryOperator("RemainingAmount", 0, BinaryOperatorType.Greater))
        Dim lv As ListView = Application.CreateListView("CreditNote_ForSalesPaymentWizard", collectionSource, False)
        e.View = lv
    End Sub

    Private Sub btnWizardSalesInvoiceForPayment_Execute(sender As Object, e As PopupWindowShowActionExecuteEventArgs) Handles btnWizardSalesInvoiceForPayment.Execute
        For Each objSalesInvoice As SalesInvoice In e.PopupWindow.View.SelectedObjects
            Dim objSalesPaymentDetail = View.ObjectSpace.CreateObject(Of SalesPaymentDetail)()
            objSalesPaymentDetail.SalesPayment = CurrentObject
            objSalesPaymentDetail.SalesInvoice = View.ObjectSpace.GetObjectByKey(Of SalesInvoice)(objSalesInvoice.Oid)
        Next
    End Sub

    Private Sub btnWizardCreditNoteForPayment_Execute(sender As Object, e As PopupWindowShowActionExecuteEventArgs) Handles btnWizardCreditNoteForPayment.Execute
        For Each objCreditNote As CreditNote In e.PopupWindow.View.SelectedObjects
            Dim objSalesPaymentDetail = View.ObjectSpace.CreateObject(Of SalesPaymentCreditNote)()
            objSalesPaymentDetail.SalesPayment = CurrentObject
            objSalesPaymentDetail.CreditNote = View.ObjectSpace.GetObjectByKey(Of CreditNote)(objCreditNote.Oid)
        Next
    End Sub

End Class
