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
        /// <summary>
        /// Used to  register user
        /// </summary>
        /// <param name="user">The UserLoginViewModel object</param>
        internal static Task<UserLoginViewModel> CreateUser(UserLoginViewModel user)
        {
            try
            {
                user = ApiHelper.Add<UserLoginViewModel>(user, URL.LocalIISURL, "adduser");
            }
            catch (Exception e)
            {
                Log.Fatal("Exception Inside UserManager Class in CreateUser Method ", e);
                user = null;
            }

            return Task.FromResult(user);
        }

        /// <summary>
        /// Used to  login user
        /// </summary>
        /// <param name="user">The UserLoginViewModel object</param>
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