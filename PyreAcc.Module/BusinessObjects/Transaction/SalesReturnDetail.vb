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
                    Quantity = SalesInvoiceDetail.Quantity - SalesInvoiceDetail.PaidQuantity
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
                If SalesReturn IsNot Nothing Then
                    SalesReturn.Total -= oldValue
                    SalesReturn.Total += Total
                End If
            End If
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SalesInvoiceDetailDatasource As XPCollection(Of SalesInvoiceDetail)
        Get
            Return New XPCollection(Of SalesInvoiceDetail)(Session, (GroupOperator.And(New BinaryOperator("SalesInvoice.Status", TransactionStatus.Submitted), New BinaryOperator("SalesInvoice.PaymentOutstandingStatus", OutstandingStatus.Cleared, BinaryOperatorType.NotEqual), New BinaryOperator("SalesInvoice.Customer", SalesReturn.Customer), New BinaryOperator("ReturnOutstandingQuantity", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
    Private Sub CalculateTotal()
        Total = SalesInvoiceDetail.UnitPrice * Quantity
    End Sub
End Class
