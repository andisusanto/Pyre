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
<CreatableItem(False)> _
<DeferredDeletion(False)>
<Appearance("Appearance Default Disabled for DebitNote", enabled:=False, targetitems:="*", AppearanceItemType:="ViewItem")>
<RuleCriteria("Rule Criteria for DebitNote.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<RuleCriteria("Rule Criteria for DebitNote.RemainingAmount >= 0", DefaultContexts.Save, "RemainingAmount >= 0")>
<RuleCriteria("Rule Criteria Delete for DebitNote.UsedAmount = 0", DefaultContexts.Delete, "UsedAmount = 0")>
<DefaultClassOptions()> _
Public Class DebitNote
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Today
    End Sub
    Private _no As String
    Private _transDate As Date
    Private _fromSupplier As Supplier
    Private _amount As Decimal
    Private _usedAmount As Decimal
    Private _remainingAmount As Decimal
    Private _note As String
    <RuleRequiredField("Rule Required for DebitNote.No", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for DebiteNote.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(ByVal value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for DebitNote.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <Association("Supplier-DebitNote")>
    <RuleRequiredField("Rule Required for DebitNote.FromSupplier", DefaultContexts.Save)>
    Public Property FromSupplier As Supplier
        Get
            Return _fromSupplier
        End Get
        Set(ByVal value As Supplier)
            SetPropertyValue("FromSupplier", _fromSupplier, value)
        End Set
    End Property
    Public Property Amount As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Amount", _amount, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property UsedAmount As Decimal
        Get
            Return _usedAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("UsedAmount", _usedAmount, value)
            If Not IsLoading Then
                CalculateRemainingAmount()
            End If
        End Set
    End Property
    Public Property RemainingAmount As Decimal
        Get
            Return _remainingAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("RemainingAmount", _remainingAmount, value)
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
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    Private Sub CalculateRemainingAmount()
        RemainingAmount = Amount - UsedAmount
    End Sub
End Class
