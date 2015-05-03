SELECT TOP 1 * FROM CompanyInformation

SELECT Customer.Name AS Customer, Location.Name AS CustomerLocation, CustomerContactInformation.ContactInformation,
SalesReturn.TransDate, SalesReturn.No AS SalesReturnNo, SalesReturn.Discount AS SRDiscount,
Item.Code AS Item, Item.Name AS ItemName, Unit.Name AS Unit, SalesReturnDetail.UnitPrice, SalesReturnDetail.Discount AS SRDDiscount, 
SalesReturnDetail.GrandTotal AS SRDGrandTotal, SalesReturn.GrandTotal AS SRGrandTotal, SalesReturn.Total AS SRTotal,
Salesman.Name AS Salesman, SalesReturnDetail.Quantity, SalesReturnDetail.BatchNo, SalesReturnDetail.ExpiryDate
FROM SalesReturn
LEFT JOIN SalesReturnDetail ON SalesReturnDetail.SalesReturn = SalesReturn.Oid
INNER JOIN Item ON Item.Oid = SalesReturnDetail.Item
INNER JOIN Unit ON Item.BaseUnit = Unit.Oid
INNER JOIN Customer ON Customer.Oid = SalesReturn.Customer
LEFT JOIN Location ON Location.Oid = Customer.Location
LEFT JOIN CustomerContactInformation ON CustomerContactInformation.Customer = Customer.Oid
LEFT JOIN Salesman ON Salesman.Oid = SalesReturn.Salesman
