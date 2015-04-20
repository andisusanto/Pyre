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

<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class AccountLinkingConfig
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private fPurchaseInvoiceAccount As Account
    Private fSalesInvoiceAccount As Account
    Private fSalesAccount As Account
    Private fPurchaseAccount As Account
    Private fInventoryAccount As Account
    Private fCostOfSalesAccount As Account
    Private fDebitNoteAccount As Account
    Private fCreditNoteAccount As Account

    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.PurchaseInvoiceAccount", DefaultContexts.Save)>
    Public Property PurchaseInvoiceAccount As Account
        Get
            Return fPurchaseInvoiceAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("PurchaseInvoiceAccount", fPurchaseInvoiceAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.SalesInvoiceAccount", DefaultContexts.Save)>
    Public Property SalesInvoiceAccount As Account
        Get
            Return fSalesInvoiceAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("SalesInvoiceAccount", fSalesInvoiceAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.SalesAccount", DefaultContexts.Save)>
    Public Property SalesAccount As Account
        Get
            Return fSalesAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("SalesAccount", fSalesAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.PurchaseAccount", DefaultContexts.Save)>
    Public Property PurchaseAccount As Account
        Get
            Return fPurchaseAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("PurchaseAccount", fPurchaseAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.InventoryAccount", DefaultContexts.Save)>
    Public Property InventoryAccount As Account
        Get
            Return fInventoryAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("InventoryAccount", fInventoryAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.CostOfSalesAccount", DefaultContexts.Save)>
    Public Property CostOfSalesAccount As Account
        Get
            Return fCostOfSalesAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("CostOfSalesAccount", fCostOfSalesAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.DebitNoteAccount", DefaultContexts.Save)>
    Public Property DebitNoteAccount As Account
        Get
            Return fDebitNoteAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("DebitNoteAccount", fDebitNoteAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.CreditNoteAccount", DefaultContexts.Save)>
    Public Property CreditNoteAccount As Account
        Get
            Return fCreditNoteAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("CreditNoteAccount", fCreditNoteAccount, value)
        End Set
    End Property

End Class
