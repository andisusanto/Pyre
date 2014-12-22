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

<CreatableItem(False)> _
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Category
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        Active = True
    End Sub
    Private _code As String
    Private _description As String
    Private _active As Boolean
    <RuleRequiredField("Rule Required for Category.Code", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for Category.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", _code, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Category.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return _active
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Active", _active, value)
        End Set
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Description
        End Get
    End Property
End Class
