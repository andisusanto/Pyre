Imports DevExpress.Xpo
Imports System.Text
Imports Evaluator
Imports System.Linq


Public Class GlobalFunction
    Public Shared Function GetServerNow(ByVal Session As Session) As Date
        Return Session.ExecuteScalar("SELECT GETDATE()")
    End Function
    Public Shared Function GetExecuteRunTimeDirectoryPath() As String
        Return New IO.FileInfo(System.Reflection.Assembly.GetAssembly(GetType(AutoNo)).Location).Directory.FullName & "\"
    End Function
    Public Shared Function ExecuteRunTimeFormula(ByVal Formula As String, ByVal ParamArray Objects As Object()) As Object
        Dim source As New StringBuilder()
        source.Append("Public Function myFunction(")
        For i = 1 To Objects.Count
            source.Append("ByVal prm" & i & " As " & Objects(i - 1).GetType().FullName & ", ")
        Next
        source.Remove(source.Length - 2, 2)
        source.Append(")As Object" & Environment.NewLine)

        source.Append(Formula & Environment.NewLine)
        source.Append("End Function")
        Try
            Dim myFunction As MethodResults = Eval.CreateVirtualMethod( _
                System.CodeDom.Compiler.CodeDomProvider.CreateProvider("VB").CreateCompiler(), _
                source.ToString(), _
                "myFunction", _
                New VBLanguage(), _
                   False, New String() { _
                           GetExecuteRunTimeDirectoryPath() & GlobalVar.SystemModule, GetExecuteRunTimeDirectoryPath() & GlobalVar.DevExpressPersistentBase, _
                         GetExecuteRunTimeDirectoryPath() & GlobalVar.DevExpressPersistentBaseImpl, GetExecuteRunTimeDirectoryPath() & GlobalVar.DevExpressXpo}, _
                          GlobalVar.SystemModuleName, GlobalVar.DevExpressPersistentBaseName, _
                          GlobalVar.DevExpressPersistentBaseImplName, GlobalVar.DevExpressXpoName, "Microsoft.VisualBasic.Strings")
            Try
                Return CStr(myFunction.Invoke(Objects))
            Catch tie As System.Reflection.TargetInvocationException
                Throw tie
                Exit Function
            End Try
        Catch ce As CompilationException
            Dim str As String = ""
            For i As Integer = 0 To ce.Errors.Count - 1
                str &= ce.Errors(i).ToString & vbNewLine
            Next
            Throw New Exception("Compilation Errors: " + Environment.NewLine + str)
            Exit Function
        End Try
    End Function

    Public Shared Function IsDescendant(Of T)(ByVal prmType As Type) As Boolean
        Dim curType As Type = prmType
        Do Until curType Is GetType(Object)
            If curType.BaseType Is GetType(T) Then Return True
            curType = curType.BaseType
        Loop
        Return False
    End Function
End Class
