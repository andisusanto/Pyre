Imports System
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Data.Filtering
Imports System.ComponentModel

<CreatableItem(False)>
<NonPersistent()>
<DeferredDeletion(False)>
<DefaultProperty("DefaultDisplay")>
<DefaultClassOptions()> _
Public Class AppBase
    Inherits DevExpress.Persistent.BaseImpl.BaseObject
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
    Private fSysConfig As SystemConfig
    Protected ReadOnly Property SysConfig As SystemConfig
        Get
            If fSysConfig Is Nothing Then fSysConfig = SystemConfig.GetInstance(Session)
            Return fSysConfig
        End Get
    End Property

    Protected Overrides Sub OnSaving()
        If Not TypeOf Session Is NestedUnitOfWork AndAlso Session.IsNewObject(Me) Then
            Using autoNos As New XPCollection(Of AutoNo)(Session, New BinaryOperator("TargetType", Me.GetType().FullName))
                For Each objAutoNo In autoNos
                    SetMemberValue(objAutoNo.TargetProperty, objAutoNo.GetAutoNo(Me))
                Next
            End Using
        End If
        MyBase.OnSaving()
    End Sub
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public Overridable ReadOnly Property DefaultDisplay As String
        Get
            Return Nothing
        End Get
    End Property
End Class
