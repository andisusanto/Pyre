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
Public Class ContactPersonContactInformation
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ContactInformation = New ContactInformation(Session)
    End Sub

    Private _contactPerson As ContactPerson
    Private _contactInformation As ContactInformation

    <Association("ContactPerson-ContactPersonContactInformation")>
    <RuleRequiredField("Rule Required for ContactPersonContactInformation.ContactPerson", DefaultContexts.Save)>
    Public Property ContactPerson As ContactPerson
        Get
            Return _contactPerson
        End Get
        Set(ByVal value As ContactPerson)
            SetPropertyValue("ContactPerson", _contactPerson, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ContactPersonContactInformation.ContactInformation", DefaultContexts.Save)>
    Public Property ContactInformation As ContactInformation
        Get
            Return _contactInformation
        End Get
        Set(ByVal value As ContactInformation)
            SetPropertyValue("ContactInformation", _contactInformation, value)
        End Set
    End Property

End Class
