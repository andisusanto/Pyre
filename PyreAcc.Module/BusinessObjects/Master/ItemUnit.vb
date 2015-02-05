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
<CreatableItem(False)> _
<RuleCombinationOfPropertiesIsUnique("Rule Combination Unique for ItemUnit", DefaultContexts.Save, "Item, Unit")>
<RuleCriteria("Rule Criteria for ItemUnit.ConversionRate > 0", DefaultContexts.Save, "ConversionRate > 0")>
<RuleCriteria("Rule Criteria for ItemUnit.Unit <> @Item.BaseUnit", DefaultContexts.Save, "Unit <> @Item.BaseUnit")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ItemUnit
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private fItem As Item
    Private fUnit As Unit
    Private fConversionRate As Double
    <Association("Item-ItemUnit")>
    <RuleRequiredField("Rule Required for ItemUnit.Item", DefaultContexts.Save)>
    Public Property Item As Item
        Get
            Return fItem
        End Get
        Set(ByVal value As Item)
            SetPropertyValue("Item", fItem, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ItemUnit.Unit", DefaultContexts.Save)>
    Public Property Unit As Unit
        Get
            Return fUnit
        End Get
        Set(ByVal value As Unit)
            SetPropertyValue("Unit", fUnit, value)
        End Set
    End Property
    Public Property ConversionRate As Double
        Get
            Return fConversionRate
        End Get
        Set(ByVal value As Double)
            SetPropertyValue("ConversionRate", fConversionRate, value)
        End Set
    End Property

End Class
