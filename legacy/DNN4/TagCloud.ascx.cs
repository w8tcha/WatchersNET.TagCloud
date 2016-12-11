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

namespace WatchersNET.DNN.Modules.TagCloud
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Framework;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Search;

    using Ventrian.SimpleGallery.Entities;

    using VRK.Controls;

    using WatchersNET.DNN.Modules.TagCloud.Constants;
    using WatchersNET.DNN.Modules.TagCloud.Objects;
    using DotNetNuke.Entities.Portals;

    #endregion

    /// <summary>
    /// The tag cloud.
    /// </summary>
    public partial class TagCloud : ModuleSettingsBase
    {
        #region Constants and Fields

        /// <summary>
        /// The font sizes.
        /// </summary>
        private readonly string[] fontSizes = new[] { "6.94", "8.3", "10", "12", "14.4", "17.3", "20.7" };

        /// <summary>
        /// The Module Settings
        /// </summary>
        private TagCloudSettings settings;

        /// <summary>
        /// The tag list items.
        /// </summary>
        private List<CloudItem> tagListItems = new List<CloudItem>();

        /// <summary>
        /// The words.
        /// </summary>
        private List<TagWords> words = new List<TagWords>();

        /// <summary>
        /// Gets or sets The tab module id.
        /// </summary>
        public static int CurrentTabModuleId { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets The item weights.
        /// </summary>
        private IEnumerable<double> ItemWeights
        {
            get
            {
                return this.tagListItems.Select(item => item.Weight);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Main Entry to load Tags
        /// </summary>
        /// <returns>
        /// Item List
        /// </returns>
        public List<CloudItem> GetItems()
        {
            string sLogCacheKey = string.Format("CloudItems{0}", this.TabModuleId);

            List<CloudItem> itemList;

            if (DataCache.GetCache(sLogCacheKey) != null)
            {
                // Get Items From Cache
                itemList = (List<CloudItem>)DataCache.GetCache(sLogCacheKey);

                if (itemList.Count == 0)
                {
                    itemList = this.GetItemsFromDb();
                }
            }
            else
            {
                // Get Items From DB
                itemList = this.GetItemsFromDb();
            }

            switch (this.settings.renderMode)
            {
                case RenderMode.Flash:
                    {
                        this.tagListItems = itemList;
                        this.AddFlashScript();
                    }

                    break;
                case RenderMode.HTML5:
                    {
                        this.AddCanvasScript();
                    }

                    break;
                case RenderMode.WordCloud:
                    {
                        this.AddWordCloudScript(itemList);
                    }

                    break;
            }

            return itemList;
        }

        /// <summary>
        /// Get Tag Items From the Database
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        public List<CloudItem> GetItemsFromDb()
        {
            List<CloudItem> list = new List<CloudItem>();

            if (this.settings.ModeCustom)
            {
                if (this.settings.TagsLinkChk)
                {
                    this.settings.CacheItems = false;
                }

                list.AddRange(this.GetItemsFromCustom());
            }

            if (this.settings.ModeReferrals)
            {
                this.settings.TagsLinkChk = false;
                list.AddRange(this.GetItemsFromReferrals());
            }

            if (this.settings.ModeNewsarticles && Utility.IsNewsArticlesInstalled())
            {
                this.settings.TagsLinkChk = false;
                list.AddRange(this.GetItemsFromNewsArticles());
            }

            if (this.settings.ModeSimplegallery && Utility.IsSimplyGalleryInstalled())
            {
                this.settings.TagsLinkChk = false;
                list.AddRange(this.GetItemsFromSimpleGallery());
            }

            if (this.settings.ModeActiveforums && Utility.IsActiveForumsInstalled())
            {
                this.settings.TagsLinkChk = false;
                list.AddRange(this.GetItemsFromActiveForums());
            }

            if (this.settings.ModeSearch)
            {
                if (this.settings.TagsLinkChk)
                {
                    this.settings.CacheItems = false;
                }

                list.AddRange(this.GetItemsFromSql());
            }

            // Exclude Words
            if (this.settings.exlusionWords.Count > 0)
            {
                foreach (ExcludeWord exWord in this.settings.exlusionWords)
                {
                    string value = exWord.Word.ToLower();

                    switch (exWord.ExcludeWordType)
                    {
                        case ExcludeType.Contains:
                            {
                                list.RemoveAll(check => check.Text.ToLower().Contains(value));
                            }

                            break;
                        case ExcludeType.Equals:
                            {
                                list.RemoveAll(check => check.Text.ToLower().Equals(value));
                            }

                            break;
                        case ExcludeType.StartsWith:
                            {
                                list.RemoveAll(check => check.Text.ToLower().StartsWith(value));
                            }

                            break;
                        case ExcludeType.EndsWith:
                            {
                                list.RemoveAll(check => check.Text.ToLower().EndsWith(value));
                            }

                            break;
                    }
                }
            }

            // Make Sure the Tag Count is correct
            if (list.Count > this.settings.TagCount)
            {
                var removeIndex = list.Count - this.settings.TagCount;
                list.RemoveRange(this.settings.TagCount, removeIndex);
            }

            // Sort the List?
            switch (this.settings.CloudSortType)
            {
                case SortType.AlphabeticAsc:
                    {
                        Utility.SortAscending(list, item => item.Text);
                    }

                    break;
                case SortType.AlphabeticDsc:
                    {
                        Utility.SortDescending(list, item => item.Text);
                    }

                    break;
                case SortType.WeightedAsc:
                    {
                        Utility.SortAscending(list, item => item.Weight);
                    }

                    break;
                case SortType.WeightedDsc:
                    {
                        Utility.SortDescending(list, item => item.Weight);
                    }

                    break;
            }

            if (this.settings.CacheItems)
            {
                DataCache.SetCache(string.Format("CloudItems{0}", this.TabModuleId), list);
            }

            return list;
        }

        /// <summary>
        /// Get List from Site Log and generate Tags from Referrals
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        public List<CloudItem> GetItemsFromReferrals()
        {
            List<CloudItem> list = new List<CloudItem>();

            try
            {
                List<ReferrerItems> sqlList = DataControl.TagCloudSiteLogInfo(
                    this.PortalId, this.PortalAlias.ToString(), this.settings.StartDate, DateTime.Now);

                List<CustomTags> searchTagsList = new List<CustomTags>();

                ///////////////////////
                for (int ib = 0; ib != sqlList.Count; ib++)
                {
                    string keyword = Utility.SortKeyWord(sqlList[ib].Referrer);

                    // Split Keywords
                    if (keyword.Contains("+"))
                    {
                        string[] keywords = keyword.Split('+');

                        foreach (string sNewKeyword in keywords)
                        {
                            string newKeyword = sNewKeyword;
                            CustomTags oldTag =
                                searchTagsList.Find(check => (check.sTag.ToLower() == newKeyword.ToLower()));

                            if (oldTag != null)
                            {
                                oldTag.iWeight = oldTag.iWeight + 1;
                            }
                            else
                            {
                                CustomTags tag = new CustomTags { sTag = newKeyword, iWeight = 1 };
                                searchTagsList.Add(tag);
                            }
                        }
                    }
                    else
                    {
                        CustomTags oldTag = searchTagsList.Find(check => (check.sTag.ToLower() == keyword.ToLower()));

                        if (oldTag != null)
                        {
                            oldTag.iWeight = oldTag.iWeight + 1;
                        }
                        else
                        {
                            CustomTags tag = new CustomTags { sTag = keyword, iWeight = 1 };
                            searchTagsList.Add(tag);
                        }
                    }
                }

                this.words = DataControl.GetTagWords(this.settings.TagCount, PortalSettings.PortalId);

                ////////////////////////
                foreach (CustomTags tag in searchTagsList)
                {
                    // Check
                    if (Utility.IsUrl(tag.sTag) || Utility.IsNumeric(tag.sTag) || Utility.ContainsNumber(tag.sTag)
                        || !Utility.IsLengthOk(tag.sTag))
                    {
                        continue;
                    }

                    CustomTags tag1 = tag;
                    TagWords word = this.words.Find(check => (check.Word.ToLower() == tag1.sTag.ToLower()));

                    int iKeyWCountOnSite = tag.iWeight;

                    // Check)
                    if (tag.iWeight < this.settings.OccurCount)
                    {
                        continue;
                    }

                    CloudItem entry;

                    if (this.settings.TagsLink && word != null)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.sTag),
                            tag.iWeight,
                            string.Format(
                                "{0}?{2}{1}",
                                Globals.NavigateURL(this.settings.searchTabId),
                                Utility.RemoveIllegalCharecters(tag.sTag),
                                this.settings.SearchQueryString),
                            string.Format(
                                "Tag \"{0}\" searched {1} times",
                                Utility.RemoveIllegalCharecters(tag.sTag),
                                iKeyWCountOnSite));
                    }
                    else
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.sTag), 
                            tag.iWeight, 
                            null, 
                            string.Format(
                                "Tag \"{0}\" searched {1} times", 
                                Utility.RemoveIllegalCharecters(tag.sTag), 
                                iKeyWCountOnSite));
                    }

                    if (list.Count > this.settings.TagCount)
                    {
                        return list;
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            CurrentTabModuleId = this.TabModuleId;

            this.InitializeComponent();
            base.OnInit(e);
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
        /// The std Dev.
        /// </param>
        /// <returns>
        /// Returns the normalized weight.
        /// </returns>
        private static int NormalizeWeight(double weight, double mean, double stdDev)
        {
            double factor = weight - mean;

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
        ///  Check if the User can see the Page where the Word (Tag) came from
        /// </summary>
        /// <param name="iPortalId">
        ///  Portal ID
        /// </param>
        /// <param name="sWord">
        ///  Word to Check
        /// </param>
        /// <returns>
        ///  Returns if the User can see the Page or not.
        /// </returns>
        private static bool UserSeeTag(int iPortalId, string sWord)
        {
            bool bCanSee = false;
            SearchResultsInfoCollection results = SearchDataStoreProvider.Instance().GetSearchResults(iPortalId, sWord);

            foreach (TabInfo objTab in from SearchResultsInfo result in results
                                       let tabcontroller = new TabController()
                                       select tabcontroller.GetTab(result.TabId, iPortalId, true)
                                       into objTab
                                       where PortalSecurity.IsInRoles(objTab.AuthorizedRoles) 
                                       select objTab)
            {
                bCanSee = true;
            }

            return bCanSee;
        }

        /// <summary>
        /// Check if the User can see the Page
        /// </summary>
        /// <param name="iPortalId">
        /// Portal ID
        /// </param>
        /// <param name="iTabId">
        /// Tab to Check
        /// </param>
        /// <returns>
        /// Returns if the User can see the Page or not.
        /// </returns>
        private static bool UserSeeTag(int iPortalId, int iTabId)
        {
            bool bCanSee = false;

            TabController tabcontroller = new TabController();

            TabInfo objTab = tabcontroller.GetTab(iTabId, iPortalId, true);

            // Get The Tab
            if (PortalSecurity.IsInRoles(objTab.AuthorizedRoles))
            {
                bCanSee = true;
            }

            return bCanSee;
        }

        /// <summary>
        /// Build Entire Script Link and Block for the HTML5 Canvas Tag Cloud
        /// </summary>
        private void AddCanvasScript()
        {
            // Register jQuery
            if (HttpContext.Current.Items["jquery_registered"] == null)
            {
                ScriptManager.RegisterClientScriptInclude(
                    this, typeof(Page), "jquery", "//ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js");

                HttpContext.Current.Items.Add("jquery_registered", "true");
            }

            if (HttpContext.Current.Items["tagcanvas_registered"] == null)
            {
                ScriptManager.RegisterClientScriptInclude(
                    this, typeof(Page), "tagcanvas", this.ResolveUrl("js/jquery.tagcanvas.min.js"));

                HttpContext.Current.Items.Add("tagcanvas_registered", "true");
            }

            var canvasControl = new HtmlGenericControl("canvas");

            canvasControl.Attributes.Add("width", this.settings.FlashWidth);
            canvasControl.Attributes.Add("height", this.settings.FlashHeight);

            this.tagCloudDiv.Controls.Add(canvasControl);

            this.tagCloudDiv.Controls.Remove(this.c1);

            canvasControl.Controls.Add(this.c1);

            var canvasScript = new StringBuilder();

            canvasScript.Append("jQuery(document).ready(function() {");

            canvasScript.AppendFormat("jQuery('#{0}').tagcanvas({{", canvasControl.ClientID);

            var textColor = this.settings.Tcolor.Replace("0x", "#");
            var outlineColor = this.settings.Hicolor.Replace("0x", "#");

            canvasScript.AppendFormat("textFont : '{0}',", this.settings.FontFamily);
            canvasScript.AppendFormat("textColour : '{0}',", textColor);
            canvasScript.AppendFormat("outlineColour : '{0}',", outlineColor);
            canvasScript.Append("maxSpeed : 0.03,");
            canvasScript.Append("outlineThickness : 1,");
            canvasScript.Append("weight : true,");
            canvasScript.Append("weightFrom : 'data-weight',");
            canvasScript.AppendFormat("weightMode : '{0}'", this.settings.weightMode);

            canvasScript.Append("});");

            if (!this.settings.Bgcolor.Contains("transparent"))
            {
                var backColor = this.settings.Bgcolor.Replace("0x", "#");

                canvasScript.AppendFormat("jQuery('#{0}').css('background-color','{1}')", canvasControl.ClientID, backColor);
            }

            canvasScript.Append("});");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                string.Format("CanvasScript{0}", this.tagCloudDiv.ClientID),
                canvasScript.ToString(),
                true);
        }

        /// <summary>
        /// Build Entire Script Link and Block for the HTML5 Canvas Tag Cloud
        /// </summary>
        /// <param name="cloudItems">The item list.</param>
        private void AddWordCloudScript(IEnumerable<CloudItem> cloudItems)
        {
            // Register jQuery
            if (HttpContext.Current.Items["jquery_registered"] == null)
            {
                ScriptManager.RegisterClientScriptInclude(
                    this, typeof(Page), "jquery", "//ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js");

                HttpContext.Current.Items.Add("jquery_registered", "true");
            }

            if (HttpContext.Current.Items["wordcloudjs_registered"] == null)
            {
                ScriptManager.RegisterClientScriptInclude(
                    this, typeof(Page), "wordcloudjs", this.ResolveUrl("js/jquery.wordcloud.js"));

                HttpContext.Current.Items.Add("wordcloudjs_registered", "true");
            }

            var canvasControl = new HtmlGenericControl("canvas");

            canvasControl.Attributes.Add("width", this.settings.FlashWidth);
            canvasControl.Attributes.Add("height", this.settings.FlashHeight);

            this.tagCloudDiv.Controls.Add(canvasControl);

            this.tagCloudDiv.Controls.Remove(this.c1);

            canvasControl.Controls.Add(this.c1);

            var canvasScript = new StringBuilder();

            canvasScript.Append("jQuery(document).ready(function() {");

            // Setup WordCloud
            canvasScript.AppendFormat("jQuery('#{0}').wordCloud({{", canvasControl.ClientID);

            canvasScript.AppendFormat("fontFamily:'{0}',", this.settings.FontFamily);
            canvasScript.AppendFormat("gridSize: {0},", this.settings.WordCloudSettings.GridSize);
            canvasScript.AppendFormat("ellipticity: {0},", this.settings.WordCloudSettings.Ellipticity);
            canvasScript.Append("center: false,");
            //canvasScript.AppendFormat("wordColor: '{0}',", this.settings.Tcolor.Replace("0x", "#"));

            if (!this.settings.Bgcolor.Contains("transparent"))
            {
                var backColor = this.settings.Bgcolor.Replace("0x", "#");

                canvasScript.AppendFormat("backgroundColor: '{0}',", backColor);
            }
            else
            {
                canvasScript.Append("backgroundColor: 'transparent',");
            }

            canvasScript.AppendFormat("weightFactor: {0},", this.settings.WordCloudSettings.WeightFactor);
            canvasScript.AppendFormat("minSize: {0},", this.settings.WordCloudSettings.MinSize);
            canvasScript.Append("clearCanvas: true,");
            canvasScript.AppendFormat("fillBox: {0},", this.settings.WordCloudSettings.FillBox.ToString().ToLower());
            canvasScript.AppendFormat(
                "shape: '{0}',", this.settings.WordCloudSettings.Shape.ToString().Replace("_", "-"));

            canvasScript.Append("wordList:  [");

            var currentItem = 1;
            var enumerable = cloudItems as IList<CloudItem> ?? cloudItems.ToList();
            var itemCount = enumerable.Count();

            foreach (var item in enumerable)
            {
                canvasScript.AppendFormat(currentItem.Equals(itemCount) ? "['{0}', {1}]" : "['{0}', {1}],", item.Text, item.Weight);

                currentItem++;
            }

            canvasScript.Append("]");

            canvasScript.Append("});");

            canvasScript.Append("});");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                string.Format("CanvasScript{0}", this.tagCloudDiv.ClientID),
                canvasScript.ToString(),
                true);
        }

        /// <summary>
        /// Build Entire Script Link and Block for Flash
        /// </summary>
        private void AddFlashScript()
        {
            if (HttpContext.Current.Items["swfobject_registered"] == null)
            {
                ScriptManager.RegisterClientScriptInclude(
                    this, typeof(Page), "swfObject", this.ResolveUrl("js/swfobject.js"));

                HttpContext.Current.Items.Add("swfobject_registered", "true");
            }

            double mean;
            double stdDev = Statistics.StdDev(this.ItemWeights, out mean);

            StringBuilder sBTags = new StringBuilder();

            sBTags.AppendLine("<tags>");

            foreach (CloudItem cloudItem in this.tagListItems)
            {
                int normalWeight = NormalizeWeight(cloudItem.Weight, mean, stdDev);

                sBTags.AppendLine(
                    string.Format(
                        "<a href='{0}' title='{1}' style='font-size:{2}px'>{3}</a>", 
                        cloudItem.Href,
                        HttpUtility.HtmlEncode(cloudItem.Title), 
                        this.fontSizes[normalWeight - 1], 
                        cloudItem.Text));
            }

            sBTags.AppendLine("</tags>");

            string sWmode = this.settings.Bgcolor.Contains("transparent")
                                ? "params.wmode = \"transparent\";"
                                : string.Format("params.bgcolor = \"{0}\";", this.settings.Bgcolor);

            var flashScript = new StringBuilder();

            flashScript.AppendLine("var flashvars = {};");
            flashScript.AppendFormat("flashvars.tcolor = \"{0}\";", this.settings.Tcolor);
            flashScript.AppendFormat("flashvars.tcolor2 = \"{0}\";", this.settings.Tcolor2);
            flashScript.AppendFormat("flashvars.hicolor = \"{0}\"; ", this.settings.Hicolor);

            flashScript.AppendFormat("flashvars.tspeed = \"{0}\";", this.settings.Tspeed);
            flashScript.AppendLine("flashvars.distr = \"true\";");
            flashScript.AppendLine("flashvars.mode = \"tags\";");
            flashScript.AppendFormat("flashvars.tagcloud = \"{0}\";", HttpUtility.UrlEncode(sBTags.ToString()));
            flashScript.AppendLine("  ");
            flashScript.AppendLine("var params = {};");
            flashScript.AppendLine(sWmode);
            flashScript.AppendLine("params.allowScriptAccess = \"always\";");

            flashScript.AppendFormat(
                "swfobject.embedSWF(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"9.0.0\", \"\" , flashvars, params);",
                this.ResolveUrl("tagcloud.swf"),
                this.tagCloudDiv.ClientID,
                this.settings.FlashWidth,
                this.settings.FlashHeight);

            Type csType = typeof(Page);

            ScriptManager.RegisterStartupScript(
                this, 
                csType, 
                string.Format("TagCloudFlashScript{0}", this.tagCloudDiv.ClientID),
                flashScript.ToString(), 
                true);
        }

        /// <summary>
        /// Format the Url from FileID to File Path Url
        /// </summary>
        /// <param name="sInputUrl">
        /// The Input Url.
        /// </param>
        /// <returns>
        /// Returns the formatted URL
        /// </returns>
        private string FormatUrl(string sInputUrl)
        {
            string sNewUrl = sInputUrl;

            if (string.IsNullOrEmpty(sNewUrl) || sNewUrl.StartsWith("http://"))
            {
                return sNewUrl;
            }

            int iTabId = int.Parse(sNewUrl);

            TabController objTabController = new TabController();

            TabInfo objTabInfo = objTabController.GetTab(iTabId, this.PortalId, true);

            sNewUrl = objTabInfo.FullUrl;

            return sNewUrl;
        }

        /// <summary>
        /// Get Tags from ActiveForums Module
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromActiveForums()
        {
            List<CloudItem> list = new List<CloudItem>();

            try
            {
                ////////////////////////
                foreach (CloudItem tag in
                    DataControl.TagCloudActiveForumsTags(this.PortalId, this.settings.AfModule).Where(tag => tag.Weight >= this.settings.OccurCount))
                {
                    CloudItem entry;

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.Text), 
                            tag.Weight,
                            Globals.NavigateURL(this.settings.AfTab, string.Empty, "afv=search", string.Format("aftg={0}", tag.Text)), 
                            tag.Text);
                    }
                    else
                    {
                        entry = new CloudItem(Utility.RemoveIllegalCharecters(tag.Text), tag.Weight, null, tag.Text);
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        /// <summary>
        /// Get Custom Tag Items From the Sql Tables
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromCustom()
        {
            List<CloudItem> list = new List<CloudItem>();

            try
            {
                ////////////////////////
                foreach (CustomTags tag in
                    DataControl.TagCloudItemsGetByModule(this.ModuleId).Where(tag => tag.iWeight >= this.settings.OccurCount))
                {
                    if (Utility.IsNumeric(tag.sUrl) && this.settings.TagsLinkChk)
                    {
                        if (!UserSeeTag(this.PortalId, int.Parse(tag.sUrl)))
                        {
                            continue;
                        }
                    }

                    CloudItem entry;

                    // Has Localized Value?
                    CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;

                    tag.localTags = DataControl.TagCloudItemsGetByLocale(this.ModuleId, tag.iTagId);

                    string sTag = null, sTagUrl = null;

                    foreach (Locales locales in
                        tag.localTags.Where(locales => locales.Locale.Equals(currentCulture.ToString())))
                    {
                        sTag = locales.TagMl;
                        sTagUrl = locales.UrlMl;

                        break;
                    }

                    if (string.IsNullOrEmpty(sTag))
                    {
                        sTag = tag.sTag;
                    }

                    if (string.IsNullOrEmpty(sTagUrl))
                    {
                        sTagUrl = tag.sUrl;
                    }

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(sTag), tag.iWeight, this.FormatUrl(sTagUrl), sTag);
                    }
                    else
                    {
                        entry = new CloudItem(Utility.RemoveIllegalCharecters(sTag), tag.iWeight, null, sTag);
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        /// <summary>
        /// Get Tags from Ventrian.NewsArticles Module
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromNewsArticles()
        {
            List<CloudItem> list = new List<CloudItem>();

            Ventrian.NewsArticles.TagController tc = new Ventrian.NewsArticles.TagController();

            try
            {
                ////////////////////////
                foreach (Ventrian.NewsArticles.TagInfo tag in
                    tc.List(this.settings.VentrianModuleNews, this.settings.TagCount).Cast<Ventrian.NewsArticles.TagInfo>().Where(tag => tag.Usages >= this.settings.OccurCount))
                {
                    CloudItem entry;

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.Name), 
                            tag.Usages, 
                            Globals.NavigateURL(
                                this.settings.VentrianTabNews, 
                                string.Empty, 
                                string.Format("articletype=tagview&tag={0}", Server.UrlEncode(tag.NameLowered))), 
                            tag.Name);
                    }
                    else
                    {
                        entry = new CloudItem(Utility.RemoveIllegalCharecters(tag.Name), tag.Usages, null, tag.Name);
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        /// <summary>
        /// Get Tags from Ventrian.SimpleGallery Module
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromSimpleGallery()
        {
            List<CloudItem> list = new List<CloudItem>();

            TagController tc = new TagController();

            var simpleGalleryInstance = new ModuleController().GetModule(this.settings.VentrianModuleSimple, this.settings.VentrianTabSimple);

            try
            {
                ////////////////////////
                foreach (TagInfo tag in
                    tc.List(this.settings.VentrianModuleSimple, -1, this.settings.TagCount, true).Cast<TagInfo>().Where(tag => tag.Usages >= this.settings.OccurCount))
                {
                    CloudItem entry;

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.Name), 
                            tag.Usages, 
                            Globals.NavigateURL(
                                this.settings.VentrianTabSimple, 
                                string.Empty, 
                                string.Format("Tag={0}", tag.Name),
                                string.Format("Tags={0}", simpleGalleryInstance.TabModuleID)), 
                            tag.Name);
                    }
                    else
                    {
                        entry = new CloudItem(Utility.RemoveIllegalCharecters(tag.Name), tag.Usages, null, tag.Name);
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        /// <summary>
        /// Get Tag Items From the Sql Tables
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromSql()
        {
            List<CloudItem> list = new List<CloudItem>();

            try
            {
                this.words = DataControl.GetTagWords(this.settings.TagCount, PortalSettings.PortalId);

                ////////////////////////
                foreach (TagWords word in this.words)
                {
                    // Check
                    if (Utility.IsUrl(word.Word) || Utility.IsNumeric(word.Word) || Utility.ContainsNumber(word.Word)
                        || !Utility.IsLengthOk(word.Word))
                    {
                        continue;
                    }

                    int iKeyWCountOnSite = DataControl.TagCloudSearchWordsOccur(word.WordsId);

                    TagWords sCurWord = word;

                    if (this.settings.TagsLinkChk)
                    {
                        if (!UserSeeTag(this.PortalId, word.Word))
                        {
                            continue;
                        }
                    }

                    CloudItem entry;

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(sCurWord.Word), 
                            iKeyWCountOnSite, 
                            string.Format(
                                "{0}?{2}{1}",
                                Globals.NavigateURL(this.settings.searchTabId),
                                Utility.RemoveIllegalCharecters(sCurWord.Word),
                                this.settings.SearchQueryString), 
                            string.Format(
                                Localization.GetString("TagFound.Text", this.LocalResourceFile),
                                Utility.RemoveIllegalCharecters(sCurWord.Word), 
                                iKeyWCountOnSite));
                    }
                    else
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(sCurWord.Word), 
                            iKeyWCountOnSite, 
                            null, 
                            string.Format(
                                Localization.GetString("TagFound.Text", this.LocalResourceFile),
                                Utility.RemoveIllegalCharecters(sCurWord.Word), 
                                iKeyWCountOnSite));
                    }

                    list.RemoveAll(check => (check.Text.ToLower() == sCurWord.Word.ToLower()));

                    // Check
                    if (iKeyWCountOnSite < this.settings.OccurCount)
                    {
                        continue;
                    }

                    list.Add(entry);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return list;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            try
            {
                this.LoadModulSettings();
            }
            catch (Exception exception)
            {
                Exceptions.ProcessModuleLoadException(this, exception);
            }

            // Add Tag Separator
            if (!string.IsNullOrEmpty(this.settings.TagSeparator))
            {
                this.c1.ItemSeparator = this.settings.TagSeparator;
            }

            if (this.settings.RenderUl)
            {
                this.c1.RenderAsUl = true;
            }

            this.c1.DataSource = this.GetItems();
            this.c1.DataBind();
        }

        /// <summary>
        /// Load the Setting for this Module
        /// </summary>
        private void LoadModulSettings()
        {
            ModuleController objModuleController = new ModuleController();

            Hashtable moduleSettings = objModuleController.GetTabModuleSettings(this.TabModuleId);

            // Set Default Settings
            this.settings = new TagCloudSettings
                                {
                                    CacheItems = true,
                                    StartDate = DateTime.Parse("01/01/1999", new CultureInfo("en-US")),
                                    RenderUl = true,
                                    TagsLink = true,
                                    exlusionWords = new List<ExcludeWord>(),
                                    ExcludeCommon = true,
                                    AfModule = -1,
                                    AfTab = -1,
                                    DnnBlogTab = -1,
                                    VentrianModuleNews = -1,
                                    VentrianModuleSimple = -1,
                                    VentrianTabNews = -1,
                                    VentrianTabSimple = -1,
                                    weightMode = WeightMode.size,
                                    FontFamily = "Georgia, Arial, sans-serif",
                                    WordCloudSettings =
                                        new WordCloudSettings
                                            {
                                                Ellipticity = 0.85,
                                                GridSize = 8,
                                                MinSize = 0,
                                                Shape = WordCloudShape.circle,
                                                WeightFactor = 2.1,
                                                FillBox = false
                                            }
                                };
            ////////////////////////

            // Setting Font Family
            if (!string.IsNullOrEmpty((string)moduleSettings["FontFamily"]))
            {
                try
                {
                    this.settings.FontFamily = (string)moduleSettings["FontFamily"];
                }
                catch (Exception)
                {
                    this.settings.FontFamily = "Georgia, Arial, sans-serif";
                }
            }

            // Setting WeightMode Type
            if (!string.IsNullOrEmpty((string)moduleSettings["WeightMode"]))
            {
                try
                {
                    this.settings.weightMode = (WeightMode)Enum.Parse(typeof(WeightMode), (string)moduleSettings["WeightMode"]);
                }
                catch (Exception)
                {
                    this.settings.weightMode = WeightMode.size;
                }
            }

            // Setting RenderMode Type
            if (!string.IsNullOrEmpty((string)moduleSettings["RenderMode"]))
            {
                try
                {
                    this.settings.renderMode = (RenderMode)Enum.Parse(typeof(RenderMode), (string)moduleSettings["RenderMode"]);
                }
                catch (Exception)
                {
                    this.settings.renderMode = RenderMode.HTML5;
                }
            }
            else
            {
                // Import old setting
                if (!string.IsNullOrEmpty((string)moduleSettings["flashenabled"]))
                {
                    bool flashEnabled = bool.Parse((string)moduleSettings["flashenabled"]);

                    this.settings.renderMode = flashEnabled
                                                            ? RenderMode.Flash
                                                            : RenderMode.BasicHTML;
                }
                else
                {
                    this.settings.renderMode = RenderMode.HTML5;
                }
            }

            // Setting Render Item Weight
            if (!string.IsNullOrEmpty((string)moduleSettings["RenderItemWeight"]))
            {
                bool renderItemWeight;
                bool.TryParse((string)moduleSettings["RenderItemWeight"], out renderItemWeight);

                this.settings.RenderItemWeight = renderItemWeight;
            }

            // Get Search Tab
            if (!string.IsNullOrEmpty((string)this.TabModuleSettings["CustomSearchPage"]))
            {
                this.settings.searchTabId = Convert.ToInt32((string)this.TabModuleSettings["CustomSearchPage"]);
            }
            else
            {
                if (this.Settings["SearchResultsModule"] != null)
                {
                    this.settings.searchTabId = Convert.ToInt32((string)this.Settings["SearchResultsModule"]);
                }
                else
                {
                    var searchModule = new ModuleController().GetModuleByDefinition(PortalSettings.PortalId, "Search Results");

                    if (searchModule == null)
                    {
                        this.settings.searchTabId = -1;
                    }
                    else
                    {
                        this.settings.searchTabId = searchModule.TabID;
                    }
                }
            }

            this.settings.SearchQueryString =
               !string.IsNullOrEmpty((string)this.TabModuleSettings["SearchPageQueryString"])
                   ? (string)this.TabModuleSettings["SearchPageQueryString"]
                   : "Search=";

            if (!string.IsNullOrEmpty((string)moduleSettings["TagSeparator"]))
            {
                try
                {
                    this.settings.TagSeparator = (string)moduleSettings["TagSeparator"];
                }
                catch (Exception)
                {
                    this.settings.TagSeparator = string.Empty;
                }
            }
            else
            {
                this.settings.TagSeparator = string.Empty;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ReferralStart"]))
            {
                try
                {
                    DateTime startDate;
                    DateTime.TryParse((string)moduleSettings["ReferralStart"], out startDate);

                    this.settings.StartDate = startDate;
                }
                catch (Exception)
                {
                    this.settings.StartDate = DateTime.Parse("01/01/1999", new CultureInfo("en-US"));
                }
            }
            else
            {
                this.settings.StartDate = DateTime.Parse("01/01/1999", new CultureInfo("en-US"));
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeCustom"]))
            {
                bool modeCustom;
                bool.TryParse((string)moduleSettings["ModeCustom"], out modeCustom);

                this.settings.ModeCustom = modeCustom;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeCustom"]))
            {
                bool modeCustom;
                bool.TryParse((string)moduleSettings["ModeCustom"], out modeCustom);

                this.settings.ModeCustom = modeCustom;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeReferrals"]))
            {
                bool moderef;
                bool.TryParse((string)moduleSettings["ModeReferrals"], out moderef);

                this.settings.ModeReferrals = moderef;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeNewsarticles"]))
            {
                bool modenews;
                bool.TryParse((string)moduleSettings["ModeNewsarticles"], out modenews);

                this.settings.ModeNewsarticles = modenews;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeSimplegallery"]))
            {
                bool modesimple;
                bool.TryParse((string)moduleSettings["ModeSimplegallery"], out modesimple);

                this.settings.ModeSimplegallery = modesimple;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeActiveforums"]))
            {
                bool modeactive;
                bool.TryParse((string)moduleSettings["ModeActiveforums"], out modeactive);

                this.settings.ModeActiveforums = modeactive;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeSearch"]))
            {
                bool modesearch;
                bool.TryParse((string)moduleSettings["ModeSearch"], out modesearch);

                this.settings.ModeSearch = modesearch;
            }

            // Load old Setting 
            if (!string.IsNullOrEmpty((string)moduleSettings["TagMode"]))
            {
                string sTagMode = (string)moduleSettings["TagMode"];

                switch (sTagMode)
                {
                    case "custom":
                        this.settings.ModeCustom = true;
                        break;
                    case "newsarticles":
                        this.settings.ModeNewsarticles = true;
                        break;
                    case "simplegallery":
                        this.settings.ModeSimplegallery = true;
                        break;
                    case "activeforums":
                        this.settings.ModeActiveforums = true;
                        break;
                    case "referrals":
                        this.settings.ModeReferrals = true;
                        break;
                    case "search":
                        this.settings.ModeSearch = true;
                        break;
                }
            }

            // Check if all false, set search
            if (!this.settings.ModeCustom && !this.settings.ModeReferrals && !this.settings.ModeNewsarticles && !this.settings.ModeSimplegallery &&
                !this.settings.ModeActiveforums && !this.settings.ModeSearch)
            {
                this.settings.ModeSearch = true;
            }

            // Load Ventrian Tab & Module from Settings and Set
            if (!string.IsNullOrEmpty((string)moduleSettings["NewsArticlesTab"]) &&
                !string.IsNullOrEmpty((string)moduleSettings["NewsArticlesModule"]))
            {
                try
                {
                    this.settings.VentrianTabNews = int.Parse((string)moduleSettings["NewsArticlesTab"]);

                    this.settings.VentrianModuleNews = int.Parse((string)moduleSettings["NewsArticlesModule"]);

                    if (this.settings.VentrianTabNews.Equals(-1) || this.settings.VentrianModuleNews.Equals(-1))
                    {
                        this.settings.ModeNewsarticles = false;
                    }
                }
                catch (Exception)
                {
                    this.settings.ModeNewsarticles = false;
                }
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["SimpleGalleryTab"]) &&
                !string.IsNullOrEmpty((string)moduleSettings["SimpleGalleryModule"]))
            {
                try
                {
                    this.settings.VentrianTabSimple = int.Parse((string)moduleSettings["SimpleGalleryTab"]);

                    this.settings.VentrianModuleSimple = int.Parse((string)moduleSettings["SimpleGalleryModule"]);

                    if (this.settings.VentrianTabSimple.Equals(-1) || this.settings.VentrianModuleSimple.Equals(-1))
                    {
                        this.settings.ModeSimplegallery = false;
                    }
                }
                catch (Exception)
                {
                    this.settings.ModeSimplegallery = false;
                }
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ActiveForumsTab"]) &&
                !string.IsNullOrEmpty((string)moduleSettings["ActiveForumsModule"]))
            {
                try
                {
                    this.settings.AfTab = int.Parse((string)moduleSettings["ActiveForumsTab"]);

                    this.settings.AfModule = int.Parse((string)moduleSettings["ActiveForumsModule"]);

                    if (this.settings.AfTab.Equals(-1) || this.settings.AfModule.Equals(-1))
                    {
                        this.settings.ModeActiveforums = false;
                    }
                }
                catch (Exception)
                {
                    this.settings.ModeActiveforums = false;
                }
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["SkinName"]))
            {
                try
                {
                    this.settings.SkinName = (string)moduleSettings["SkinName"];
                }
                catch (Exception)
                {
                    this.settings.SkinName = "Standard";
                }
            }
            else
            {
                this.settings.SkinName = "Standard";
            }

            if (!this.settings.SkinName.Equals("None"))
            {
                ((CDefault)this.Page).AddStyleSheet(
                    "Skin" + this.settings.SkinName, string.Format("{0}/{1}.css", this.ResolveUrl("Skins"), this.settings.SkinName));

                this.tagCloudDiv.CssClass += string.Format("-{0}", this.settings.SkinName);
                this.c1.ItemCssClassPrefix += string.Format("-{0}", this.settings.SkinName);
            }

            this.settings.OccurCount = !string.IsNullOrEmpty((string)moduleSettings["occurcount"])
                                   ? int.Parse((string)moduleSettings["occurcount"])
                                   : 1;

            this.settings.TagCount = !string.IsNullOrEmpty((string)moduleSettings["tagscount"])
                                 ? int.Parse((string)moduleSettings["tagscount"])
                                 : 30;

            if (!string.IsNullOrEmpty((string)moduleSettings["ExcludeCommon"]))
            {
                bool excludeCommon;
                bool.TryParse((string)moduleSettings["ExcludeCommon"], out excludeCommon);

                this.settings.ExcludeCommon = excludeCommon;
            }

            if (this.settings.ExcludeCommon)
            {
                // Load Common Words, current Language
                string commonWords = Localization.GetString("CommonWords.Text", this.LocalResourceFile);

                if (!string.IsNullOrEmpty(commonWords))
                {
                    string[] comnmonWordList = commonWords.Split(',');

                    foreach (string word in comnmonWordList)
                    {
                        this.settings.exlusionWords.Add(new ExcludeWord { Word = word, ExcludeWordType = ExcludeType.Equals });
                    }
                }
            }

            // Import Old List
            string strExlusionList = (string)moduleSettings["exlusLst"];

            if (!string.IsNullOrEmpty(strExlusionList))
            {
                string[] oldList = strExlusionList.Split(',');

                foreach (string word in oldList)
                {
                    this.settings.exlusionWords.Add(new ExcludeWord { Word = word, ExcludeWordType = ExcludeType.Equals });
                }
            }

            // Load New Exclude Word List
            this.settings.exlusionWords.AddRange(DataControl.TagCloudExcludeWordsGetByModule(this.TabModuleId));

            try
            {
                this.settings.CloudSortType = (SortType)Enum.Parse(typeof(SortType), (string)moduleSettings["SortType"]);
            }
            catch (Exception)
            {
                this.settings.CloudSortType = SortType.AlphabeticAsc;
            }

            string sRenderUl = (string)moduleSettings["RenderUl"];

            this.settings.RenderUl = !string.IsNullOrEmpty(sRenderUl) && bool.Parse(sRenderUl);

            if (this.settings.renderMode.Equals(RenderMode.HTML5))
            {
                this.settings.RenderUl = true;
                this.c1.CanvasEnabled = true;
            }

            // Fix for sliding theme
            if (this.settings.SkinName.Contains("Sliding"))
            {
                this.settings.RenderUl = true;
            }

            this.c1.Skin = this.settings.SkinName;

            this.c1.RenderItemWeight = this.settings.RenderItemWeight;

            string strTagsLink = (string)moduleSettings["tagslink"];

            this.settings.TagsLink = string.IsNullOrEmpty(strTagsLink) || bool.Parse(strTagsLink);

            string strTagsLinkChk = (string)moduleSettings["tagslinkChk"];

            this.settings.TagsLinkChk = !string.IsNullOrEmpty(strTagsLinkChk) && bool.Parse(strTagsLinkChk);

            string sCacheItems = (string)moduleSettings["CacheItems"];

            this.settings.CacheItems = string.IsNullOrEmpty(sCacheItems) || bool.Parse(sCacheItems);

            try
            {
                this.settings.FlashWidth = (string)moduleSettings["flashwidth"];
            }
            finally
            {
                if (string.IsNullOrEmpty(this.settings.FlashWidth))
                {
                    this.settings.FlashWidth = "500";
                }
            }

            try
            {
                this.settings.FlashHeight = (string)moduleSettings["flashheight"];
            }
            finally
            {
                if (string.IsNullOrEmpty(this.settings.FlashHeight))
                {
                    this.settings.FlashHeight = "300";
                }
            }

            try
            {
                this.settings.TagCloudWidth = (string)moduleSettings["tagcloudwidth"];
            }
            finally
            {
                if (string.IsNullOrEmpty(this.settings.TagCloudWidth))
                {
                    this.settings.TagCloudWidth = "500";
                    this.settings.TagCloudWValue = "px";
                }

                if (this.settings.TagCloudWidth.Contains("px"))
                {
                    this.settings.TagCloudWidth = this.settings.TagCloudWidth.Replace("px", string.Empty);
                    this.settings.TagCloudWValue = "px";
                }

                if (this.settings.TagCloudWidth.Contains("%"))
                {
                    this.settings.TagCloudWidth = this.settings.TagCloudWidth.Replace("%", string.Empty);
                    this.settings.TagCloudWValue = "%";
                }
            }

            /* try
             {
                 sTagCloudHeight = ((string)moduleSettings["tagcloudheight"]);
             }
             finally
             {
                 if (string.IsNullOrEmpty(sTagCloudHeight))
                 {
                     sTagCloudHeight = "300";
                     //sTagCloudHValue = "px";
                 }

                 if (sTagCloudHeight.Contains("px"))
                 {
                     sTagCloudHeight = sTagCloudHeight.Replace("px", "");
                    // sTagCloudHValue = "px";
                 }

                 if (sTagCloudHeight.Contains("%"))
                 {
                     sTagCloudHeight = sTagCloudHeight.Replace("%", "");
                     //sTagCloudHValue = "%";
                 }
             }
            
             if (sTagCloudHeight.Equals("-1"))
             {
                 sTagCloudHeight = "auto";
                 //sTagCloudHValue = "";
             }*/
            if (this.settings.TagCloudWidth.Equals("-1"))
            {
                this.settings.TagCloudWidth = "auto";
                this.settings.TagCloudWValue = string.Empty;
            }

            try
            {
                this.settings.Tcolor = (string)moduleSettings["tcolor"];
            }
            finally
            {
                this.settings.Tcolor = string.IsNullOrEmpty(this.settings.Tcolor) ? "0x000000" : string.Format("0x{0}", this.settings.Tcolor);
            }

            try
            {
                this.settings.Tcolor2 = (string)moduleSettings["tcolor2"];
            }
            finally
            {
                this.settings.Tcolor2 = string.IsNullOrEmpty(this.settings.Tcolor2) ? "0x000000" : string.Format("0x{0}", this.settings.Tcolor2);
            }

            try
            {
                this.settings.Hicolor = (string)moduleSettings["hicolor"];
            }
            finally
            {
                this.settings.Hicolor = string.IsNullOrEmpty(this.settings.Hicolor) ? "0x42a5ff" : string.Format("0x{0}", this.settings.Hicolor);
            }

            try
            {
                this.settings.Bgcolor = (string)moduleSettings["bgcolor"];
            }
            finally
            {
                this.settings.Bgcolor = string.IsNullOrEmpty(this.settings.Bgcolor)
                                    ? "transparent"
                                    : string.Format("#{0}", this.settings.Bgcolor);
            }

            try
            {
                this.settings.Tspeed = (string)moduleSettings["tspeed"];
            }
            finally
            {
                if (string.IsNullOrEmpty(this.settings.Tspeed))
                {
                    this.settings.Tspeed = "75";
                }
            }

            // Load World Cloud Settings only when needed
            if (this.settings.renderMode.Equals(RenderMode.WordCloud))
            {
                // Word Cloud Shape Setting
                if (!string.IsNullOrEmpty((string)moduleSettings["Shape"]))
                {
                    try
                    {
                        this.settings.WordCloudSettings.Shape = (WordCloudShape)Enum.Parse(typeof(WordCloudShape), (string)moduleSettings["Shape"]);
                    }
                    catch (Exception)
                    {
                        this.settings.WordCloudSettings.Shape = WordCloudShape.circle;
                    }
                }
                else
                {
                    this.settings.WordCloudSettings.Shape = WordCloudShape.circle;
                }

                // World Cloud GridSize Setting
                this.settings.WordCloudSettings.GridSize = !string.IsNullOrEmpty((string)moduleSettings["GridSize"])
                                                               ? Convert.ToInt32(moduleSettings["GridSize"])
                                                               : 8;

                // World Cloud Ellipticity Setting
                this.settings.WordCloudSettings.Ellipticity =
                    !string.IsNullOrEmpty((string)moduleSettings["Ellipticity"])
                        ? Convert.ToDouble(moduleSettings["Ellipticity"])
                        : 0.65;

                // World Cloud WeightFactor Setting
                this.settings.WordCloudSettings.WeightFactor =
                    !string.IsNullOrEmpty((string)moduleSettings["WeightFactor"])
                        ? Convert.ToDouble(moduleSettings["WeightFactor"])
                        : 2.1;

                // World Cloud MinSize Setting
                this.settings.WordCloudSettings.MinSize = !string.IsNullOrEmpty((string)moduleSettings["MinSize"]) ? Convert.ToInt32(moduleSettings["MinSize"]) : 0;

                // World Cloud FillBox Setting
                if (!string.IsNullOrEmpty((string)moduleSettings["FillBox"]))
                {
                    try
                    {
                        this.settings.WordCloudSettings.FillBox = bool.Parse((string)moduleSettings["FillBox"]);
                    }
                    catch (Exception)
                    {
                        this.settings.WordCloudSettings.FillBox = false;
                    }
                }
                else
                {
                    this.settings.WordCloudSettings.FillBox = false;
                }
            }

            // Set TagCloud width & Height to Main DIV Tag
            this.tagCloudDiv.Attributes["style"] =
                string.Format(
                    "width:{0}{1};height:auto;",
                    this.settings.TagCloudWidth,
                    this.settings.TagCloudWValue);
        }

        #endregion
    }
}