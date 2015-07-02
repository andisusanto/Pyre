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
<Appearance("Appearance for InventoryItemAdjusment.BaseUnitQuantity < 0", visibility:=Editors.ViewItemVisibility.Hide, targetitems:="UnitPrice, ExpiryDate, BatchNo", criteria:="BaseUnitQuantity < 0")>
<Appearance("Appearance Default Disabled for InventoryItemAdjustment", AppearanceItemType:="ViewItem", targetitems:="BaseUnitQuantity, Total", enabled:=False)>
<RuleCriteria("Rule Criteria for InventoryItemAdjustment.BaseUnitQuantity <> 0", "Submit", "BaseUnitQuantity <> 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class InventoryItemAdjustment
    Inherits TransactionBase

    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Today
    End Sub

    Private _no As String
    Private _inventory As Inventory
    Private _item As Item
    Private _quantity As Decimal
    Private _unit As Unit
    Private _transDate As Date
    Private _baseUnitQuantity As Integer
    Private _unitPrice As Double
    Private _expiryDate As Date
    Private _batchNo As String
    Private _periodCutOffInventoryItem As PeriodCutOffInventoryItem
    Private _periodCutOffInventoryItemDeductTransaction As PeriodCutOffInventoryItemDeductTransaction

    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.No", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for InventoryItemAdjustment.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
            If Not IsLoading Then
                CalculateBaseUnitQuantity()
                Unit = Item.BaseUnit
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Quantity As Decimal
        Get
            Return _quantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Quantity", _quantity, value)
            If Not IsLoading Then CalculateBaseUnitQuantity()
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.Unit", DefaultContexts.Save)>
    <DataSourceProperty("Item.UnitSource")>
    Public Property Unit As Unit
        Get
            Return _unit
        End Get
        Set(ByVal value As Unit)
            SetPropertyValue("Unit", _unit, value)
            If Not IsLoading Then CalculateBaseUnitQuantity()
        End Set
    End Property

    Private Sub CalculateBaseUnitQuantity()
        If Unit IsNot Nothing AndAlso Item IsNot Nothing Then
            Dim tmpRate As Decimal = Item.GetUnitRate(Unit)
            BaseUnitQuantity = Quantity * tmpRate
        Else
            BaseUnitQuantity = 0
        End If
    End Sub
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    Public Property BaseUnitQuantity As Integer
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
        End Set
    End Property
    Public Property UnitPrice As Double
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("UnitPrice", _unitPrice, value)
        End Set
    End Property
    Public Property ExpiryDate As Date
        Get
            Return _expiryDate
        End Get
        Set(value As Date)
            SetPropertyValue("ExpiryDate", _expiryDate, value)
        End Set
    End Property
    Public Property BatchNo As String
        Get
            Return _batchNo
        End Get
        Set(value As String)
            SetPropertyValue("BatchNo", _batchNo, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffInventoryItem As PeriodCutOffInventoryItem
        Get
            Return _periodCutOffInventoryItem
        End Get
        Set(value As PeriodCutOffInventoryItem)
            SetPropertyValue("PeriodCutOffInventoryItem", _periodCutOffInventoryItem, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffInventoryItemDeductTransaction As PeriodCutOffInventoryItemDeductTransaction
        Get
            Return _periodCutOffInventoryItemDeductTransaction
        End Get
        Set(value As PeriodCutOffInventoryItemDeductTransaction)
            SetPropertyValue("PeriodCutOffInventoryItemInventoryItemDeductTransaction", _periodCutOffInventoryItemDeductTransaction, value)
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
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    Public Sub RecreateCreateJournal()
        Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
        CreateJournal()
    End Sub
    Private Sub CreateJournal()
        Dim objAccountLinkingConfig As AccountLinkingConfig = AccountLinkingConfig.GetInstance(Session)

        Dim objSystemJournalEntry As New SystemJournalEntry
        objSystemJournalEntry.Description = "Inventory Adjustment dengan no " & No
        objSystemJournalEntry.TransDate = TransDate

        If BaseUnitQuantity > 0 Then
            Dim tmpUnitPrice As Decimal = UnitPrice * Quantity / BaseUnitQuantity
            Dim total = tmpUnitPrice * BaseUnitQuantity
            Dim objSystemJournalEntrySalesAccount As New SystemJournalEntryCredit
            objSystemJournalEntrySalesAccount.Account = objAccountLinkingConfig.AdjustmentPlusAccount
            objSystemJournalEntrySalesAccount.Amount = total
            objSystemJournalEntry.Credits.Add(objSystemJournalEntrySalesAccount)

            Dim objSystemJournalEntrySalesInvoiceAccount As New SystemJournalEntryDebit
            objSystemJournalEntrySalesInvoiceAccount.Account = objAccountLinkingConfig.GetInventoryAccountLinking(Inventory)
            objSystemJournalEntrySalesInvoiceAccount.Amount = total
            objSystemJournalEntry.Debits.Add(objSystemJournalEntrySalesInvoiceAccount)
        Else
            Dim total As Decimal = 0
            For Each objDeductTransactionDetail In PeriodCutOffInventoryItemDeductTransaction.Details
                total += objDeductTransactionDetail.DeductedBaseUnitQuantity * objDeductTransactionDetail.PeriodCutOffInventoryItem.UnitPrice
            Next
            Dim objSystemJournalEntrySalesAccount As New SystemJournalEntryDebit
            objSystemJournalEntrySalesAccount.Account = objAccountLinkingConfig.AdjustmentMinusAccount
            objSystemJournalEntrySalesAccount.Amount = total
            objSystemJournalEntry.Debits.Add(objSystemJournalEntrySalesAccount)

            Dim objSystemJournalEntrySalesInvoiceAccount As New SystemJournalEntryCredit
            objSystemJournalEntrySalesInvoiceAccount.Account = objAccountLinkingConfig.GetInventoryAccountLinking(Inventory)
            objSystemJournalEntrySalesInvoiceAccount.Amount = total
            objSystemJournalEntry.Credits.Add(objSystemJournalEntrySalesInvoiceAccount)
        End If
        PeriodCutOffJournal = PeriodCutOffService.CreatePeriodCutOffJournal(Session, objSystemJournalEntry)
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        If BaseUnitQuantity > 0 Then
            Dim tmpUnitPrice As Decimal = UnitPrice * Quantity / BaseUnitQuantity
            PeriodCutOffInventoryItem = PeriodCutOffService.CreatePeriodCutOffInventoryItem(Inventory, Item, TransDate, BaseUnitQuantity, tmpUnitPrice, ExpiryDate, BatchNo)
        Else
            PeriodCutOffInventoryItemDeductTransaction = PeriodCutOffService.CreatePeriodCutOffInventoryItemDeductTransaction(Inventory, Item, TransDate, -1 * BaseUnitQuantity, PeriodCutOffInventoryItemDeductTransactionType.Adjustment)
        End If
        CreateJournal()
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        If BaseUnitQuantity > 0 Then
            Dim tmp = PeriodCutOffInventoryItem
            PeriodCutOffInventoryItem = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItem(tmp)
        Else
            Dim tmp = PeriodCutOffInventoryItemDeductTransaction
            PeriodCutOffInventoryItemDeductTransaction = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItemDeductTransaction(tmp)
        End If
        Dim tmpPeriodCutOffJournal = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmpPeriodCutOffJournal)
    End Sub
End Class
