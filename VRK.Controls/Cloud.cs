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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    #endregion

    /// <summary>
    /// The cloud.
    /// </summary>
    public class Cloud : CompositeDataBoundControl, IPostBackEventHandler
    {
        #region Constants and Fields

        /// <summary>
        /// The font sizes.
        /// </summary>
        private static readonly string[] FontSizes = new[]
            {
               "xx-small", "x-small", "small", "medium", "large", "x-large", "xx-large" 
            };

        /// <summary>
        /// The _items.
        /// </summary>
        private readonly Collection<CloudItem> items = new Collection<CloudItem>();

        #endregion

        #region Events

        /// <summary>
        /// The item click.
        /// </summary>
        public event EventHandler<CloudItemClickEventArgs> ItemClick;

        #endregion

        // static readonly string[] FontColors = new[] { "Black", "Blue", "Fuchsia", "Green", "Maroon", "Navy", "Purple" };
        #region Properties

        /// <summary>
        ///   Gets or sets the data field which is bound to the Href property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataHrefField
        {
            get
            {
                var val = this.ViewState["DataHrefField"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataHrefField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the format string to format the Href property value.
        /// </summary>
        [Category("Data")]
        public string DataHrefFormatString
        {
            get
            {
                var val = this.ViewState["DataHrefFormatString"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataHrefFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the name of the data field that is bound to the Text property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataTextField
        {
            get
            {
                var val = this.ViewState["DataTextField"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataTextField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the format string for the Text property.
        /// </summary>
        [Category("Data")]
        public string DataTextFormatString
        {
            get
            {
                var val = this.ViewState["DataTextFormatString"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataTextFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the data field which is bound to the Title property of an item.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataTitleField
        {
            get
            {
                var val = this.ViewState["DataTitleField"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataTitleField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets The format string for the title(tooltip) of an item. {0} in this string is replaced with the
        ///   value of the field specified as the DataTitleField.
        /// </summary>
        [Category("Data")]
        public string DataTitleFormatString
        {
            get
            {
                var val = this.ViewState["DataTitleFormatString"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataTitleFormatString"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///  Gets or sets The field from the Data Source where the weight of an item is to be obtained.
        /// </summary>
        [Category("Data")]
        [TypeConverter(typeof(DataFieldConverter))]
        public string DataWeightField
        {
            get
            {
                var val = this.ViewState["DataWeightField"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["DataWeightField"] = value;

                if (this.Initialized)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the prefix for CSS class names for individual items.
        /// </summary>
        [Category("Appearance")]
        public string ItemCssClassPrefix
        {
            get
            {
                var val = this.ViewState["ItemCssClassPrefix"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["ItemCssClassPrefix"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Separator for the Items.
        /// </summary>
        [Category("Appearance")]
        public string ItemSeparator
        {
            get
            {
                var val = this.ViewState["ItemSeparator"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["ItemSeparator"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Skin Name for the Cloud.
        /// </summary>
        [Category("Appearance")]
        public string Skin
        {
            get
            {
                var val = this.ViewState["Skin"] as string;

                return val ?? string.Empty;
            }

            set
            {
                this.ViewState["Skin"] = value;
            }
        }

        /// <summary>
        /// Gets the Collection of CloudItems. <see cref = "CloudItem" />
        /// </summary>
        [Themeable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public Collection<CloudItem> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Renders Tags as UL
        /// </summary>
        [Category("Appearance")]
        public bool RenderAsUl
        {
            get
            {
                var ret = false;
                var obj = this.ViewState["RenderAsUl"];
                if (obj != null)
                {
                    ret = Convert.ToBoolean(obj);
                }

                return ret;
            }

            set
            {
                this.ViewState["RenderAsUl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Renders Tags as UL inside a Canvas
        /// </summary>
        [Category("Appearance")]
        public bool CanvasEnabled
        {
            get
            {
                var ret = false;
                var obj = this.ViewState["CanvasEnabled"];
                if (obj != null)
                {
                    ret = Convert.ToBoolean(obj);
                }

                return ret;
            }

            set
            {
                this.ViewState["CanvasEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Render the Item Weigh
        /// </summary>
        [Category("Appearance")]
        public bool RenderItemWeight
        {
            get
            {
                var ret = false;
                var obj = this.ViewState["RenderItemWeight"];
                if (obj != null)
                {
                    ret = Convert.ToBoolean(obj);
                }

                return ret;
            }

            set
            {
                this.ViewState["RenderItemWeight"] = value;
            }
        }

        /// <summary>
        /// Gets TagKey.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return this.RenderAsUl ? HtmlTextWriterTag.Ul : HtmlTextWriterTag.Span;
            }
        }

        /// <summary>
        /// Gets ItemWeights.
        /// </summary>
        private IEnumerable<double> ItemWeights
        {
            get
            {
                return this.Items.Select(item => item.Weight);
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IPostBackEventHandler

        /// <summary>
        /// The raise post back event.
        /// </summary>
        /// <param name="eventArgument">
        /// The event argument.
        /// </param>
        public void RaisePostBackEvent(string eventArgument)
        {
            int selectedIndex;

            if (!Int32.TryParse(eventArgument, out selectedIndex))
            {
                return;
            }

            this.RequiresDataBinding = true;
            this.EnsureDataBound();

            if (selectedIndex >= 0 && selectedIndex < this.Items.Count)
            {
                this.OnItemClick(new CloudItemClickEventArgs(this.Items[selectedIndex]));
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The create child controls.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <param name="dataBinding">
        /// The data binding.
        /// </param>
        /// <returns>
        /// The create child controls.
        /// </returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            if (dataBinding && !this.DesignMode)
            {
                this.CreateItemsFromData(dataSource);
            }

            double mean;
            var stdDev = Statistics.StdDev(this.ItemWeights, out mean);

            var hasCssClassPrefix = !string.IsNullOrEmpty(this.ItemCssClassPrefix);
            var index = 0;
            var index2 = 10;

            foreach (var item in this.Items)
            {
                /*HtmlAnchor a = new HtmlAnchor
                                   {
                                      HRef = string.IsNullOrEmpty(item.Href)
                                                  ?
                                                      Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString())
                                                  :

                                                      item.Href,
                                       InnerText = item.Text,
                                       Title = item.Title
                                   };*/

                // UL Wrapper
                var li = new HtmlGenericControl("li");

                var a = new HtmlAnchor
                                   {
                                       HRef = item.Href,
                                       InnerHtml =
                                           this.RenderItemWeight
                                               ? string.Format(
                                                   this.Skin.Contains("Sliding")
                                                       ? "{0}&nbsp;<span>{1}</span>"
                                                       : "{0}&nbsp;<span>({1})</span>",
                                                   item.Text,
                                                   item.Weight)
                                               : item.Text,
                                       Title = item.Title
                                   };

                var normalWeight = NormalizeWeight(item.Weight, mean, stdDev);

                if (hasCssClassPrefix)
                {
                    if (this.RenderAsUl)
                    {
                        // li.Attributes["class"] = string.Format("{0}_{1}", ItemCssClassPrefix, rnd.Next(7));
                        li.Attributes["class"] = string.Format("{0}_{1}", this.ItemCssClassPrefix, normalWeight - 1);

                        if (this.CanvasEnabled)
                        {
                            a.Attributes.Add(
                                "data-weight",
                                normalWeight < 10 ? (item.Weight + 15).ToString() : item.Weight.ToString());
                        }
                    }
                    else
                    {
                        // a.Attributes["class"] = string.Format("{0}_{1}", ItemCssClassPrefix, rnd.Next(7));
                        a.Attributes["class"] = string.Format("{0}_{1}", this.ItemCssClassPrefix, normalWeight - 1);
                    }
                }

                a.Style.Add(HtmlTextWriterStyle.FontSize, FontSizes[normalWeight - 1]);

                // a.Style.Add("color", FontColors[rnd.Next(7)]);
                if (this.RenderAsUl)
                {
                    li.Controls.Add(a);
                    this.Controls.Add(li);
                }
                else
                {
                    this.Controls.Add(a);
                }

                // TAG Separator
                // Controls.Add(new LiteralControl(" "));
                if (index != this.Items.Count - 1)
                {
                    if (index == index2)
                    {
                        if (this.RenderAsUl)
                        {
                            li.Controls.Add(new LiteralControl(this.ItemSeparator));
                        }
                        else
                        {
                            this.Controls.Add(new LiteralControl(string.Format("&nbsp;{0}", this.ItemSeparator)));
                        }

                        // Controls.Add(new LiteralControl(string.Format("&nbsp;{0}<br />", ItemSeparator)));
                        index2 = index2 + 10;
                    }
                    else
                    {
                        if (this.RenderAsUl)
                        {
                            li.Controls.Add(new LiteralControl(this.ItemSeparator));
                        }
                        else
                        {
                            this.Controls.Add(new LiteralControl(string.Format("&nbsp;{0}", this.ItemSeparator)));
                        }
                    }
                }

                index++;
            }

            if (this.DesignMode && this.Items.Count == 0)
            {
                var a = new HtmlAnchor { HRef = "javascript:void(0)", InnerText = SR.Cloud };
                this.Controls.Add(a);
            }

            return this.Items.Count;
        }

        /// <summary>
        /// The on item click.
        /// </summary>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        protected void OnItemClick(CloudItemClickEventArgs e)
        {
            if (this.ItemClick != null)
            {
                this.ItemClick(this, e);
            }
        }

        /// <summary>
        /// The normalize weight.
        /// </summary>
        /// <param name="weight">
        /// The weight.
        /// </param>
        /// <param name="mean">
        /// The mean.
        /// </param>
        /// <param name="stdDev">
        /// The std dev.
        /// </param>
        /// <returns>
        /// The normalize weight.
        /// </returns>
        private static int NormalizeWeight(double weight, double mean, double stdDev)
        {
            var factor = weight - mean;

            if (factor != 0 && stdDev != 0)
            {
                factor /= stdDev;
            }

            return (factor > 2)
                       ? 7
                       : (factor > 1)
                             ? 6
                             : (factor > 0.5) ? 5 : (factor > -0.5) ? 4 : (factor > -1) ? 3 : (factor > -2) ? 2 : 1;
        }

        /// <summary>
        /// The create items from data.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        private void CreateItemsFromData(IEnumerable dataSource)
        {
            foreach (var data in dataSource)
            {
                var item = new CloudItem();

                if (string.IsNullOrEmpty(this.DataHrefField))
                {
                    if (string.IsNullOrEmpty(this.DataHrefFormatString))
                    {
                        item.Href = String.Empty;
                    }
                    else
                    {
                        string.Format(CultureInfo.CurrentCulture, this.DataHrefFormatString, new[] { data });
                    }
                }
                else
                {
                    item.Href = DataBinder.Eval(data, this.DataHrefField, this.DataHrefFormatString);
                }

                if (!string.IsNullOrEmpty(this.DataTextField))
                {
                    item.Text = DataBinder.Eval(data, this.DataTextField, this.DataTextFormatString);
                }

                if (!string.IsNullOrEmpty(this.DataTitleField))
                {
                    item.Title = DataBinder.Eval(data, this.DataTitleField, this.DataTitleFormatString);
                }

                if (!string.IsNullOrEmpty(this.DataWeightField))
                {
                    item.Weight = Convert.ToDouble(DataBinder.GetPropertyValue(data, this.DataWeightField));
                }

                this.Items.Add(item);
            }
        }

        /// <summary>
        /// The should serialize data href field.
        /// </summary>
        /// <returns>
        /// The should serialize data href field.
        /// </returns>
        private bool ShouldSerializeDataHrefField()
        {
            return !string.IsNullOrEmpty(this.DataHrefField);
        }

        /// <summary>
        /// The should serialize data href format string.
        /// </summary>
        /// <returns>
        /// The should serialize data href format string.
        /// </returns>
        private bool ShouldSerializeDataHrefFormatString()
        {
            return !string.IsNullOrEmpty(this.DataHrefFormatString);
        }

        /// <summary>
        /// The should serialize data text format string.
        /// </summary>
        /// <returns>
        /// The should serialize data text format string.
        /// </returns>
        private bool ShouldSerializeDataTextFormatString()
        {
            return !string.IsNullOrEmpty(this.DataTextFormatString);
        }

        /// <summary>
        /// The should serialize data title field.
        /// </summary>
        /// <returns>
        /// The should serialize data title field.
        /// </returns>
        private bool ShouldSerializeDataTitleField()
        {
            return !string.IsNullOrEmpty(this.DataTitleField);
        }

        /// <summary>
        /// The should serialize data title format string.
        /// </summary>
        /// <returns>
        /// The should serialize data title format string.
        /// </returns>
        private bool ShouldSerializeDataTitleFormatString()
        {
            return !string.IsNullOrEmpty(this.DataTitleFormatString);
        }

        /// <summary>
        /// The should serialize data weight field.
        /// </summary>
        /// <returns>
        /// The should serialize data weight field.
        /// </returns>
        private bool ShouldSerializeDataWeightField()
        {
            return !string.IsNullOrEmpty(this.DataWeightField);
        }

        /// <summary>
        /// The should serialize item css class prefix.
        /// </summary>
        /// <returns>
        /// The should serialize item css class prefix.
        /// </returns>
        private bool ShouldSerializeItemCssClassPrefix()
        {
            return !string.IsNullOrEmpty(this.ItemCssClassPrefix);
        }

        /// <summary>
        /// The should serialize item separator.
        /// </summary>
        /// <returns>
        /// The should serialize item separator.
        /// </returns>
        private bool ShouldSerializeItemSeparator()
        {
            return !string.IsNullOrEmpty(this.ItemSeparator);
        }

        /// <summary>
        /// The should serialize render as ul.
        /// </summary>
        /// <returns>
        /// The should serialize render as ul.
        /// </returns>
        private bool ShouldSerializeRenderAsUl()
        {
            return this.RenderAsUl;
        }

        #endregion
    }
}