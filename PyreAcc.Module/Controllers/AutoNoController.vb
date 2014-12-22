Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.DC.Xpo

Public Class AutoNoController
    Inherits DevExpress.ExpressApp.ViewController

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()
        RegisterActions(components)

    End Sub

    Protected Overrides Sub OnActivated()
        If View Is Nothing OrElse TryCast(View.CurrentObject, AppBase) Is Nothing Then Exit Sub
        Dim tmpObject As AppBase = CType(View.CurrentObject, AppBase)
        Dim autoNo As New XPCollection(Of AutoNo)(tmpObject.Session, CriteriaOperator.And(New BinaryOperator("TargetType", tmpObject.ClassInfo.FullName), New BinaryOperator("IsActive", True)))
        If TypeOf View Is DetailView Then
            For Each obj In autoNo
                For Each pe As PropertyEditor In CType(View, DetailView).GetItems(Of PropertyEditor)()
                    If pe.Id = obj.TargetProperty Then
                        If View.ObjectSpace.IsNewObject(View.CurrentObject) Then pe.PropertyValue = "-Auto-"
                        pe.AllowEdit(pe.Id) = False
                    End If
                Next
            Next
        ElseIf TypeOf View Is ListView Then
            For Each obj In autoNo
                For Each pe As PropertyEditor In CType(View, ListView).GetItems(Of PropertyEditor)()
                    If pe.Id = obj.TargetProperty Then
                        If View.ObjectSpace.IsNewObject(View.CurrentObject) Then pe.PropertyValue = "-Auto-"
                        pe.AllowEdit(pe.Id) = False
                    End If
                Next
            Next
        End If
        MyBase.OnActivated()
    End Sub
End Class
