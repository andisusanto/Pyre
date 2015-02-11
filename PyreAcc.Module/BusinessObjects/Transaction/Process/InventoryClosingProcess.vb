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
Public Class InventoryClosingProcess
    Inherits ProcessBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _period As Period
    <RuleRequiredField("Rule Required for InventoryClosingProcess.Period", DefaultContexts.Save)>
    Public Property Period As Period
        Get
            Return _period
        End Get
        Set(value As Period)
            SetPropertyValue("Period", _period, value)
        End Set
    End Property

    Public Overrides Sub Execute()
        Dim nextPeriod As Period = Session.FindObject(Of Period)(PersistentCriteriaEvaluationBehavior.InTransaction, New BinaryOperator("StartDate", DateAdd(DateInterval.Day, 1, Period.EndDate)))
        If nextPeriod Is Nothing Then
            nextPeriod = New Period(Session)
            Dim tmpStartDate As Date = Period.EndDate.AddDays(1)
            nextPeriod.Code = tmpStartDate.ToString("yyyyMM")
            nextPeriod.Description = tmpStartDate.ToString("MMMM yyyy")
            nextPeriod.Year = tmpStartDate.Year
            nextPeriod.Month = tmpStartDate.Month
            nextPeriod.StartDate = tmpStartDate
            nextPeriod.EndDate = tmpStartDate.AddMonths(1).AddDays(-1)
        End If
        Dim nextBalanceSheet As BalanceSheet = Session.FindObject(Of BalanceSheet)(PersistentCriteriaEvaluationBehavior.InTransaction, New BinaryOperator("Period", nextPeriod))
        If nextBalanceSheet Is Nothing Then nextBalanceSheet = New BalanceSheet(Session) With {.Period = nextPeriod}
        Period.Closed = True
        Dim BalanceSheet As BalanceSheet = Session.FindObject(Of BalanceSheet)(PersistentCriteriaEvaluationBehavior.InTransaction, New BinaryOperator("Period", Period))
        For i = 0 To nextBalanceSheet.Accounts.Count - 1
            nextBalanceSheet.Accounts(0).Delete()
        Next
        For i = 0 To nextBalanceSheet.InventoryItemDeductTransactions.Count - 1
            nextBalanceSheet.InventoryItemDeductTransactions(0).ResetDetail()
        Next
        For i = 0 To nextBalanceSheet.InventoryItems.Count - 1
            nextBalanceSheet.InventoryItems(0).Delete()
        Next

        For Each objAccount In BalanceSheet.Accounts
            Dim tmpAccount As New BalanceSheetAccount(Session) With {.Account = objAccount.Account, .BalanceSheet = nextBalanceSheet, .InitialBalance = objAccount.LastBalance}
        Next
        For Each objInventoryItem In BalanceSheet.InventoryItems
            Dim tmpInventoryItem As New BalanceSheetInventoryItem(Session) With {.BalanceSheet = nextBalanceSheet, _
                                                                                 .TransDate = objInventoryItem.TransDate, _
                                                                                 .BaseUnitQuantity = objInventoryItem.RemainingBaseUnitQuantity, _
                                                                                 .ExpiryDate = objInventoryItem.ExpiryDate, _
                                                                                 .UnitPrice = objInventoryItem.UnitPrice, _
                                                                                 .Inventory = objInventoryItem.Inventory, _
                                                                                 .Item = objInventoryItem.Item}
        Next
        For Each objInventoryItemDeductTransaction In nextBalanceSheet.InventoryItemDeductTransactions
            objInventoryItemDeductTransaction.DistributeDeduction()
        Next
    End Sub
End Class
