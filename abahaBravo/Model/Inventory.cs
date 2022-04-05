namespace abahaBravo.Model
{
    public class Inventory
    {
	    public readonly string query = @"SELECT  SUM(Quantity) as Amount FROM
			(
			SELECT ItemCode, (OpenQuantity) AS Quantity  FROM vB30OpenInventoryHdrabaha
			WHERE ItemCode = @_ItemCode AND Year = YEAR(Getdate())
			UNION 
			SELECT Code, (CASE WHEN DocGroup = 1 THEN Quantity ELSE -Quantity END) AS Quantity
			FROM B30StockLedger a INNER JOIN B20Item b ON a.ItemId = b.Id
			WHERE  a.Isactive = 1 AND Code = @_ItemCode AND YEAR(DocDate) = YEAR(Getdate())
			) AS a
			GROUP BY ItemCode";
    }
}