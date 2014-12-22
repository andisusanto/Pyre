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
<DefaultClassOptions()> _
Public Class ApplicationRole
    Inherits SecuritySystemRole

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

    Private _submitExceedMaximumOutstandingPaymentInvoice As Boolean
    Private _submitOutOfPriceRangeInvoice As Boolean

    Public Property SubmitExceedMaximumOutstandingPaymentInvoice As Boolean
        Get
            Return _submitExceedMaximumOutstandingPaymentInvoice
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("SubmitExceedMaximumOutstandingPaymentInvoice", _submitExceedMaximumOutstandingPaymentInvoice, value)
        End Set
    End Property
    Public Property SubmitOutOfPriceRangeInvoice As Boolean
        Get
            Return _submitOutOfPriceRangeInvoice
        End Get
        Set(ByVal value As Boolean)
            SetPropertyValue("SubmitOutOfPriceRangeInvoice", _submitOutOfPriceRangeInvoice, value)
        End Set
    End Property

End Class
