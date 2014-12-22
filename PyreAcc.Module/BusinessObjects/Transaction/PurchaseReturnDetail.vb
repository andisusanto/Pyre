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
<Appearance("Appearance Default Disabled for PurchaseReturnDetail", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<RuleCriteria("Rule Criteria for PurchaseReturnDetail.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchaseReturnDetail", DefaultContexts.Save, "PurchaseReturn, PurchaseInvoiceDetail")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseReturnDetail
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _sequence As Integer
    Private _purchaseReturn As PurchaseReturn
    Private _purchaseInvoiceDetail As PurchaseInvoiceDetail
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
    <Association("PurchaseReturn-PurchaseReturnDetail")>
    <RuleRequiredField("Rule Required for PurchaseReturnDetail.PurchaseReturn", DefaultContexts.Save)>
    Public Property PurchaseReturn As PurchaseReturn
        Get
            Return _purchaseReturn
        End Get
        Set(ByVal value As PurchaseReturn)
            Dim oldValue = PurchaseReturn
            SetPropertyValue("PurchaseReturn", _purchaseReturn, value)
            If Not IsLoading Then
                If PurchaseReturn IsNot Nothing Then
                    PurchaseReturn.Total += Total
                    If PurchaseReturn.Details.Count = 0 Then
                        Sequence = 0
                    Else
                        PurchaseReturn.Details.Sorting = New SortingCollection(New SortProperty("Sequence", DB.SortingDirection.Ascending))
                        Sequence = PurchaseReturn.Details(PurchaseReturn.Details.Count - 1).Sequence + 1
                    End If
                End If
                If oldValue IsNot Nothing Then oldValue.Total -= Total
            End If
        End Set
    End Property
    <DataSourceProperty("PurchaseInvoiceDetailDatasource")>
    <RuleRequiredField("Rule Required for PurchaseReturnDetail.PurchaseInvoiceDetail", DefaultContexts.Save)>
    Public Property PurchaseInvoiceDetail As PurchaseInvoiceDetail
        Get
            Return _purchaseInvoiceDetail
        End Get
        Set(ByVal value As PurchaseInvoiceDetail)
            SetPropertyValue("PurchaseInvoiceDetail", _purchaseInvoiceDetail, value)
            If Not IsLoading Then
                If PurchaseInvoiceDetail IsNot Nothing Then
                    Quantity = PurchaseInvoiceDetail.Quantity - PurchaseInvoiceDetail.PaidQuantity
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
                If PurchaseReturn IsNot Nothing Then
                    PurchaseReturn.Total -= oldValue
                    PurchaseReturn.Total += Total
                End If
            End If
        End Set
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If PurchaseReturn Is Nothing OrElse PurchaseInvoiceDetail Is Nothing Then Return Nothing
            Return PurchaseReturn.DefaultDisplay & " ~ " & PurchaseInvoiceDetail.DefaultDisplay
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property PurchaseInvoiceDetailDatasource As XPCollection(Of PurchaseInvoiceDetail)
        Get
            Return New XPCollection(Of PurchaseInvoiceDetail)(Session, (GroupOperator.And(New BinaryOperator("PurchaseInvoice.Status", TransactionStatus.Submitted), New BinaryOperator("PurchaseInvoice.PaymentOutstandingStatus", OutstandingStatus.Cleared, BinaryOperatorType.NotEqual), New BinaryOperator("PurchaseInvoice.Supplier", PurchaseReturn.Supplier), New BinaryOperator("ReturnOutstandingQuantity", 0, BinaryOperatorType.Greater))))
        End Get
    End Property
    Private Sub CalculateTotal()
        Total = PurchaseInvoiceDetail.UnitPrice * Quantity
    End Sub
End Class
