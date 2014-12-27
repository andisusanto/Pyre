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
<RuleCriteria("Rule Criteria for PurchasePaymentDetail.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<DeferredDeletion(False)>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchasePaymentDetail", DefaultContexts.Save, "PurchasePayment, PurchaseInvoice")>
<DefaultClassOptions()> _
Public Class PurchasePaymentDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _sequence As Integer
    Private _purchasePayment As PurchasePayment
    Private _purchaseInvoice As PurchaseInvoice
    Private _amount As Decimal
    Public Property Sequence As Integer
        Get
            Return _sequence
        End Get
        Set(value As Integer)
            SetPropertyValue("Sequence", _sequence, value)
        End Set
    End Property
    <Association("PurchasePayment-PurchasePaymentDetail")>
    <RuleRequiredField("Rule Required for PurchasePaymentDetail.PurchasePayment", DefaultContexts.Save)>
    Public Property PurchasePayment As PurchasePayment
        Get
            Return _purchasePayment
        End Get
        Set(ByVal value As PurchasePayment)
            Dim oldValue = PurchasePayment
            SetPropertyValue("PurchasePayment", _purchasePayment, value)
            If Not IsLoading Then
                If PurchasePayment IsNot Nothing Then
                    PurchasePayment.Total += Amount
                    If PurchasePayment.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        PurchasePayment.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PurchasePayment.Details(PurchasePayment.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Amount
            End If
        End Set
    End Property
    <DataSourceProperty("PurchaseInvoiceDatasource")>
    <RuleRequiredField("Rule Required for PurchasePaymentDetail.PurchaseInvoice", DefaultContexts.Save)>
    Public Property PurchaseInvoice As PurchaseInvoice
        Get
            Return _purchaseInvoice
        End Get
        Set(ByVal value As PurchaseInvoice)
            SetPropertyValue("PurchaseInvoice", _purchaseInvoice, value)
            If Not IsLoading Then
                If PurchaseInvoice IsNot Nothing Then
                    Amount = PurchaseInvoice.PaymentOutstandingAmount
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
                If PurchasePayment IsNot Nothing Then
                    PurchasePayment.Total -= oldValue
                    PurchasePayment.Total += Amount
                End If
            End If
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property PurchaseInvoiceDatasource As XPCollection(Of PurchaseInvoice)
        Get
            Return New XPCollection(Of PurchaseInvoice)(Session, (GroupOperator.And(New BinaryOperator("Status", TransactionStatus.Submitted), New BinaryOperator("PaymentOutstandingStatus", OutstandingStatus.Cleared, BinaryOperatorType.NotEqual), New BinaryOperator("Supplier", PurchasePayment.Supplier), New BinaryOperator("PaymentOutstandingAmount", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
End Class
