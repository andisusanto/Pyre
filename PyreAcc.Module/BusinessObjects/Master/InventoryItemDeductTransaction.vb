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
<Appearance("Appearance Default Disabled for InventoryItemDeductTransaction", enabled:=False, targetitems:="*", AppearanceItemType:="ViewItem")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class InventoryItemDeductTransaction
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private fInventory As Inventory
    Private fItem As Item
    Private fTransDate As Date
    Private fType As InventoryItemDeductTransactionType
    Private fQuantity As Integer
    Private fNote As String
    <RuleRequiredField("Rule Required for InventoryItemDeductTransaction.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return fInventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", fInventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItemDeductTransaction.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return fItem
        End Get
        Set(value As Item)
            SetPropertyValue("Item", fItem, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryItemDeductTransaction.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return fTransDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", fTransDate, value)
        End Set
    End Property
    Public Property Type As InventoryItemDeductTransactionType
        Get
            Return fType
        End Get
        Set(ByVal value As InventoryItemDeductTransactionType)
            SetPropertyValue("Type", fType, value)
        End Set
    End Property
    Public Property Quantity As Integer
        Get
            Return fQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Quantity", fQuantity, value)
        End Set
    End Property
    Public Property Note As String
        Get
            Return fNote
        End Get
        Set(value As String)
            SetPropertyValue("Note", fNote, value)
        End Set
    End Property
    <Association("InventoryItemDeductTransaction-InventoryItemDeductTransactionDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of InventoryItemDeductTransactionDetail)
        Get
            Return GetCollection(Of InventoryItemDeductTransactionDetail)("Details")
        End Get
    End Property

    Public Sub ResetDetail()
        For Each obj In Details
            obj.InventoryItem.DeductedQuantity -= obj.DeductedQuantity
        Next
        For i = 0 To Details.Count - 1
            Details(0).Delete()
        Next
    End Sub
    Public Sub DistributeDeduction()
        Dim tmpInventoryItem As New XPCollection(Of InventoryItem)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, GroupOperator.And(New BinaryOperator("Inventory", Inventory), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("Item", Item), New BinaryOperator("RemainingQuantity", 0, BinaryOperatorType.Greater), New BinaryOperator("IsDeleted", False))) With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        Dim xpInventoryItem As New XPCollection(Of InventoryItem)(Session, False)
        For Each obj In tmpInventoryItem
            xpInventoryItem.Add(obj)
        Next
        Dim availableQuantity As Integer = Quantity
        For Each obj In xpInventoryItem
            If availableQuantity = 0 Then Exit For
            Dim tmpDeductQuantity = availableQuantity
            If tmpDeductQuantity > obj.RemainingQuantity Then tmpDeductQuantity = obj.RemainingQuantity
            Dim objDeductDetail As New InventoryItemDeductTransactionDetail(Session) With {.InventoryItem = obj, .DeductedQuantity = tmpDeductQuantity, .InventoryItemDeductTransaction = Me}
            objDeductDetail.InventoryItem.DeductedQuantity += tmpDeductQuantity
            availableQuantity -= tmpDeductQuantity
        Next
        If availableQuantity > 0 Then Throw New Exception("Not enough balance")
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Inventory.DefaultDisplay & " ~ " & Item.DefaultDisplay & "-" & Type.ToString & "/" & TransDate.ToString
        End Get
    End Property
End Class
Public Enum InventoryItemDeductTransactionType
    Adjustment
    Sale
End Enum