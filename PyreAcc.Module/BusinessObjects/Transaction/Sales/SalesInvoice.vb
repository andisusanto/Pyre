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
<RuleCriteria("Rule Criteria for Cancel SalesInvoice.PaymentOutstandingStatus", "Cancel", "PaymentOutstandingStatus = 'Full'")>
<RuleCriteria("Rule Criteria for Cancel SalesInvoice.ReturnOutstandingStatus", "Cancel", "ReturnOutstandingStatus = 'Full'")>
<RuleCriteria("Rule Criteria for SalesInvoice.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for SalesInvoice.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for SalesPayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, PaymentOutstandingStatus, PaidAmount, PaymentOutstandingAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesInvoice
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Now
    End Sub
    Private _no As String
    Private _transDate As Date
    Private _term As Integer
    Private _dueDate As Date
    Private _customer As Customer
    Private _inventory As Inventory
    Private _total As Decimal
    Private _paidAmount As Decimal
    Private _paymentOutstandingAmount As Decimal
    Private _salesman As Salesman
    Private _paymentOutstandingStatus As OutstandingStatus
    Private _returnOutstandingStatus As OutstandingStatus
    <RuleUniqueValue("Rule Unique for SalesInvoice.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for SalesInvoice.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    DueDate = TransDate.AddDays(Term)
                End If
            End If
        End Set
    End Property
    Public Property Term As Integer
        Get
            Return _term
        End Get
        Set(value As Integer)
            SetPropertyValue("Term", _term, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    DueDate = TransDate.AddDays(Term)
                End If
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.DueDate", DefaultContexts.Save)>
    Public Property DueDate As Date
        Get
            Return _dueDate
        End Get
        Set(value As Date)
            SetPropertyValue("DueDate", _dueDate, value)
            If Not IsLoading Then
                If TransDate.AddDays(Term) <> DueDate Then
                    Term = DateDiff(DateInterval.Day, TransDate, DueDate)
                End If
            End If
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesInvoice.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculatePaymentOutstandingAmount()
            End If
        End Set
    End Property
    Public Property PaidAmount As Decimal
        Get
            Return _paidAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("PaidAmount", _paidAmount, value)
            If Not IsLoading Then
                CalculatePaymentOutstandingAmount()
            End If
        End Set
    End Property
    Public Property PaymentOutstandingAmount As Decimal
        Get
            Return _paymentOutstandingAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("PaymentOutstandingAmount", _paymentOutstandingAmount, value)
        End Set
    End Property
    Public Property Salesman As Salesman
        Get
            Return _salesman
        End Get
        Set(value As Salesman)
            SetPropertyValue("Salesman", _salesman, value)
        End Set
    End Property
    Public Property PaymentOutstandingStatus As OutstandingStatus
        Get
            Return _paymentOutstandingStatus
        End Get
        Set(value As OutstandingStatus)
            SetPropertyValue("PaymentOutstandingStatus", _paymentOutstandingStatus, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property ReturnOutstandingStatus As OutstandingStatus
        Get
            Return _returnOutstandingStatus
        End Get
        Set(value As OutstandingStatus)
            SetPropertyValue("ReturnOutstandingStatus", _returnOutstandingStatus, value)
        End Set
    End Property
    <Association("SalesInvoice-SalesInvoiceDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesInvoiceDetail)
        Get
            Return GetCollection(Of SalesInvoiceDetail)("Details")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsPeriodClosed As Boolean
        Get
            Dim period As Period = Session.FindObject(Of Period)(GroupOperator.And(New BinaryOperator("StartDate", TransDate, BinaryOperatorType.LessOrEqual), New BinaryOperator("EndDate", TransDate, BinaryOperatorType.GreaterOrEqual)))
            If period Is Nothing Then Return True
            Return period.Closed
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsExceedingMaximumOutstandingPaymentAmount As Boolean
        Get
            If CType(SecuritySystem.CurrentUser, ApplicationUser).SubmitExceedMaximumOutstandingPaymentInvoice Then Return False
            If Customer.MaximumOutstandingPaymentAmount < Customer.OutstandingPaymentAmount + PaymentOutstandingAmount Then Return True
            Return False
        End Get
    End Property
    Private Sub CalculatePaymentOutstandingAmount()
        PaymentOutstandingAmount = Total - PaidAmount
    End Sub
    <Action(autoCommit:=False, Caption:="Recalculate Outstanding Status", _
    confirmationMessage:="Are you really want to recalculate these transactions' PaymentOutstandingStatus?", _
    selectiondependencytype:=MethodActionSelectionDependencyType.RequireMultipleObjects, _
     targetobjectscriteria:="PaymentOutstandingStatus = 'Cleared'", _
    ImageName:="Recalculate")>
    Public Sub UpdatePaymentOutstandingStatus()
        Dim tmpTotalOutstandingPayment As Decimal = Total
        Dim xp As New XPCollection(Of SalesPaymentDetail)(PersistentCriteriaEvaluationBehavior.InTransaction, Session, GroupOperator.And(New BinaryOperator("SalesInvoice", Me), New BinaryOperator("Status", TransactionStatus.Submitted)))
        For Each objPaymentDetail In xp
            tmpTotalOutstandingPayment -= objPaymentDetail.Amount
        Next
        If tmpTotalOutstandingPayment <> Total Then
            If tmpTotalOutstandingPayment = 0 Then
                PaymentOutstandingStatus = OutstandingStatus.Cleared
            Else
                PaymentOutstandingStatus = OutstandingStatus.PartiallyPaid
            End If
        Else
            PaymentOutstandingStatus = OutstandingStatus.Full
        End If
    End Sub
    <Action(autoCommit:=False, Caption:="Set as clear", _
     confirmationMessage:="Are you sure want to set these transactions' PaymentOutstandingStatus as cleared?", _
     selectiondependencytype:=MethodActionSelectionDependencyType.RequireMultipleObjects, _
     targetobjectscriteria:="PaymentOutstandingStatus <> 'Cleared'", _
     imageName:="SetAsClear")>
    Public Sub SetAsClear()
        PaymentOutstandingStatus = OutstandingStatus.Cleared
    End Sub
    Public Sub UpdateReturnOutstandingStatus()
        Dim totalBaseUnitQuantity As Decimal = 0
        Dim totalOutstandingBaseUnitQuantity As Decimal = 0
        For Each objDetail In Details
            totalBaseUnitQuantity += objDetail.BaseUnitQuantity
            totalOutstandingBaseUnitQuantity += objDetail.ReturnOutstandingBaseUnitQuantity
        Next
        If totalBaseUnitQuantity <> totalOutstandingBaseUnitQuantity Then
            If totalOutstandingBaseUnitQuantity = 0 Then
                ReturnOutstandingStatus = OutstandingStatus.Cleared
            Else
                ReturnOutstandingStatus = OutstandingStatus.PartiallyPaid
            End If
        Else
            ReturnOutstandingStatus = OutstandingStatus.Full
        End If
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        Dim CheckPriceRange = Not CType(SecuritySystem.CurrentUser, ApplicationUser).SubmitOutOfPriceRangeInvoice
        For Each objDetail In Details
            If CheckPriceRange Then
                Dim itemPrice As ItemPrice = objDetail.Item.GetPrice(TransDate)
                If objDetail.UnitPrice > itemPrice.MaximumPrice OrElse objDetail.UnitPrice < itemPrice.MinimumPrice Then Throw New Exception(String.Format("Line with item {0}'s price out of range", objDetail.Item.Name))
            End If
            objDetail.BalanceSheetInventoryItemDeductTransaction = BalanceSheetService.CreateBalanceSheetInventoryItemDeductTransaction(Inventory, objDetail.Item, TransDate, objDetail.BaseUnitQuantity, BalanceSheetInventoryItemDeductTransactionType.Sale)
        Next
        Customer.OutstandingPaymentAmount += Total
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmp = objDetail.BalanceSheetInventoryItemDeductTransaction
            objDetail.BalanceSheetInventoryItemDeductTransaction = Nothing
            BalanceSheetService.DeleteBalanceSheetInventoryItemDeductTransaction(tmp)
        Next
        Customer.OutstandingPaymentAmount -= Total
    End Sub
End Class