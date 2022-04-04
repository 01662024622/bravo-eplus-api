namespace abahaBravo.Model
{
    public class B20Customer
    {
        public string ParentId { get; set; } = "402281";
        public int IsGroup { get; set; } = 0;
        public string BranchCode { get; set; } = "A01";
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string BillingAddress { get; set; }
        public string PersonTel { get; set; }
        public string Email { get; set; }
        public int CustomerType { get; set; } = 1;
    }
}