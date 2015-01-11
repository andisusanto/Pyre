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

<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for BalanceSheetInventoryItemDeductTransaction", DefaultContexts.Save, "BalanceSheetInventoryItemDeductTransaction, BalanceSheetInventoryItem")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class BalanceSheetInventoryItemDeductTransactionDetail
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _balanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
    Private _balanceSheetInventoryItem As BalanceSheetInventoryItem
    Private _deductedQuantity As Decimal
    <Association("BalanceSheetInventoryItemDeductTransaction-BalanceSheetInventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransactionDetail.BalanceSheetInventoryItemDeductTransaction", DefaultContexts.Save)>
    Public Property BalanceSheetInventoryItemDeductTransaction As BalanceSheetInventoryItemDeductTransaction
        Get
            Return _balanceSheetInventoryItemDeductTransaction
        End Get
        Set(ByVal value As BalanceSheetInventoryItemDeductTransaction)
            SetPropertyValue("BalanceSheetInventoryItemDeductTransaction", _balanceSheetInventoryItemDeductTransaction, value)
        End Set
    End Property
    <Association("BalanceSheetInventoryItem-BalanceSheetInventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for BalanceSheetInventoryItemDeductTransactionDetail.BalanceSheetInventoryItem", DefaultContexts.Save)>
    Public Property BalanceSheetInventoryItem As BalanceSheetInventoryItem
        Get
            Return _balanceSheetInventoryItem
        End Get
        Set(ByVal value As BalanceSheetInventoryItem)
            SetPropertyValue("BalanceSheetInventoryItem", _balanceSheetInventoryItem, value)
        End Set
    End Property
    Public Property DeductedQuantity As Decimal
        Get
            Return _deductedQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DeductedQuantity", _deductedQuantity, value)
        End Set
    End Property

End Class
