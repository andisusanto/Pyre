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
<RuleCriteria("Rule Criteria for OtherExpenseDetail.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for OtherExpenseDetail", DefaultContexts.Save, "OtherExpense, OtherExpenseItem")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class OtherExpenseDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _otherExpense As OtherExpense
    Private _otherExpenseItem As OtherExpenseItem
    Private _amount As Double
    <Association("OtherExpense-OtherExpenseDetail")>
    <RuleRequiredField("Rule Required for OtherExpenseDetail.OtherExpense", DefaultContexts.Save)>
     Public Property OtherExpense As OtherExpense
        Get
            Return _otherExpense
        End Get
        Set(ByVal value As OtherExpense)
            SetPropertyValue("OtherExpense", _otherExpense, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for OtherExpenseDetail.OtherExpenseItem", DefaultContexts.Save)>
    Public Property OtherExpenseItem As OtherExpenseItem
        Get
            Return _otherExpenseItem
        End Get
        Set(ByVal value As OtherExpenseItem)
            SetPropertyValue("OtherExpenseItem", _otherExpenseItem, value)
        End Set
    End Property
    Public Property Amount As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("Amount", _amount, value)
        End Set
    End Property
End Class
