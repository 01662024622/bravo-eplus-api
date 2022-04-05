using System;
using System.Data;
using System.Data.SqlClient;
using abahaBravo.Model;
using abahaBravo.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace abahaBravo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Create(CustomerRequest.Create request)
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            Request.Headers.TryGetValue("Cookie", out var cookie);
            if (!CheckAuth.GetAuth(token, cookie))
            {
                var res = new JsonResult(new Response.Response(401, "Lỗi xác thực!"));
                res.StatusCode = 401;
                return res;
            }

            B20Customer customer = new B20Customer();

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(customer.SelectQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Code", request.Code);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    if (table.Rows.Count > 0)
                    {
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult(new Response.Response(200, "Đối tượng đã tồn tại!"));
                        // return new JsonResult("{\"status\":200,\"message\":\"Đối tượng đã tồn tại!\"}");
                    }
                }

                using (SqlCommand myCommand = new SqlCommand(customer.CreatQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Code", request.Code);
                    myCommand.Parameters.AddWithValue("@Name", request.Name);
                    myCommand.Parameters.AddWithValue("@Address", request.Address);
                    myCommand.Parameters.AddWithValue("@BillingAddress", request.Address);
                    myCommand.Parameters.AddWithValue("@Phone", request.Phone);
                    myCommand.Parameters.AddWithValue("@Email", request.Email);

                    myCommand.ExecuteReader();
                }

                myReader.Close();
                myCon.Close();
            }


            return new JsonResult(new Response.Response(200, "Thêm mới đối tượng thành công!"));
            // return new JsonResult("{\"status\":200,\"message\":\"Thêm mới đối tượng thành công!\"}");
        }
    }
}