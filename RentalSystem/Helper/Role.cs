using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalSystem.Helper
{
    /// <summary>
    /// Enum to assign roles
    /// </summary>
    public enum Role
    {
        Admin = 1,
        Vendor,
        Customer
    }


    public static class URL
    {
        /// <summary>
        /// Used to  get the api url running on Local IIS
        /// </summary>
        public static string LocalIISURL
        {
            get
            {
                return "http://rentalsystem:96/api/";
            }
        }

        /// <summary>
        /// Used to  get the api url running on IIS EXPRESS
        /// </summary>
        public static string IISEXPRESSURL
        {
            get
            {
                return "http://localhost:65491/api/";
            }
        }
    }

}