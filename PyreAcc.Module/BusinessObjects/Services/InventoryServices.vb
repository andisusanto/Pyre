Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Public Class InventoryServices
    Public Shared Function CreateInventoryItem(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal Quantity As Integer, ByVal UnitPrice As Double) As InventoryItem
        Dim session As Session = Inventory.Session
        Dim objInventoryItem As New InventoryItem(session) With {.Inventory = Inventory, .Item = Item, .TransDate = TransDate, .Quantity = Quantity, .UnitPrice = UnitPrice}
        Dim xpInventoryDeductTransaction As New XPCollection(Of InventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, GroupOperator.And(New BinaryOperator("Inventory", Inventory), New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Greater), New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}

        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
            obj.DistributeDeduction()
        Next
        Return objInventoryItem
    End Function
    Public Shared Sub DeleteInventoryItem(ByVal InventoryItem As InventoryItem)
        Dim Session As Session = InventoryItem.Session
        Dim xpInventoryDeductTransaction As New XPCollection(Of InventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, GroupOperator.And(New BinaryOperator("Inventory", InventoryItem.Inventory), New BinaryOperator("Item", InventoryItem.Item), New BinaryOperator("TransDate", InventoryItem.TransDate, BinaryOperatorType.Greater), New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        InventoryItem.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
            obj.DistributeDeduction()
        Next
    End Sub
    Public Shared Function CreateInventoryDeductTransaction(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal Quantity As Integer, ByVal Type As InventoryItemDeductTransactionType) As InventoryItemDeductTransaction
        Dim session As Session = Inventory.Session
        Dim objInventoryItemDeductTransaction As New InventoryItemDeductTransaction(session) With {.Inventory = Inventory, .Item = Item, .TransDate = TransDate, .Quantity = Quantity, .Type = Type}
        Dim xpInventoryDeductTransaction As New XPCollection(Of InventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, GroupOperator.And(New BinaryOperator("Inventory", Inventory), New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Greater), New BinaryOperator("IsDeleted", False))) _
             With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}

        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        objInventoryItemDeductTransaction.DistributeDeduction()
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
        Return objInventoryItemDeductTransaction
    End Function
    Public Shared Sub DeleteInventoryItemDeductTransaction(ByVal InventoryDeductTransaction As InventoryItemDeductTransaction)
        Dim Session As Session = InventoryDeductTransaction.Session
        Dim xpInventoryDeductTransaction As New XPCollection(Of InventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, GroupOperator.And(New BinaryOperator("Inventory", InventoryDeductTransaction.Inventory), New BinaryOperator("Item", InventoryDeductTransaction.Item), New BinaryOperator("TransDate", InventoryDeductTransaction.TransDate, BinaryOperatorType.Greater), New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        InventoryDeductTransaction.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
            obj.DistributeDeduction()
        Next
    End Sub
End Class
