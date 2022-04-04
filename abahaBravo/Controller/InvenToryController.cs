using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json.Serialization;
using abahaBravo.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace abahaBravo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvenToryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InvenToryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{sku}")]
        public JsonResult Get(int sku)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            Request.Headers.TryGetValue("Cookie", out var cookie);
            if (!CheckAuth.GetAuth(token,cookie))
            {
                var res = new JsonResult(new Response.Response(401, "Lỗi xác thực!"));
                res.StatusCode = 401;
                return res;
            }
            string query = @"SELECT SUM(Quantity) AS Amount FROM
	                            (
	                            SELECT ItemId, (OpenQuantity) AS Quantity  FROM vB30OpenInventoryHdr
	                            WHERE ItemId = @ItemId AND Year = YEAR(Getdate())
	                            UNION 
	                            SELECT ItemId, (CASE WHEN DocGroup = 1 THEN Quantity ELSE -Quantity END) AS Quantity
	                            FROM B30StockLedger 
	                            WHERE  Isactive = 1 AND ItemId = @ItemId AND YEAR(DocDate) = YEAR(Getdate())
	                            ) AS a
	                            GROUP BY ItemId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Itemid", sku);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            if (table.Rows.Count > 0)
            {
                return new JsonResult(new Response.Response(200, Decimal.ToInt32((Decimal) table.Rows[0]["Amount"])));
            }

            
            return new JsonResult(new Response.Response(200, "SKU không tồn tại!"));
        }
    }
}