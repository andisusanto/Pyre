Imports System
Imports System.ComponentModel

Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security
Imports System.Security
Imports DevExpress.ExpressApp.Security.Strategy

<CreatableItem(False)> _
<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class ApplicationUser
    Inherits SecuritySystemUser

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
        IsActive = True
        ' Place here your initialization code.
    End Sub
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SubmitExceedMaximumOutstandingPaymentInvoice As Boolean
        Get
            For Each objRole In Roles
                If CType(objRole, ApplicationRole).SubmitExceedMaximumOutstandingPaymentInvoice Then Return True
            Next
            Return False
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SubmitOutOfPriceRangeInvoice As Boolean
        Get
            For Each objRole In Roles
                If CType(objRole, ApplicationRole).SubmitOutOfPriceRangeInvoice Then Return True
            Next
            Return False
        End Get
    End Property
    <VisibleInDetailView(False), VisibleInListView(False), Browsable(False)>
    Public ReadOnly Property SubmitOverDueDateSalesInvoice As Boolean
        Get
            For Each objRole In Roles
                If CType(objRole, ApplicationRole).SubmitOverDueDateSalesInvoice Then Return True
            Next
            Return False
        End Get
    End Property
End Class
