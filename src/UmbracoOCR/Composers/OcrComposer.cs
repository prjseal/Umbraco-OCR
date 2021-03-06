﻿using Umbraco.Core;
using Umbraco.Core.Composing;
using UmbracoOCR.Components;

namespace UmbracoOCR.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class OcrComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<OcrComponent>();
        }
    }
}