using System.Configuration;
using System.Threading.Tasks;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using UmbracoOCR.Services;

namespace UmbracoOCR.Controllers
{
    [PluginController("OCR")]
    public class OcrBackofficeApiController : UmbracoAuthorizedJsonController
    {
        private readonly IVisionService _visionService;

        public OcrBackofficeApiController(IVisionService visionService)
        {
            _visionService = visionService;
        }

        [System.Web.Http.HttpPost]
        public Task<string> GetTextFromImage(ApiInstruction apiInstruction)
        {
            string subscriptionKey = ConfigurationManager.AppSettings["OcrSubscriptionKey"];
            string uriBase = ConfigurationManager.AppSettings["OcrApiUrl"];
            return _visionService.MakeAnalysisRequestAsync(apiInstruction.ImageUri, subscriptionKey, uriBase);
        }
        
        public class ApiInstruction
        {
            public string ImageUri { get; set; }
        }
    }
}