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
<Appearance("Appearance Default Disabled for SalesReturnDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<RuleCriteria("Rule Criteria for SalesReturnDetail.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesReturnDetail", DefaultContexts.Save, "SalesReturn, SalesInvoiceDetail")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesReturnDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _sequence As Integer
    Private _salesReturn As SalesReturn
    Private _salesInvoiceDetail As SalesInvoiceDetail
    Private _quantity As Decimal
    Private _unit As Unit
    Private _baseUnitQuantity As Decimal
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
    <Association("SalesReturn-SalesReturnDetail")>
    <RuleRequiredField("Rule Required for SalesReturnDetail.SalesReturn", DefaultContexts.Save)>
    Public Property SalesReturn As SalesReturn
        Get
            Return _salesReturn
        End Get
        Set(ByVal value As SalesReturn)
            Dim oldValue = SalesReturn
            SetPropertyValue("SalesReturn", _salesReturn, value)
            If Not IsLoading Then
                If SalesReturn IsNot Nothing Then
                    SalesReturn.Total += Total
                    If SalesReturn.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        SalesReturn.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = SalesReturn.Details(SalesReturn.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    <DataSourceProperty("SalesInvoiceDetailDatasource")>
    <RuleRequiredField("Rule Required for SalesReturnDetail.SalesInvoiceDetail", DefaultContexts.Save)>
    Public Property SalesInvoiceDetail As SalesInvoiceDetail
        Get
            Return _salesInvoiceDetail
        End Get
        Set(ByVal value As SalesInvoiceDetail)
            SetPropertyValue("SalesInvoiceDetail", _salesInvoiceDetail, value)
            If Not IsLoading Then
                If SalesInvoiceDetail IsNot Nothing Then
                    'BaseUnitQuantity = SalesInvoiceDetail.BaseUnitQuantity - SalesInvoiceDetail.PaidBaseUnitQuantity
                Else
                    Total = 0
                End If
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
            If Not IsLoading Then CalculateBaseUnitQuantity()
        End Set
    End Property
    <ImmediatePostData(True)>
    <DataSourceProperty("SalesInvoiceDetail.Item.UnitSource")>
    Public Property Unit As Unit
        Get
            Return _unit
        End Get
        Set(ByVal value As Unit)
            SetPropertyValue("Unit", _unit, value)
            If Not IsLoading Then CalculateBaseUnitQuantity()
        End Set
    End Property
    Private Sub CalculateBaseUnitQuantity()
        If Unit IsNot Nothing AndAlso SalesInvoiceDetail IsNot Nothing Then
            Dim tmpRate As Decimal = SalesInvoiceDetail.Item.GetUnitRate(Unit)
            BaseUnitQuantity = Quantity * tmpRate
        Else
            BaseUnitQuantity = 0
        End If
    End Sub
    Public Property BaseUnitQuantity As Decimal
        Get
            Return _baseUnitQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("BaseUnitQuantity", _baseUnitQuantity, value)
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
                If SalesReturn IsNot Nothing Then
                    SalesReturn.Total -= oldValue
                    SalesReturn.Total += Total
                End If
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property BalanceSheetInventoryItem As BalanceSheetInventoryItem
        Get
            Return _balanceSheetInventoryItem
        End Get
        Set(ByVal value As BalanceSheetInventoryItem)
            SetPropertyValue("BalanceSheetInventoryItem", _balanceSheetInventoryItem, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SalesInvoiceDetailDatasource As XPCollection(Of SalesInvoiceDetail)
        Get
            Return New XPCollection(Of SalesInvoiceDetail)(Session, (GroupOperator.And(New BinaryOperator("SalesInvoice.Status", TransactionStatus.Submitted), New BinaryOperator("SalesInvoice.PaymentOutstandingStatus", OutstandingStatus.Cleared, BinaryOperatorType.NotEqual), New BinaryOperator("SalesInvoice.Customer", SalesReturn.Customer), New BinaryOperator("ReturnOutstandingBaseUnitQuantity", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
    Private Sub CalculateTotal()
        Total = SalesInvoiceDetail.UnitPrice * BaseUnitQuantity
    End Sub
End Class
