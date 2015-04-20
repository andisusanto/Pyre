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

<RuleCriteria("Rule Criteria for JournalEntryCredit.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class JournalEntryCredit
    Inherits BaseObject
    Implements IJournalEntryCredit
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _journalEntry As JournalEntry
    Private _account As Account
    Private _amount As Decimal

    <VisibleInDetailView(False)>
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("JournalEntry-JournalEntryCredit")>
    <RuleRequiredField("Rule Required for JournalEntryCredit.JournalEntry", DefaultContexts.Save)>
    Public Property JournalEntry As JournalEntry
        Get
            Return _journalEntry
        End Get
        Set(ByVal value As JournalEntry)
            SetPropertyValue("JournalEntry", _journalEntry, value)
            If Not IsLoading Then
                If JournalEntry IsNot Nothing Then
                    If JournalEntry.Credits.Count = 0 Then
                        Sequence = 0
                    Else
                        JournalEntry.Credits.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = JournalEntry.Credits(JournalEntry.Credits.Count - 1).Sequence + 1
                    End If
                End If
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for JournalEntryCredit.Account", DefaultContexts.Save)>
    Public Property Account As Account Implements IJournalEntryCredit.Account
        Get
            Return _account
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("Account", _account, value)
        End Set
    End Property
    Public Property Amount As Decimal Implements IJournalEntryCredit.Amount
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Amount", _amount, value)
        End Set
    End Property

End Class
