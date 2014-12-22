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
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class CustomerContactInformation
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ContactInformation = New ContactInformation(Session)
    End Sub
    Private _customer As Customer
    Private _contactInformation As ContactInformation
    <Association("Customer-CustomerContactInformation")>
    <RuleRequiredField("Rule Required for CustomerContactInformation.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(ByVal value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for CustomerContactInformation.ContactInformation", DefaultContexts.Save)>
    Public Property ContactInformation As ContactInformation
        Get
            Return _contactInformation
        End Get
        Set(ByVal value As ContactInformation)
            SetPropertyValue("ContactInformation", _contactInformation, value)
        End Set
    End Property

End Class
