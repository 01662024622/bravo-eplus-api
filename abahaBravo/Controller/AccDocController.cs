using System;
using System.Data.SqlClient;
using abahaBravo.Model;
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
        public JsonResult Create(AccDocRequest.CreateAcc request)
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
            AccDocc accDocc = new AccDocc();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(accDocc.Query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@_Id", time);
                    myCommand.Parameters.AddWithValue("@_Code", request.Code);
                    myCommand.Parameters.AddWithValue("@_DiscountRate", request.DiscountRate);
                    myCommand.Parameters.AddWithValue("@_Address", request.Address);
                    myCommand.Parameters.AddWithValue("@_Contact", request.Contact);
                    myCommand.Parameters.AddWithValue("@_Phone", request.Phone);

                    myCommand.ExecuteReader();
                }

                foreach (var accDocSale in request.AccDocSales)
                {
                    using (SqlCommand myCommand = new SqlCommand(accDocc.QueryAccDocSale, myCon))
                    {
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@_Sku", accDocSale.Sku);
                        myCommand.Parameters.AddWithValue("@_Quantity", accDocSale.Quantity);
                        myCommand.Parameters.AddWithValue("@_Price", accDocSale.Price);
                        myCommand.Parameters.AddWithValue("@_Total", accDocSale.Price * accDocSale.Quantity);
                        myCommand.Parameters.AddWithValue("@_BillId", time);
                        myCommand.ExecuteReader();
                    }
                }

                using (SqlCommand myCommand = new SqlCommand(accDocc.QueryExec, myCon))
                {
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("@_Id", time);
                    myCommand.ExecuteReader();
                }

                myCon.Close();
            }


            return new JsonResult(new Response.Response(200, "Thêm mới đối tượng thành công!"));
            // return new JsonResult("{\"status\":200,\"message\":\"Thêm mới đối tượng thành công!\"}");
        }
    }
}