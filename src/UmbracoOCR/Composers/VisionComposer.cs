using Umbraco.Core;
using Umbraco.Core.Composing;
using UmbracoOCR.Services;

namespace UmbracoOCR.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class VisionComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IVisionService, VisionService>();
        }
    }
}
