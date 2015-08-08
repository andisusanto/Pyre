Partial Class PrintPurchaseReturnController

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
        Me.PrintPurchaseReturn = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        '
        'PrintPurchaseReturn
        '
        Me.PrintPurchaseReturn.Caption = "Print Purchase Return"
        Me.PrintPurchaseReturn.ConfirmationMessage = Nothing
        Me.PrintPurchaseReturn.Id = "PrintPurchaseReturn"
        Me.PrintPurchaseReturn.Shortcut = "ctrl + p"
        Me.PrintPurchaseReturn.TargetObjectType = GetType(PyreAcc.[Module].PurchaseReturn)
        Me.PrintPurchaseReturn.ToolTip = Nothing

    End Sub
    Friend WithEvents PrintPurchaseReturn As DevExpress.ExpressApp.Actions.SimpleAction

End Class
