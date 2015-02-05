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
<RuleCriteria("Rule Criteria for PurchaseReturn.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for Cancel PurchaseReturn.DebitNote.UsedAmount = 0", "Cancel", "DebitNote.UsedAmount = 0")>
<Appearance("Appearance Default Disabled for PurchasePayment", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total")>
<Appearance("Appearance for PurchaseReturn.EnableDetails = FALSE", AppearanceItemType:="ViewItem", criteria:="EnableDetails = FALSE", enabled:=False, targetitems:="Details")>
<Appearance("Appearance for PurchaseReturn.Details.Count > 0", AppearanceItemType:="ViewItem", criteria:="@Details.Count > 0", enabled:=False, targetitems:="Supplier")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PurchaseReturn
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
    Private _supplier As Supplier
    Private _total As Decimal
    Private _inventory As Inventory
    Private _debitNote As DebitNote
    <RuleRequiredField("Rule Required for PurchaseReturn.No", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for PurchaseReturn.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(ByVal value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for PurchaseReturn.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <ImmediatePostData(True)>
    <RuleRequiredField("Rule Required for PurchaseReturn.Supplier", DefaultContexts.Save)>
    Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(ByVal value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
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
    <RuleRequiredField("Rule Required for PurchaseReturn.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return _inventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("Inventory", _inventory, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property DebitNote As DebitNote
        Get
            Return _debitNote
        End Get
        Set(value As DebitNote)
            SetPropertyValue("DebitNote", _debitNote, value)
        End Set
    End Property
    <Association("PurchaseReturn-PurchaseReturnDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of PurchaseReturnDetail)
        Get
            Return GetCollection(Of PurchaseReturnDetail)("Details")
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
            Return Supplier IsNot Nothing
        End Get
    End Property
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            If objDetail.PurchaseInvoiceDetail.PurchaseInvoice.Status <> TransactionStatus.Submitted Then Throw New Exception(String.Format("Purchase Invoice with No {0} has not been submitted", objDetail.PurchaseInvoiceDetail.PurchaseInvoice.No))
            If objDetail.PurchaseInvoiceDetail.ReturnOutstandingBaseUnitQuantity < objDetail.BaseUnitQuantity Then Throw New Exception(String.Format("Invalid return quantity. Invalid line : {0}", objDetail.ToString))
            objDetail.PurchaseInvoiceDetail.ReturnedBaseUnitQuantity += objDetail.BaseUnitQuantity
            objDetail.BalanceSheetInventoryDeductionTransaction = BalanceSheetService.CreateBalanceSheetInventoryItemDeductTransaction(objDetail.PurchaseInvoiceDetail.BalanceSheetInventoryItem.Inventory, objDetail.PurchaseInvoiceDetail.BalanceSheetInventoryItem.Item, TransDate, objDetail.BaseUnitQuantity, BalanceSheetInventoryItemDeductTransactionType.Returned)
        Next
        Dim objAutoNo As AutoNo = Session.FindObject(Of AutoNo)(GroupOperator.And(New BinaryOperator("TargetType", "PyreAcc.Module.DebitNote"), New BinaryOperator("IsActive", True)))
        Dim objDebitNote As New DebitNote(Session) With {.FromSupplier = Supplier, .TransDate = TransDate, .Amount = Total, .Note = "Create from return transaction with no " & No}
        objDebitNote.No = objAutoNo.GetAutoNo(objDebitNote)
        DebitNote = objDebitNote
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            objDetail.PurchaseInvoiceDetail.ReturnedBaseUnitQuantity -= objDetail.BaseUnitQuantity
            BalanceSheetService.DeleteBalanceSheetInventoryItemDeductTransaction(objDetail.BalanceSheetInventoryDeductionTransaction)
        Next
        Dim tmp = DebitNote
        DebitNote = Nothing
        tmp.Delete()
    End Sub
End Class
