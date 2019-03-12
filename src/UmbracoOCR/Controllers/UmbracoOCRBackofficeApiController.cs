using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using File = System.IO.File;

namespace UmbracoOCR.Controllers
{
    [PluginController("UmbracoOCR")]
    public class UmbracoOCRBackofficeApiController : UmbracoAuthorizedJsonController
    {
        public Task<string> GetTextFromImageAsync()
        {
            string imageFilePath = @"D:\GitHub\Umbraco-OCR\src\UmbracoOCR.Web\img\umbraco-home-text.png"; 
            string subscriptionKey = ConfigurationManager.AppSettings["SubscriptionKey"];
            string uriBase = ConfigurationManager.AppSettings["ApiUrl"];
            if (File.Exists(imageFilePath))
            {
                // Call the REST API method.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                return MakeAnalysisRequest(imageFilePath, subscriptionKey, uriBase);
            }
            else
            {
                Console.WriteLine("\nInvalid file path");
            }

            return null;
        }

        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        public static async Task<string> MakeAnalysisRequest(string imageFilePath, string subscriptionKey, string uriBase)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters =
                    "visualFeatures=Categories,Description,Color";

                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                return contentString;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}