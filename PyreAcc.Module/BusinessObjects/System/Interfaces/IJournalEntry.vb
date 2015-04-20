Public Interface IJournalEntry
    Property TransDate As Date
    Property Description As String

    Function GetDebits() As ICollection(Of IJournalEntryDebit)
    Function GetCredits() As ICollection(Of IJournalEntryCredit)
End Interface
