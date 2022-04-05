namespace abahaBravo.Model
{
    public class AccDocc
    {
        public readonly string Query =
            @"INSERT INTO AbahaAccdoc (Id,Code,DiscountRate,Address,Contact,Phone)
                    VALUES (@_Id,@_Code,@_DiscountRate,@_Address,@_Contact,@_Phone)";

        public readonly string QueryAccDocSale =
            @"INSERT INTO AbahaAccdocSale (Sku,Quantity,Price,Total,BillId)
                    VALUES (@_Sku,@_Quantity,@_Price,@_Total,@_BillId)";

        public readonly string QueryExec = @"EXEC uspAbahaConvertHDBravo @Id  = @_Id";
    }
}