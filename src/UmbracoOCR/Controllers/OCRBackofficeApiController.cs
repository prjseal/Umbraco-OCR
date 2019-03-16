using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace UmbracoOCR.Controllers
{
    [PluginController("OCR")]
    public class OcrBackofficeApiController : UmbracoAuthorizedJsonController
    {
        [System.Web.Http.HttpPost]
        public Task<string> GetTextFromImage(UmbracoOCRInstruction umbracoOCRInstruction)
        {
            string subscriptionKey = ConfigurationManager.AppSettings["OcrSubscriptionKey"];
            string uriBase = ConfigurationManager.AppSettings["OcrApiUrl"];
            return MakeAnalysisRequest(umbracoOCRInstruction.ImageUri, subscriptionKey, uriBase);
        }

        public static async Task<string> MakeAnalysisRequest(string imageUri, string subscriptionKey, string uriBase)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "visualFeatures=Categories,Description,Color";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;

                var match = Regex.Match(imageUri, @"data:image/(?<type>.+?);base64,(?<data>.+)");
                if (!match.Success) throw new ArgumentException("Invalid data uri", "dataUri");
                var base64Data = match.Groups["data"].Value;
                var byteData = Convert.FromBase64String(base64Data);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                string contentString = await response.Content.ReadAsStringAsync();
                return contentString;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public class UmbracoOCRInstruction
        {
            public string ImageUri { get; set; }
        }
    }
}