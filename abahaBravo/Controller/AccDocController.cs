using System;
using System.Data.SqlClient;
using abahaBravo.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace abahaBravo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccDocController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccDocController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Create(AccDocRequest.Create request)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            Request.Headers.TryGetValue("Cookie", out var cookie);
            if (!CheckAuth.GetAuth(token, cookie))
            {
                var res = new JsonResult(new Response.Response(401, "Lỗi xác thực!"));
                res.StatusCode = 401;
                return res;
            }

            long time = DateTime.Now.Ticks;
            Console.WriteLine(time.ToString());
            string query =
                @"INSERT INTO AbahaAccdoc (Id,Code,DiscountRate,Address,Contact,Phone)
                    VALUES (@Code,@DiscountRate,@Address,@Contact,@Phone)";
            string queryAccDocSale =
                @"INSERT INTO AbahaAccdocSale (Id,Sku,Quantity,Price,Total,BillId)
                    VALUES (@Id,@Sku,@Quantity,@Price,@Total,@BillId)";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", time);
                    myCommand.Parameters.AddWithValue("@Code", request.Code);
                    myCommand.Parameters.AddWithValue("@DiscountRate", request.DiscountRate);
                    myCommand.Parameters.AddWithValue("@Address", request.Address);
                    myCommand.Parameters.AddWithValue("@Contact", request.Contact);
                    myCommand.Parameters.AddWithValue("@Phone", request.Phone);

                    myCommand.ExecuteReader();
                }
            }

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryAccDocSale, myCon))
                {
                    foreach (var accDocSale in request.AccDocSales)
                    {
                        myCommand.Parameters.AddWithValue("@Sku", accDocSale.Sku);
                        myCommand.Parameters.AddWithValue("@Quantity", accDocSale.Quantity);
                        myCommand.Parameters.AddWithValue("@Price", accDocSale.Price);
                        myCommand.Parameters.AddWithValue("@Total", accDocSale.Total);
                        myCommand.Parameters.AddWithValue("@BillId", time);
                        myCommand.ExecuteReader();
                    }
                }
            }


            return new JsonResult(new Response.Response(200, "Thêm mới đối tượng thành công!"));
            // return new JsonResult("{\"status\":200,\"message\":\"Thêm mới đối tượng thành công!\"}");
        }
    }
}