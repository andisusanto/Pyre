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
<RuleCriteria("Rule Criteria for Cancel SalesInvoice.PaidAmount = 0", "Cancel", "PaidAmount = 0")>
<RuleCriteria("Rule Criteria for SalesInvoice.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for SalesInvoice.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for SalesInvoice", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="DetailsTotal, DetailsDiscount, Total, Discount, GrandTotal, IndonesianWordSays, Rounding, PaidAmount, PaymentOutstandingAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesInvoice
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = GlobalFunction.GetServerNow(Session).Date
    End Sub
    Private _no As String
    Private _transDate As Date
    Private _term As Integer
    Private _dueDate As Date
    Private _customer As Customer
    Private _inventory As Inventory
    Private _detailsTotal As Decimal
    Private _detailsDiscount As Decimal
    Private _total As Decimal
    Private _discountType As DiscountType
    Private _discountValue As Decimal
    Private _discount As Decimal
    Private _grandTotal As Decimal
    Private _rounding As Decimal
    Private _indonesianWordSays As String
    Private _paidAmount As Decimal
    Private _paymentOutstandingAmount As Decimal
    Private _salesman As Salesman
    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleUniqueValue("Rule Unique for SalesInvoice.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for SalesInvoice.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.TransDate", DefaultContexts.Save)>
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
    <RuleRequiredField("Rule Required for SalesInvoice.DueDate", DefaultContexts.Save)>
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
    <RuleRequiredField("Rule Required for SalesInvoice.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.Inventory", DefaultContexts.Save)>
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
                IndonesianWordSays = PyreAcc.Module.IndonesianWordSays.GetIndonesianSays(GrandTotal)
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
    <VisibleInDetailView(False), VisibleInListView(False)>
    Public Property IndonesianWordSays As String
        Get
            Return _indonesianWordSays
        End Get
        Set(value As String)
            SetPropertyValue("IndonesianWordSays", _indonesianWordSays, value)
        End Set
    End Property
    Public Property PaidAmount As Decimal
        Get
            Return _paidAmount
        End Get
        Set(value As Decimal)
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
        Set(value As Decimal)
            SetPropertyValue("PaymentOutstandingAmount", _paymentOutstandingAmount, value)
        End Set
    End Property
    Public Property Salesman As Salesman
        Get
            Return _salesman
        End Get
        Set(value As Salesman)
            SetPropertyValue("Salesman", _salesman, value)
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
    <Association("SalesInvoice-SalesInvoiceDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesInvoiceDetail)
        Get
            Return GetCollection(Of SalesInvoiceDetail)("Details")
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
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsExceedingMaximumOutstandingPaymentAmount As Boolean
        Get
            If CType(SecuritySystem.CurrentUser, ApplicationUser).SubmitExceedMaximumOutstandingPaymentInvoice Then Return False
            If Customer.MaximumOutstandingPaymentAmount < Customer.OutstandingPaymentAmount + PaymentOutstandingAmount Then Return True
            Return False
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
        Dim CheckPriceRange = Not CType(SecuritySystem.CurrentUser, ApplicationUser).SubmitOutOfPriceRangeInvoice
        For Each objDetail In Details
            If CheckPriceRange Then
                Dim itemPrice As ItemPrice = objDetail.Item.GetPrice(TransDate)
                Dim tmpRate As Decimal = objDetail.Item.GetUnitRate(objDetail.Unit)
                If objDetail.UnitPrice * tmpRate > itemPrice.MaximumPrice OrElse objDetail.UnitPrice < itemPrice.MinimumPrice Then
                    If Not CType(SecuritySystem.CurrentUser, ApplicationUser).SubmitOutOfPriceRangeInvoice Then Throw New Exception(String.Format("Line with item {0}'s price out of range", objDetail.Item.Name))
                End If
            End If
            objDetail.PeriodCutOffInventoryItemDeductTransaction = PeriodCutOffService.CreatePeriodCutOffInventoryItemDeductTransaction(Inventory, objDetail.Item, TransDate, objDetail.BaseUnitQuantity, PeriodCutOffInventoryItemDeductTransactionType.Sale)
        Next
        Customer.OutstandingPaymentAmount += GrandTotal

        Dim objAccountLinkingConfig As AccountLinkingConfig = AccountLinkingConfig.GetInstance(Session)

        Dim objSystemJournalEntry As New SystemJournalEntry
        objSystemJournalEntry.Description = "Penjualan dengan no " & No
        objSystemJournalEntry.TransDate = TransDate

        Dim objSystemJournalEntrySalesAccount As New SystemJournalEntryCredit
        objSystemJournalEntrySalesAccount.Account = objAccountLinkingConfig.SalesAccount
        objSystemJournalEntrySalesAccount.Amount = DetailsTotal
        objSystemJournalEntry.Credits.Add(objSystemJournalEntrySalesAccount)

        If DetailsDiscount + Discount > 0 Then
            Dim objSystemJournalEntrySalesDiscountAccount As New SystemJournalEntryDebit
            objSystemJournalEntrySalesDiscountAccount.Account = objAccountLinkingConfig.SalesDiscountAccount
            objSystemJournalEntrySalesDiscountAccount.Amount = DetailsDiscount + Discount
            objSystemJournalEntry.Debits.Add(objSystemJournalEntrySalesDiscountAccount)
        End If

        Dim objSystemJournalEntrySalesInvoiceAccount As New SystemJournalEntryDebit
        objSystemJournalEntrySalesInvoiceAccount.Account = objAccountLinkingConfig.SalesInvoiceAccount
        objSystemJournalEntrySalesInvoiceAccount.Amount = GrandTotal
        objSystemJournalEntry.Debits.Add(objSystemJournalEntrySalesInvoiceAccount)
        'CoGS
        Dim tmpCoGS As Decimal = 0
        For Each objSalesInvoiceDetail In Details
            For Each objDeductTransactionDetail In objSalesInvoiceDetail.PeriodCutOffInventoryItemDeductTransaction.Details
                tmpCoGS += objDeductTransactionDetail.DeductedBaseUnitQuantity * objDeductTransactionDetail.PeriodCutOffInventoryItem.UnitPrice
            Next
        Next
        Dim objSystemJournalEntryInventoryAccount As New SystemJournalEntryCredit
        objSystemJournalEntryInventoryAccount.Account = objAccountLinkingConfig.GetInventoryAccountLinking(Inventory)
        objSystemJournalEntryInventoryAccount.Amount = tmpCoGS
        objSystemJournalEntry.Credits.Add(objSystemJournalEntryInventoryAccount)
        PeriodCutOffJournal = PeriodCutOffService.CreatePeriodCutOffJournal(Session, objSystemJournalEntry)
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmp = objDetail.PeriodCutOffInventoryItemDeductTransaction
            objDetail.PeriodCutOffInventoryItemDeductTransaction = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItemDeductTransaction(tmp)
        Next
        Customer.OutstandingPaymentAmount -= GrandTotal
        Rounding = 0
        Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
    End Sub
End Class