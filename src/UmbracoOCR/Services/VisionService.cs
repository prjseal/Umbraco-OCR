using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UmbracoOCR.Services
{
    public class VisionService : IVisionService
    {
        public async Task<string> MakeAnalysisRequestAsync(string imageUri, string subscriptionKey, string uriBase)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                HttpResponseMessage response;

                var byteData = GetBytesFromImageDataUri(imageUri);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uriBase, content);
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private byte[] GetBytesFromImageDataUri(string imageDataUri)
        {
            var match = Regex.Match(imageDataUri, @"data:image/(?<type>.+?);base64,(?<data>.+)");
            if (!match.Success) throw new ArgumentException("Invalid data uri", "dataUri");
            var base64Data = match.Groups["data"].Value;
            return Convert.FromBase64String(base64Data);
        }
    }
}
