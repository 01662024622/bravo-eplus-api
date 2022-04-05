namespace abahaBravo.Model
{
    public class B20Customer
    {
        public string SelectQuery = @"SELECT * FROM B20Customer WHERE Code = @Code";
        public string CreatQuery =
            @"INSERT INTO B20Customer (ParentId,IsGroup,BranchCode,Code,Name,Address,BillingAddress,PersonTel, Email,CustomerType)
                    VALUES ('402281',0,'A01',@Code,@Name,@Address,@BillingAddress,@Phone,@Email,1)";
    }
}