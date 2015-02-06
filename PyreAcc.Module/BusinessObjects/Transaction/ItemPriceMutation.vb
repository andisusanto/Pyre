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
<RuleObjectExists("Rule Object Exist for ItemPriceMutation for ItemPrice", "Submit", "@Item = Item AND @Since = EffectiveDate", invertresult:=False, LooksFor:=GetType(ItemPrice))>
<RuleCriteria("Rule Criteria for ItemPriceMutation.MinimumPrice > 0", DefaultContexts.Save, "MinimumPrice > 0")>
<RuleCriteria("Rule Criteria for ItemPriceMutation.MaximumPrice > 0", DefaultContexts.Save, "MaximumPrice > 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ItemPriceMutation
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        EffectiveDate = Today
    End Sub

    Private _no As String
    Private _item As Item
    Private _effectiveDate As Date
    Private _minimumPrice As Decimal
    Private _maximumPrice As Decimal
    Private _itemPrice As ItemPrice

    <RuleUniqueValue("Rule Unique for ItemPriceMutation.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for ItemPriceMutation.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(ByVal value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ItemPriceMutation.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ItemPriceMutation.EffectiveDate", DefaultContexts.Save)>
    Public Property EffectiveDate As Date
        Get
            Return _effectiveDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("EffectiveDate", _effectiveDate, value)
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
    Public Property MaximumPrice As Decimal
        Get
            Return _maximumPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("MaximumPrice", _maximumPrice, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property ItemPrice As ItemPrice
        Get
            Return _itemPrice
        End Get
        Set(value As ItemPrice)
            SetPropertyValue("ItemPrice", _itemPrice, value)
        End Set
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property

    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        ItemPrice = PyreAcc.Module.ItemPrice.CreateItemPrice(Session, Item, EffectiveDate, MinimumPrice, MaximumPrice)
    End Sub

    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        Dim tmpItemPrice = ItemPrice
        ItemPrice = Nothing
        PyreAcc.Module.ItemPrice.DeleteItemPrice(tmpItemPrice)
    End Sub
End Class
