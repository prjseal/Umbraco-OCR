using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;
using UmbracoOCR.Controllers;

namespace UmbracoOCR.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class OcrComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // Append our component to the collection of Components
            // It will be the last one to be run
            composition.Components().Append<OcrComponent>();
        }
    }

    public class OcrComponent : IComponent
    {
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            //Create a .NET MVC URL Helper
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
                            controller => controller.GetTextFromImage(new OcrBackofficeApiController.UmbracoOCRInstruction() { ImageUri = ""}))
                    }
                });
        }
    }
}