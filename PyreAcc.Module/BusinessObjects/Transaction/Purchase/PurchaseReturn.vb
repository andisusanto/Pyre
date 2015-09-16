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
<RuleCriteria("Rule Criteria for Cancel PurchaseReturn.PaidAmount = 0", "Cancel", "PaidAmount = 0")>
<RuleCriteria("Rule Criteria for PurchaseReturn.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for PurchaseReturn.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for PurchaseReturn", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, Discount, GrandTotal, PaidAmount, PaymentOutstandingAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseReturn
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = GlobalFunction.GetServerNow(Session).Date
        Inventory = SysConfig.ReturnItemInventory
    End Sub
    Private _no As String
    Private _referenceNo As String
    Private _transDate As Date
    Private _term As Integer
    Private _dueDate As Date
    Private _supplier As Supplier
    Private _inventory As Inventory
    Private _total As Decimal
    Private _discountType As DiscountType
    Private _discountValue As Decimal
    Private _discount As Decimal
    Private _grandTotal As Decimal
    Private _debitNote As DebitNote
    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleUniqueValue("Rule Unique for PurchaseReturn.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for PurchaseReturn.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseReturn.ReferenceNo", DefaultContexts.Save)>
    Public Property ReferenceNo As String
        Get
            Return _referenceNo
        End Get
        Set(value As String)
            SetPropertyValue("ReferenceNo", _referenceNo, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseReturn.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseReturn.Supplier", DefaultContexts.Save)>
        Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseReturn.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
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
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property DebitNote As DebitNote
        Get
            Return _debitNote
        End Get
        Set(value As DebitNote)
            SetPropertyValue("DebitNote", _debitNote, value)
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
    <Association("PurchaseReturn-PurchaseReturnDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PurchaseReturnDetail)
        Get
            Return GetCollection(Of PurchaseReturnDetail)("Details")
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
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
    Public Sub RecreateCreateJournal()
        Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
        CreateJournal()
    End Sub
    Private Sub CreateJournal()
        Dim objAccountLinkingConfig As AccountLinkingConfig = AccountLinkingConfig.GetInstance(Session)

        Dim objSystemJournalEntry As New SystemJournalEntry
        objSystemJournalEntry.Description = "Retur Pembelian dengan no " & No & "(" & ReferenceNo & ")"
        objSystemJournalEntry.TransDate = TransDate

        Dim objSystemJournalEntryDebitNoteAccount As New SystemJournalEntryDebit
        objSystemJournalEntryDebitNoteAccount.Account = objAccountLinkingConfig.DebitNoteAccount
        objSystemJournalEntryDebitNoteAccount.Amount = GrandTotal
        objSystemJournalEntry.Debits.Add(objSystemJournalEntryDebitNoteAccount)

        Dim objSystemJournalEntryPurchaseReturnAccount As New SystemJournalEntryCredit
        objSystemJournalEntryPurchaseReturnAccount.Account = objAccountLinkingConfig.PurchaseReturnAccount
        objSystemJournalEntryPurchaseReturnAccount.Amount = GrandTotal
        objSystemJournalEntry.Credits.Add(objSystemJournalEntryPurchaseReturnAccount)

        Dim objSystemJournalEntryInventoryAccount As New SystemJournalEntryCredit
        objSystemJournalEntryInventoryAccount.Account = objAccountLinkingConfig.GetInventoryAccountLinking(Inventory)
        objSystemJournalEntryInventoryAccount.Amount = GrandTotal
        objSystemJournalEntry.Credits.Add(objSystemJournalEntryInventoryAccount)
        PeriodCutOffJournal = PeriodCutOffService.CreatePeriodCutOffJournal(Session, objSystemJournalEntry)
    End Sub

    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        Dim inventoryValue As Decimal = 0
        For Each objDetail In Details
            objDetail.PeriodCutOffInventoryItemDeductTransaction = PeriodCutOffService.CreatePeriodCutOffInventoryItemDeductTransaction(Inventory, objDetail.Item, TransDate, objDetail.BaseUnitQuantity, PeriodCutOffInventoryItemDeductTransactionType.Returned)
            For Each obj In objDetail.PeriodCutOffInventoryItemDeductTransaction.Details
                inventoryValue += obj.DeductedBaseUnitQuantity * obj.PeriodCutOffInventoryItem.UnitPrice
            Next
        Next
        Dim objAutoNo As AutoNo = Session.FindObject(Of AutoNo)(GroupOperator.And(New BinaryOperator("TargetType", "PyreAcc.Module.DebitNote"), New BinaryOperator("IsActive", True)))
        Dim objDebitNote As New DebitNote(Session) With {.FromSupplier = Supplier, .TransDate = TransDate, .Amount = Total, .Note = "Create from return transaction with no " & No}
        objDebitNote.No = objAutoNo.GetAutoNo(objDebitNote)
        DebitNote = objDebitNote
        CreateJournal()
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmpPeriodCutOffInventoryItemDeductTransaction = objDetail.PeriodCutOffInventoryItemDeductTransaction
            objDetail.PeriodCutOffInventoryItemDeductTransaction = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItemDeductTransaction(tmpPeriodCutOffInventoryItemDeductTransaction)
        Next
        Dim tmp = DebitNote
        DebitNote = Nothing
        tmp.Delete()
        Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
    End Sub
End Class