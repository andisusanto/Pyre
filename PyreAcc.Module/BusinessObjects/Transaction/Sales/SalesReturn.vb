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
<RuleCriteria("Rule Criteria for Cancel SalesReturn.CreditNote.UsedAmount = 0", "Cancel", "CreditNote.UsedAmount = 0")>
<RuleCriteria("Rule Criteria for SalesReturn.Total > 0", DefaultContexts.Save, "Total > 0")>
<Appearance("Appearance Default Disabled for SalesPayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<Appearance("Appearance for SalesReturn.EnableDetails = FALSE", AppearanceItemType:="ViewItem", criteria:="EnableDetails = FALSE", enabled:=False, targetitems:="Details")>
<Appearance("Appearance for SalesReturn.Details.Count > 0", AppearanceItemType:="ViewItem", criteria:="@Details.Count > 0", enabled:=False, targetitems:="Customer")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesReturn
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Today
    End Sub

    Private _no As String
    Private _transDate As Date
    Private _customer As Customer
    Private _total As Decimal
    Private _inventory As Inventory
    Private _creditNote As CreditNote
    <RuleRequiredField("Rule Required for SalesReturn.No", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for SalesReturn.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(ByVal value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesReturn.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for SalesReturn.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(ByVal value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(value As Decimal)
            SetPropertyValue("Total", _total, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesReturn.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property CreditNote As CreditNote
        Get
            Return _creditNote
        End Get
        Set(value As CreditNote)
            SetPropertyValue("CreditNote", _creditNote, value)
        End Set
    End Property
    <Association("SalesReturn-SalesReturnDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesReturnDetail)
        Get
            Return GetCollection(Of SalesReturnDetail)("Details")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <Browsable(False), VisibleInDetailView(False), VisibleInListView(False)>
    Public ReadOnly Property EnableDetails As Boolean
        Get
            Return Customer IsNot Nothing
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            If objDetail.SalesInvoiceDetail.SalesInvoice.Status <> TransactionStatus.Submitted Then Throw New Exception(String.Format("Sales Invoice with No {0} has not been submitted", objDetail.SalesInvoiceDetail.SalesInvoice.No))
            If objDetail.SalesInvoiceDetail.ReturnOutstandingQuantity < objDetail.Quantity Then Throw New Exception(String.Format("Invalid Quantity for submitting Invoice. Invalid line : {0}", objDetail.ToString))
            objDetail.SalesInvoiceDetail.ReturnedQuantity += objDetail.Quantity
        Next
        Dim objAutoNo As AutoNo = Session.FindObject(Of AutoNo)(GroupOperator.And(New BinaryOperator("TargetType", "XuperACC.Module.CreditNote"), New BinaryOperator("IsActive", True)))
        Dim objCreditNote As New CreditNote(Session) With {.ForCustomer = Customer, .TransDate = TransDate, .Amount = Total, .Note = "Create from return transaction with no " & No}
        objCreditNote.No = objAutoNo.GetAutoNo(objCreditNote)
        CreditNote = objCreditNote
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            objDetail.SalesInvoiceDetail.ReturnedQuantity -= objDetail.Quantity
        Next
        Dim tmp = CreditNote
        CreditNote = Nothing
        tmp.Delete()
    End Sub
End Class
