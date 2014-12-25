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
Public Class Currency
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _code As String
    Private _name As String
    Private _symbol As String

    <RuleUniqueValue("Rule Unique for Currency.Code", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for Currency.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", _code, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Currency.Name", DefaultContexts.Save)>
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Name", _name, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Currency.Symbol", DefaultContexts.Save)>
    Public Property Symbol As String
        Get
            Return _symbol
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Symbol", _symbol, value)
        End Set
    End Property

End Class
