using LocalAccountsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace LocalAccountsApp.Helpers
{
    public static class Common
    {
        public static async Task<UserModel> GetUserFromRepo(string username, string password)
        {
            var user = username == "test2@test.com" && password == "123456Aa!"
                ? new UserModel() { UserName = username }
                : null;

            return user;
        }
    }
}