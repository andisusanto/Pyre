Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

<CreatableItem(False)> _
<DefaultProperty("Description")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class AutoNo
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
    Public Const AUTO_NO As String = "--Auto--"
    Private fTargetType As String
    Private fTargetProperty As String
    Private fDescription As String
    Private fPrefix As String
    Private fSuffix As String
    Private fLookUpCriteria As String
    Private fStartNumber As Integer
    Private fIncrement As Byte
    Private fTotalDigit As Byte
    Private fIsActive As Boolean

    <Size(80)>
    <RuleRequiredField("Rule Required for AutoNo.TargetType", DefaultContexts.Save)>
    Public Property TargetType As String
        Get
            Return fTargetType
        End Get
        Set(ByVal value As String)
            SetPropertyValue("TargetType", fTargetType, value)
        End Set
    End Property

    <Size(50)>
    <RuleRequiredField("Rule Required for AutoNo.TargetProperty", DefaultContexts.Save)>
    Public Property TargetProperty As String
        Get
            Return fTargetProperty
        End Get
        Set(ByVal value As String)
            SetPropertyValue("TargetProperty", fTargetProperty, value)
        End Set
    End Property

    <Size(255)>
    <RuleRequiredField("Rule Required for AutoNo.Description", DefaultContexts.Save)>
    Public Property Description As String
        Get
            Return fDescription
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Description", fDescription, value)
        End Set
    End Property

    <Size(1023)>
    Public Property Prefix As String
        Get
            Return fPrefix
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Prefix", fPrefix, value)
        End Set
    End Property

    <Size(1023)>
    Public Property Suffix As String
        Get
            Return fSuffix
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Suffix", fSuffix, value)
        End Set
    End Property

    <Size(1023)>
    Public Property LookUpCriteria As String
        Get
            Return fLookUpCriteria
        End Get
        Set(ByVal value As String)
            SetPropertyValue("LookUpCriteria", fLookUpCriteria, value)
        End Set
    End Property

    Public Property StartNumber As Integer
        Get
            Return fStartNumber
        End Get
        Set(ByVal value As Integer)
            SetPropertyValue("StartNumber", fStartNumber, value)
        End Set
    End Property

    <DefaultValue(1)>
    Public Property Increment As Byte
        Get
            Return fIncrement
        End Get
        Set(ByVal value As Byte)
            SetPropertyValue("Increment", fIncrement, value)
        End Set
    End Property

    <DefaultValue(1)>
    <RuleRange("Rule Range for AutoNo.TotalDigit", DefaultContexts.Save, 1, 10)>
    Public Property TotalDigit As Byte
        Get
            Return fTotalDigit
        End Get
        Set(ByVal value As Byte)
            SetPropertyValue("TotalDigit", fTotalDigit, value)
        End Set
    End Property

    <DefaultValue(True)>
    Public Property IsActive As Boolean
        Get
            Return fIsActive
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("IsActive", fIsActive, value)
        End Set
    End Property

    <Association("AutoNo-AutoNoDetail", GetType(AutoNoDetail)), Aggregated()>
    Public ReadOnly Property Details As XPCollection(Of AutoNoDetail)
        Get
            Return GetCollection(Of AutoNoDetail)("Details")
        End Get
    End Property

    Public Function GetAutoNo(ByVal obj As Object) As String
        Dim lastNumber As Integer = 0
        Dim strPrefix As String = ""
        Dim strSuffix As String = ""
        Dim strLookUpCriteria As String = ""
        If Prefix <> "" Then strPrefix = GlobalFunction.ExecuteRunTimeFormula(Prefix, obj)
        If Suffix <> "" Then strSuffix = GlobalFunction.ExecuteRunTimeFormula(Suffix, obj)
        If LookUpCriteria <> "" Then strLookUpCriteria = GlobalFunction.ExecuteRunTimeFormula(LookUpCriteria, obj)
        Details.Filter = New BinaryOperator("LookUpCriteria", strLookUpCriteria)
        Dim autoNoDetail As AutoNoDetail
        If Details.Count = 0 Then
            autoNoDetail = New AutoNoDetail(Session) With {.AutoNo = Me, .LookUpCriteria = strLookUpCriteria, .LastNumber = StartNumber}
        Else
            autoNoDetail = Details(0)
        End If
        lastNumber = autoNoDetail.LastNumber
        autoNoDetail.LastNumber += Increment
        Details.Filter = Nothing
        Dim strNo As String = "000000000" & lastNumber
        strNo = Right(strNo, TotalDigit)
        Return strPrefix + strNo + strSuffix
    End Function
End Class
