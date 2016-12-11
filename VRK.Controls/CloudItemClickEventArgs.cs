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

namespace VRK.Controls
{
    #region

    using System;

    #endregion

    /// <summary>
    /// The cloud item click event args.
    /// </summary>
    public class CloudItemClickEventArgs : EventArgs
    {
        #region Constants and Fields

        /// <summary>
        /// The _item.
        /// </summary>
        private readonly CloudItem cloudItem;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudItemClickEventArgs"/> class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        internal CloudItemClickEventArgs(CloudItem item)
        {
            this.cloudItem = item;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the item which is clicked.
        /// </summary>
        public CloudItem Item
        {
            get
            {
                return this.cloudItem;
            }
        }

        #endregion
    }
}