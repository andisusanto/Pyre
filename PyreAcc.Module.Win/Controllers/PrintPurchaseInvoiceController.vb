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

Partial Public Class PrintPurchaseInvoiceController
    Inherits ViewController
    Public ReadOnly Property CurObject As PurchaseInvoice
        Get
            Return View.CurrentObject
        End Get
    End Property
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        RegisterActions(components)
    End Sub

    Private Sub PrintPurchaseInvoice_Execute(sender As Object, e As SimpleActionExecuteEventArgs) Handles PrintPurchaseInvoice.Execute
        View.ObjectSpace.CommitChanges()
        Dim tmpName As String = "Report Purchase Invoice"
        Dim rpt As Report.Module.Report = View.ObjectSpace.FindObject(Of Report.Module.Report)(New BinaryOperator("Name", tmpName))
        Dim lst As New List(Of IReportParameterControl)
        lst.Add(New Report.Module.SystemReportParameterControl With {.ControlName = "PurchaseInvoice", .IsActive = True, .Values = {"('" & CurObject.Oid.ToString() & "')"}, .CriteriaString = {CurObject.No}})
        lst.Add(New Report.Module.SystemReportParameterControl With {.ControlName = "TransDate", .IsActive = False})
        Dim Xrpt = rpt.GetXtraReport(lst)
        Xrpt.ShowRibbonPreviewDialog()
    End Sub
End Class
