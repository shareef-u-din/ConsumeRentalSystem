using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ViewModels;

namespace RentalSystem.Helper
{
    public static class UserManager
    {
        public static Task<UserLoginViewModel> CreateUser(UserLoginViewModel user)
        {
            try
            {
                user=ApiHelper.Add<UserLoginViewModel>(user, URL.LocalIISURL, "adduser");
            }
            catch (Exception e)
            {
                Log.Fatal("Exception Inside UserManager Class in CreateUser Method ", e);
                user = null;
            }

            return Task.FromResult(user);
        }

        internal static Task<UserLoginViewModel> Login(UserLoginViewModel user)
        {
            try
            {
                user = ApiHelper.Add<UserLoginViewModel>(user, URL.LocalIISURL, "user");
            }
            catch (Exception e)
            {
                user = null;
                Log.Fatal("Exception Inside UserManager Class in Login Method ", e);

            }

            return Task.FromResult(user);
        }


    }

}