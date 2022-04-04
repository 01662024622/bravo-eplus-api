using System;

namespace abahaBravo
{
    public class CheckAuth
    {
        public static readonly string TOKEN = "Bear eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NDkwNjAyMTYsImV4cCI6MTY0OTY2NTAxNiwiaWF0IjoxNjQ5MDYwMjE2fQ.2PHHB2WXSQBhlGdAClp4ezwZhIomlqZUweSU_f3L8qA";
        public static readonly string COOKIE = "tickid_session=45gn982347tvq4839045871rofqsh124";
        public static bool GetAuth(string token, string cookie)
        {
            if (token.Equals(TOKEN) && cookie.Equals(COOKIE))
            {
                return true;
            }

            return false;
        }
    }
}