/*  *********************************************************************************************
*
*   Cloud Control for ASP.NET
*   http://www.codeproject.com/KB/custom-controls/cloud.aspx
*   By Rama Krishna Vavilala
*
*    This cloud control displays a list of hyperlinks in varying styles depending on a weight. 
*  This is similar to tag clouds in del.icio.us or flickr.
*
*   VRK.Controls is under the The Code Project Open License (CPOL)
*   http://www.codeproject.com/info/cpol10.aspx
*
*  *********************************************************************************************
*/

namespace WatchersNET.DNN.Modules.TagCloud.VRK.Controls
{
    #region

    using System;

    #endregion

    /// <summary>
    /// The cloud item.
    /// </summary>
    public class CloudItem
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudItem"/> class.
        /// </summary>
        public CloudItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudItem"/> class.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        public CloudItem(string text, double weight)
        {
            this.Text = text;
            this.Weight = weight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudItem"/> class.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="href">
        /// The href.
        /// </param>
        public CloudItem(string text, double weight, string href)
            : this(text, weight)
        {
            this.Href = href;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudItem"/> class.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="href">
        /// The href.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public CloudItem(string text, double weight, string href, string title)
            : this(text, weight, href)
        {
            this.Title = title;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets The address of the HTML anchor.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        ///   Gets or sets the text for individual hyperlinks.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the title (tooltip) of the HTML anchor.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the weight of the item.
        /// </summary>
        public double Weight { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The should serialize href.
        /// </summary>
        /// <returns>
        /// The should serialize href.
        /// </returns>
        private bool ShouldSerializeHref()
        {
            return !string.IsNullOrEmpty(this.Href);
        }

        /// <summary>
        /// The should serialize text.
        /// </summary>
        /// <returns>
        /// The should serialize text.
        /// </returns>
        private bool ShouldSerializeText()
        {
            return !string.IsNullOrEmpty(this.Text);
        }

        /// <summary>
        /// The should serialize title.
        /// </summary>
        /// <returns>
        /// The should serialize title.
        /// </returns>
        private bool ShouldSerializeTitle()
        {
            return !string.IsNullOrEmpty(this.Title);
        }

        /// <summary>
        /// The should serialize weight.
        /// </summary>
        /// <returns>
        /// The should serialize weight.
        /// </returns>
        private bool ShouldSerializeWeight()
        {
            return this.Weight != 0;
        }

        #endregion
    }
}