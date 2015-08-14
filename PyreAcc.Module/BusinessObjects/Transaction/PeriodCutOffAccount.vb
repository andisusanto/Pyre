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
<DefaultClassOptions()> _
Public Class PeriodCutOffAccount
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _periodCutOff As PeriodCutOff
    Private _account As Account
    Private _initialBalance As Decimal
    Private _lastBalance As Decimal
    <Association("PeriodCutOff-PeriodCutOffAccount")>
    <RuleRequiredField("Rule Required for PeriodCutOffAccount.PeriodCutOff", DefaultContexts.Save)>
    Public Property PeriodCutOff As PeriodCutOff
        Get
            Return _periodCutOff
        End Get
        Set(ByVal value As PeriodCutOff)
            SetPropertyValue("PeriodCutOff", _periodCutOff, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOffAccount.Account", DefaultContexts.Save)>
    Public Property Account As Account
        Get
            Return _account
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("Account", _account, value)
        End Set
    End Property
    Public Property InitialBalance As Decimal
        Get
            Return _initialBalance
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("InitialBalance", _initialBalance, value)
        End Set
    End Property
    Public Property LastBalance As Decimal
        Get
            Return _lastBalance
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("LastBalance", _lastBalance, value)
        End Set
    End Property

    Public ReadOnly Property AccountMutations As XPCollection(Of PeriodCutOffJournalAccountMutation)
        Get
            Return New XPCollection(Of PeriodCutOffJournalAccountMutation)(Session, GroupOperator.And(New BinaryOperator("Account", Account), New BinaryOperator("PeriodCutOffJournal.PeriodCutOff", PeriodCutOff)))
        End Get
    End Property
End Class
