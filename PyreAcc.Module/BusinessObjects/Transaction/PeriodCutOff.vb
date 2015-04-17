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
Imports DevExpress.ExpressApp.ConditionalAppearance
<Appearance("Appearance Default for PeriodCutOff", enabled:=False, targetitems:="*")>
<CreatableItem(False)>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PeriodCutOff
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _startDate As Date
    Private _endDate As Date
    Private _closed As Boolean
    <RuleRequiredField("Rule Required for PeriodCutOff.StartDate", DefaultContexts.Save)>
    Public Property StartDate As Date
        Get
            Return _startDate
        End Get
        Set(value As Date)
            SetPropertyValue("StartDate", _startDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PeriodCutOff.EndDate", DefaultContexts.Save)>
    Public Property EndDate As Date
        Get
            Return _endDate
        End Get
        Set(value As Date)
            SetPropertyValue("EndDate", _endDate, value)
        End Set
    End Property
    Public Property Closed As Boolean
        Get
            Return _closed
        End Get
        Set(value As Boolean)
            SetPropertyValue("Closed", _closed, value)
        End Set
    End Property
    <Association("PeriodCutOff-PeriodCutOffAccount"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Accounts As XPCollection(Of PeriodCutOffAccount)
        Get
            Return GetCollection(Of PeriodCutOffAccount)("Accounts")
        End Get
    End Property
    <Association("PeriodCutOff-PeriodCutOffInventoryItem"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property InventoryItems As XPCollection(Of PeriodCutOffInventoryItem)
        Get
            Return GetCollection(Of PeriodCutOffInventoryItem)("InventoryItems")
        End Get
    End Property
    <Association("PeriodCutOff-PeriodCutOffInventoryItemDeductTransaction"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property InventoryItemDeductTransactions As XPCollection(Of PeriodCutOffInventoryItemDeductTransaction)
        Get
            Return GetCollection(Of PeriodCutOffInventoryItemDeductTransaction)("InventoryItemDeductTransactions")
        End Get
    End Property
    <Association("PeriodCutOff-PeriodCutOffJournal"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Journals As XPCollection(Of PeriodCutOffJournal)
        Get
            Return GetCollection(Of PeriodCutOffJournal)("Journals")
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return StartDate & " - " & EndDate
        End Get
    End Property
End Class
