using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;
using UmbracoOCR.Controllers;

namespace UmbracoOCR.Components
{
    public class OcrComponent : IComponent
    {
        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        public void Terminate()
        {
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            var urlHelper =
                new UrlHelper(
                    new RequestContext(
                        new HttpContextWrapper(
                            HttpContext.Current),
                        new RouteData()));

            if (!e.ContainsKey("OCR"))
                e.Add("OCR", new Dictionary<string, object>
                {
                    {
                        "OcrApiUrl",
                        urlHelper.GetUmbracoApiServiceBaseUrl<OcrBackofficeApiController>(
                            controller => controller.GetTextFromImage(new OcrBackofficeApiController.ApiInstruction { ImageUri = string.Empty }))
                    }
                });
        }
    }
}
