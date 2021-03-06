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

Partial Public Class PrintSalesReturnController
    Inherits ViewController
    Public ReadOnly Property CurObject As SalesReturn
        Get
            Return View.CurrentObject
        End Get
    End Property
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        RegisterActions(components)
    End Sub

    Private Sub PrintReturn_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles PrintSalesReturn.Execute
        View.ObjectSpace.CommitChanges()
        Dim tmpName As String = "Report Sales Return"
        Dim rpt As Report.Module.Report = View.ObjectSpace.FindObject(Of Report.Module.Report)(New BinaryOperator("Name", tmpName))
        Dim lst As New List(Of IReportParameterControl)
        lst.Add(New Report.Module.SystemReportParameterControl With {.ControlName = "SalesReturn", .IsActive = True, .Values = {"('" & CurObject.Oid.ToString() & "')"}, .CriteriaString = {CurObject.No}})
        Dim Xrpt = rpt.GetXtraReport(lst)
        Xrpt.ShowRibbonPreviewDialog()
    End Sub
End Class
