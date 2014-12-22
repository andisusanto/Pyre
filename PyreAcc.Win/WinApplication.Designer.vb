Imports Microsoft.VisualBasic
Imports System

Partial Public Class PyreAccWindowsFormsApplication
	''' <summary> 
	''' Required designer variable.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary> 
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso (Not components Is Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

#Region "Component Designer generated code"

	''' <summary> 
	''' Required method for Designer support - do not modify 
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
        Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
        Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
        Me.SecurityStrategyComplex1 = New DevExpress.ExpressApp.Security.SecurityStrategyComplex()
        Me.SecurityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
        Me.AuthenticationStandard1 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
        Me.CloneObjectModule1 = New DevExpress.ExpressApp.CloneObject.CloneObjectModule()
        Me.ConditionalAppearanceModule1 = New DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule()
        Me.ValidationModule1 = New DevExpress.ExpressApp.Validation.ValidationModule()
        Me.AuditTrailModule1 = New DevExpress.ExpressApp.AuditTrail.AuditTrailModule()
        Me.module3 = New PyreAcc.[Module].PyreAccModule()
        Me.module4 = New PyreAcc.[Module].Win.PyreAccWindowsFormsModule()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'SecurityStrategyComplex1
        '
        Me.SecurityStrategyComplex1.Authentication = Me.AuthenticationStandard1
        Me.SecurityStrategyComplex1.RoleType = GetType(PyreAcc.[Module].ApplicationRole)
        Me.SecurityStrategyComplex1.UserType = GetType(PyreAcc.[Module].ApplicationUser)
        '
        'AuthenticationStandard1
        '
        Me.AuthenticationStandard1.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
        '
        'ValidationModule1
        '
        Me.ValidationModule1.AllowValidationDetailsAccess = True
        Me.ValidationModule1.IgnoreWarningAndInformationRules = False
        '
        'AuditTrailModule1
        '
        Me.AuditTrailModule1.AuditDataItemPersistentType = GetType(DevExpress.Persistent.BaseImpl.AuditDataItemPersistent)
        '
        'PyreAccWindowsFormsApplication
        '
        Me.ApplicationName = "PyreAcc"
        Me.Modules.Add(Me.module1)
        Me.Modules.Add(Me.module2)
        Me.Modules.Add(Me.CloneObjectModule1)
        Me.Modules.Add(Me.ConditionalAppearanceModule1)
        Me.Modules.Add(Me.ValidationModule1)
        Me.Modules.Add(Me.AuditTrailModule1)
        Me.Modules.Add(Me.SecurityModule1)
        Me.Modules.Add(Me.module3)
        Me.Modules.Add(Me.module4)
        Me.Security = Me.SecurityStrategyComplex1
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

#End Region

	Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
    Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
	Private module3 As Global.PyreAcc.Module.PyreAccModule
    Private module4 As Global.PyreAcc.Module.Win.PyreAccWindowsFormsModule
    Friend WithEvents SecurityStrategyComplex1 As DevExpress.ExpressApp.Security.SecurityStrategyComplex
    Friend WithEvents AuthenticationStandard1 As DevExpress.ExpressApp.Security.AuthenticationStandard
    Friend WithEvents SecurityModule1 As DevExpress.ExpressApp.Security.SecurityModule
    Friend WithEvents CloneObjectModule1 As DevExpress.ExpressApp.CloneObject.CloneObjectModule
    Friend WithEvents ConditionalAppearanceModule1 As DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule
    Friend WithEvents ValidationModule1 As DevExpress.ExpressApp.Validation.ValidationModule
    Friend WithEvents AuditTrailModule1 As DevExpress.ExpressApp.AuditTrail.AuditTrailModule
End Class
