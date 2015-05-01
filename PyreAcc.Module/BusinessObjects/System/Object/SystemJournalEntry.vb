Public Class SystemJournalEntry
    Implements IJournalEntry
    Public Property TransDate As Date Implements IJournalEntry.TransDate
    Public Property Description As String Implements IJournalEntry.Description

    Public Debits As New List(Of IJournalEntryDebit)
    Public Credits As New List(Of IJournalEntryCredit)
    Public Function GetDebits() As ICollection(Of IJournalEntryDebit) Implements IJournalEntry.GetDebits
        Return Debits
    End Function
    Public Function GetCredits() As ICollection(Of IJournalEntryCredit) Implements IJournalEntry.GetCredits
        Return Credits
    End Function

End Class
