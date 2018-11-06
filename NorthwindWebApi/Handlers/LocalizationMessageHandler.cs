using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NorthwindWebApi.Handlers
{
    public class LocalizationMessageHandler : DelegatingHandler
    {
        //oluşturmuş olduğumuz LocalizationMessageHandler’ın pipeline içerisinde devreye girebilmesi için WebApiConfig içerisinde MessageHandlers listesine eklememiz gerekmektedir.
        private readonly List<string> _supportedLanguages = new List<string>() { "tr-TR", "en-US" };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SetCulture(request);

            var response=await base.SendAsync(request, cancellationToken);
            return response;

        }

        private void SetCulture(HttpRequestMessage request)
        {
            foreach (var loopLanguage in request.Headers.AcceptLanguage)
            {
                // Desteklediğimiz dillerden biri var ise culture bilgisini o dile göre güncelliyoruz.
                if (_supportedLanguages.Contains(loopLanguage.Value))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(loopLanguage.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(loopLanguage.Value);

                    break;
                }

            }
        }
    }
}