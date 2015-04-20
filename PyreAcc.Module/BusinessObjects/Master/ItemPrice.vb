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

<CreatableItem(False)> _
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for ItemPrice", DefaultContexts.Save, "Item, Since")>
<RuleCriteria("Rule Criteria for ItemPrice.MinimumPrice > 0", DefaultContexts.Save, "MinimumPrice > 0")>
<RuleCriteria("Rule Criteria for ItemPrice.MaximumPrice > 0", DefaultContexts.Save, "MaximumPrice > 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ItemPrice
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _item As Item
    Private _since As Date
    Private _until As Date
    Private _minimumPrice As Decimal
    Private _standardPrice As Decimal
    Private _maximumPrice As Decimal

    <Association("Item-ItemPrice")>
    <RuleRequiredField("Rule Required for ItemPrice.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ItemPrice.Since", DefaultContexts.Save)>
    Public Property Since As Date
        Get
            Return _since
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("Since", _since, value)
        End Set
    End Property
    Public Property Until As Date
        Get
            Return _until
        End Get
        Set(value As Date)
            SetPropertyValue("Until", _until, value)
        End Set
    End Property
    Public Property MinimumPrice As Decimal
        Get
            Return _minimumPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("MinimumPrice", _minimumPrice, value)
        End Set
    End Property
    Public Property StandardPrice As Decimal
        Get
            Return _standardPrice
        End Get
        Set(value As Decimal)
            SetPropertyValue("StandardPrice", _standardPrice, value)
        End Set
    End Property
    Public Property MaximumPrice As Decimal
        Get
            Return _maximumPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("MaximumPrice", _maximumPrice, value)
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property PreviousRecord As ItemPrice
        Get
            Return Session.FindObject(Of ItemPrice)(GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("Until", DateAdd(DateInterval.Day, -1, Since))))
        End Get
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property NextRecord As ItemPrice
        Get
            If Until = Nothing Then Return Nothing
            Return Session.FindObject(Of ItemPrice)(GroupOperator.And(New BinaryOperator("Item", Item), New BinaryOperator("Since", DateAdd(DateInterval.Day, 1, Until))))
        End Get
    End Property

    Public Shared Function CreateItemPrice(ByVal prmUOW As UnitOfWork, ByVal prmItem As Item, ByVal prmEffectiveDate As Date, ByVal prmMinimumPrice As Decimal, ByVal prmStandardPrice As Decimal, ByVal prmMaximumPrice As Decimal) As ItemPrice
        Dim xpItemPrice As New XPCollection(Of ItemPrice)(prmUOW, GroupOperator.And(New BinaryOperator("Item", prmItem), New BinaryOperator("Since", prmEffectiveDate, BinaryOperatorType.LessOrEqual)))
        xpItemPrice.TopReturnedObjects = 1
        If xpItemPrice.Count = 0 Then Throw New Exception("Previous history not found")
        Dim tmpUntil = xpItemPrice(0).Until
        xpItemPrice(0).Until = DateAdd(DateInterval.Day, -1, prmEffectiveDate)
        Dim objItemPrice As New ItemPrice(prmUOW)
        objItemPrice.Item = prmItem
        objItemPrice.Since = prmEffectiveDate
        objItemPrice.Until = tmpUntil
        objItemPrice.MinimumPrice = prmMinimumPrice
        objItemPrice.StandardPrice = prmStandardPrice
        objItemPrice.MaximumPrice = prmMaximumPrice
        Return objItemPrice
    End Function

    Public Shared Sub DeleteItemPrice(ByVal prmItemPrice As ItemPrice)
        Dim previousRecord = prmItemPrice.PreviousRecord
        If previousRecord IsNot Nothing Then previousRecord.Until = prmItemPrice.Until
        prmItemPrice.Delete()
    End Sub
End Class
