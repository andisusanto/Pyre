Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.ConditionalAppearance

<CreatableItem(False)> _
<RuleCriteria("Rule Criteria for SalesPayment.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for SalesPayment.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Actions for SalesPayment.EnableDetails = FALSE", appearanceitemtype:="Action", enabled:=False, targetitems:="btnSalesPaymentWizardSalesInvoiceForPayment; btnSalesPaymentWizardCreditNoteForPayment", criteria:="EnableDetails = FALSE")>
<Appearance("Appearance Default Disabled for SalesPayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, CreditNoteAmount, RemainingAmount")>
<Appearance("Appearance for SalesPayment.EnableDetails = FALSE", AppearanceItemType:="ViewItem", criteria:="EnableDetails = FALSE", enabled:=False, targetitems:="Details")>
<Appearance("Appearance for SalesPayment.Details.Count > 0", AppearanceItemType:="ViewItem", criteria:="@Details.Count > 0", enabled:=False, targetitems:="Customer")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesPayment
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Today
    End Sub
    Private _no As String
    Private _transDate As Date
    Private _customer As Customer
    Private _total As Decimal
    Private _creditNoteAmount As Decimal
    Private _remainingAmount As Decimal
    Private _toAccount As Account
    Private _balanceSheetAccountMutation As BalanceSheetAccountMutation
    <RuleUniqueValue("Rule Unique for SalesPayment.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for SalesPayment.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesPayment.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for SalesPayment.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(value As Decimal)
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property CreditNoteAmount As Decimal
        Get
            Return _creditNoteAmount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("CreditNoteAmount", _creditNoteAmount, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property RemainingAmount As Decimal
        Get
            Return _remainingAmount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("RemainingAmount", _remainingAmount, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesPayment.ToAccount", DefaultContexts.Save)>
    Public Property ToAccount As Account
        Get
            Return _toAccount
        End Get
        Set(value As Account)
            SetPropertyValue("ToAccount", _toAccount, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetAccountMutation As BalanceSheetAccountMutation
        Get
            Return _balanceSheetAccountMutation
        End Get
        Set(value As BalanceSheetAccountMutation)
            SetPropertyValue("BalanceSheetAccountMutation", _balanceSheetAccountMutation, value)
        End Set
    End Property
    Private Sub CalculateRemainingAmount()
        RemainingAmount = Total - CreditNoteAmount
    End Sub
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsPeriodClosed As Boolean
        Get
            Dim period As Period = Session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
            If period Is Nothing Then Return True
            Return period.Closed
        End Get
    End Property
    <Association("SalesPayment-SalesPaymentDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesPaymentDetail)
        Get
            Return GetCollection(Of SalesPaymentDetail)("Details")
        End Get
    End Property
    <Association("SalesPayment-SalesPaymentCreditNote"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property CreditNotes As XPCollection(Of SalesPaymentCreditNote)
        Get
            Return GetCollection(Of SalesPaymentCreditNote)("CreditNotes")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <Browsable(False), VisibleInDetailView(False), VisibleInListView(False)>
    Public ReadOnly Property EnableDetails As Boolean
        Get
            Return Customer IsNot Nothing
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            If objDetail.SalesInvoice.Status <> TransactionStatus.Submitted Then Throw New Exception(String.Format("Sales Invoice with No {0} has not been submitted", objDetail.SalesInvoice.No))
            If objDetail.SalesInvoice.PaymentOutstandingAmount < objDetail.Amount Then Throw New Exception(String.Format("Invalid amount for submitting payment. Invalid line : {0}", objDetail.ToString))
            objDetail.SalesInvoice.PaidAmount += objDetail.Amount
        Next
        For Each obj In CreditNotes
            If obj.CreditNote.RemainingAmount < obj.Amount Then Throw New Exception(String.Format("Credit note with no {0} has no enough balance", obj.CreditNote.No))
            obj.CreditNote.UsedAmount += obj.Amount
        Next
        BalanceSheetAccountMutation = BalanceSheetService.CreateBalanceSheetAccountMutation(ToAccount, TransDate, RemainingAmount, "Sales")
        Customer.OutstandingPaymentAmount -= Total
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            objDetail.SalesInvoice.PaidAmount -= objDetail.Amount
        Next
        For Each obj In CreditNotes
            obj.CreditNote.UsedAmount -= obj.Amount
        Next
        Dim tmp = BalanceSheetAccountMutation
        BalanceSheetAccountMutation = Nothing
        If tmp IsNot Nothing Then BalanceSheetService.DeleteBalanceSheetAccountMutation(tmp)
        Customer.OutstandingPaymentAmount += Total
    End Sub
End Class
