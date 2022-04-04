using System;

namespace abahaBravo.Response
{
    public class Response
    {
        public Response(int status,Object message)
        {
            Status = status;
            Message = message;
        }
        public int Status { get; set; } = 200;
        public Object Message { get; set; }
    }
}