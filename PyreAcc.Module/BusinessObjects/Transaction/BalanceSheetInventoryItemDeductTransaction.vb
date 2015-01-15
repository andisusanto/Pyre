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
<DefaultClassOptions()> _
Public Class BalanceSheetInventoryItemDeductTransaction
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
    Private _type As BalanceSheetInventoryItemDeductTransactionType
    Private _quantity As Decimal
    <Association("BalanceSheet-BalanceSheetInventoryItemDeductTransaction")>
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransaction.BalanceSheet", DefaultContexts.Save)>
    Public Property BalanceSheet As BalanceSheet
        Get
            Return _balanceSheet
        End Get
        Set(ByVal value As BalanceSheet)
            SetPropertyValue("BalanceSheet", _balanceSheet, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransaction.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransaction.Item", DefaultContexts.Save)>
    Public Property Item As Item

        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransaction.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    Public Property Type As BalanceSheetInventoryItemDeductTransactionType
        Get
            Return _type
        End Get
        Set(ByVal value As BalanceSheetInventoryItemDeductTransactionType)
            SetPropertyValue("Type", _type, value)
        End Set
    End Property
    Public Property Quantity As Decimal
        Get
            Return _quantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Quantity", _quantity, value)
        End Set
    End Property
    <Association("BalanceSheetInventoryItemDeductTransaction-BalanceSheetInventoryItemDeductTransactionDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of BalanceSheetInventoryItemDeductTransactionDetail)
        Get
            Return GetCollection(Of BalanceSheetInventoryItemDeductTransactionDetail)("Details")
        End Get
    End Property
    Public Sub ResetDetail()
        For Each obj In Details
            obj.BalanceSheetInventoryItem.DeductedQuantity -= obj.DeductedQuantity
        Next
        For i = 0 To Details.Count - 1
            Details(0).Delete()
        Next
    End Sub
    Public Sub DistributeDeduction()
        Dim tmpInventoryItem As New XPCollection(Of BalanceSheetInventoryItem)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                               GroupOperator.And(New BinaryOperator("BalanceSheet", BalanceSheet), _
                                                                                                 New BinaryOperator("Inventory", Inventory), _
                                                                                                 New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual), _
                                                                                                 New BinaryOperator("Item", Item), _
                                                                                                 New BinaryOperator("RemainingQuantity", 0, BinaryOperatorType.Greater), _
                                                                                                 New BinaryOperator("IsDeleted", False), _
                                                                                                 GroupOperator.Or(New BinaryOperator("Item.HasExpiryDate", False), _
                                                                                                                  New BinaryOperator("ExpiryDate", TransDate, BinaryOperatorType.GreaterOrEqual)))) _
        With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        Dim xpInventoryItem As New XPCollection(Of BalanceSheetInventoryItem)(Session, False)
        For Each obj In tmpInventoryItem
            xpInventoryItem.Add(obj)
        Next
        Dim availableQuantity As Decimal = Quantity
        For Each obj In xpInventoryItem
            If availableQuantity = 0 Then Exit For
            Dim tmpDeductQuantity = availableQuantity
            If tmpDeductQuantity > obj.RemainingQuantity Then tmpDeductQuantity = obj.RemainingQuantity
            Dim objDeductDetail As New BalanceSheetInventoryItemDeductTransactionDetail(Session) With {.BalanceSheetInventoryItem = obj, .DeductedQuantity = tmpDeductQuantity, .BalanceSheetInventoryItemDeductTransaction = Me}
            objDeductDetail.BalanceSheetInventoryItem.DeductedQuantity += tmpDeductQuantity
            availableQuantity -= tmpDeductQuantity
        Next
        If availableQuantity > 0 Then Throw New Exception("Not enough balance")
    End Sub
End Class

Public Enum BalanceSheetInventoryItemDeductTransactionType
    Adjustment
    Sale
End Enum