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
Public Class InventoryAccountLinkingConfig
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private fAccountLinkingConfig As AccountLinkingConfig
    Private fInventory As Inventory
    Private fAccount As Account
    <RuleRequiredField("Rule Required for InventoryAccountLinkingConfig.AccountLinkingConfig", DefaultContexts.Save)>
    Public Property AccountLinkingConfig As AccountLinkingConfig
        Get
            Return fAccountLinkingConfig
        End Get
        Set(ByVal value As AccountLinkingConfig)
            SetPropertyValue("AccountLinkingConfig", fAccountLinkingConfig, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for InventoryAccountLinkingConfig.Inventory", DefaultContexts.Save)>
    Public Property Inventory As Inventory
        Get
            Return fInventory
        End Get
        Set(ByVal value As Inventory)
            SetPropertyValue("Inventory", fInventory, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = False AND IsActive = True")>
    <RuleRequiredField("Rule Required for InventoryAccountLinkingConfig.Account", DefaultContexts.Save)>
    Public Property Account As Account
        Get
            Return fAccount
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("Account", fAccount, value)
        End Set
    End Property

End Class
