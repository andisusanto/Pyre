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
            'If objInventoryItem.Item.HasExpiryDate AndAlso objInventoryItem.ExpiryDate < TransDate Then tmpBalance -= objInventoryItem.RemainingBaseUnitQuantity
        Next
        'For Each objInventoryItemDeductTransaction In PeriodCutOff.InventoryItemDeductTransactions
        '    tmpBalance -= objInventoryItemDeductTransaction.BaseUnitQuantity
        'Next
        Return tmpBalance
    End Function

    Public Shared Function GetAccountAmount(ByVal TransDate As Date, ByVal Account As Account) As Decimal
        Dim Session As Session = Account.Session
        Dim xp As New XPCollection(Of PeriodCutOffJournalAccountMutation)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                          GroupOperator.And(New BinaryOperator("Account", Account), New BinaryOperator("PeriodCutOffJournal.TransDate", TransDate, BinaryOperatorType.LessOrEqual))) _
                                                                      With {.TopReturnedObjects = 1, .Sorting = New SortingCollection(New SortProperty("PeriodCutOffJournal.EntryDate", DB.SortingDirection.Descending))}
        If xp.Count > 0 Then
            Return xp(0).Amount
        Else
            Dim xpPeriodCutOffAccount As New XPCollection(Of PeriodCutOffAccount)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, GroupOperator.And(New BinaryOperator("Account", Account), New BinaryOperator("PeriodCutOff.StartDate", TransDate, BinaryOperatorType.LessOrEqual))) _
                With {.TopReturnedObjects = 1, .Sorting = New SortingCollection(New SortProperty("PeriodCutOff.StartDate", DB.SortingDirection.Descending))}
            If xpPeriodCutOffAccount.Count > 0 Then Return xpPeriodCutOffAccount(0).InitialBalance
        End If
        Return 0
    End Function

    Private Shared Function CreatePeriodCutOffJournalMutation(ByVal PeriodCutOffJournal As PeriodCutOffJournal, ByVal MutationType As AccountMutationType, ByVal Account As Account, ByVal Amount As Decimal) As PeriodCutOffJournalAccountMutation
        Dim Session As Session = PeriodCutOffJournal.Session
        Dim objPeriodCutOffJournalAccountMutation As New PeriodCutOffJournalAccountMutation(Session) _
            With {.PeriodCutOffJournal = PeriodCutOffJournal, _
                  .MutationType = AccountMutationType.Debit, _
                  .Account = Account}
        If Account.NormalBalance = MutationType Then
            objPeriodCutOffJournalAccountMutation.Amount = Amount
        Else
            objPeriodCutOffJournalAccountMutation.Amount = -1 * Amount
        End If
        Dim tmpBalance As Decimal = GetAccountAmount(PeriodCutOffJournal.TransDate, Account)
        objPeriodCutOffJournalAccountMutation.AfterMutationAmount = tmpBalance + objPeriodCutOffJournalAccountMutation.Amount
        Dim xp As New XPCollection(Of PeriodCutOffJournalAccountMutation)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                          GroupOperator.And(New BinaryOperator("Account", Account), _
                                                                                            GroupOperator.Or(New BinaryOperator("PeriodCutOffJournal.TransDate", PeriodCutOffJournal.TransDate, BinaryOperatorType.Greater), _
                                                                                                             GroupOperator.And(New BinaryOperator("PeriodCutOffJournal.EntryDate", PeriodCutOffJournal.EntryDate, BinaryOperatorType.Greater), _
                                                                                                                               New BinaryOperator("PeriodCutOffJournal.TransDate", PeriodCutOffJournal.TransDate, BinaryOperatorType.Equal)))))
        For Each obj In xp
            obj.Amount += objPeriodCutOffJournalAccountMutation.Amount
            obj.AfterMutationAmount += objPeriodCutOffJournalAccountMutation.Amount
        Next
        Dim periodCutOffAccount As PeriodCutOffAccount = Session.FindObject(Of PeriodCutOffAccount)(PersistentCriteriaEvaluationBehavior.InTransaction, GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOffJournal.PeriodCutOff), New BinaryOperator("Account", Account)))
        If periodCutOffAccount Is Nothing Then
            periodCutOffAccount = New PeriodCutOffAccount(Session) With {.Account = Account, .PeriodCutOff = PeriodCutOffJournal.PeriodCutOff, .InitialBalance = 0}
        End If
        periodCutOffAccount.LastBalance += objPeriodCutOffJournalAccountMutation.Amount
        Return objPeriodCutOffJournalAccountMutation
    End Function

    Private Shared Sub DeletePeriodCutOffJournalAccountMutation(ByVal PeriodCutOffJournalAccountMutation As PeriodCutOffJournalAccountMutation)
        Dim Session As Session = PeriodCutOffJournalAccountMutation.Session
        Dim xp As New XPCollection(Of PeriodCutOffJournalAccountMutation)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, _
                                                                          GroupOperator.And(New BinaryOperator("Account", PeriodCutOffJournalAccountMutation.Account), _
                                                                                            GroupOperator.Or(New BinaryOperator("PeriodCutOffJournal.TransDate", PeriodCutOffJournalAccountMutation.PeriodCutOffJournal.TransDate, BinaryOperatorType.Greater), _
                                                                                                             GroupOperator.And(New BinaryOperator("PeriodCutOffJournal.EntryDate", PeriodCutOffJournalAccountMutation.PeriodCutOffJournal.EntryDate, BinaryOperatorType.Greater), _
                                                                                                                               New BinaryOperator("PeriodCutOffJournal.TransDate", PeriodCutOffJournalAccountMutation.PeriodCutOffJournal.TransDate, BinaryOperatorType.Equal)))))
        For Each obj In xp
            obj.Amount -= PeriodCutOffJournalAccountMutation.Amount
            obj.AfterMutationAmount -= PeriodCutOffJournalAccountMutation.Amount
        Next
        Dim periodCutOffAccount As PeriodCutOffAccount = Session.FindObject(Of PeriodCutOffAccount)(PersistentCriteriaEvaluationBehavior.InTransaction, GroupOperator.And(New BinaryOperator("PeriodCutOff", PeriodCutOffJournalAccountMutation.PeriodCutOffJournal.PeriodCutOff), New BinaryOperator("Account", PeriodCutOffJournalAccountMutation.Account)))
        periodCutOffAccount.LastBalance -= PeriodCutOffJournalAccountMutation.Amount
        PeriodCutOffJournalAccountMutation.Delete()
    End Sub

    Public Shared Function CreatePeriodCutOffJournal(ByVal session As Session, ByVal JournalEntry As IJournalEntry) As PeriodCutOffJournal
        If TransactionConfig.IsInClosedPeriod(session, JournalEntry.TransDate) Then Throw New Exception("Not in open period")
        Dim PeriodCutOff As PeriodCutOff = session.FindObject(Of PeriodCutOff)(GroupOperator.And(New BinaryOperator("StartDate", JournalEntry.TransDate, BinaryOperatorType.LessOrEqual), GroupOperator.Or(New NullOperator("EndDate"), New BinaryOperator("EndDate", JournalEntry.TransDate, BinaryOperatorType.GreaterOrEqual))))
        If PeriodCutOff Is Nothing Then Throw New Exception("Cut off period not found")
        If PeriodCutOff.Closed Then Throw New Exception("Cut off period already closed")
        Dim entryDate As Date = GlobalFunction.GetServerNow(session)
        Dim periodCutOffJournal As New PeriodCutOffJournal(session) With {.PeriodCutOff = PeriodCutOff, .TransDate = JournalEntry.TransDate, .Description = JournalEntry.Description, .EntryDate = entryDate}

        For Each objJournalEntryDebit As IJournalEntryDebit In JournalEntry.GetDebits
            CreatePeriodCutOffJournalMutation(periodCutOffJournal, AccountMutationType.Debit, objJournalEntryDebit.Account, objJournalEntryDebit.Amount)
        Next
        For Each objJournalEntryCredit As IJournalEntryCredit In JournalEntry.GetCredits
            CreatePeriodCutOffJournalMutation(periodCutOffJournal, AccountMutationType.Credit, objJournalEntryCredit.Account, objJournalEntryCredit.Amount)
        Next
        Return periodCutOffJournal
    End Function

    Public Shared Sub DeletePeriodCutOffJournal(ByVal PeriodCutOffJournal As PeriodCutOffJournal)
        For i = 0 To PeriodCutOffJournal.AccountMutations.Count - 1
            DeletePeriodCutOffJournalAccountMutation(PeriodCutOffJournal.AccountMutations(0))
        Next
        PeriodCutOffJournal.Delete()
    End Sub

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
