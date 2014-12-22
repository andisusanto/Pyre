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
Public Class ContactPerson
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _name As String
    Private _positionTitle As String

    <RuleRequiredField("Rule Required for ContactPerson.Name", DefaultContexts.Save)>
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Name", _name, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ContactPerson.PositionTitle", DefaultContexts.Save)>
    Public Property PositionTitle As String
        Get
            Return _positionTitle
        End Get
        Set(ByVal value As String)
            SetPropertyValue("PositionTitle", _positionTitle, value)
        End Set
    End Property

    <Association("ContactPerson-ContactPersonContactInformation"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Contacts As XPCollection(Of ContactPersonContactInformation)
        Get
            Return GetCollection(Of ContactPersonContactInformation)("Contacts")
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Name
        End Get
    End Property
End Class
