Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base

Public Class SuppressTransactionBaseConfirmation
    Inherits DevExpress.ExpressApp.Win.SystemModule.WinModificationsController

	Public Sub New()
		MyBase.New()

		'This call is required by the Component Designer.
		InitializeComponent()
		RegisterActions(components) 

    End Sub
    Private WithEvents SubmitAction As PopupWindowShowAction
    Private WithEvents CancelSubmitAction As PopupWindowShowAction
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
        If PyreAcc.Module.GlobalFunction.IsDescendant(Of TransactionBase)(View.ObjectTypeInfo.Type) Then
            SubmitAction = Frame.GetController(Of PyreAcc.Module.TransactionBaseController)().MySubmitAction
            CancelSubmitAction = Frame.GetController(Of PyreAcc.Module.TransactionBaseController).MyCancelSubmitAction
        End If
    End Sub

    Private Sub TransactionConfirmationAction_Executing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CancelSubmitAction.Executing, SubmitAction.Executing
        ModificationsHandlingMode = DevExpress.ExpressApp.SystemModule.ModificationsHandlingMode.AutoRollback
    End Sub
End Class
