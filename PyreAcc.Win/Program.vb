Imports System
Imports System.Configuration
Imports System.Windows.Forms

Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Win

Imports PyreAcc.Win
Imports DevExpress.Persistent.AuditTrail

Imports PyreAcc.Module

Public Class Program

    <STAThread()> _
    Public Shared Sub Main(ByVal arguments() As String)
#If EASYTEST Then
              DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register()
#End If

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached
        Dim _application As PyreAccWindowsFormsApplication = New PyreAccWindowsFormsApplication()
        ' Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument2680 help article for more details on how to provide a custom splash form.
        '_application.SplashScreen = New DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png")

        If (Not ConfigurationManager.ConnectionStrings.Item("ConnectionString") Is Nothing) Then
            _application.ConnectionString = ConfigurationManager.ConnectionStrings.Item("ConnectionString").ConnectionString
        End If
#If EASYTEST Then
        If (Not ConfigurationManager.ConnectionStrings.Item("EasyTestConnectionString") Is Nothing) Then
            _application.ConnectionString = ConfigurationManager.ConnectionStrings.Item("EasyTestConnectionString").ConnectionString
        End If
#End If
        If System.Diagnostics.Debugger.IsAttached Then
            _application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways
        End If
        Try
            AddHandler AuditTrailService.Instance.CustomizeAuditTrailSettings, AddressOf Instance_CustomizeAuditTrailSettings
            _application.Setup()
            _application.Start()
        Catch e As Exception
            _application.HandleException(e)
        End Try

    End Sub
    Shared Sub Instance_CustomizeAuditTrailSettings(ByVal sender As Object, _
ByVal e As CustomizeAuditTrailSettingsEventArgs)
        'e.AuditTrailSettings.Clear()
        e.AuditTrailSettings.RemoveType(GetType(PeriodCutOffJournalAccountMutation))
        e.AuditTrailSettings.RemoveType(GetType(PeriodCutOffInventoryItem))
        e.AuditTrailSettings.RemoveType(GetType(PeriodCutOffInventoryItemDeductTransaction))
        e.AuditTrailSettings.RemoveType(GetType(PeriodCutOffInventoryItemDeductTransactionDetail))
        e.AuditTrailSettings.RemoveType(GetType(PeriodCutOffJournal))
        e.AuditTrailSettings.RemoveType(GetType(CutOffProcess))
        e.AuditTrailSettings.RemoveType(GetType(RecreateJournalProcess))
    End Sub
End Class
