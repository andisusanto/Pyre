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
<Appearance("Appearance Default Disabled for PurchaseInvoiceDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="BaseUnitQuantity, ReturnedBaseUnitQuantity, Total, Discount, GrandTotal")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchaseInvoiceDetail", DefaultContexts.Save, "PurchaseInvoice, Item, BatchNo")>
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
    Private _batchNo As String
    Private _expiryDate As Date
    Private _baseUnitQuantity As Decimal
    Private _unitPrice As Decimal
    Private _total As Decimal
    Private _discountType As DiscountType
    Private _discountValue As Decimal
    Private _discount As Decimal
    Private _grandTotal As Decimal
    Private _periodCutOffInventoryItem As PeriodCutOffInventoryItem
    <VisibleInListView(False)>
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
                    PurchaseInvoice.DetailsTotal += Total
                    PurchaseInvoice.DetailsDiscount += Discount
                    PurchaseInvoice.Total += GrandTotal
                    If PurchaseInvoice.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        PurchaseInvoice.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PurchaseInvoice.Details(PurchaseInvoice.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then
                    oldValue.Total -= GrandTotal
                    oldValue.DetailsTotal -= Total
                    oldValue.DetailsDiscount -= Discount
                End If
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
                CalculateBaseUnitQuantity()
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
    <NonCloneable()>
    Public Property BatchNo As String
        Get
            Return _batchNo
        End Get
        Set(value As String)
            SetPropertyValue("BatchNo", _batchNo, value)
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
    <RuleRequiredField("Rule Required for PurchaseInvoiceDetail.ExpiryDate", DefaultContexts.Save, targetcriteria:="Item.HasExpiryDate = TRUE")>
    Public Property ExpiryDate As Date
        Get
            Return _expiryDate
        End Get
        Set(value As Date)
            SetPropertyValue("ExpiryDate", _expiryDate, value)
        End Set
    End Property
    <VisibleInListView(False)>
    Public Property BaseUnitQuantity As Decimal
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
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
                CalculateDiscount()
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.DetailsTotal -= oldValue
                    PurchaseInvoice.DetailsTotal += Total
                End If
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    Public Property DiscountType As DiscountType
        Get
            Return _discountType
        End Get
        Set(ByVal value As DiscountType)
            SetPropertyValue("DiscountType", _discountType, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    <RuleRange(0, 100, targetcriteria:="DiscountType = 'ByPercentage'")>
    Public Property DiscountValue As Decimal
        Get
            Return _discountValue
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DiscountValue", _discountValue, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Discount As Decimal
        Get
            Return _discount
        End Get
        Set(ByVal value As Decimal)
            Dim oldValue = Discount
            SetPropertyValue("Discount", _discount, value)
            If Not IsLoading Then
                CalculateGrandTotal()
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.DetailsDiscount -= oldValue
                    PurchaseInvoice.DetailsDiscount += Discount
                End If
            End If
        End Set
    End Property
    Public Property GrandTotal As Decimal
        Get
            Return _grandTotal
        End Get
        Set(ByVal value As Decimal)
            Dim oldValue = GrandTotal
            SetPropertyValue("GrandTotal", _grandTotal, value)
            If Not IsLoading Then
                If PurchaseInvoice IsNot Nothing Then
                    PurchaseInvoice.Total -= oldValue
                    PurchaseInvoice.Total += GrandTotal
                End If
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffInventoryItem As PeriodCutOffInventoryItem
        Get
            Return _periodCutOffInventoryItem
        End Get
        Set(value As PeriodCutOffInventoryItem)
            SetPropertyValue("PeriodCutOffInventoryItem", _periodCutOffInventoryItem, value)
        End Set
    End Property
    Private Sub CalculateTotal()
        Total = UnitPrice * Quantity
    End Sub
    Private Sub CalculateDiscount()
        Select Case DiscountType
            Case [Module].DiscountType.ByAmount
                Discount = DiscountValue
            Case [Module].DiscountType.ByPercentage
                Discount = GlobalFunction.Round(Total * DiscountValue / 100)
        End Select
    End Sub
    Private Sub CalculateGrandTotal()
        GrandTotal = Total - Discount
    End Sub
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If PurchaseInvoice Is Nothing OrElse Item Is Nothing Then Return Nothing
            Return PurchaseInvoice.DefaultDisplay & " ~ " & Item.DefaultDisplay
        End Get
    End Property
End Class
