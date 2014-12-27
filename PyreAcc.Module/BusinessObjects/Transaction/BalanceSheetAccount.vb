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
Public Class BalanceSheetAccount
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _balanceSheet As BalanceSheet
    Private _account As Account
    Private _initialBalance As Decimal
    Private _lastBalance As Decimal
    <Association("BalanceSheet-BalanceSheetAccount")>
    <RuleRequiredField("Rule Required for BalanceSheetAccount.BalanceSheet", DefaultContexts.Save)>
    Public Property BalanceSheet As BalanceSheet
        Get
            Return _balanceSheet
        End Get
        Set(ByVal value As BalanceSheet)
            SetPropertyValue("BalanceSheet", _balanceSheet, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetAccount.Account", DefaultContexts.Save)>
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
    <Association("BalanceSheetAccount-BalanceSheetAccountMutation"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Mutations As XPCollection(Of BalanceSheetAccountMutation)
        Get
            Return GetCollection(Of BalanceSheetAccountMutation)("Mutations")
        End Get
    End Property
End Class
