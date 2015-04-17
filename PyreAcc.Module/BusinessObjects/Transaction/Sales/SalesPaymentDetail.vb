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
<RuleCriteria("Rule Criteria for SalesPaymentDetail.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<DeferredDeletion(False)>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesPaymentDetail", DefaultContexts.Save, "SalesPayment, SalesInvoice")>
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
    Private _salesInvoice As SalesInvoice
    Private _amount As Decimal
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
                    SalesPayment.Total += Amount
                    If SalesPayment.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        SalesPayment.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = SalesPayment.Details(SalesPayment.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Amount
            End If
        End Set
    End Property
    <DataSourceProperty("SalesInvoiceDatasource")>
    <RuleRequiredField("Rule Required for SalesPaymentDetail.SalesInvoice", DefaultContexts.Save)>
    Public Property SalesInvoice As SalesInvoice
        Get
            Return _salesInvoice
        End Get
        Set(ByVal value As SalesInvoice)
            SetPropertyValue("SalesInvoice", _salesInvoice, value)
            If Not IsLoading Then
                If SalesInvoice IsNot Nothing Then
                    Amount = SalesInvoice.PaymentOutstandingAmount
                Else
                    Amount = 0
                End If
            End If
        End Set
    End Property
    Public Property Amount As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal value As Decimal)
            Dim oldValue = Amount
            SetPropertyValue("Amount", _amount, value)
            If Not IsLoading Then
                If SalesPayment IsNot Nothing Then
                    SalesPayment.Total -= oldValue
                    SalesPayment.Total += Amount
                End If
            End If
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SalesInvoiceDatasource As XPCollection(Of SalesInvoice)
        Get
            Return New XPCollection(Of SalesInvoice)(Session, (GroupOperator.And(New BinaryOperator("Status", TransactionStatus.Submitted), New BinaryOperator("Customer", SalesPayment.Customer), New BinaryOperator("PaymentOutstandingAmount", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
End Class
