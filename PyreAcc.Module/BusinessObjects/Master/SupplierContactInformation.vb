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
Public Class SupplierContactInformation
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ContactInformation = New ContactInformation(Session)
    End Sub
    Private _supplier As Supplier
    Private _contactInformation As ContactInformation
    <Association("Supplier-SupplierContactInformation")>
    <RuleRequiredField("Rule Required for SupplierContactInformation.Supplier", DefaultContexts.Save)>
    Public Property Supplier As Supplier
        Get
            Return _supplier
        End Get
        Set(ByVal value As Supplier)
            SetPropertyValue("Supplier", _supplier, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for SupplierContactInformation.ContactInformation", DefaultContexts.Save)>
    Public Property ContactInformation As ContactInformation
        Get
            Return _contactInformation
        End Get
        Set(ByVal value As ContactInformation)
            SetPropertyValue("ContactInformation", _contactInformation, value)
        End Set
    End Property

End Class
