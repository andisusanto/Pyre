Partial Class ProcessBaseViewController

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
        Me.saExecuteAction = New DevExpress.ExpressApp.Actions.SimpleAction(Me.components)
        '
        'saExecuteAction
        '
        Me.saExecuteAction.Caption = "Execute"
        Me.saExecuteAction.Category = "RecordEdit"
        Me.saExecuteAction.ConfirmationMessage = Nothing
        Me.saExecuteAction.Id = "saExecuteAction"
        Me.saExecuteAction.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root
        Me.saExecuteAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView
        Me.saExecuteAction.ToolTip = Nothing
        Me.saExecuteAction.TypeOfView = GetType(DevExpress.ExpressApp.DetailView)
        '
        'ProcessBaseViewController
        '
        Me.TargetObjectType = GetType(PyreAcc.[Module].ProcessBase)
        Me.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root

    End Sub
    Friend WithEvents saExecuteAction As DevExpress.ExpressApp.Actions.SimpleAction

End Class
