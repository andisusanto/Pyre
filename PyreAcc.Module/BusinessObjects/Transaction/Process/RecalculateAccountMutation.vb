Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

<DeferredDeletion(False)>
<DefaultClassOptions()> _
Public Class RecalculateAccountMutation
    Inherits ProcessBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Public Overrides Sub Execute()
        Dim xp As New XPCollection(Of PeriodCutOffAccount)(Session)
        For Each objPeriodCutOffAccount In xp
            Dim curBalance = objPeriodCutOffAccount.InitialBalance
            Dim xpAccountMutation As New XPCollection(Of PeriodCutOffJournalAccountMutation)(Session, GroupOperator.And(New BinaryOperator("Account", objPeriodCutOffAccount.Account))) With {.Sorting = New SortingCollection(New SortProperty("PeriodCutOffJournal.TransDate", DB.SortingDirection.Ascending), New SortProperty("PeriodCutOffJournal.EntryDate", DB.SortingDirection.Ascending))}
            For Each objAccountMutation In xpAccountMutation
                Dim tmpAmount = IIf(objAccountMutation.MutationType = objAccountMutation.Account.NormalBalance, 1, -1) * objAccountMutation.Amount
                objAccountMutation.AfterMutationAmount = curBalance + tmpAmount
                curBalance = objAccountMutation.AfterMutationAmount
                objPeriodCutOffAccount.LastBalance = objAccountMutation.AfterMutationAmount
            Next
        Next
        CType(Session, UnitOfWork).CommitChanges()
    End Sub
End Class
