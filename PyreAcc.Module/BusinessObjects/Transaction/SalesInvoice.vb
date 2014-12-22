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
<Appearance("Appearance Default Disabled for SalesPayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, PaymentOutstandingStatus")>
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
    Private _customer As Customer
    Private _inventory As Inventory
    Private _total As Decimal
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
    <Association("SalesInvoice-SalesInvoiceBonusItem"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property BonusItems As XPCollection(Of SalesInvoiceBonusItem)
        Get
            Return GetCollection(Of SalesInvoiceBonusItem)("BonusItems")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <Action(autoCommit:=False, Caption:="Recalculate Outstanding Status", _
    confirmationMessage:="Are you really want to recalculate these transactions' PaymentOutstandingStatus?", _
    selectiondependencytype:=MethodActionSelectionDependencyType.RequireMultipleObjects, _
     targetobjectscriteria:="PaymentOutstandingStatus = 'Cleared'", _
    ImageName:="Recalculate")>
    Public Sub UpdatePaymentOutstandingStatus()
        Dim totalQuantity As Decimal = 0
        Dim totalOutstandingQuantity As Decimal = 0
        For Each objDetail In Details
            totalQuantity += objDetail.Quantity
            totalOutstandingQuantity += objDetail.PaymentOutstandingQuantity
        Next
        If totalQuantity <> totalOutstandingQuantity Then
            If totalOutstandingQuantity = 0 Then
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
        Dim totalQuantity As Decimal = 0
        Dim totalOutstandingQuantity As Decimal = 0
        For Each objDetail In Details
            totalQuantity += objDetail.Quantity
            totalOutstandingQuantity += objDetail.ReturnOutstandingQuantity
        Next
        If totalQuantity <> totalOutstandingQuantity Then
            If totalOutstandingQuantity = 0 Then
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
        For Each objDetail In Details
            objDetail.InventoryItemDeductTransaction = InventoryServices.CreateInventoryDeductTransaction(Inventory, objDetail.Item, TransDate, objDetail.Quantity, InventoryItemDeductTransactionType.Sale)
        Next
        For Each objDetail In BonusItems
            objDetail.InventoryItemDeductTransaction = InventoryServices.CreateInventoryDeductTransaction(Inventory, objDetail.Item, TransDate, objDetail.Quantity, InventoryItemDeductTransactionType.Sale)
        Next
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmp = objDetail.InventoryItemDeductTransaction
            objDetail.InventoryItemDeductTransaction = Nothing
            InventoryServices.DeleteInventoryItemDeductTransaction(tmp)
        Next
        For Each objDetail In BonusItems
            Dim tmp = objDetail.InventoryItemDeductTransaction
            objDetail.InventoryItemDeductTransaction = Nothing
            InventoryServices.DeleteInventoryItemDeductTransaction(tmp)
        Next
    End Sub
End Class