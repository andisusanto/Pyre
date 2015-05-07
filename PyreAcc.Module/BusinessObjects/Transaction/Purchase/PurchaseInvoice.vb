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
<RuleCriteria("Rule Criteria for Cancel PurchaseInvoice.PaidAmount = 0", "Cancel", "PaidAmount = 0")>
<RuleCriteria("Rule Criteria for PurchaseInvoice.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for PurchaseInvoice.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for PurchaseInvoice", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="DetailsTotal, DetailsDiscount, Total, Discount, GrandTotal, Rounding, PaidAmount, PaymentOutstandingAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseInvoice
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = GlobalFunction.GetServerNow(Session).Date
    End Sub
    Private _no As String
    Private _referenceNo As String
    Private _transDate As Date
    Private _term As Integer
    Private _dueDate As Date
    Private _supplier As Supplier
    Private _inventory As Inventory
    Private _detailsTotal As Decimal
    Private _detailsDiscount As Decimal
    Private _total As Decimal
    Private _discountType As DiscountType
    Private _discountValue As Decimal
    Private _discount As Decimal
    Private _grandTotal As Decimal
    Private _rounding As Decimal
    Private _paidAmount As Decimal
    Private _paymentOutstandingAmount As Decimal

    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleUniqueValue("Rule Unique for PurchaseInvoice.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for PurchaseInvoice.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.ReferenceNo", DefaultContexts.Save)>
    Public Property ReferenceNo As String
        Get
            Return _referenceNo
        End Get
        Set(value As String)
            SetPropertyValue("ReferenceNo", _referenceNo, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    DueDate = TransDate.AddDays(Term)
                End If
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    Public Property Term As Integer
        Get
            Return _term
        End Get
        Set(value As Integer)
            SetPropertyValue("Term", _term, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    DueDate = TransDate.AddDays(Term)
                End If
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <RuleRequiredField("Rule Required for PurchaseInvoice.DueDate", DefaultContexts.Save)>
    Public Property DueDate As Date
        Get
            Return _dueDate
        End Get
        Set(value As Date)
            SetPropertyValue("DueDate", _dueDate, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    Term = DateDiff(DateInterval.Day, TransDate, DueDate)
                End If
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.Supplier", DefaultContexts.Save)>
    Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseInvoice.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <VisibleInDetailView(False)>
    Public Property DetailsTotal As Decimal
        Get
            Return _detailsTotal
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DetailsTotal", _detailsTotal, value)
        End Set
    End Property
    <VisibleInDetailView(False)>
    Public Property DetailsDiscount As Decimal
        Get
            Return _detailsDiscount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DetailsDiscount", _detailsDiscount, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    Public Property DiscountType As DiscountType
        Get
            Return _discountType
        End Get
        Set(ByVal value As DiscountType)
            SetPropertyValue("DiscountType", _discountType, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    <RuleRange(0, 100, targetcriteria:="DiscountType = 'ByPercentage'")>
    Public Property DiscountValue As Decimal
        Get
            Return _discountValue
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DiscountValue", _discountValue, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Discount As Decimal
        Get
            Return _discount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Discount", _discount, value)
            If Not IsLoading Then
                CalculateGrandTotal()
            End If
        End Set
    End Property
    Public Property GrandTotal As Decimal
        Get
            Return _grandTotal
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("GrandTotal", _grandTotal, value)
            If Not IsLoading Then
                CalculatePaymentOutstandingAmount()
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False)>
    Public Property Rounding As Decimal
        Get
            Return _rounding
        End Get
        Set(value As Decimal)
            SetPropertyValue("Rounding", _rounding, value)
        End Set
    End Property
    <VisibleInListView(False)>
    Public Property PaidAmount As Decimal
        Get
            Return _paidAmount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("PaidAmount", _paidAmount, value)
            If Not IsLoading Then
                CalculatePaymentOutstandingAmount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    Public Property PaymentOutstandingAmount As Decimal
        Get
            Return _paymentOutstandingAmount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("PaymentOutstandingAmount", _paymentOutstandingAmount, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffJournal As PeriodCutOffJournal
        Get
            Return _periodCutOffJournal
        End Get
        Set(value As PeriodCutOffJournal)
            SetPropertyValue("PeriodCutOffJournal", _periodCutOffJournal, value)
        End Set
    End Property
    <Association("PurchaseInvoice-PurchaseInvoiceDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PurchaseInvoiceDetail)
        Get
            Return GetCollection(Of PurchaseInvoiceDetail)("Details")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsPeriodClosed As Boolean
        Get
            Return TransactionConfig.IsInClosedPeriod(Session, TransDate)
        End Get
    End Property
    Private Sub CalculateDiscount()
        Select Case DiscountType
            Case [Module].DiscountType.ByAmount
                Discount = DiscountValue
            Case [Module].DiscountType.ByPercentage
                Discount = GlobalFunction.Round(Total * DiscountValue / 100)
        End Select
    End Sub
    Private Sub CalculateGrandTotal()
        GrandTotal = Total - Discount
    End Sub
    Private Sub CalculatePaymentOutstandingAmount()
        PaymentOutstandingAmount = GrandTotal - PaidAmount
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            Dim tmpUnitPrice As Decimal = (objDetail.UnitPrice - (objDetail.Discount / objDetail.Quantity) - (Discount * objDetail.GrandTotal / Total / objDetail.Quantity)) * objDetail.Quantity / objDetail.BaseUnitQuantity
            objDetail.PeriodCutOffInventoryItem = PeriodCutOffService.CreatePeriodCutOffInventoryItem(Inventory, objDetail.Item, TransDate, objDetail.BaseUnitQuantity, tmpUnitPrice, IIf(objDetail.Item.HasExpiryDate, objDetail.ExpiryDate, New Date), objDetail.BatchNo)
        Next
        'Dim objAccountLinkingConfig As AccountLinkingConfig = AccountLinkingConfig.GetInstance(Session)

        'Dim objSystemJournalEntry As New SystemJournalEntry
        'objSystemJournalEntry.Description = "Pembelian dengan no " & No & "(" & ReferenceNo & ")"
        'objSystemJournalEntry.TransDate = TransDate

        'Dim objSystemJournalEntryPurchaseAccount As New SystemJournalEntryDebit
        'objSystemJournalEntryPurchaseAccount.Account = objAccountLinkingConfig.PurchaseAccount
        'objSystemJournalEntryPurchaseAccount.Amount = DetailsTotal
        'objSystemJournalEntry.Debits.Add(objSystemJournalEntryPurchaseAccount)

        'If DetailsDiscount + Discount > 0 Then
        '    Dim objSystemJournalEntryPurchaseDiscountAccount As New SystemJournalEntryCredit
        '    objSystemJournalEntryPurchaseDiscountAccount.Account = objAccountLinkingConfig.PurchaseDiscountAccount
        '    objSystemJournalEntryPurchaseDiscountAccount.Amount = DetailsDiscount + Discount
        '    objSystemJournalEntry.Credits.Add(objSystemJournalEntryPurchaseDiscountAccount)
        'End If

        'Dim objSystemJournalEntryPurchaseInvoiceAccount As New SystemJournalEntryCredit
        'objSystemJournalEntryPurchaseInvoiceAccount.Account = objAccountLinkingConfig.PurchaseInvoiceAccount
        'objSystemJournalEntryPurchaseInvoiceAccount.Amount = GrandTotal
        'objSystemJournalEntry.Credits.Add(objSystemJournalEntryPurchaseInvoiceAccount)

        'Dim objSystemJournalEntryInventoryAccount As New SystemJournalEntryDebit
        'objSystemJournalEntryInventoryAccount.Account = objAccountLinkingConfig.GetInventoryAccountLinking(Inventory)
        'objSystemJournalEntryInventoryAccount.Amount = GrandTotal
        'objSystemJournalEntry.Debits.Add(objSystemJournalEntryInventoryAccount)
        'PeriodCutOffJournal = PeriodCutOffService.CreatePeriodCutOffJournal(Session, objSystemJournalEntry)
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmp = objDetail.PeriodCutOffInventoryItem
            objDetail.PeriodCutOffInventoryItem = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItem(tmp)
        Next
        Rounding = 0
        'Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        'PeriodCutOffJournal = Nothing
        'PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
    End Sub
End Class
Public Enum OutstandingStatus
    Full
    [PartiallyPaid]
    Cleared
End Enum