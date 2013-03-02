using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Plugins.ImageSlider.Utils
{
    public enum ThumbLagerThanOriginalImageBehavior
    {
        /// <summary>
        /// Zoom out the original image.
        /// </summary>
        ZoomOut = 0,

        /// <summary>
        /// Keep original size.
        /// </summary>
        Ignore = 1
    }
}
