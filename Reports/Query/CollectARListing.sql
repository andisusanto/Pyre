SELECT Salesman.Name AS Salesman, Location.Name AS Location, Customer.Name AS Customer, 
	SalesInvoice.TransDate, SalesInvoice.DueDate, SalesInvoice.No AS SalesInvoiceNo, SalesInvoice.GrandTotal,
	SalesReturn.GrandTotal AS SRGrandTotal, SUM(SalesPaymentDetail.Amount) AS SPDAmount
FROM SalesInvoice
LEFT JOIN Salesman ON Salesman.Oid = SalesInvoice.Salesman
INNER JOIN Customer ON Customer.Oid = SalesInvoice.Customer
LEFT JOIN Location ON Location.Oid = Customer.Location
LEFT JOIN SalesReturn ON Salesreturn.ReferenceNo = SalesInvoice.No
LEFT JOIN SalesPaymentDetail ON SalesPaymentDetail.SalesInvoice = SalesInvoice.Oid
GROUP BY Salesman.Name, Location.Name, Customer.Name, SalesInvoice.TransDate, SalesInvoice.DueDate,
	SalesInvoice.No, SalesInvoice.GrandTotal, SalesReturn.GrandTotal