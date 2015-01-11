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

<DeferredDeletion(False)>
<RuleCriteria("Rule Criteria for BalanceSheetInventoryItem.UnitPrice > 0", DefaultContexts.Save, "UnitPrice > 0")>
<DefaultClassOptions()> _
Public Class BalanceSheetInventoryItem
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _balanceSheet As BalanceSheet
    Private _inventory As Inventory
    Private _item As Item
    Private _transDate As Date
    Private _unitPrice As Decimal
    Private _expiryDate As Date
    Private _quantity As Decimal
    Private _deductedQuantity As Decimal
    Private _remainingQuantity As Decimal
    <Association("BalanceSheet-BalanceSheetInventoryItem")>
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItem.BalanceSheet", DefaultContexts.Save)>
    Public Property BalanceSheet As BalanceSheet
        Get
            Return _balanceSheet
        End Get
        Set(ByVal value As BalanceSheet)
            SetPropertyValue("BalanceSheet", _balanceSheet, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItem.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItem.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItem.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    Public Property UnitPrice As Decimal
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("UnitPrice", _unitPrice, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItem.ExpiryDate", DefaultContexts.Save, targetcriteria:="Item.HasExpiryDate = TRUE")>
    Public Property ExpiryDate As Date
        Get
            Return _expiryDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("ExpiryDate", _expiryDate, value)
        End Set
    End Property
    Public Property Quantity As Decimal
        Get
            Return _quantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Quantity", _quantity, value)
            If Not IsLoading Then
                CalculateRemainingQuantity()
            End If
        End Set
    End Property
    Public Property DeductedQuantity As Decimal
        Get
            Return _deductedQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DeductedQuantity", _deductedQuantity, value)
            If Not IsLoading Then
                CalculateRemainingQuantity()
            End If
        End Set
    End Property
    Public Property RemainingQuantity As Decimal
        Get
            Return _remainingQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("RemainingQuantity", _remainingQuantity, value)
        End Set
    End Property
    Private Sub CalculateRemainingQuantity()
        RemainingQuantity = Quantity - DeductedQuantity
    End Sub
    <Association("BalanceSheetInventoryItem-BalanceSheetInventoryItemDeductTransactionDetail")>
    Public ReadOnly Property DeductDetails As XPCollection(Of BalanceSheetInventoryItemDeductTransactionDetail)
        Get
            Return GetCollection(Of BalanceSheetInventoryItemDeductTransactionDetail)("DeductDetails")
        End Get
    End Property
End Class
