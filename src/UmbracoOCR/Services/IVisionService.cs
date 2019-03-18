using System.Threading.Tasks;

namespace UmbracoOCR.Services
{
    public interface IVisionService
    {
        Task<string> MakeAnalysisRequestAsync(string imageUri, string subscriptionKey, string uriBase);
    }
}
