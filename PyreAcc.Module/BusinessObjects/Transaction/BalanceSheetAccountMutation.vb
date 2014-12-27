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
<RuleCriteria("Rule Criteria for BalanceSheetAccountMutation.Amount <> 0", DefaultContexts.Save, "Amount <> 0")>
Public Class BalanceSheetAccountMutation
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _balanceSheetAccount As BalanceSheetAccount
    Private _transDate As Date
    Private _amount As Decimal
    Private _note As String
    <Association("BalanceSheetAccount-BalanceSheetAccountMutation")>
    <RuleRequiredField("Rule Required for BalanceSheetAccountMutation.BalanceSheetAccount", DefaultContexts.Save)>
    Public Property BalanceSheetAccount As BalanceSheetAccount
        Get
            Return _balanceSheetAccount
        End Get
        Set(ByVal value As BalanceSheetAccount)
            SetPropertyValue("BalanceSheetAccount", _balanceSheetAccount, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for BalanceSheetAccountMutation.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
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
    Public Property Note As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Note", _note, value)
        End Set
    End Property

End Class
