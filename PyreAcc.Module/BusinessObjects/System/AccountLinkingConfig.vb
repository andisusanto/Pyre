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
    Private fPurchaseAccount As Account
    Private fSalesAccount As Account
    Private fPurchaseDiscountAccount As Account
    Private fSalesDiscountAccount As Account
    Private fInventoryAccount As Account
    Private fCostOfSalesAccount As Account
    Private fDebitNoteAccount As Account
    Private fCreditNoteAccount As Account

    Private fRoundingPlusAccount As Account
    Private fRoundingMinusAccount As Account

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
    <RuleRequiredField("Rule Required for AccountLinkingConfig.PurchaseDiscountAccount", DefaultContexts.Save)>
    Public Property PurchaseDiscountAccount As Account
        Get
            Return fPurchaseDiscountAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("PurchaseDiscountAccount", fPurchaseDiscountAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.SalesDiscountAccount", DefaultContexts.Save)>
    Public Property SalesDiscountAccount As Account
        Get
            Return fSalesDiscountAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("SalesDiscountAccount", fSalesDiscountAccount, value)
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
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.RoundingPlusAccount", DefaultContexts.Save)>
    Public Property RoundingPlusAccount As Account
        Get
            Return fRoundingPlusAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("RoundingPlusAccount", fRoundingPlusAccount, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for AccountLinkingConfig.RoundingMinusAccount", DefaultContexts.Save)>
    Public Property RoundingMinusAccount As Account
        Get
            Return fRoundingMinusAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("RoundingMinusAccount", fRoundingMinusAccount, value)
        End Set
    End Property
    <Association("AccountLinkingConfig-InventoryAccountLinkingConfig"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property InventoryAccountLinkingConfigs As XPCollection(Of InventoryAccountLinkingConfig)
        Get
            Return GetCollection(Of InventoryAccountLinkingConfig)("InventoryAccountLinkingConfigs")
        End Get
    End Property

    Public Function GetInventoryAccountLinking(ByVal Inventory As Inventory) As Account
        InventoryAccountLinkingConfigs.Filter = New BinaryOperator("Inventory", Inventory)
        If InventoryAccountLinkingConfigs.Count > 0 Then Return InventoryAccountLinkingConfigs(0).Account
        Return Nothing
    End Function

    Public Shared Function GetInstance(ByVal Session As Session) As AccountLinkingConfig
        Return Session.FindObject(Of AccountLinkingConfig)(Nothing)
    End Function
End Class
