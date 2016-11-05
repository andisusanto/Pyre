Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security
Imports DevExpress.ExpressApp.Security.Strategy

<CreatableItem(False)> _
<DeferredDeletion(False)>
<DefaultClassOptions()>
Public Class ApplicationRoleReport
    Inherits BaseObject

    Public Sub New(ByVal session As Session)
        MyBase.New(session)
        ' This constructor is used when an object is loaded from a persistent storage.
        ' Do not place any code here or place it only when the IsLoading property is false:
        ' if (!IsLoading){
        '   It is now OK to place your initialization code here.
        ' }
        ' or as an alternative, move your initialization code into the AfterConstruction method.
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        ' Place here your initialization code.
    End Sub

    Private _applicationRole As ApplicationRole
    Private _report As Report.Module.Report

    <Association("ApplicationRole-ApplicationRoleReport")>
    Public Property ApplicationRole As ApplicationRole
        Get
            Return _applicationRole
        End Get
        Set(ByVal value As ApplicationRole)
            SetPropertyValue("ApplicationRole", _applicationRole, value)
        End Set
    End Property
    Public Property Report As Report.Module.Report
        Get
            Return _report
        End Get
        Set(ByVal value As ApplicationRole)
            SetPropertyValue("Report", _report, value)
        End Set
    End Property
End Class
