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
<RuleCriteria("Rule Criteria for Journal.IsAmountBalance = True", DefaultContexts.Save, "IsAmountBalance = True")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Journal
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _transDate As Date
    Private _entryDate As Date
    Private _description As String
    <RuleRequiredField("Rule Required for Journal.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Journal.EntryDate", DefaultContexts.Save)>
    Public Property EntryDate As Date
        Get
            Return _entryDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("EntryDate", _entryDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Journal.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property

    <Association("Journal-JournalDebit"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Debits As XPCollection(Of JournalDebit)
        Get
            Return GetCollection(Of JournalDebit)("Debits")
        End Get
    End Property
    <Association("Journal-JournalCredit"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Credits As XPCollection(Of JournalCredit)
        Get
            Return GetCollection(Of JournalCredit)("Credits")
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
End Class
