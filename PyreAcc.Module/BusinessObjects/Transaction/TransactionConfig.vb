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
Public Class TransactionConfig
    Inherits BaseObject
    Public Sub New(ByVal session As Session)
        MyBase.New(session)
    End Sub
    Public Overrides Sub AfterConstruction()
        MyBase.AfterConstruction()

    End Sub

    Private _activeTransactionDate As Date
    Public Property ActiveTransactionDate As Date
        Get
            Return _activeTransactionDate
        End Get
        Set(value As Date)
            SetPropertyValue("ActiveTransactionDate", _activeTransactionDate, value)
        End Set
    End Property

    Public Shared Function GetInstance(ByVal session As Session) As TransactionConfig
        Return session.FindObject(Of TransactionConfig)(Nothing)
    End Function

    Public Shared Function IsInClosedPeriod(ByVal session As Session, ByVal transDate As Date) As Boolean
        Dim transactionConfig As TransactionConfig = transactionConfig.GetInstance(session)
        If transactionConfig Is Nothing Then Return True
        Return transactionConfig.ActiveTransactionDate >= transDate
    End Function
End Class
