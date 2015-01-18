Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Layout
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.ExpressApp.Model.NodeGenerators
Imports DevExpress.ExpressApp.Xpo

' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
Partial Public Class ProcessBaseViewController
    Inherits ViewController
    Public Sub New()
        InitializeComponent()
        RegisterActions(components)
        ' Target required Views (via the TargetXXX properties) and create their Actions.

    End Sub
    Protected Overrides Sub OnActivated()
        MyBase.OnActivated()
        ' Perform various tasks depending on the target View.
    End Sub
    Protected Overrides Sub OnViewControlsCreated()
        MyBase.OnViewControlsCreated()
        ' Access and customize the target View control.
    End Sub
    Protected Overrides Sub OnDeactivated()
        ' Unsubscribe from previously subscribed events and release other references and resources.
        MyBase.OnDeactivated()
    End Sub

    Private Sub ExecuteAction_Execute(sender As Object, e As DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs) Handles saExecuteAction.Execute
        View.ObjectSpace.CommitChanges()
        If View.CurrentObject.Status <> ProcessStatus.Waiting Then Exit Sub
        View.CurrentObject.Status = ProcessStatus.Executing
        View.ObjectSpace.CommitChanges()
        Dim ctx As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim t As New Threading.Thread(New Threading.ThreadStart(Sub()
                                                                    System.Web.HttpContext.Current = ctx
                                                                    Execute()
                                                                End Sub
                                                                ))

        t.Start()
    End Sub
    Private Sub Execute()
        Dim objSpace As XPObjectSpace = Application.CreateObjectSpace()
        Dim objProcess As ProcessBase = objSpace.GetObject(View.CurrentObject)
        Dim id As Guid = objProcess.Oid
        Dim type As Type = objProcess.GetType
        Dim IsError As Boolean = False
        Dim ErrorMessage As String = ""
        Try
            objProcess.Start = GlobalFunction.GetServerNow(objSpace.Session)
            objSpace.CommitChanges()
            objProcess.Execute()
            objProcess.Status = ProcessStatus.FinishSuccessfully
        Catch ex As Exception
            IsError = True
            ErrorMessage += "Error with message: " & ex.Message & Environment.NewLine
            ErrorMessage += "Stack Trace: " & ex.StackTrace & Environment.NewLine
            objSpace.Rollback()
        Finally
            objProcess = objSpace.GetObjectByKey(type, id)
            objProcess.Finish = GlobalFunction.GetServerNow(objSpace.Session)
            If IsError Then
                objProcess.Status = ProcessStatus.FinishWithError
                If ErrorMessage.Length > 4000 Then ErrorMessage = ErrorMessage.Substring(0, 3800)
                objProcess.Note += ErrorMessage
            End If
            objSpace.CommitChanges()
        End Try
    End Sub
End Class
