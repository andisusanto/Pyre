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
<CreatableItem(False)>
<DeferredDeletion(False)>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for SalesPaymentCreditNote", DefaultContexts.Save, "SalesPayment, CreditNote")>
<RuleCriteria("Rule Criteria for SalesPaymentCreditNote.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<DefaultClassOptions()> _
Public Class SalesPaymentCreditNote
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private fSalesPayment As SalesPayment
    Private fCreditNote As CreditNote
    Private fAmount As Decimal
    <Association("SalesPayment-SalesPaymentCreditNote")>
    <RuleRequiredField("Rule Required for SalesPaymentCreditNote.SalesPayment", DefaultContexts.Save)>
    Public Property SalesPayment As SalesPayment
        Get
            Return fSalesPayment
        End Get
        Set(ByVal value As SalesPayment)
            Dim oldValue = SalesPayment
            SetPropertyValue("SalesPayment", fSalesPayment, value)
            If Not IsLoading Then
                If oldValue IsNot Nothing Then oldValue.CreditNoteAmount -= Amount
                If SalesPayment IsNot Nothing Then SalesPayment.CreditNoteAmount += Amount
            End If
        End Set
    End Property
    <DataSourceProperty("SalesPayment.Supplier.CreditNotes")>
    <DataSourceCriteria("RemainingAmount > 0")>
    <RuleRequiredField("Rule Required for SalesPaymentCreditNote.CreditNote", DefaultContexts.Save)>
    Public Property CreditNote As CreditNote
        Get
            Return fCreditNote
        End Get
        Set(ByVal value As CreditNote)
            SetPropertyValue("CreditNote", fCreditNote, value)
            If Not IsLoading Then
                Amount = CreditNote.RemainingAmount
            End If
        End Set
    End Property
    Public Property Amount As Decimal
        Get
            Return fAmount
        End Get
        Set(ByVal value As Decimal)
            Dim oldValue = Amount
            SetPropertyValue("Amount", fAmount, value)
            If Not IsLoading Then
                If SalesPayment IsNot Nothing Then
                    SalesPayment.CreditNoteAmount -= oldValue
                    SalesPayment.CreditNoteAmount += Amount
                End If
            End If
        End Set
    End Property

End Class
