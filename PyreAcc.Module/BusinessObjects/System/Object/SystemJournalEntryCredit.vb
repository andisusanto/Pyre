Public Class SystemJournalEntryCredit
    Implements IJournalEntryCredit

    Public Property Account As Account Implements IJournalEntryCredit.Account

    Public Property Amount As Decimal Implements IJournalEntryCredit.Amount
End Class
