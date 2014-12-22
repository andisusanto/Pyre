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
<CreatableItem(False)>
<DeferredDeletion(False)>
<Appearance("Appearance Default Display for InventoryItem", AppearanceItemType:="ViewItem", targetitems:="*", enabled:=False)>
<DefaultClassOptions()> _
Public Class InventoryItem
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _inventory As Inventory
    Private _item As Item
    Private _transDate As Date
    Private _quantity As Integer
    Private _deductedQuantity As Integer
    Private _remainingQuantity As Integer
    Private _unitPrice As Double
    Private _note As String
    <Association("Inventory-InventoryItem")>
    <RuleRequiredField("Rule Required for InventoryItem.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItem.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItem.TransDate", DefaultContexts.Save)>
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
            If Not IsLoading Then
                CalculateRemainingQuantity()
            End If
        End Set
    End Property
    Public Property DeductedQuantity As Integer
        Get
            Return _deductedQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("DeductedQuantity", _deductedQuantity, value)
            If Not IsLoading Then
                CalculateRemainingQuantity()
            End If
        End Set
    End Property
    Public Property RemainingQuantity As Integer
        Get
            Return _remainingQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("RemainingQuantity", _remainingQuantity, value)
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
    Public Property Note As String
        Get
            Return _note
        End Get
        Set(value As String)
            SetPropertyValue("Note", _note, value)
        End Set
    End Property
    Private Sub CalculateRemainingQuantity()
        RemainingQuantity = Quantity - DeductedQuantity
    End Sub
    <Association("InventoryItem-InventoryItemDeductTransactionDetail")>
    Public ReadOnly Property DeductDetails As XPCollection(Of InventoryItemDeductTransactionDetail)
        Get
            Return GetCollection(Of InventoryItemDeductTransactionDetail)("DeductDetails")
        End Get
    End Property
End Class
