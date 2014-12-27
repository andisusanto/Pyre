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
Imports DevExpress.Persistent.Base.General

<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Account
    Inherits AppBase
    Implements ITreeNode

    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        IsActive = True
    End Sub

    Private _code As String
    Private _description As String
    Private _parent As Account
    Private _isParent As Boolean
    Private _isActive As Boolean
    <RuleUniqueValue("Rule Unique for Account.Code", DefaultContexts.Save)>
    <RuleRequiredField("Rule Required for Account.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", _code, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Account.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", _description, value)
        End Set
    End Property
    <DataSourceCriteria("IsParent = TRUE AND IsActive = TRUE")>
    <Association("Account-Account")>
    Public Property Parent As Account
        Get
            Return _parent
        End Get
        Set(ByVal value As Account)
            SetPropertyValue("Parent", _parent, value)
        End Set
    End Property
    Public Property IsActive As Boolean
        Get
            Return _isActive
        End Get
        Set(value As Boolean)
            SetPropertyValue("IsActive", _isActive, value)
        End Set
    End Property
    <Association("Account-Account")>
    Public ReadOnly Property Children As XPCollection(Of Account)
        Get
            Return GetCollection(Of Account)("Children")
        End Get
    End Property
    Public Property IsParent As Boolean
        Get
            Return _isParent
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("IsParent", _isParent, value)
        End Set
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property ITreeNode_Children As IBindingList Implements ITreeNode.Children
        Get
            Return Children
        End Get
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property Name As String Implements ITreeNode.Name
        Get
            Return Description
        End Get
    End Property

    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property ITreeNode_Parent As ITreeNode Implements ITreeNode.Parent
        Get
            Return Parent
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Code
        End Get
    End Property
End Class
