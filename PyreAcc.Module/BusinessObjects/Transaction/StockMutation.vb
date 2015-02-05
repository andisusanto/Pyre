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
<RuleCriteria("Rule Criteria for StockMutation.FromInventory <> ToInventory", DefaultContexts.Save, "FromInventory <> ToInventory")>
<RuleCriteria("Rule Criteria for StockMutation.BaseUnitQuantity > 0", DefaultContexts.Save, "BaseUnitQuantity > 0")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class StockMutation
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Now
    End Sub
    Private _no As String
    Private _description As String
    Private _fromInventory As Inventory
    Private _toInventory As Inventory
    Private _transDate As Date
    Private _item As Item
    Private _baseUnitQuantity As Integer
    <RuleRequiredField("Rule Required for StockMutation.No", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for StockMutation.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for StockMutation.FromInventory", DefaultContexts.Save)>
    Public Property FromInventory As Inventory
        Get
            Return _fromInventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("FromInventory", _fromInventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for StockMutation.ToInventory", DefaultContexts.Save)>
    Public Property ToInventory As Inventory
        Get
            Return _toInventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("ToInventory", _toInventory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for StockMutation.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for StockMutation.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    Public Property BaseUnitQuantity As Integer
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
        End Set
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No & " ~ " & Description
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        If BaseUnitQuantity > 0 Then

        End If
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        If BaseUnitQuantity > 0 Then

        End If
    End Sub
End Class
