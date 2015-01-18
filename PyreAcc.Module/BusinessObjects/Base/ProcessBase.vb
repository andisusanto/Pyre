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
<Appearance("Appearance for ProcessBase - Save and Delete Action", enabled:=False, Visibility:=DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, AppearanceItemType:="Action", targetitems:="Delete, Save, SaveAndClose, SaveAndNew")>
<Appearance("Appearance for ProcessBase.Status", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="*", criteria:="Status <> 'Waiting'")>
<Appearance("Appearance for ProcessBase.DefaultDisplay", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="User, Start, Finish, Note, Processed, Total, Status")>
<NonPersistent()>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public MustInherit Class ProcessBase
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        User = SecuritySystem.CurrentUserName
    End Sub
    Private _user As String
    Private _start As Date
    Private _finish As Date
    Private _note As String
    Private _processed As Integer
    Private _total As Integer
    Private _status As ProcessStatus
    <Size(50)>
    <NonCloneable()>
    Public Property User As String
        Get
            Return _user
        End Get
        Set(ByVal value As String)
            SetPropertyValue("User", _user, value)
        End Set
    End Property
    <NonCloneable()>
    <Model.ModelDefault("DisplayFormat", "{0:dd MMM yyyy HH:mm:ss}")> _
    <Model.ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")> _
    Public Property Start As Date
        Get
            Return _start
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("Start", _start, value)
        End Set
    End Property
    <NonCloneable()>
    <Model.ModelDefault("DisplayFormat", "{0:dd MMM yyyy HH:mm:ss}")> _
    <Model.ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")> _
    Public Property Finish As Date
        Get
            Return _finish
        End Get
        Set(ByVal value As Date)
            SetPropertyValue("Finish", _finish, value)
        End Set
    End Property
    <NonCloneable()>
    <Size(4000)>
    Public Property Note As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            If Not IsLoading Then
                If value.Length > 4000 Then value = value.Substring(0, 3999)
            End If
            SetPropertyValue("Note", _note, value)
        End Set
    End Property
    <NonCloneable()>
    Public Property Processed As Integer
        Get
            Return _processed
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Processed", _processed, value)
        End Set
    End Property
    <NonCloneable()>
    Public Property Total As Integer
        Get
            Return _total
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("Total", _total, value)
        End Set
    End Property

    <NonCloneable()>
    Public Property Status As ProcessStatus
        Get
            Return _status
        End Get
        Set(ByVal value As ProcessStatus)
            SetPropertyValue("Status", _status, value)
        End Set
    End Property

    Public MustOverride Sub Execute()
End Class
Public Enum ProcessStatus
    Waiting
    Executing
    FinishWithError
    FinishSuccessfully
End Enum