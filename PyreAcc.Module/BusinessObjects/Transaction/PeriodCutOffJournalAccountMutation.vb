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

<RuleCriteria("Rule Criteria for PeriodCutOffJournalAccountMutation.Amount <> 0", DefaultContexts.Save, "Amount <> 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PeriodCutOffJournalAccountMutation
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _journal As PeriodCutOffJournal
    Private _account As Account
    Private _mutationType As AccountBehaviour
    Private _amount As Decimal
    Private _afterMutationAmount As Decimal

    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("PeriodCutOffJournal-PeriodCutOffJournalAccountMutation")>
    <RuleRequiredField("Rule Required for PeriodCutOffJournalAccountMutation.PeriodCutOffJournal", DefaultContexts.Save)>
    Public Property PeriodCutOffJournal As PeriodCutOffJournal
        Get
            Return _journal
        End Get
        Set(ByVal value As PeriodCutOffJournal)
            SetPropertyValue("PeriodCutOffJournal", _journal, value)
            If Not IsLoading Then
                If PeriodCutOffJournal IsNot Nothing Then
                    If PeriodCutOffJournal.AccountMutations.Count = 0 Then
                        Sequence = 0
                    Else
                        PeriodCutOffJournal.AccountMutations.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PeriodCutOffJournal.AccountMutations(PeriodCutOffJournal.AccountMutations.Count - 1).Sequence + 1
                    End If
                End If
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffJournalAccountMutation.Account", DefaultContexts.Save)>
    Public Property Account As Account
        Get
            Return _account
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("Account", _account, value)
        End Set
    End Property
    Public Property MutationType As AccountBehaviour
        Get
            Return _mutationType
        End Get
        Set(value As AccountBehaviour)
            SetPropertyValue("MutationType", _mutationType, value)
        End Set
    End Property
    Public Property Amount As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Amount", _amount, value)
        End Set
    End Property
    Public Property AfterMutationAmount As Decimal
        Get
            Return _afterMutationAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("AfterMutationAmount", _afterMutationAmount, value)
        End Set
    End Property
End Class
