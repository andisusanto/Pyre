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
Public Class AutoNoDetail
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
    Private fAutoNo As AutoNo
    Private fLookUpCriteria As String
    Private fLastNumber As Integer

    <Association("AutoNo-AutoNoDetail", GetType(AutoNo)), Aggregated()>
    Public Property AutoNo As AutoNo
        Get
            Return fAutoNo
        End Get
        Set(ByVal value As AutoNo)
            SetPropertyValue("AutoNo", fAutoNo, value)
        End Set
    End Property

    Public Property LookUpCriteria As String
        Get
            Return fLookUpCriteria
        End Get
        Set(ByVal value As String)
            SetPropertyValue("LookUpCriteria", fLookUpCriteria, value)
        End Set
    End Property

    Public Property LastNumber As Integer
        Get
            Return fLastNumber
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("LastNumber", fLastNumber, value)
        End Set
    End Property
End Class
