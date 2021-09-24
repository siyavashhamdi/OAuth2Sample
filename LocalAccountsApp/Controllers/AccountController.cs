using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using LocalAccountsApp.Models;
using System.Text;
using System.Web.Http.Results;

namespace LocalAccountsApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public partial class AccountController : ApiController
    {
        private IAuthenticationManager Authentication { get { return Request.GetOwinContext().Authentication; } }
        
        private static HttpClient client = new HttpClient();

        // POST api/Account/Login
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginBindingModel model)
        {
            try
            {
                var url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}/Token";
                var body = $"grant_type=password&username={model.Username}&password={model.Password}";
                var response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                var x = response.Content.ReadAsStringAsync();   // TODO: Can be customized later...

                return new ResponseMessageResult(response);
            }
            catch
            {
                throw new Exception("Error 500 - Internal Error!");
            }
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }
    }
}
