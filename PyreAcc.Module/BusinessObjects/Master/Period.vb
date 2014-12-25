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
Public Class Period
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _code As String
    Private _description As String
    Private _startDate As Date
    Private _endDate As Date
    Private _closed As Boolean

    <RuleUniqueValue("Rule Unique Value for Period.Code", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for Period.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", _code, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Period.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Period.StartDate", DefaultContexts.Save)>
    Public Property StartDate As Date
        Get
            Return _startDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("StartDate", _startDate, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Period.EndDate", DefaultContexts.Save)>
    Public Property EndDate As Date
        Get
            Return _endDate
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("EndDate", _endDate, value)
        End Set
    End Property
    Public Property Closed As Boolean
        Get
            Return _closed
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Closed", _closed, value)
        End Set
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Code
        End Get
    End Property
End Class
