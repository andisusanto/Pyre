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
<Appearance("Appearance Default Disabled for SalesPaymentDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<RuleCriteria("Rule Criteria for SalesPaymentDetail.Total > 0", DefaultContexts.Save, "Total > 0")>
<DeferredDeletion(False)>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesPaymentDetail", DefaultContexts.Save, "SalesPayment, SalesInvoiceDetail")>
<DefaultClassOptions()> _
Public Class SalesPaymentDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _sequence As Integer
    Private _salesPayment As SalesPayment
    Private _salesInvoiceDetail As SalesInvoiceDetail
    Private _quantity As Integer
    Private _total As Decimal
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("SalesPayment-SalesPaymentDetail")>
    <RuleRequiredField("Rule Required for SalesPaymentDetail.SalesPayment", DefaultContexts.Save)>
    Public Property SalesPayment As SalesPayment
        Get
            Return _salesPayment
        End Get
        Set(ByVal value As SalesPayment)
            Dim oldValue = SalesPayment
            SetPropertyValue("SalesPayment", _salesPayment, value)
            If Not IsLoading Then
                If SalesPayment IsNot Nothing Then
                    SalesPayment.Total += Total
                    If SalesPayment.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        SalesPayment.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = SalesPayment.Details(SalesPayment.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <DataSourceProperty("SalesInvoiceDetailDatasource")>
    <RuleRequiredField("Rule Required for SalesPaymentDetail.SalesInvoiceDetail", DefaultContexts.Save)>
    Public Property SalesInvoiceDetail As SalesInvoiceDetail
        Get
            Return _salesInvoiceDetail
        End Get
        Set(ByVal value As SalesInvoiceDetail)
            SetPropertyValue("SalesInvoiceDetail", _salesInvoiceDetail, value)
            If Not IsLoading Then
                If SalesInvoiceDetail IsNot Nothing Then
                    Quantity = SalesInvoiceDetail.Quantity - SalesInvoiceDetail.ReturnedQuantity
                Else
                    Total = 0
                End If
            End If
        End Set
    End Property
    Public Property Quantity As Integer
        Get
            Return _quantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Quantity", _quantity, value)
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
                If SalesPayment IsNot Nothing Then
                    SalesPayment.Total -= oldValue
                    SalesPayment.Total += Total
                End If
            End If
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SalesInvoiceDetailDatasource As XPCollection(Of SalesInvoiceDetail)
        Get
            Return New XPCollection(Of SalesInvoiceDetail)(Session, (GroupOperator.And(New BinaryOperator("SalesInvoice.Status", TransactionStatus.Submitted), New BinaryOperator("SalesInvoice.PaymentOutstandingStatus", OutstandingStatus.Cleared, BinaryOperatorType.NotEqual), New BinaryOperator("SalesInvoice.Customer", SalesPayment.Customer), New BinaryOperator("PaymentOutstandingQuantity", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
    Private Sub CalculateTotal()
        Total = SalesInvoiceDetail.UnitPrice * Quantity
    End Sub
End Class
