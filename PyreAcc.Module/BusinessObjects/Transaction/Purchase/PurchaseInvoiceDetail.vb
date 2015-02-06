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
<Appearance("Appearance Default Disabled for PurchaseInvoiceDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="BaseUnitQuantity, ReturnedBaseUnitQuantity, Total")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchaseInvoiceDetail", DefaultContexts.Save, "PurchaseInvoice, Item")>
<RuleCriteria("Rule Criteria for PurchaseInvoiceDetail.Total > 0", DefaultContexts.Save, "Total > 0", "Total must be greater than zero")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseInvoiceDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _sequence As Integer
    Private _purchaseInvoice As PurchaseInvoice
    Private _item As Item
    Private _unit As Unit
    Private _quantity As Decimal
    Private _expiryDate As Date
    Private _baseUnitQuantity As Decimal
    Private _returnedBaseUnitQuantity As Decimal
    Private _unitPrice As Decimal
    Private _total As Decimal
    Private _balanceSheetInventoryItem As BalanceSheetInventoryItem
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("PurchaseInvoice-PurchaseInvoiceDetail")>
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.PurchaseInvoice", DefaultContexts.Save)>
    Public Property PurchaseInvoice As PurchaseInvoice
        Get
            Return _purchaseInvoice
        End Get
        Set(ByVal value As PurchaseInvoice)
            Dim oldValue = PurchaseInvoice
            SetPropertyValue("PurchaseInvoice", _purchaseInvoice, value)
            If Not IsLoading Then
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.Total += Total
                    If PurchaseInvoice.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        PurchaseInvoice.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PurchaseInvoice.Details(PurchaseInvoice.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return _item
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", _item, value)
            If Not IsLoading Then
                If Item IsNot Nothing Then
                    Unit = Item.BaseUnit
                Else
                    Unit = Nothing
                End If
            End If
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
                CalculateBaseUnit()
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
                CalculateBaseUnit()
                CalculateTotal()
            End If
        End Set
    End Property
    Private Sub CalculateBaseUnit()
        If Unit IsNot Nothing AndAlso Item IsNot Nothing Then
            Dim tmpRate As Decimal = Item.GetUnitRate(Unit)
            BaseUnitQuantity = Quantity * tmpRate
        Else
            BaseUnitQuantity = 0
        End If
    End Sub
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.ExpiryDate", DefaultContexts.Save, targetcriteria:="Item.HasExpiryDate = TRUE")>
    Public Property ExpiryDate As Date
        Get
            Return _expiryDate
        End Get
        Set(value As Date)
            SetPropertyValue("ExpiryDate", _expiryDate, value)
        End Set
    End Property
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
                PurchaseInvoice.UpdateReturnOutstandingStatus()
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
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.Total -= oldValue
                    PurchaseInvoice.Total += Total
                End If
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetInventoryItem As BalanceSheetInventoryItem
        Get
            Return _balanceSheetInventoryItem
        End Get
        Set(value As BalanceSheetInventoryItem)
            SetPropertyValue("BalanceSheetInventoryItem", _balanceSheetInventoryItem, value)
        End Set
    End Property
    Private Sub CalculateTotal()
        Total = UnitPrice * Quantity
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If PurchaseInvoice Is Nothing OrElse Item Is Nothing Then Return Nothing
            Return PurchaseInvoice.DefaultDisplay & " ~ " & Item.DefaultDisplay
        End Get
    End Property
End Class
