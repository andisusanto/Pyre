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
<Appearance("Appearance Default Disable for Customer", enabled:=False, appearanceitemtype:="ViewItem", targetitems:="OutstandingPaymentAmount")>
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class Customer
    Inherits AppBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)

    End Sub

    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()
        Active = True
    End Sub

    Private fCode As String
    Private fName As String
    Private fLocation As Location
    Private fActive As Boolean
    Private fMaximumOutstandingPaymentAmount As Decimal
    Private fOutstandingPaymentAmount As Decimal

    Private fSunday As Boolean
    Private fMonday As Boolean
    Private fTuesday As Boolean
    Private fWednesday As Boolean
    Private fThursday As Boolean
    Private fFriday As Boolean
    Private fSaturday As Boolean

    <RuleRequiredField("Rule Required for Customer.Code", DefaultContexts.Save)>
    <RuleUniqueValue("Rule Unique for Customer.Code", DefaultContexts.Save)>
    Public Property Code As String
        Get
            Return fCode
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Code", fCode, value)
        End Set
    End Property
    <RuleRequiredField("Rule Required for Customer.Name", DefaultContexts.Save)>
    Public Property Name As String
        Get
            Return fName
        End Get
        Set(ByVal value As String)
            SetPropertyValue("Name", fName, value)
        End Set
    End Property
    <DataSourceCriteria("Active = TRUE")>
    Public Property Location As Location
        Get
            Return fLocation
        End Get
        Set(value As Location)
            SetPropertyValue("Location", fLocation, value)
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return fActive
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Active", fActive, value)
        End Set
    End Property
    Public Property MaximumOutstandingPaymentAmount As Decimal
        Get
            Return fMaximumOutstandingPaymentAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("MaximumOutstandingPaymentAmount", fMaximumOutstandingPaymentAmount, value)
        End Set
    End Property
    Public Property OutstandingPaymentAmount As Decimal
        Get
            Return fOutstandingPaymentAmount
        End Get
        Set(value As Decimal)
            SetPropertyValue("OutstandingPaymentAmount", fOutstandingPaymentAmount, value)
        End Set
    End Property

    Public Property Sunday As Boolean
        Get
            Return fSunday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Sunday", fSunday, value)
        End Set
    End Property
    Public Property Monday As Boolean
        Get
            Return fMonday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Monday", fMonday, value)
        End Set
    End Property
    Public Property Tuesday As Boolean
        Get
            Return fTuesday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Tuesday", fTuesday, value)
        End Set
    End Property
    Public Property Wednesday As Boolean
        Get
            Return fWednesday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Wednesday", fWednesday, value)
        End Set
    End Property
    Public Property Thursday As Boolean
        Get
            Return fThursday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Thursday", fThursday, value)
        End Set
    End Property
    Public Property Friday As Boolean
        Get
            Return fFriday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Friday", fFriday, value)
        End Set
    End Property
    Public Property Saturday As Boolean
        Get
            Return fSaturday
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("Saturday", fSaturday, value)
        End Set
    End Property


    <Association("Customer-CreditNote")>
    Public ReadOnly Property CreditNotes As XPCollection(Of CreditNote)
        Get
            Return GetCollection(Of CreditNote)("CreditNotes")
        End Get
    End Property
    <Association("Customer-CustomerContactInformation"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property Contacts As XPCollection(Of CustomerContactInformation)
        Get
            Return GetCollection(Of CustomerContactInformation)("Contacts")
        End Get
    End Property

    <Association("Customer-CustomerContactPerson"), DevExpress.Xpo.Aggregated()>
    Public ReadOnly Property ContactPersons As XPCollection(Of CustomerContactPerson)
        Get
            Return GetCollection(Of CustomerContactPerson)("ContactPersons")
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultDisplay As String
        Get
            Return Name
        End Get
    End Property
End Class
