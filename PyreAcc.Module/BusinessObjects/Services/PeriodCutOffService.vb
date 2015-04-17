Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Public Class PeriodCutOffService
    Public Shared Function CreatePeriodCutOffInventoryItem(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal BaseUnitQuantity As Decimal, ByVal UnitPrice As Decimal, ByVal ExpiryDate As Date, ByVal BatchNo As String) As PeriodCutOffInventoryItem
        Dim session As Session = Inventory.Session
        If TransactionConfig.IsInClosedPeriod(session, TransDate) Then Throw New Exception("Not in open period")

        Dim PeriodCutOff As PeriodCutOff = session.FindObject(Of PeriodCutOff)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), GroupOperator.Or(New NullOperator("EndDate"), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual))))
        If PeriodCutOff Is Nothing Then Throw New Exception("Cut off period not found")
        If PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
        Dim entryDate As Date = GlobalFunction.GetServerNow(session)
        Dim objPeriodCutOffInventoryItem As New PeriodCutOffInventoryItem(session) With {.PeriodCutOff = PeriodCutOff, .Inventory = Inventory, .Item = Item, .TransDate = TransDate, .BaseUnitQuantity = BaseUnitQuantity, .UnitPrice = UnitPrice, .ExpiryDate = ExpiryDate, .BatchNo = BatchNo, .EntryDate = entryDate}
        Dim xpInventoryDeductTransaction As New XPCollection(Of PeriodCutOffInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, _
                                                                                                            GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOff), _
                                                                                                                              New BinaryOperator("Inventory", Inventory), _
                                                                                                                              New BinaryOperator("Item", Item), _
                                                                                                                              GroupOperator.Or(
                                                                                                                                            New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Greater), _
                                                                                                                                            GroupOperator.And(
                                                                                                                                                        New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Equal), _
                                                                                                                                                        New BinaryOperator("EntryDate", entryDate, BinaryOperatorType.Greater))), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending), New SortProperty("EntryDate", DB.SortingDirection.Ascending))}

        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
        Return objPeriodCutOffInventoryItem
    End Function
    Public Shared Sub DeletePeriodCutOffInventoryItem(ByVal PeriodCutOffInventoryItem As PeriodCutOffInventoryItem)
        Dim Session As Session = PeriodCutOffInventoryItem.Session
        If TransactionConfig.IsInClosedPeriod(Session, PeriodCutOffInventoryItem.TransDate) Then Throw New Exception("Not in open period")

        'Dim currentBalance As Decimal = PeriodCutOffService.GetBalance(PeriodCutOffInventoryItem.PeriodCutOff, PeriodCutOffInventoryItem.TransDate, PeriodCutOffInventoryItem.Inventory, PeriodCutOffInventoryItem.Item)
        'If currentBalance < PeriodCutOffInventoryItem.BaseUnitQuantity Then Throw New Exception("Balance is not enough")
        If PeriodCutOffInventoryItem.PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
        Dim xpInventoryDeductTransaction As New XPCollection(Of PeriodCutOffInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                                                            GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOffInventoryItem.PeriodCutOff), _
                                                                                                                              New BinaryOperator("Inventory", PeriodCutOffInventoryItem.Inventory), _
                                                                                                                              New BinaryOperator("Item", PeriodCutOffInventoryItem.Item), _
                                                                                                                              GroupOperator.Or(
                                                                                                                                            New BinaryOperator("TransDate", PeriodCutOffInventoryItem.TransDate, BinaryOperatorType.Greater), _
                                                                                                                                            GroupOperator.And(
                                                                                                                                                        New BinaryOperator("TransDate", PeriodCutOffInventoryItem.TransDate, BinaryOperatorType.Equal), _
                                                                                                                                                        New BinaryOperator("EntryDate", PeriodCutOffInventoryItem.EntryDate, BinaryOperatorType.Greater))), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
            With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending), New SortProperty("EntryDate", DB.SortingDirection.Ascending))}
        PeriodCutOffInventoryItem.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
    End Sub
    Public Shared Function CreatePeriodCutOffInventoryItemDeductTransaction(ByVal Inventory As Inventory, ByVal Item As Item, ByVal TransDate As Date, ByVal BaseUnitQuantity As Decimal, ByVal Type As PeriodCutOffInventoryItemDeductTransactionType) As PeriodCutOffInventoryItemDeductTransaction
        Dim session As Session = Inventory.Session
        If TransactionConfig.IsInClosedPeriod(session, TransDate) Then Throw New Exception("Not in open period")
        Dim PeriodCutOff As PeriodCutOff = session.FindObject(Of PeriodCutOff)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), GroupOperator.Or(New NullOperator("EndDate"), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual))))
        If PeriodCutOff Is Nothing Then Throw New Exception("Cut off period not found")
        If PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
        'Dim currentBalance As Decimal = PeriodCutOffService.GetBalance(PeriodCutOff, TransDate, Inventory, Item)
        'If currentBalance < BaseUnitQuantity Then Throw New Exception("Balance is not enough")
        Dim entryDate As Date = GlobalFunction.GetServerNow(session)
        Dim objPeriodCutOffInventoryItemDeductTransaction As New PeriodCutOffInventoryItemDeductTransaction(session) With {.PeriodCutOff = PeriodCutOff, .Inventory = Inventory, .Item = Item, .TransDate = TransDate, .BaseUnitQuantity = BaseUnitQuantity, .Type = Type, .EntryDate = entryDate}
        Dim xpInventoryDeductTransaction As New XPCollection(Of PeriodCutOffInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, session, _
                                                                                                            GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOff), _
                                                                                                                              New BinaryOperator("Inventory", Inventory), _
                                                                                                                              New BinaryOperator("Item", Item), _
                                                                                                                              GroupOperator.Or(
                                                                                                                                            New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Greater), _
                                                                                                                                            GroupOperator.And(
                                                                                                                                                        New BinaryOperator("TransDate", TransDate, BinaryOperatorType.Equal), _
                                                                                                                                                        New BinaryOperator("EntryDate", entryDate, BinaryOperatorType.Greater))), _
                                                                                                                              New BinaryOperator("IsDeleted", False))) _
             With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending), New SortProperty("EntryDate", DB.SortingDirection.Ascending))}
        xpInventoryDeductTransaction.Remove(objPeriodCutOffInventoryItemDeductTransaction)
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        objPeriodCutOffInventoryItemDeductTransaction.DistributeDeduction()
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
        Return objPeriodCutOffInventoryItemDeductTransaction
    End Function
    Public Shared Sub DeletePeriodCutOffInventoryItemDeductTransaction(ByVal PeriodCutOffInventoryItemDeductTransaction As PeriodCutOffInventoryItemDeductTransaction)
        Dim Session As Session = PeriodCutOffInventoryItemDeductTransaction.Session
        If TransactionConfig.IsInClosedPeriod(Session, PeriodCutOffInventoryItemDeductTransaction.TransDate) Then Throw New Exception("Not in open period")
        If PeriodCutOffInventoryItemDeductTransaction.PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
        Dim xpInventoryDeductTransaction As New XPCollection(Of PeriodCutOffInventoryItemDeductTransaction)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                                                                   GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOffInventoryItemDeductTransaction.PeriodCutOff), _
                                                                                                                                     New BinaryOperator("Inventory", PeriodCutOffInventoryItemDeductTransaction.Inventory), _
                                                                                                                                     New BinaryOperator("Item", PeriodCutOffInventoryItemDeductTransaction.Item), _
                                                                                                                                     GroupOperator.Or(
                                                                                                                                                   New BinaryOperator("TransDate", PeriodCutOffInventoryItemDeductTransaction.TransDate, BinaryOperatorType.Greater), _
                                                                                                                                                   GroupOperator.And(
                                                                                                                                                               New BinaryOperator("TransDate", PeriodCutOffInventoryItemDeductTransaction.TransDate, BinaryOperatorType.Equal), _
                                                                                                                                                               New BinaryOperator("EntryDate", PeriodCutOffInventoryItemDeductTransaction.EntryDate, BinaryOperatorType.Greater))), _
                                                                                                                                     New BinaryOperator("IsDeleted", False))) _
                   With {.Sorting = New SortingCollection(New SortProperty("TransDate", DB.SortingDirection.Ascending), New SortProperty("EntryDate", DB.SortingDirection.Ascending))}
        PeriodCutOffInventoryItemDeductTransaction.ResetDetail()
        PeriodCutOffInventoryItemDeductTransaction.Delete()
        For Each obj In xpInventoryDeductTransaction
            obj.ResetDetail()
        Next
        For Each obj In xpInventoryDeductTransaction
            obj.DistributeDeduction()
        Next
    End Sub

    Public Shared Function GetInventoryItemBalance(ByVal TransDate As Date, ByVal Inventory As Inventory, ByVal Item As Item) As Decimal
        Dim session As Session = Inventory.Session
        Dim PeriodCutOff As PeriodCutOff = session.FindObject(Of PeriodCutOff)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
        Return GetInventoryItemBalance(PeriodCutOff, TransDate, Inventory, Item)
    End Function
    Public Shared Function GetInventoryItemBalance(ByVal PeriodCutOff As PeriodCutOff, ByVal TransDate As Date, ByVal Inventory As Inventory, ByVal Item As Item) As Decimal
        Dim session As Session = Inventory.Session
        PeriodCutOff.InventoryItems.Filter = GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual))
        PeriodCutOff.InventoryItemDeductTransactions.Filter = GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("TransDate", TransDate, BinaryOperatorType.LessOrEqual))

        Dim tmpBalance As Decimal = 0
        For Each objInventoryItem In PeriodCutOff.InventoryItems
            tmpBalance += objInventoryItem.BaseUnitQuantity
            If objInventoryItem.Item.HasExpiryDate AndAlso objInventoryItem.ExpiryDate < TransDate Then tmpBalance -= objInventoryItem.RemainingBaseUnitQuantity
        Next
        For Each objInventoryItemDeductTransaction In PeriodCutOff.InventoryItemDeductTransactions
            tmpBalance -= objInventoryItemDeductTransaction.BaseUnitQuantity
        Next
        Return tmpBalance
    End Function

    'Public Shared Function CreatePeriodCutOffAccountMutation(ByVal Account As Account, ByVal TransDate As Date, ByVal Amount As Decimal, ByVal Note As String) As PeriodCutOffAccountMutation
    '    Dim session As Session = Account.Session
    '    If TransactionConfig.IsInClosedPeriod(session, TransDate) Then Throw New Exception("Not in open period")
    '    Dim PeriodCutOff As PeriodCutOff = session.FindObject(Of PeriodCutOff)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), GroupOperator.Or(New NullOperator("EndDate"), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual))))
    '    If PeriodCutOff Is Nothing Then Throw New Exception("Cut off period not found")
    '    If PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
    '    Dim PeriodCutOffAccount As PeriodCutOffAccount = session.FindObject(Of PeriodCutOffAccount)(PersistentCriteriaEvaluationBehavior.InTransaction, GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOff), New BinaryOperator("Account", Account)))
    '    If PeriodCutOffAccount Is Nothing Then
    '         PeriodCutOffAccount = New PeriodCutOffAccount(session) With {.Account = Account, .PeriodCutOff = PeriodCutOff}
    '    End If
    '    Dim PeriodCutOffAccountMutation As New PeriodCutOffAccountMutation(session) With {.PeriodCutOffAccount = PeriodCutOffAccount, .TransDate = TransDate, .Amount = Amount, .Note = Note}
    '    PeriodCutOffAccount.LastBalance += Amount
    '    Return PeriodCutOffAccountMutation
    'End Function

    'Public Shared Sub DeletePeriodCutOffAccountMutation(ByVal PeriodCutOffAccountMutation As PeriodCutOffAccountMutation)
    '    If PeriodCutOffAccountMutation.PeriodCutOffAccount.PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
    '    PeriodCutOffAccountMutation.PeriodCutOffAccount.LastBalance -= PeriodCutOffAccountMutation.Amount
    '    PeriodCutOffAccountMutation.Delete()
    'End Sub

End Class
