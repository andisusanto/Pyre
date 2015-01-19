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
<RuleCriteria("Rule Criteria for PurchasePayment.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for PurchaseInvoice.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for PurchasePayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, DebitNoteAmount, RemainingAmount")>
<Appearance("Appearance for PurchasePayment.EnableDetails = FALSE", AppearanceItemType:="ViewItem", criteria:="EnableDetails = FALSE", enabled:=False, targetitems:="Details")>
<Appearance("Appearance for PurchasePayment.Details.Count > 0", AppearanceItemType:="ViewItem", criteria:="@Details.Count > 0", enabled:=False, targetitems:="Supplier")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchasePayment
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
    Private _supplier As Supplier
    Private _total As Decimal
    Private _debitNoteAmount As Decimal
    Private _remainingAmount As Decimal
    Private _fromAccount As Account
    Private _balanceSheetAccountMutation As BalanceSheetAccountMutation
    <RuleUniqueValue("Rule Unique for PurchasePayment.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for PurchasePayment.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchasePayment.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for PurchasePayment.Supplier", DefaultContexts.Save)>
    Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
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
    Public Property DebitNoteAmount As Decimal
        Get
            Return _debitNoteAmount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DebitNoteAmount", _debitNoteAmount, value)
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
    <RuleRequiredField("Rule Required for PurchasePayment.FromAccount", DefaultContexts.Save)>
    Public Property FromAccount As Account
        Get
            Return _fromAccount
        End Get
        Set(value As Account)
            SetPropertyValue("FromAccount", _fromAccount, value)
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
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsPeriodClosed As Boolean
        Get
            Dim period As Period = Session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
            If period Is Nothing Then Return True
            Return period.Closed
        End Get
    End Property
    Private Sub CalculateRemainingAmount()
        RemainingAmount = Total - DebitNoteAmount
    End Sub
    <Association("PurchasePayment-PurchasePaymentDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PurchasePaymentDetail)
        Get
            Return GetCollection(Of PurchasePaymentDetail)("Details")
        End Get
    End Property
    <Association("PurchasePayment-PurchasePaymentDebitNote"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property DebitNotes As XPCollection(Of PurchasePaymentDebitNote)
        Get
            Return GetCollection(Of PurchasePaymentDebitNote)("DebitNotes")
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
            Return Supplier IsNot Nothing
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            If objDetail.PurchaseInvoice.Status <> TransactionStatus.Submitted Then Throw New Exception(String.Format("Purchase Invoice with No {0} has not been submitted", objDetail.PurchaseInvoice.No))
            If objDetail.PurchaseInvoice.PaymentOutstandingStatus = OutstandingStatus.Cleared Then Throw New Exception(String.Format("Purchase Invoice with No {0} already set as cleared", objDetail.PurchaseInvoice.No))
            If objDetail.PurchaseInvoice.PaymentOutstandingAmount < objDetail.Amount Then Throw New Exception(String.Format("Invalid amount for submitting Invoice. Invalid line : {0}", objDetail.ToString))
            objDetail.PurchaseInvoice.PaidAmount += objDetail.Amount
        Next
        For Each obj In DebitNotes
            If obj.DebitNote.RemainingAmount < obj.Amount Then Throw New Exception(String.Format("Debit note with no {0} has no enough balance", obj.DebitNote.No))
            obj.DebitNote.UsedAmount += obj.Amount
        Next
        BalanceSheetAccountMutation = BalanceSheetService.CreateBalanceSheetAccountMutation(FromAccount, TransDate, -1 * RemainingAmount, "Purchase")
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            objDetail.PurchaseInvoice.PaidAmount -= objDetail.Amount
        Next
        For Each obj In DebitNotes
            obj.DebitNote.UsedAmount -= obj.Amount
        Next
        Dim tmp = BalanceSheetAccountMutation
        BalanceSheetAccountMutation = Nothing
        If tmp IsNot Nothing Then BalanceSheetService.DeleteBalanceSheetAccountMutation(tmp)
    End Sub
End Class
