Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
'Imports DevExpress.ExpressApp.Reports
'Imports DevExpress.ExpressApp.PivotChart
'Imports DevExpress.ExpressApp.Security.Strategy
'Imports PyreAcc.Module.BusinessObjects

' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
Public Class Updater
    Inherits ModuleUpdater
    Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
        MyBase.New(objectSpace, currentDBVersion)
    End Sub

    Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
        MyBase.UpdateDatabaseAfterUpdateSchema()
        
        Dim administratorRole As ApplicationRole = CreateAdministratorRole()
        Dim userAdmin As ApplicationUser = ObjectSpace.FindObject(Of ApplicationUser)(New BinaryOperator("UserName", "andi"))
        If userAdmin Is Nothing Then
            userAdmin = ObjectSpace.CreateObject(Of ApplicationUser)()
            userAdmin.UserName = "andi"
            userAdmin.IsActive = True
            userAdmin.SetPassword("wfr123654")
            userAdmin.Roles.Add(administratorRole)
            userAdmin.ChangePasswordOnFirstLogon = False
            userAdmin.Save()
        End If
    End Sub
    Private Function CreateAdministratorRole() As ApplicationRole
        Dim administratorRole As ApplicationRole = ObjectSpace.FindObject(Of ApplicationRole)( _
        New BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName))
        If administratorRole Is Nothing Then
            administratorRole = ObjectSpace.CreateObject(Of ApplicationRole)()
            administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName
            administratorRole.IsAdministrative = True
        End If
        Return administratorRole
    End Function
    Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
        MyBase.UpdateDatabaseBeforeUpdateSchema()
        'If (CurrentDBVersion < New Version("1.1.0.0") AndAlso CurrentDBVersion > New Version("0.0.0.0")) Then
        '    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName")
        'End If
    End Sub
End Class
