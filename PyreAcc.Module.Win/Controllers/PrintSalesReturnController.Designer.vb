Partial Class PrintSalesReturnController

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(Me)

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PrintSalesReturn = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        '
        'PrintSalesReturn
        '
        Me.PrintSalesReturn.Caption = "Print Sales Return"
        Me.PrintSalesReturn.ConfirmationMessage = Nothing
        Me.PrintSalesReturn.Id = "PrintSalesReturn"
        Me.PrintSalesReturn.Shortcut = "ctrl + p"
        Me.PrintSalesReturn.TargetObjectType = GetType(PyreAcc.[Module].SalesReturn)
        Me.PrintSalesReturn.ToolTip = Nothing

    End Sub
    Friend WithEvents PrintSalesReturn As DevExpress.ExpressApp.Actions.SimpleAction

End Class
