/*  *********************************************************************************************
*
*   WatchersNET.TagCloud - This Module displays the most frequently used words (Tags) of your Portal as a 
*   standard Web 2.0 Tag Cloud, or You can define your own Tags list.  The Tags are links which linked to the Portal Search to 
*   show all Pages with that Tag.
*
*   The Tag Cloud will be rendered as 3D Flash Cloud, and as alternative for Non Flash 
*   Users as a list of hyperlinks in varying styles depending on a weight. 
*   This is similar to tag clouds in del.icio.us or Flickr.
*
*   Copyright(c) Ingo Herbote (thewatcher@watchersnet.de)
*   All rights reserved.
*   Internet: http://www.watchersnet.de/TagCloud
*
*   WatchersNET.TagCloud is released under the New BSD License, see below
************************************************************************************************
*
*   Redistribution and use in source and binary forms, with or without modification, 
*   are permitted provided that the following conditions are met:
*
*   * Redistributions of source code must retain the above copyright notice,
*   this list of conditions and the following disclaimer.
*
*   * Redistributions in binary form must reproduce the above copyright notice, 
*   this list of conditions and the following disclaimer in the documentation and/
*   or other materials provided with the distribution.
*
*   * Neither the name of WatchersNET nor the names of its contributors 
*   may be used to endorse or promote products derived from this software without 
*   specific prior written permission.
*
*   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
*   ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
*   OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
*   IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
*   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
*   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
*   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
*   LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
*   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*
************************************************************************************************
*/

namespace WatchersNET.DNN.Modules.TagCloud.Objects
{
    using WatchersNET.DNN.Modules.TagCloud.Constants;

    /// <summary>
    /// World Cloud Settings
    /// </summary>
    public class WordCloudSettings
    {
        /// <summary>
        /// Gets or sets the ellipticity of the circle formed by word.
        /// </summary>
        /// <value>
        /// The ellipticity.
        /// </value>
        public double Ellipticity { get; set; }

        /// <summary>
        /// Gets or sets the weight factor.
        /// </summary>
        /// <value>
        /// The weight factor.
        /// </value>
        public double WeightFactor { get; set; }

        /// <summary>
        /// Gets or sets the size of the grid.
        /// </summary>
        /// <value>
        /// The size of the grid.
        /// </value>
        public int GridSize { get; set; }

        /// <summary>
        /// Gets or sets the minium font size in pixel to draw (default: $.miniumFontSize / 2, larger than that is still look good using bilinear sampling in browser)
        /// </summary>
        /// <value>
        /// The size of the min.
        /// </value>
        public int MinSize { get; set; }

        /// <summary>
        /// Gets or sets the background color to cover entire canvas or the detect against.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the keyword or a function that represents polar equation r = fn(theta), available keywords:
        /// 'circle', (default)
        /// 'cardioid', (apple or heart shape curve, the most known polar equation)
        /// 'diamond', (alias: 'square'),
        /// 'triangle-forward',
        /// 'triangle', (alias: 'triangle-upright')
        /// 'pentagon',
        /// 'star'
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public WordCloudShape Shape { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether fill box.
        /// true will mark the entire box containing the word as filled - no subsequent smaller words can be fit in the gap.
        /// </summary>
        /// <value>
        /// The fill box.
        /// </value>
        public bool FillBox { get; set; }
    }
}