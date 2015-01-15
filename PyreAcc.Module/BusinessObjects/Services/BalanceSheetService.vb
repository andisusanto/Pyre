Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Public Class BalanceSheetService
    Public Shared Function CreateBalanceSheetInventoryItem(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal Quantity As Integer, ByVal UnitPrice As Double, ByVal ExpiryDate As Date) As BalanceSheetInventoryItem
        Dim session As Session = Inventory.Session
        Dim period As Period = session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
        If period Is Nothing OrElse period.Closed Then Throw New Exception("Not in open period")
        Dim BalanceSheet As BalanceSheet = session.FindObject(Of BalanceSheet)(New BinaryOperator("Period", period))
        Dim objBalanceSheetInventoryItem As New BalanceSheetInventoryItem(session) With {.BalanceSheet = BalanceSheet, .Inventory = Inventory, .Item = Item, .TransDate = TransDate, .Quantity = Quantity, .UnitPrice = UnitPrice, .ExpiryDate = ExpiryDate}
        Dim xpInventoryDeductTransaction As New XPCollection(Of BalanceSheetInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, _
                                                                                                            GroupOperator.And(New BinaryOperator("BalanceSheet", BalanceSheet), _
                                                                                                                              New BinaryOperator("Inventory", Inventory), _
                                                                                                                              New BinaryOperator("Item", Item), _
                                                                                                                              New BinaryOperator("TransDate", TransDate, BinaryOperatorType.GreaterOrEqual), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}

        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
        Return objBalanceSheetInventoryItem
    End Function
    Public Shared Sub DeleteBalanceSheetInventoryItem(ByVal BalanceSheetInventoryItem As BalanceSheetInventoryItem)
        Dim Session As Session = BalanceSheetInventoryItem.Session
        Dim period As Period = BalanceSheetInventoryItem.BalanceSheet.Period
        If period.Closed Then Throw New Exception("Not in open period")

        'Dim currentBalance As Decimal = BalanceSheetService.GetBalance(BalanceSheetInventoryItem.BalanceSheet, BalanceSheetInventoryItem.TransDate, BalanceSheetInventoryItem.Inventory, BalanceSheetInventoryItem.Item)
        'If currentBalance < BalanceSheetInventoryItem.Quantity Then Throw New Exception("Balance is not enough")

        Dim xpInventoryDeductTransaction As New XPCollection(Of BalanceSheetInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                                                            GroupOperator.And(New BinaryOperator("BalanceSheet", BalanceSheetInventoryItem.BalanceSheet), _
                                                                                                                              New BinaryOperator("Inventory", BalanceSheetInventoryItem.Inventory), _
                                                                                                                              New BinaryOperator("Item", BalanceSheetInventoryItem.Item), _
                                                                                                                              New BinaryOperator("TransDate", BalanceSheetInventoryItem.TransDate, BinaryOperatorType.GreaterOrEqual), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        BalanceSheetInventoryItem.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
    End Sub
    Public Shared Function CreateBalanceSheetInventoryItemDeductTransaction(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal Quantity As Decimal, ByVal Type As BalanceSheetInventoryItemDeductTransactionType) As BalanceSheetInventoryItemDeductTransaction
        Dim session As Session = Inventory.Session
        Dim period As Period = session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
        If period Is Nothing OrElse period.Closed Then Throw New Exception("Not in open period")
        Dim BalanceSheet As BalanceSheet = session.FindObject(Of BalanceSheet)(New BinaryOperator("Period", period))
        'Dim currentBalance As Decimal = BalanceSheetService.GetBalance(BalanceSheet, TransDate, Inventory, Item)
        'If currentBalance < Quantity Then Throw New Exception("Balance is not enough")
        Dim objBalanceSheetInventoryItemDeductTransaction As New BalanceSheetInventoryItemDeductTransaction(session) With {.BalanceSheet = BalanceSheet, .Inventory = Inventory, .Item = Item, .TransDate = TransDate, .Quantity = Quantity, .Type = Type}
        Dim xpInventoryDeductTransaction As New XPCollection(Of BalanceSheetInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, _
                                                                                                            GroupOperator.And(New BinaryOperator("BalanceSheet", BalanceSheet), _
                                                                                                                              New BinaryOperator("Inventory", Inventory), _
                                                                                                                              New BinaryOperator("Item", Item), _
                                                                                                                              New BinaryOperator("TransDate", TransDate, BinaryOperatorType.GreaterOrEqual), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
             With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        xpInventoryDeductTransaction.Remove(objBalanceSheetInventoryItemDeductTransaction)
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        objBalanceSheetInventoryItemDeductTransaction.DistributeDeduction()
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
        Return objBalanceSheetInventoryItemDeductTransaction
    End Function
    Public Shared Sub DeleteBalanceSheetInventoryItemDeductTransaction(ByVal BalanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction)
        Dim Session As Session = BalanceSheetInventoryItemDeductTransaction.Session
        Dim period As Period = BalanceSheetInventoryItemDeductTransaction.BalanceSheet.Period
        If period.Closed Then Throw New Exception("Not in open period")
        Dim xpInventoryDeductTransaction As New XPCollection(Of BalanceSheetInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                                                                   GroupOperator.And(New BinaryOperator("BalanceSheet", BalanceSheetInventoryItemDeductTransaction.BalanceSheet), _
                                                                                                                                     New BinaryOperator("Inventory", BalanceSheetInventoryItemDeductTransaction.Inventory), _
                                                                                                                                     New BinaryOperator("Item", BalanceSheetInventoryItemDeductTransaction.Item), _
                                                                                                                                     New BinaryOperator("TransDate", BalanceSheetInventoryItemDeductTransaction.TransDate, BinaryOperatorType.GreaterOrEqual), _
                                                                                                                                     New BinaryOperator("IsDeleted", False))) _
                   With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending))}
        BalanceSheetInventoryItemDeductTransaction.ResetDetail()
        BalanceSheetInventoryItemDeductTransaction.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
    End Sub

    Public Shared Function GetBalance(ByVal TransDate As Date, ByVal Inventory As Inventory, ByVal Item As Item) As Decimal
        Dim session As Session = Inventory.Session
        Dim period As Period = session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
        Dim BalanceSheet As BalanceSheet = session.FindObject(Of BalanceSheet)(New BinaryOperator("Period", period))
        Return GetBalance(BalanceSheet, TransDate, Inventory, Item)
    End Function
    Public Shared Function GetBalance(ByVal BalanceSheet As BalanceSheet, ByVal TransDate As Date, ByVal Inventory As Inventory, ByVal Item As Item) As Decimal
        Dim session As Session = Inventory.Session
        BalanceSheet.InventoryItems.Filter = GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual))
        BalanceSheet.InventoryItemDeductTransactions.Filter = GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual))

        Dim tmpBalance As Decimal = 0
        For Each objInventoryItem In BalanceSheet.InventoryItems
            tmpBalance += objInventoryItem.Quantity
            If objInventoryItem.Item.HasExpiryDate AndAlso objInventoryItem.ExpiryDate < TransDate Then tmpBalance -= objInventoryItem.RemainingQuantity
        Next
        For Each objInventoryItemDeductTransaction In BalanceSheet.InventoryItemDeductTransactions
            tmpBalance -= objInventoryItemDeductTransaction.Quantity
        Next
        Return tmpBalance
    End Function
End Class
