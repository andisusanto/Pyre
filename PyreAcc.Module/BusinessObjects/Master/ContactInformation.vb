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
Public Class ContactInformation
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _contactType As ContactType
    Private _contactNo As String
    <RuleRequiredField("Rule Required for ContactInformation.ContactType", DefaultContexts.Save)>
    Public Property ContactType As ContactType
        Get
            Return _contactType
        End Get
        Set(value As ContactType)
            SetPropertyValue("ContactType", _contactType, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for ContactInformation.ContactNo", DefaultContexts.Save)>
    Public Property ContactNo As String
        Get
            Return _contactNo
        End Get
        Set(value As String)
            SetPropertyValue("ContactNo", _contactNo, value)
        End Set
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            If ContactType Is Nothing Then Return Nothing
            Return ContactType.Description & " - " & ContactNo
        End Get
    End Property
End Class