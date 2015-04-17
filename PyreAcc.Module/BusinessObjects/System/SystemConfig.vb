Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

<CreatableItem(False)>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class SystemConfig
    Inherits AppBase
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
    Private fDefaultUnit As Unit
    Private fReportPath As String
    Private fReturnItemInventory As Inventory

    Public Property DefaultUnit As Unit
        Get
            Return fDefaultUnit
        End Get
        Set(value As Unit)
            SetPropertyValue("DefaultUnit", fDefaultUnit, value)
        End Set
    End Property
    <Size(150)>
    Public Property ReportPath As String
        Get
            Return fReportPath
        End Get
        Set(value As String)
            SetPropertyValue("ReportPath", fReportPath, value)
        End Set
    End Property
    Public Property ReturnItemInventory As Inventory
        Get
            Return fReturnItemInventory
        End Get
        Set(value As Inventory)
            SetPropertyValue("ReturnItemInventory", fReturnItemInventory, value)
        End Set
    End Property

    Public Shared Function GetInstance(ByVal prmSession As Session) As SystemConfig
        Return prmSession.FindObject(Of SystemConfig)(Nothing)
    End Function
End Class
