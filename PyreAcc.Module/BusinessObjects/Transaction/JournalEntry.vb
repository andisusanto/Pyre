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
<RuleCriteria("Rule Criteria for JournalEntry.IsAmountBalance = True", DefaultContexts.Save, "IsAmountBalance = True")>
<DefaultClassOptions()> _
Public Class JournalEntry
    Inherits TransactionBase
    Implements IJournalEntry
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = GlobalFunction.GetServerNow(Session).Date
    End Sub

    Private _no As String
    Private _transDate As Date
    Private _description As String

    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleRequiredField("Rule Required for JournalEntry.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for JournalEntry.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date Implements IJournalEntry.TransDate
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <Size(4000)>
    <RuleRequiredField("Rule Required for JournalEntry.Description", DefaultContexts.Save)>
    Public Property Description As String Implements IJournalEntry.Description
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffJournal As PeriodCutOffJournal
        Get
            Return _periodCutOffJournal
        End Get
        Set(value As PeriodCutOffJournal)
            SetPropertyValue("PeriodCutOffJournal", _periodCutOffJournal, value)
        End Set
    End Property
    <Association("JournalEntry-JournalEntryDebit"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Debits As XPCollection(Of JournalEntryDebit)
        Get
            Return GetCollection(Of JournalEntryDebit)("Debits")
        End Get
    End Property
    <Association("JournalEntry-JournalEntryCredit"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Credits As XPCollection(Of JournalEntryCredit)
        Get
            Return GetCollection(Of JournalEntryCredit)("Credits")
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsAmountBalance As Boolean
        Get
            Dim totalDebit As Decimal = 0
            Dim totalCredit As Decimal = 0
            For Each objDebit In Debits
                totalDebit += objDebit.Amount
            Next
            For Each objCredit In Credits
                totalCredit += objCredit.Amount
            Next
            Return totalDebit = totalCredit
        End Get
    End Property
    Public Function GetDebits() As ICollection(Of IJournalEntryDebit) Implements IJournalEntry.GetDebits
        Dim list As New List(Of IJournalEntryDebit)
        For Each obj In Debits
            list.Add(obj)
        Next
        Return list
    End Function
    Public Function GetCredits() As ICollection(Of IJournalEntryCredit) Implements IJournalEntry.GetCredits
        Dim list As New List(Of IJournalEntryCredit)
        For Each obj In Credits
            list.Add(obj)
        Next
        Return list
    End Function
    Public Sub RecreateCreateJournal()
        Dim tmp = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmp)
        CreateJournal()
    End Sub
    Private Sub CreateJournal()
        PeriodCutOffJournal = PeriodCutOffService.CreatePeriodCutOffJournal(Session, Me)
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        CreateJournal()
    End Sub

    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        Dim tmp = PeriodCutOffJournal
        PeriodCutOffJournal = Nothing
        PeriodCutOffService.DeletePeriodCutOffJournal(tmp)
    End Sub

End Class
