using Microsoft.Owin.Security.OAuth;
using NorthwindWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NorthwindWebApi.OAuth.Providers
{
    public class SimpleAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        // OAuthAuthorizationServerProvider sınıfının client erişimine izin verebilmek için ilgili ValidateClientAuthentication metotunu override ediyoruz.
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Burada client validation kullanmadık. İstersek custom client tipleri ile client tipine görede validation sağlayabiliriz.

            context.Validated();
        }

        // OAuthAuthorizationServerProvider sınıfının kaynak erişimine izin verebilmek için ilgili GrantResourceOwnerCredentials metotunu override ediyoruz.
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Domainler arası etkileşim ve kaynak paylaşımını sağlayan ve bir domainin bir başka domainin kaynağını kullanmasını sağlayan CORS ayarlarını set ediyoruz.     
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            // Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
            //Burada kendi authentication yöntemimizi belirleyebiliriz.Veritabanı bağlantısı vs...

            using (NorthwindDbContext db =new NorthwindDbContext())
            {
                if (db.Users.Any(x => x.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && x.Password == context.Password))
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    var user = db.Users.First(x => x.UserName.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && x.Password == context.Password);
                    identity.AddClaim(new Claim("UserName", user.UserName));
                    identity.AddClaim(new Claim("Role", user.Role));

                    var scopes = context.Scope[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    identity.AddClaim(new Claim("Lang",scopes[0]));
                    context.Validated(identity);// identity validate edilmezse Bad Request {"error"="invalid grant"} hatası alıyoruz.
                }
                //if (context.UserName == "Hilal" && context.Password == "123456") //Eğer kullanıcı geçerli bir kullanıcı ise bir kimlik yaratıp, context üzerinde doğruluyor.
                //{
                //    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //    identity.AddClaim(new Claim("sub", context.UserName));
                //    identity.AddClaim(new Claim("role", "user"));

                //    context.Validated(identity);
                //}
                //else
                //{
                //    context.SetError("invalid_grant", "Kullanıcı adı veya şifre yanlış.");
                //} 
            }
        }
    }
}