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
Public Class CutOffProcess
    Inherits ProcessBase
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub
    Private _periodCutOff As PeriodCutOff
    <DataSourceCriteria("Closed = False")>
    <RuleRequiredField("Rule Required for CutOffProcess.PeriodCutOff", DefaultContexts.Save)>
    Public Property PeriodCutOff As PeriodCutOff
        Get
            Return _periodCutOff
        End Get
        Set(value As PeriodCutOff)
            SetPropertyValue("PeriodCutOff", _periodCutOff, value)
        End Set
    End Property
    Public Overrides Sub Execute()
        If Not PeriodCutOff.Closed Then
            Dim nextPeriodCutOff As PeriodCutOff = Session.FindObject(Of PeriodCutOff)(PersistentCriteriaEvaluationBehavior.InTransaction, New BinaryOperator("StartDate", DateAdd(DateInterval.Day, 1, PeriodCutOff.EndDate)))
            If nextPeriodCutOff Is Nothing Then
                nextPeriodCutOff = New PeriodCutOff(Session)
                Dim tmpStartDate As Date = PeriodCutOff.EndDate.AddDays(1)
                nextPeriodCutOff.StartDate = tmpStartDate
            End If
            PeriodCutOff.Closed = True
            'For i = 0 To nextPeriodCutOff.Accounts.Count - 1
            '    nextPeriodCutOff.Accounts(0).Delete()
            'Next

            'For Each objAccount In PeriodCutOff.Accounts
            '    Dim tmpAccount As New PeriodCutOffAccount(Session) With {.Account = objAccount.Account, .PeriodCutOff = nextPeriodCutOff, .InitialBalance = objAccount.LastBalance}
            'Next

            For i = 0 To nextPeriodCutOff.InventoryItemDeductTransactions.Count - 1
                nextPeriodCutOff.InventoryItemDeductTransactions(0).ResetDetail()
            Next
            For i = 0 To nextPeriodCutOff.InventoryItems.Count - 1
                nextPeriodCutOff.InventoryItems(0).Delete()
            Next

            For Each objInventoryItem In PeriodCutOff.InventoryItems
                Dim tmpInventoryItem As New PeriodCutOffInventoryItem(Session) With {.PeriodCutOff = nextPeriodCutOff, _
                                                                                     .TransDate = objInventoryItem.TransDate, _
                                                                                     .BaseUnitQuantity = objInventoryItem.RemainingBaseUnitQuantity, _
                                                                                     .ExpiryDate = objInventoryItem.ExpiryDate, _
                                                                                     .UnitPrice = objInventoryItem.UnitPrice, _
                                                                                     .Inventory = objInventoryItem.Inventory, _
                                                                                     .Item = objInventoryItem.Item, _
                                                                                     .BatchNo = objInventoryItem.BatchNo}
            Next
            For Each objInventoryItemDeductTransaction In nextPeriodCutOff.InventoryItemDeductTransactions
                objInventoryItemDeductTransaction.DistributeDeduction()
            Next
        End If

    End Sub
End Class
