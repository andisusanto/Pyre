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
<RuleCriteria("Rule Criteria for Cancel SalesReturn.PaidAmount = 0", "Cancel", "PaidAmount = 0")>
<RuleCriteria("Rule Criteria for SalesReturn.Total > 0", DefaultContexts.Save, "Total > 0")>
<RuleCriteria("Rule Criteria for SalesReturn.IsPeriodClosed = FALSE", "Submit; CancelSubmit", "IsPeriodClosed = FALSE", "Period already closed")>
<Appearance("Appearance Default Disabled for SalesReturn", enabled:=False, AppearanceItemType:="ViewItem", targetitems:="Total, Discount, GrandTotal, IndonesianWordSays, PaidAmount, PaymentOutstandingAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SalesReturn
    Inherits TransactionBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        TransDate = Now
        Inventory = SysConfig.ReturnItemInventory
    End Sub
    Private _no As String
    Private _referenceNo As String
    Private _transDate As Date
    Private _term As Integer
    Private _dueDate As Date
    Private _customer As Customer
    Private _inventory As Inventory
    Private _total As Decimal
    Private _discountType As DiscountType
    Private _discountValue As Decimal
    Private _discount As Decimal
    Private _grandTotal As Decimal
    Private _indonesianWordSays As String
    Private _salesman As Salesman
    Private _creditNote As CreditNote
    Private _periodCutOffJournal As PeriodCutOffJournal
    <RuleUniqueValue("Rule Unique for SalesReturn.No", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for SalesReturn.No", DefaultContexts.Save)>
    Public Property No As String
        Get
            Return _no
        End Get
        Set(value As String)
            SetPropertyValue("No", _no, value)
        End Set
    End Property
    Public Property ReferenceNo As String
        Get
            Return _referenceNo
        End Get
        Set(value As String)
            SetPropertyValue("ReferenceNo", _referenceNo, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesReturn.TransDate", DefaultContexts.Save)>
    Public Property TransDate As Date
        Get
            Return _transDate
        End Get
        Set(value As Date)
            SetPropertyValue("TransDate", _transDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SalesReturn.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(value As Customer)
            SetPropertyValue("Customer", _customer, value)
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

    <ImmediatePostData(True)>
    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Total", _total, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    Public Property DiscountType As DiscountType
        Get
            Return _discountType
        End Get
        Set(ByVal value As DiscountType)
            SetPropertyValue("DiscountType", _discountType, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <VisibleInListView(False)>
    <ImmediatePostData(True)>
    <RuleRange(0, 100, targetcriteria:="DiscountType = 'ByPercentage'")>
    Public Property DiscountValue As Decimal
        Get
            Return _discountValue
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DiscountValue", _discountValue, value)
            If Not IsLoading Then
                CalculateDiscount()
            End If
        End Set
    End Property
    <ImmediatePostData(True)>
    Public Property Discount As Decimal
        Get
            Return _discount
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("Discount", _discount, value)
            If Not IsLoading Then
                CalculateGrandTotal()
            End If
        End Set
    End Property
    Public Property GrandTotal As Decimal
        Get
            Return _grandTotal
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("GrandTotal", _grandTotal, value)
            If Not IsLoading Then
                IndonesianWordSays = PyreAcc.Module.IndonesianWordSays.GetIndonesianSays(GrandTotal)
            End If
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False)>
    Public Property IndonesianWordSays As String
        Get
            Return _indonesianWordSays
        End Get
        Set(value As String)
            SetPropertyValue("IndonesianWordSays", _indonesianWordSays, value)
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
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property CreditNote As CreditNote
        Get
            Return _creditNote
        End Get
        Set(value As CreditNote)
            SetPropertyValue("CreditNote", _creditNote, value)
        End Set
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Property PeriodCutOffJournal As PeriodCutOffJournal
        Get
            Return _periodCutOffJournal
        End Get
        Set(value As PeriodCutOffJournal)
            SetPropertyValue("PeriodCutOffJournal", _periodCutOffJournal, value)
        End Set
    End Property
    <Association("SalesReturn-SalesReturnDetail"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of SalesReturnDetail)
        Get
            Return GetCollection(Of SalesReturnDetail)("Details")
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return No
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property IsPeriodClosed As Boolean
        Get
            Return TransactionConfig.IsInClosedPeriod(Session, TransDate)
        End Get
    End Property
    Private Sub CalculateDiscount()
        Select Case DiscountType
            Case [Module].DiscountType.ByAmount
                Discount = DiscountValue
            Case [Module].DiscountType.ByPercentage
                Discount = GlobalFunction.Round(Total * DiscountValue / 100)
        End Select
    End Sub
    Private Sub CalculateGrandTotal()
        GrandTotal = Total - Discount
    End Sub
    Protected Overrides Sub OnSubmitted()
        MyBase.OnSubmitted()
        For Each objDetail In Details
            objDetail.PeriodCutOffInventoryItem = PeriodCutOffService.CreatePeriodCutOffInventoryItem(Inventory, objDetail.Item, TransDate, objDetail.BaseUnitQuantity, objDetail.UnitPrice, If(objDetail.Item.HasExpiryDate, objDetail.ExpiryDate, New Date), objDetail.BatchNo)
        Next
        Dim objAutoNo As AutoNo = Session.FindObject(Of AutoNo)(GroupOperator.And(New BinaryOperator("TargetType", "PyreAcc.Module.CreditNote"), New BinaryOperator("IsActive", True)))
        Dim objCreditNote As New CreditNote(Session) With {.ForCustomer = Customer, .TransDate = TransDate, .Amount = Total, .Note = "Create from return transaction with no " & No}
        objCreditNote.No = objAutoNo.GetAutoNo(objCreditNote)
        CreditNote = objCreditNote
    End Sub
    Protected Overrides Sub OnCanceled()
        MyBase.OnCanceled()
        For Each objDetail In Details
            Dim tmpPeriodCutOffInventoryItem = objDetail.PeriodCutOffInventoryItem
            objDetail.PeriodCutOffInventoryItem = Nothing
            PeriodCutOffService.DeletePeriodCutOffInventoryItem(tmpPeriodCutOffInventoryItem)
        Next
        Dim tmp = CreditNote
        CreditNote = Nothing
        tmp.Delete()
    End Sub
End Class