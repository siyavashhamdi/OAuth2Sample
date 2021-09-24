using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LocalAccountsApp.Models;

namespace LocalAccountsApp.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException("publicClientId");
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await Helpers.Common.GetUserFromRepo(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
            var oAuthClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            var properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthClaimIdentity, properties);

            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            // Note: If there is no need to have extra fields, this method can be deleted

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        private static AuthenticationProperties CreateProperties(string username)
        {
            var data = new Dictionary<string, string> { { "userName", username } };

            return new AuthenticationProperties(data);
        }
    }
}
