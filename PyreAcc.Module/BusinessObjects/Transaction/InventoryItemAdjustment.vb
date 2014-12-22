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
    Private _total As Double
    Private _inventoryItem As InventoryItem
    Private _inventoryItemDeductTransaction As InventoryItemDeductTransaction
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
    Public Property Total As Double
        Get
            Return _total
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("Total", _total, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property InventoryItem As InventoryItem
        Get
            Return _inventoryItem
        End Get
        Set(value As InventoryItem)
            SetPropertyValue("InventoryItem", _inventoryItem, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property InventoryItemDeductTransaction As InventoryItemDeductTransaction
        Get
            Return _inventoryItemDeductTransaction
        End Get
        Set(value As InventoryItemDeductTransaction)
            SetPropertyValue("InventoryItemDeductTransaction", _inventoryItemDeductTransaction, value)
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
            InventoryItem = InventoryServices.CreateInventoryItem(Inventory, Item, TransDate, Quantity, UnitPrice)
        Else
            InventoryItemDeductTransaction = InventoryServices.CreateInventoryDeductTransaction(Inventory, Item, TransDate, -1 * Quantity, InventoryItemDeductTransactionType.Adjustment)
        End If
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        If Quantity > 0 Then
            Dim tmp = InventoryItem
            InventoryItem = Nothing
            InventoryServices.DeleteInventoryItem(tmp)
        Else
            Dim tmp = InventoryItemDeductTransaction
            InventoryItemDeductTransaction = Nothing
            InventoryServices.DeleteInventoryItemDeductTransaction(tmp)
        End If
    End Sub
End Class
