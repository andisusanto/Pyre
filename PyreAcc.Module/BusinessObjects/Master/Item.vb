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
<Appearance("Appearance Default Disabled for Item", enabled:=False, targetitems:="PriceHistory")>
<Appearance("Appearance for Item.PriceHistory.Count > 1", enabled:=False, criteria:="@PriceHistory.Count > 1", targetitems:="Since, MinimumPrice, MaximumPrice")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Item
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        Active = True
        Dim objItemPrice As New ItemPrice(Session)
        objItemPrice.Item = Me
    End Sub

    Private fCode As String
    Private fName As String
    Private fCategory As Category
    Private fBrand As Brand
    Private fMainSupplier As Supplier
    Private fBaseUnit As Unit
    Private fActive As Boolean

    Private fSince As Date
    Private fMinimumPrice As Decimal
    Private fMaximumPrice As Decimal
    <RuleRequiredField("Rule Required for Item.Code", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for Item.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return fCode
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", fCode, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.Name", DefaultContexts.Save)>
    Public Property Name As String
        Get
            Return fName
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Name", fName, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.Category", DefaultContexts.Save)>
    Public Property Category As Category
        Get
            Return fCategory
        End Get
        Set(value As Category)
            SetPropertyValue("Category", fCategory, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.Brand", DefaultContexts.Save)>
    Public Property Brand As Brand
        Get
            Return fBrand
        End Get
        Set(value As Brand)
            SetPropertyValue("Brand", fBrand, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.MainSupplier", DefaultContexts.Save)>
    Public Property MainSupplier As Supplier
        Get
            Return fMainSupplier
        End Get
        Set(value As Supplier)
            SetPropertyValue("MainSupplier", fMainSupplier, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.BaseUnit", DefaultContexts.Save)>
    Public Property BaseUnit As Unit
        Get
            Return fBaseUnit
        End Get
        Set(value As Unit)
            SetPropertyValue("BaseUnit", fBaseUnit, value)
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return fActive
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Active", fActive, value)
        End Set
    End Property

    <RuleRequiredField("Rule Required for Item.Since", DefaultContexts.Save)>
    Public Property Since As Date
        Get
            Return fSince
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("Since", fSince, value)
            If Not IsLoading Then
                PriceHistory(0).Since = Since
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.MinimumPrice", DefaultContexts.Save)>
    Public Property MinimumPrice As Decimal
        Get
            Return fMinimumPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("MinimumPrice", fMinimumPrice, value)
            If Not IsLoading Then
                PriceHistory(0).MinimumPrice = MinimumPrice
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for Item.MaximumPrice", DefaultContexts.Save)>
    Public Property MaximumPrice As Decimal
        Get
            Return fMaximumPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("MaximumPrice", fMaximumPrice, value)
            If Not IsLoading Then
                PriceHistory(0).MaximumPrice = MaximumPrice
            End If
        End Set
    End Property

    <Association("Item-ItemPrice"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property PriceHistory As XPCollection(Of ItemPrice)
        Get
            Return GetCollection(Of ItemPrice)("PriceHistory")
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Name
        End Get
    End Property

    Public Function GetPrice(ByVal prmTransDate As Date) As ItemPrice
        Return Session.FindObject(Of ItemPrice)(PersistentCriteriaEvaluationBehavior.InTransaction, GroupOperator.And(New BinaryOperator("Item", Me), New BinaryOperator("Since", prmTransDate, BinaryOperatorType.LessOrEqual), GroupOperator.Or(New NullOperator("Until"), New BinaryOperator("Until", prmTransDate, BinaryOperatorType.GreaterOrEqual))))
    End Function
End Class
