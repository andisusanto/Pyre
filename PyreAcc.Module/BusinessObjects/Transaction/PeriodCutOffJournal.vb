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
'<RuleCriteria("Rule Criteria for PeriodCutOffJournal.IsAmountBalance = True", DefaultContexts.Save, "IsAmountBalance = True")>
<DefaultProperty("Description")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PeriodCutOffJournal
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _periodCutOff As PeriodCutOff
    Private _transDate As Date
    Private _entryDate As Date
    Private _description As String
    <Association("PeriodCutOff-PeriodCutOffJournal")>
    <RuleRequiredField("Rule Required for PeriodCutOffJournal.PeriodCutOff", DefaultContexts.Save)>
    Public Property PeriodCutOff As PeriodCutOff
        Get
            Return _periodCutOff
        End Get
        Set(value As PeriodCutOff)
            SetPropertyValue("PeriodCutOff", _periodCutOff, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffJournal.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffJournal.EntryDate", DefaultContexts.Save)>
    Public Property EntryDate As Date
        Get
            Return _entryDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("EntryDate", _entryDate, value)
        End Set
    End Property
    <Size(4000)>
    <RuleRequiredField("Rule Required for PeriodCutOffJournal.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property

    <Association("PeriodCutOffJournal-PeriodCutOffJournalAccountMutation"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property AccountMutations As XPCollection(Of PeriodCutOffJournalAccountMutation)
        Get
            Return GetCollection(Of PeriodCutOffJournalAccountMutation)("AccountMutations")
        End Get
    End Property
    '<VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    'Public ReadOnly Property IsAmountBalance As Boolean
    '    Get
    '        Dim totalDebit As Decimal = 0
    '        Dim totalCredit As Decimal = 0
    '        For Each obj In AccountMutations
    '            Select Case obj.Account.NormalBalance
    '                Case AccountMutationType.Debit
    '                    totalDebit += obj.Amount
    '                Case AccountMutationType.Credit
    '                    totalCredit += obj.Amount
    '                Case Else
    '                    Throw New NotImplementedException
    '            End Select
    '        Next
    '        Return totalDebit = totalCredit
    '    End Get
    'End Property
End Class
