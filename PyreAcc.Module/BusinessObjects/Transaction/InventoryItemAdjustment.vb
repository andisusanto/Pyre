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
<Appearance("Appearance for InventoryItemAdjusment.Quantity < 0", visibility:=Editors.ViewItemVisibility.Hide, targetitems:="UnitPrice, ExpiryDate", criteria:="Quantity < 0")>
<Appearance("Appearance for InventoryItemAdjusment.Item.HasExpiryDate = FALSE", visibility:=Editors.ViewItemVisibility.Hide, targetitems:="ExpiryDate", criteria:="Item.HasExpiryDate = FALSE")>
<Appearance("Appearance Default Disabled for InventoryItemAdjustment", AppearanceItemType:="ViewItem", targetitems:="Total", enabled:=False)>
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
    Private _transDate As Date
    Private _quantity As Integer
    Private _unitPrice As Double
    Private _expiryDate As Date
    Private _balanceSheetInventoryItem As BalanceSheetInventoryItem
    Private _balanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
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
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Quantity As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Quantity", _quantity, value)
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
    <RuleRequiredField("Rule Required for InventoryItemAdjustment.ExpiryDate", DefaultContexts.Save, targetcriteria:="Item.HasExpiryDate AND Quantity > 0")>
    Public Property ExpiryDate As Date
        Get
            Return _expiryDate
        End Get
        Set(value As Date)
            SetPropertyValue("ExpiryDate", _expiryDate, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetInventoryItem As BalanceSheetInventoryItem
        Get
            Return _balanceSheetInventoryItem
        End Get
        Set(value As BalanceSheetInventoryItem)
            SetPropertyValue("BalanceSheetInventoryItem", _balanceSheetInventoryItem, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
        Get
            Return _balanceSheetInventoryItemDeductTransaction
        End Get
        Set(value As BalanceSheetInventoryItemDeductTransaction)
            SetPropertyValue("BalanceSheetInventoryItemInventoryItemDeductTransaction", _balanceSheetInventoryItemDeductTransaction, value)
        End Set
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        If Quantity > 0 Then
            BalanceSheetInventoryItem = BalanceSheetService.CreateBalanceSheetInventoryItem(Inventory, Item, TransDate, Quantity, UnitPrice, ExpiryDate)
        Else
            BalanceSheetInventoryItemDeductTransaction = BalanceSheetService.CreateBalanceSheetInventoryItemDeductTransaction(Inventory, Item, TransDate, -1 * Quantity, BalanceSheetInventoryItemDeductTransactionType.Adjustment)
        End If
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        If Quantity > 0 Then
            Dim tmp = BalanceSheetInventoryItem
            BalanceSheetInventoryItem = Nothing
            BalanceSheetService.DeleteBalanceSheetInventoryItem(tmp)
        Else
            Dim tmp = BalanceSheetInventoryItemDeductTransaction
            BalanceSheetInventoryItemDeductTransaction = Nothing
            BalanceSheetService.DeleteBalanceSheetInventoryItemDeductTransaction(tmp)
        End If
    End Sub
End Class
