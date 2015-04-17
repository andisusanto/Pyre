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

<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for PeriodCutOffInventoryItemDeductTransaction", DefaultContexts.Save, "PeriodCutOffInventoryItemDeductTransaction, PeriodCutOffInventoryItem")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class PeriodCutOffInventoryItemDeductTransactionDetail
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _periodCutOffInventoryItemDeductTransaction As PeriodCutOffInventoryItemDeductTransaction
    Private _periodCutOffInventoryItem As PeriodCutOffInventoryItem
    Private _deductedBaseUnitQuantity As Decimal
    <Association("PeriodCutOffInventoryItemDeductTransaction-PeriodCutOffInventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransactionDetail.PeriodCutOffInventoryItemDeductTransaction", DefaultContexts.Save)>
    Public Property PeriodCutOffInventoryItemDeductTransaction As PeriodCutOffInventoryItemDeductTransaction
        Get
            Return _periodCutOffInventoryItemDeductTransaction
        End Get
        Set(ByVal value As PeriodCutOffInventoryItemDeductTransaction)
            SetPropertyValue("PeriodCutOffInventoryItemDeductTransaction", _periodCutOffInventoryItemDeductTransaction, value)
        End Set
    End Property
    <Association("PeriodCutOffInventoryItem-PeriodCutOffInventoryItemDeductTransactionDetail")>
    <RuleRequiredField("Rule Required for PeriodCutOffInventoryItemDeductTransactionDetail.PeriodCutOffInventoryItem", DefaultContexts.Save)>
    Public Property PeriodCutOffInventoryItem As PeriodCutOffInventoryItem
        Get
            Return _periodCutOffInventoryItem
        End Get
        Set(ByVal value As PeriodCutOffInventoryItem)
            SetPropertyValue("PeriodCutOffInventoryItem", _periodCutOffInventoryItem, value)
        End Set
    End Property
    Public Property DeductedBaseUnitQuantity As Decimal
        Get
            Return _deductedBaseUnitQuantity
        End Get
        Set(ByVal value As Decimal)
            SetPropertyValue("DeductedBaseUnitQuantity", _deductedBaseUnitQuantity, value)
        End Set
    End Property

End Class
