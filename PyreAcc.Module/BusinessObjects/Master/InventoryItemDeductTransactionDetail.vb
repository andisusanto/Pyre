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
<CreatableItem(False)>
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for InventoryItemDeductTransactionDetail", DefaultContexts.Save, "InventoryItemDeductTransaction, InventoryItem")>
<Appearance("Appearance Default Disabled for InventoryItemDeductDetail", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="*")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class InventoryItemDeductTransactionDetail
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _inventoryItemDeductTransaction As InventoryItemDeductTransaction
    Private _inventoryItem As InventoryItem
    Private _deductedQuantity As Integer
    <Association("InventoryItemDeductTransaction-InventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for InventoryItemDeductTransactionDetail.InventoryItemDeductTransaction", DefaultContexts.Save)>
    Public Property InventoryItemDeductTransaction As InventoryItemDeductTransaction
        Get
            Return _inventoryItemDeductTransaction
        End Get
        Set(value As InventoryItemDeductTransaction)
            SetPropertyValue("InventoryItemDeductTransaction", _inventoryItemDeductTransaction, value)
        End Set
    End Property
    <Association("InventoryItem-InventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for InventoryItemDeductDetail.InventoryItem", DefaultContexts.Save)>
    Public Property InventoryItem As InventoryItem
        Get
            Return _inventoryItem
        End Get
        Set(ByVal value As InventoryItem)
            SetPropertyValue("InventoryItem", _inventoryItem, value)
        End Set
    End Property
    Public Property DeductedQuantity As Integer
        Get
            Return _deductedQuantity
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("DeductedQuantity", _deductedQuantity, value)
        End Set
    End Property

End Class
