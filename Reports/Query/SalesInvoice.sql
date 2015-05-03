SELECT TOP 1 * FROM CompanyInformation

SELECT Customer.Name AS Customer, Location.Name AS CustomerLocation, CustomerContactInformation.ContactInformation,
SalesInvoice.TransDate, SalesInvoice.DueDate, SalesInvoice.No AS SalesInvoiceNo, SalesInvoice.Discount AS SIDiscount,
Item.Code AS Item, Item.Name AS ItemName, SalesInvoiceDetail.Quantity, Unit.Name AS Unit, SalesInvoiceDetail.UnitPrice, SalesInvoiceDetail.Discount AS SIDDiscount, 
SalesInvoiceDetail.GrandTotal AS SIDGrandTotal, SalesInvoice.GrandTotal AS SIGrandTotal, SalesInvoice.Total AS SITotal,
Salesman.Name AS Salesman, PeriodCutOffInventoryItem.BatchNo AS BatchNo, PeriodCutOffInventoryItem.ExpiryDate AS ExpDate,
	SalesInvoice.Oid AS SalesInvoiceOid, SalesInvoiceDetail.Oid AS SalesInvoiceDetailOid
FROM SalesInvoice
LEFT JOIN SalesInvoiceDetail ON SalesInvoiceDetail.SalesInvoice = SalesInvoice.Oid
LEFT JOIN PeriodCutOffInventoryItemDeductTransaction ON SalesInvoiceDetail.PeriodCutOffInventoryItemDeductTransaction = PeriodCutOffInventoryItemDeductTransaction
LEFT JOIN PeriodCutOffInventoryItemDeductTransactionDetail ON PeriodCutOffInventoryItemDeductTransactionDetail.PeriodCutOffInventoryItemDeductTransaction = PeriodCutOffInventoryItemDeductTransaction.Oid
LEFT JOIN PeriodCutOffInventoryItem ON PeriodCutOffInventoryItemDeductTransactionDetail.PeriodCutOffInventoryItem = PeriodCutOffInventoryItem.Oid
INNER JOIN Item ON Item.Oid = SalesInvoiceDetail.Item
INNER JOIN Unit ON Item.BaseUnit = Unit.Oid
INNER JOIN Customer ON Customer.Oid = SalesInvoice.Customer
LEFT JOIN Location ON Location.Oid = Customer.Location
LEFT JOIN CustomerContactInformation ON CustomerContactInformation.Customer = Customer.Oid
LEFT JOIN Salesman ON Salesman.Oid = SalesInvoice.Salesman