using Litium.Globalization;
using Litium.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Distancify.LitiumAddOns.OData
{
    public class WebApiSetup : IWebApiSetup
    {
        private readonly LanguageService _languageService;

        public WebApiSetup(LanguageService languageService)
        {
            _languageService = languageService;
        }

        public void SetupWebApi(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new LanguageMessageHandler(_languageService));
        }
    }
}
