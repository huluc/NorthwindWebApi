using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using NorthwindWebApi.OAuth.Providers;
using Owin;
//tanımladığımız assembly ile serverımız ilk çalıştığında çalıştırılacak klasımızı tanımladık.
[assembly: OwinStartup(typeof(NorthwindWebApi.OAuth.Startup))]

namespace NorthwindWebApi.OAuth
{
    // Servis çalışmaya başlarken Owin pipeline'ını ayağa kaldırabilmek için Startup'u hazırlıyoruz.
    public class Startup
    {
        //Configuration kısmında yer alan parametre ise bu çağrılma sırasında host tarafından sağlanmakta. 
        public void Configuration(IAppBuilder app)
        {
           
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            //Token üretimi için authorization ayarlarını belirliyoruz.
            ConfigureOAuth(app);

            //HttpConfiguration ayarlarımızı yaptıktan sonra App_Start’da yer alan Register metodumuzu bu configrasyon ile çağıracağız.Bu işlemin ardından projemizde yer alan Global.asax.cs dosyamızın bulunmasına gerek kalmamaktadır. Çünkü startup işlemini artık buradan sağlıyoruz.
            WebApiConfig.Register(httpConfiguration);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfiguration);
        }

        //Token üretimi için authorization ayarlarını belirliyoruz.
        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new Microsoft.Owin.PathString("/token"), // geçerli token alacağımız adresi belirtiyoruz.
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),//sağlanan token’ın geçerlilik süresini belirtir.
                AllowInsecureHttp = true,//token requestlerin HTTPS olmayan protokollere açılmasını sağlar.
                Provider = new SimpleAuthorizationServerProvider()
            };

            // AppBuilder'a token üretimini gerçekleştirebilmek için ilgili authorization ayarlarımızı veriyoruz.
            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);

            // Authentication type olarak ise Bearer Authentication'ı kullanacağımızı belirtiyoruz.
            // Bearer token OAuth 2.0 ile gelen standartlaşmış token türüdür.
            // Herhangi kriptolu bir veriye ihtiyaç duymadan client tarafından token isteğinde bulunulur ve server belirli bir expire date'e sahip bir access_token üretir.
            // Bearer token üzerinde güvenlik SSL'e dayanır.
            // Bir diğer tip ise MAC token'dır. OAuth 1.0 versiyonunda kullanılıyor, hem client'a, hemde server tarafına implementasyonlardan dolayı ek maliyet çıkartmaktadır. Bu maliyetin yanı sıra ise Bearer token'a göre kaynak alış verişinin biraz daha güvenli olduğu söyleniyor çünkü client her request'inde veriyi hmac ile imzalayıp verileri kriptolu bir şekilde göndermeleri gerektiği için.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
