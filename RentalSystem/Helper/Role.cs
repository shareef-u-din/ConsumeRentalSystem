using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalSystem.Helper
{
    public enum Role
    {
        Admin = 1,
        Vendor,
        Customer
    }

    public static class URL
    {
        public static string LocalIISURL
        {
            get
            {
                return "http://rentalsystem:96/api/";
            }
        }

        public static string IISEXPRESSURL
        {
            get
            {
                return "http://localhost:65491/api/";
            }
        }
    }

}