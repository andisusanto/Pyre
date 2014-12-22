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
Public Class CustomerContactPerson
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ContactPerson = New ContactPerson(Session)
    End Sub
    Private _customer As Customer
    Private _note As String
    Private _contactPerson As ContactPerson
    <RuleRequiredField("Rule Required for CustomerContactPerson.Customer", DefaultContexts.Save)>
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(ByVal value As Customer)
            SetPropertyValue("Customer", _customer, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for CustomerContactPerson.Note", DefaultContexts.Save)>
    Public Property Note As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Note", _note, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for CustomerContactPerson.ContactPerson", DefaultContexts.Save)>
    Public Property ContactPerson As ContactPerson
        Get
            Return _contactPerson
        End Get
        Set(ByVal value As ContactPerson)
            SetPropertyValue("ContactPerson", _contactPerson, value)
        End Set
    End Property

End Class
