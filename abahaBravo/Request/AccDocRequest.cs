using System;
using System.Collections.Generic;

namespace abahaBravo.Request
{
    public class AccDocRequest
    {
        public class CreateAcc
        {
            public string Code { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public int DiscountRate { get; set; }
            public string Contact { get; set; }
            public List<AccDocSale> AccDocSales { get; set; }
        }

        public class AccDocSale
        {
            public string Sku { get; set; }
            public int Quantity { get; set; }
            public int Price { get; set; }
        }
    }
}