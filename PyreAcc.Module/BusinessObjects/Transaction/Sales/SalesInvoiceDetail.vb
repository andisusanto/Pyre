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
<CreatableItem(False)>
<Appearance("Appearance Default Disabled for SalesInvoiceDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="BaseUnitQuantity, ReturnedBaseUnitQuantity, Total")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesInvoiceDetail", DefaultContexts.Save, "SalesInvoice, Item")>
<RuleCriteria("Rule Criteria for SalesInvoiceDetail.Total > 0", DefaultContexts.Save, "Total > 0", "Total must be greater than zero")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesInvoiceDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _salesInvoice As SalesInvoice
    Private _item As Item
    Private _unit As Unit
    Private _quantity As Decimal
    Private _baseUnitQuantity As Decimal
    Private _returnedBaseUnitQuantity As Decimal
    Private _unitPrice As Decimal
    Private _total As Decimal
    Private _balanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("SalesInvoice-SalesInvoiceDetail")>
    <RuleRequiredField("Rule Required for SalesInvoiceDetail.SalesInvoice", DefaultContexts.Save)>
    Public Property SalesInvoice As SalesInvoice
        Get
            Return _salesInvoice
        End Get
        Set(ByVal value As SalesInvoice)
            Dim oldValue = SalesInvoice
            SetPropertyValue("SalesInvoice", _salesInvoice, value)
            If Not IsLoading Then
                If SalesInvoice IsNot Nothing Then
                    SalesInvoice.Total += Total
                    If SalesInvoice.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        SalesInvoice.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = SalesInvoice.Details(SalesInvoice.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for SalesInvoiceDetail.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <DataSourceProperty("Item.UnitSource")>
    Public Property Unit As Unit
        Get
            Return _unit
        End Get
        Set(ByVal value As Unit)
            SetPropertyValue("Unit", _unit, value)
            If Not IsLoading Then
                CalculateBaseUnitQuantity()
                CalculateUnitPrice()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Quantity As Decimal
        Get
            Return _quantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Quantity", _quantity, value)
            If Not IsLoading Then
                CalculateBaseUnitQuantity()
                CalculateTotal()
            End If
        End Set
    End Property
    Private Sub CalculateBaseUnitQuantity()
        If Unit IsNot Nothing AndAlso Item IsNot Nothing Then
            Dim tmpRate As Decimal = Item.GetUnitRate(Unit)
            BaseUnitQuantity = Quantity * tmpRate
        Else
            BaseUnitQuantity = 0
        End If
    End Sub
    Private Sub CalculateUnitPrice()
        If Unit IsNot Nothing AndAlso Item IsNot Nothing Then
            Dim tmpRate As Decimal = Item.GetUnitRate(Unit)
            Dim tmpItemPrice As ItemPrice = Item.GetPrice(SalesInvoice.TransDate)
            UnitPrice = tmpItemPrice.MaximumPrice * tmpRate
        Else
            UnitPrice = 0
        End If
    End Sub
    Public Property BaseUnitQuantity As Decimal
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
        End Set
    End Property
    Public Property ReturnedBaseUnitQuantity As Decimal
        Get
            Return _returnedBaseUnitQuantity
        End Get
        Set(value As Decimal)
            SetPropertyValue("ReturnedBaseUnitQuantity", _returnedBaseUnitQuantity, value)
            If Not IsLoading Then
                SalesInvoice.UpdateReturnOutstandingStatus()
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    <PersistentAlias("BaseUnitQuantity - ReturnedBaseUnitQuantity")>
    Public ReadOnly Property ReturnOutstandingBaseUnitQuantity As Decimal
        Get
            Return EvaluateAlias("ReturnOutstandingBaseUnitQuantity")
        End Get
    End Property
    Public Property UnitPrice As Decimal
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("UnitPrice", _unitPrice, value)
            If Not IsLoading Then
                CalculateTotal()
            End If
        End Set
    End Property
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            Dim oldValue = Total
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                If SalesInvoice IsNot Nothing Then
                    SalesInvoice.Total -= oldValue
                    SalesInvoice.Total += Total
                End If
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
        Get
            Return _balanceSheetInventoryItemDeductTransaction
        End Get
        Set(value As BalanceSheetInventoryItemDeductTransaction)
            SetPropertyValue("BalanceSheetInventoryItemDeductTransaction", _balanceSheetInventoryItemDeductTransaction, value)
        End Set
    End Property
    Private Sub CalculateTotal()
        Total = UnitPrice * Quantity
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If SalesInvoice Is Nothing OrElse Item Is Nothing Then Return Nothing
            Return SalesInvoice.DefaultDisplay & " ~ " & Item.DefaultDisplay
        End Get
    End Property
End Class
