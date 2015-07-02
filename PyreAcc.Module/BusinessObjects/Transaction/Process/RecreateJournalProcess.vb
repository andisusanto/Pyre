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
Public Class RecreateJournalProcess
    Inherits ProcessBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _since As Date
    Private _until As Date
    Public Property Since As Date
        Get
            Return _since
        End Get
        Set(value As Date)
            SetPropertyValue("Since", _since, value)
        End Set
    End Property
    Public Property Until As Date
        Get
            Return _until
        End Get
        Set(value As Date)
            SetPropertyValue("Until", _until, value)
        End Set
    End Property
    Public Overrides Sub Execute()
        Dim xpPurchaseInvoice As New XPCollection(Of PurchaseInvoice)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpPurchaseInvoice
            obj.RecreateCreateJournal()
        Next
        Dim xpSalesInvoice As New XPCollection(Of SalesInvoice)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpSalesInvoice
            obj.RecreateCreateJournal()
        Next
        Dim xpPurchasePayment As New XPCollection(Of PurchasePayment)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpPurchasePayment
            obj.RecreateCreateJournal()
        Next
        Dim xpSalesPayment As New XPCollection(Of SalesPayment)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpSalesPayment
            obj.RecreateCreateJournal()
        Next
        Dim xpPurchaseReturn As New XPCollection(Of PurchaseReturn)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpPurchaseReturn
            obj.RecreateCreateJournal()
        Next
        Dim xpSalesReturn As New XPCollection(Of SalesReturn)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("No", "IMP%", BinaryOperatorType.Like), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpSalesReturn
            obj.RecreateCreateJournal()
        Next
        Dim xpInventoryItemAdjustment As New XPCollection(Of InventoryItemAdjustment)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpInventoryItemAdjustment
            obj.RecreateCreateJournal()
        Next
        Dim xpJournalEntry As New XPCollection(Of JournalEntry)(Session, GroupOperator.And(New BetweenOperator("TransDate", Since, Until), New BinaryOperator("Status", TransactionStatus.Submitted))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        For Each obj In xpJournalEntry
            obj.RecreateCreateJournal()
        Next
    End Sub
End Class
