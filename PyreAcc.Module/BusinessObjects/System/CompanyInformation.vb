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
Public Class CompanyInformation
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
    Private fName As String
    Private fAddress As String
    Private fEmail As String
    Private fTelephone As String
    Private fFacsimile As String
    Private fWebSite As String

    <Size(80)>
    Public Property Name As String
        Get
            Return fName
        End Get
        Set(value As String)
            SetPropertyValue("Name", fName, value)
        End Set
    End Property

    <Size(255)>
    Public Property Address As String
        Get
            Return fAddress
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Address", fAddress, value)
        End Set
    End Property
    <Size(80)>
    Public Property Email As String
        Get
            Return fEmail
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Email", fEmail, value)
        End Set
    End Property
    <Size(50)>
    Public Property Telephone As String
        Get
            Return fTelephone
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Telephone", fTelephone, value)
        End Set
    End Property
    <Size(50)>
    Public Property Facsimile As String
        Get
            Return fFacsimile
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Facsimile", fFacsimile, value)
        End Set
    End Property
    <Size(80)>
    Public Property WebSite As String
        Get
            Return fWebSite
        End Get
        Set(ByVal value As String)
            SetPropertyValue("WebSite", fWebSite, value)
        End Set
    End Property

    Public Shared Function GetInstance(ByVal prmSession As Session) As CompanyInformation
        Dim tmp As CompanyInformation = prmSession.FindObject(Of CompanyInformation)(Nothing)
        If tmp Is Nothing Then
            tmp = New CompanyInformation(prmSession)
            tmp.Save()
        End If
        Return tmp
    End Function
End Class
