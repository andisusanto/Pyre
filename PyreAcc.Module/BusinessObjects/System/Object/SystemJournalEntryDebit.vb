Public Class SystemJournalEntryDebit
    Implements IJournalEntryDebit

    Public Property Account As Account Implements IJournalEntryDebit.Account

    Public Property Amount As Decimal Implements IJournalEntryDebit.Amount
End Class
