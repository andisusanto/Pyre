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
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PurchasePaymentDebitNote", DefaultContexts.Save, "PurchasePayment, DebitNote")>
<RuleCriteria("Rule Criteria for PurchasePaymentDebitNote.Amount > 0", DefaultContexts.Save, "Amount > 0")>
<DefaultClassOptions()> _
Public Class PurchasePaymentDebitNote
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private fPurchasePayment As PurchasePayment
    Private fDebitNote As DebitNote
    Private fAmount As Decimal
    <Association("PurchasePayment-PurchasePaymentDebitNote")>
    <RuleRequiredField("Rule Required for PurchasePaymentDebitNote.PurchasePayment", DefaultContexts.Save)>
    Public Property PurchasePayment As PurchasePayment
        Get
            Return fPurchasePayment
        End Get
        Set(ByVal value As PurchasePayment)
            Dim oldValue = PurchasePayment
            SetPropertyValue("PurchasePayment", fPurchasePayment, value)
            If Not IsLoading Then
                If oldValue IsNot Nothing Then oldValue.DebitNoteAmount -= Amount
                If PurchasePayment IsNot Nothing Then PurchasePayment.DebitNoteAmount += Amount
            End If
        End Set
    End Property
    <DataSourceProperty("PurchasePayment.Supplier.DebitNotes")>
    <DataSourceCriteria("RemainingAmount > 0")>
    <RuleRequiredField("Rule Required for PurchasePaymentDebitNote.DebitNote", DefaultContexts.Save)>
    Public Property DebitNote As DebitNote
        Get
            Return fDebitNote
        End Get
        Set(ByVal value As DebitNote)
            SetPropertyValue("DebitNote", fDebitNote, value)
            If Not IsLoading Then
                Amount = DebitNote.RemainingAmount
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
                If PurchasePayment IsNot Nothing Then
                    PurchasePayment.DebitNoteAmount -= oldValue
                    PurchasePayment.DebitNoteAmount += Amount
                End If
            End If
        End Set
    End Property

End Class
