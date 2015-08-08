Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.XtraReports.UI
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo
Imports System.Data.SqlClient
Imports Report.Module

Partial Public Class PrintPurchaseReturnController
    Inherits ViewController
    Public ReadOnly Property CurObject As PurchaseReturn
        Get
            Return View.CurrentObject
        End Get
    End Property
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        RegisterActions(components)
    End Sub

    Private Sub PrintPurchaseReturn_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles PrintPurchaseReturn.Execute
        View.ObjectSpace.CommitChanges()
        Dim tmpName As String = "Report Purchase Return"
        Dim rpt As Report.Module.Report = View.ObjectSpace.FindObject(Of Report.Module.Report)(New BinaryOperator("Name", tmpName))
        Dim lst As New List(Of IReportParameterControl)
        lst.Add(New Report.Module.SystemReportParameterControl With {.ControlName = "PurchaseReturn", .IsActive = True, .Values = {"('" & CurObject.Oid.ToString() & "')"}, .CriteriaString = {CurObject.No}})
        Dim Xrpt = rpt.GetXtraReport(lst)
        Xrpt.ShowRibbonPreviewDialog()
    End Sub
End Class
