using Litium.Globalization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Distancify.LitiumAddOns.OData
{
    internal class LanguageMessageHandler : DelegatingHandler
    {
        private readonly LanguageService _languageService;

        public LanguageMessageHandler(LanguageService languageService)
        {
            _languageService = languageService;
        }

        private bool SetHeaderIfAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                var match = _languageService.GetAll().FirstOrDefault(r => r.Id == lang.Value);
                if (match != null)
                {
                    SetCulture(request, match);
                    return true;
                }
            }

            return false;
        }

        private bool SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                var globalLang = lang.Value.Substring(0, 2);
                var match = _languageService.GetAll().FirstOrDefault(r => r.Id.StartsWith(globalLang));
                if (match != null)
                {
                    SetCulture(request, match);
                    return true;
                }
            }

            return false;
        }

        private void SetCulture(HttpRequestMessage request, Language language)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(language.Id));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language.Id);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language.Id);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.PathAndQuery.StartsWith("/odata"))
            {
                if (!SetHeaderIfAcceptLanguageMatchesSupportedLanguage(request))
                {
                    if (!SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(request))
                    {
                        SetCulture(request, _languageService.GetDefault());
                    }
                }
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
