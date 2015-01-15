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
<CreatableItem(False)>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class BalanceSheet
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _period As Period
    <RuleRequiredField("Rule Required for BalanceSheet.Period", DefaultContexts.Save)>
    Public Property Period As Period
        Get
            Return _period
        End Get
        Set(value As Period)
            SetPropertyValue("Period", _period, value)
        End Set
    End Property
    <Association("BalanceSheet-BalanceSheetAccount"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Accounts As XPCollection(Of BalanceSheetAccount)
        Get
            Return GetCollection(Of BalanceSheetAccount)("Accounts")
        End Get
    End Property
    <Association("BalanceSheet-BalanceSheetInventoryItem"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property InventoryItems As XPCollection(Of BalanceSheetInventoryItem)
        Get
            Return GetCollection(Of BalanceSheetInventoryItem)("InventoryItems")
        End Get
    End Property
    <Association("BalanceSheet-BalanceSheetInventoryItemDeductTransaction"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property InventoryItemDeductTransactions As XPCollection(Of BalanceSheetInventoryItemDeductTransaction)
        Get
            Return GetCollection(Of BalanceSheetInventoryItemDeductTransaction)("InventoryItemDeductTransactions")
        End Get
    End Property
End Class
