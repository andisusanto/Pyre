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
Public Class PeriodCutOffInventoryItemDeductTransaction
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _periodCutOff As PeriodCutOff
    Private _inventory As Inventory
    Private _item As Item
    Private _transDate As Date
    Private _entryDate As Date
    Private _type As PeriodCutOffInventoryItemDeductTransactionType
    Private _baseUnitQuantity As Decimal
    <Association("PeriodCutOff-PeriodCutOffInventoryItemDeductTransaction")>
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransaction.PeriodCutOff", DefaultContexts.Save)>
    Public Property PeriodCutOff As PeriodCutOff
        Get
            Return _periodCutOff
        End Get
        Set(ByVal value As PeriodCutOff)
            SetPropertyValue("PeriodCutOff", _periodCutOff, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransaction.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransaction.Item", DefaultContexts.Save)>
    Public Property Item As Item

        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransaction.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransaction.EntryDate", DefaultContexts.Save)>
    <VisibleInDetailView(False), VisibleInListView(False)>
    Public Property EntryDate As Date
        Get
            Return _entryDate
        End Get
        Set(value As Date)
            SetPropertyValue("EntryDate", _entryDate, value)
        End Set
    End Property
    Public Property Type As PeriodCutOffInventoryItemDeductTransactionType
        Get
            Return _type
        End Get
        Set(ByVal value As PeriodCutOffInventoryItemDeductTransactionType)
            SetPropertyValue("Type", _type, value)
        End Set
    End Property
    Public Property BaseUnitQuantity As Decimal
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
        End Set
    End Property
    <Association("PeriodCutOffInventoryItemDeductTransaction-PeriodCutOffInventoryItemDeductTransactionDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PeriodCutOffInventoryItemDeductTransactionDetail)
        Get
            Return GetCollection(Of PeriodCutOffInventoryItemDeductTransactionDetail)("Details")
        End Get
    End Property
    Public Sub ResetDetail()
        For Each obj In Details
            obj.PeriodCutOffInventoryItem.DeductedBaseUnitQuantity -= obj.DeductedBaseUnitQuantity
        Next
        For i = 0 To Details.Count - 1
            Details(0).Delete()
        Next
    End Sub
    Public Sub DistributeDeduction()
        Dim tmpInventoryItem As New XPCollection(Of PeriodCutOffInventoryItem)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                               GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOff), _
                                                                                                 New BinaryOperator("Inventory", Inventory), _
                                                                                                 GroupOperator.Or(
                                                                                                               New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Less), _
                                                                                                               GroupOperator.And(
                                                                                                                             New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Equal), _
                                                                                                                             New BinaryOperator("EntryDate", EntryDate, BinaryOperatorType.Less))), _
                                                                                                 New BinaryOperator("Item", Item), _
                                                                                                 New BinaryOperator("RemainingBaseUnitQuantity", 0, BinaryOperatorType.Greater), _
                                                                                                 New BinaryOperator("IsDeleted", False), _
                                                                                                 GroupOperator.Or(New BinaryOperator("Item.HasExpiryDate", False), _
                                                                                                                  New BinaryOperator("ExpiryDate", TransDate, BinaryOperatorType.GreaterOrEqual)))) _
        With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        Dim xpInventoryItem As New XPCollection(Of PeriodCutOffInventoryItem)(Session, False)
        For Each obj In tmpInventoryItem
            xpInventoryItem.Add(obj)
        Next
        Dim availableBaseUnitQuantity As Decimal = BaseUnitQuantity
        For Each obj In xpInventoryItem
            If availableBaseUnitQuantity = 0 Then Exit For
            Dim tmpDeductBaseUnitQuantity = availableBaseUnitQuantity
            If tmpDeductBaseUnitQuantity > obj.RemainingBaseUnitQuantity Then tmpDeductBaseUnitQuantity = obj.RemainingBaseUnitQuantity
            Dim objDeductDetail As New PeriodCutOffInventoryItemDeductTransactionDetail(Session) With {.PeriodCutOffInventoryItem = obj, .DeductedBaseUnitQuantity = tmpDeductBaseUnitQuantity, .PeriodCutOffInventoryItemDeductTransaction = Me}
            objDeductDetail.PeriodCutOffInventoryItem.DeductedBaseUnitQuantity += tmpDeductBaseUnitQuantity
            availableBaseUnitQuantity -= tmpDeductBaseUnitQuantity
        Next
        If availableBaseUnitQuantity > 0 Then Throw New Exception("Not enough balance")
    End Sub
End Class

Public Enum PeriodCutOffInventoryItemDeductTransactionType
    Adjustment
    Sale
    Returned
End Enum