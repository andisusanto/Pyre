Imports System
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.ExpressApp.Xpo
<CreatableItem(False)> _
<Appearance("Appearance for TransactionBase.Default", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="Status, EntryDateTime, SubmitDateTime, CancelDateTime,EnteredBy, SubmittedBy, CanceledBy, SubmitNote, CancelNote")>
<Appearance("Appearance for TransactionBase.Status = Submitted", AppearanceItemType:="ViewItem", enabled:=False, targetitems:="*", criteria:="Status = 'Submitted'")> _
<RuleCriteria("RuleCriteria for TransactionBase.DeletePolicy", DefaultContexts.Delete, "Status='Entered' OR Status='Cancelled'", "Only transaction with Entered status can be deleted")> _
<NonPersistent()> _
<DeferredDeletion(True)> _
<DefaultClassOptions()> _
Public Class TransactionBase
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
    Private _status As TransactionStatus
    Private _entryDateTime As DateTime
    Private _submitDateTime As DateTime
    Private _cancelDateTime As DateTime
    Private _enteredBy As String
    Private _submittedBy As String
    Private _canceledBy As String
    Private _submitNote As String
    Private _cancelNote As String

    Public Property Status() As TransactionStatus
        Get
            Return _status
        End Get
        Set(ByVal value As TransactionStatus)
            SetPropertyValue("Status", _status, value)
        End Set
    End Property
    <Model.ModelDefault("DisplayFormat", "{0:dd MMM yyyy HH:mm:ss}")> _
    <Model.ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")> _
    Public Property EntryDateTime() As DateTime
        Get
            Return _entryDateTime
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue("EntryDate", _entryDateTime, value)
        End Set
    End Property
    <Model.ModelDefault("DisplayFormat", "{0:dd MMM yyyy HH:mm:ss}")> _
    <Model.ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")> _
    Public Property SubmitDateTime() As DateTime
        Get
            Return _submitDateTime
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue("SubmitDateTime", _submitDateTime, value)
        End Set
    End Property
    <Model.ModelDefault("DisplayFormat", "{0:dd MMM yyyy HH:mm:ss}")> _
    <Model.ModelDefault("EditMask", "dd MMM yyyy HH:mm:ss")> _
    Public Property CancelDateTime As DateTime
        Get
            Return _cancelDateTime
        End Get
        Set(ByVal value As DateTime)
            SetPropertyValue("CancelDateTime", _cancelDateTime, value)
        End Set
    End Property
    Public Property EnteredBy As String
        Get
            Return _enteredBy
        End Get
        Set(value As String)
            SetPropertyValue("EnteredBy", _enteredBy, value)
        End Set
    End Property
    Public Property SubmittedBy As String
        Get
            Return _submittedBy
        End Get
        Set(ByVal value As String)
            SetPropertyValue("SubmittedBy", _submittedBy, value)
        End Set
    End Property
    Public Property CanceledBy As String
        Get
            Return _canceledBy
        End Get
        Set(ByVal value As String)
            SetPropertyValue("CanceledBy", _canceledBy, value)
        End Set
    End Property


    <DbType("Text")> _
    Public Property SubmitNote As String
        Get
            Return _submitNote
        End Get
        Set(ByVal value As String)
            SetPropertyValue("SubmitNote", _submitNote, value)
        End Set
    End Property

    <DbType("Text")> _
    Public Property CancelNote As String
        Get
            Return _cancelNote
        End Get
        Set(ByVal value As String)
            SetPropertyValue("CancelNote", _cancelNote, value)
        End Set
    End Property
    Public Sub SubmitBySystem()
        If Status <> TransactionStatus.Submitted Then
            Dim boolValid As Boolean = True
            Dim strInvalidMessage As String = "Invalid transaction to submit." & Environment.NewLine & "Message:"
            For Each obj As RuleSetValidationResultItem In Validator.RuleSet.ValidateTarget(XPObjectSpace.FindObjectSpaceByObject(Me), Me, "Submit").Results
                If obj.State = ValidationState.Invalid Then
                    boolValid = False
                    strInvalidMessage &= Environment.NewLine & obj.ErrorMessage
                End If
            Next
            If Not boolValid Then Throw New InvalidOperationException(strInvalidMessage)
            Submit()
        End If
    End Sub
    Public Sub CancelBySystem()
        If Status = TransactionStatus.Submitted Then
            Dim boolValid As Boolean = True
            Dim strInvalidMessage As String = "Invalid transaction to cancel." & Environment.NewLine & "Message:"
            For Each obj As RuleSetValidationResultItem In Validator.RuleSet.ValidateTarget(XPObjectSpace.FindObjectSpaceByObject(Me), Me, "Cancel").Results
                If obj.State = ValidationState.Invalid Then
                    boolValid = False
                    strInvalidMessage &= Environment.NewLine & obj.ErrorMessage
                End If
            Next
            If Not boolValid Then Throw New InvalidOperationException(strInvalidMessage)
            Cancel()
        End If
    End Sub
    Public Sub Submit()
        If Status <> TransactionStatus.Submitted Then
            OnSubmitting()
            Status = TransactionStatus.Submitted
            SubmitDateTime = GlobalFunction.GetServerNow(Session)
            SubmittedBy = SecuritySystem.CurrentUserName
            OnSubmitted()
        End If
    End Sub
    Public Sub Cancel()
        If Status = TransactionStatus.Submitted Then
            OnCanceling()
            Status = TransactionStatus.Cancelled
            CancelDateTime = GlobalFunction.GetServerNow(Session)
            CanceledBy = SecuritySystem.CurrentUserName
            OnCanceled()
        End If
    End Sub
    Protected Overridable Sub OnSubmitting()
    End Sub
    Protected Overridable Sub OnSubmitted()
    End Sub
    Protected Overridable Sub OnCanceling()
    End Sub
    Protected Overridable Sub OnCanceled()
    End Sub
    Protected Overrides Sub OnSaving()
        MyBase.OnSaving()
        If Not TypeOf Session Is NestedUnitOfWork AndAlso Session.IsNewObject(Me) Then
            EntryDateTime = GlobalFunction.GetServerNow(Session)
            EnteredBy = SecuritySystem.CurrentUserName
        End If
    End Sub
End Class
Public Enum TransactionStatus
    Entered
    Submitted
    Cancelled
End Enum