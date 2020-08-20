/*  *********************************************************************************************
*
*   WatchersNET.TagCloud - This Module displays the most frequently used words (Tags) of your Portal as a
*   standard Web 2.0 Tag Cloud, or You can define your own Tags list.  The Tags are links which linked to the Portal Search to
*   show all Pages with that Tag.
*
*   The Tag Cloud will be rendered as 3D Cloud, and as alternative for Non Flash
*   Users as a list of hyperlinks in varying styles depending on a weight.
*   This is similar to tag clouds in del.icio.us or Flickr.
*
*   Copyright(c) Ingo Herbote (thewatcher@watchersnet.de)
*   All rights reserved.
*   Internet: https://github.com/w8tcha/WatchersNET.TagCloud
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
    using DotNetNuke.Entities.Content.Common;
    using DotNetNuke.Entities.Content.Taxonomy;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Framework.JavaScriptLibraries;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Web.Client.ClientResourceManagement;

    using VRK.Controls;

    using WatchersNET.DNN.Modules.TagCloud.Constants;
    using WatchersNET.DNN.Modules.TagCloud.Objects;

    using TagController = Ventrian.SimpleGallery.Entities.TagController;
    using TagInfo = Ventrian.SimpleGallery.Entities.TagInfo;

    #endregion

    /// <summary>
    /// The tag cloud.
    /// </summary>
    public partial class TagCloud : ModuleSettingsBase, IActionable
    {
        #region Constants and Fields

        /// <summary>
        /// The font sizes.
        /// </summary>
        private readonly string[] fontSizes = { "6.94", "8.3", "10", "12", "14.4", "17.3", "20.7" };

        /// <summary>
        /// The tag list items.
        /// </summary>
        private List<CloudItem> tagListItems = new List<CloudItem>();

        /// <summary>
        /// The vocabularies.
        /// </summary>
        private string[] vocabularies;

        /// <summary>
        /// The Module Settings
        /// </summary>
        private TagCloudSettings settings;

        /// <summary>
        /// Gets or sets The tab module id.
        /// </summary>
        public static int CurrentTabModuleId { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///  Gets Add Menu Entries to Module Container
        /// </summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString("Manage.Text", this.LocalResourceFile),
                            ModuleActionType.AddContent, string.Empty, string.Empty, this.EditUrl(), false,
                            SecurityAccessLevel.Edit, true, false
                            }
                    };

                return actions;
            }
        }

        /// <summary>
        /// Gets ItemWeights.
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
        private List<CloudItem> GetItems()
        {
            var sLogCacheKey = $"CloudItems{this.TabModuleId}";

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
        private List<CloudItem> GetItemsFromDb()
        {
            var list = new List<CloudItem>();

            if (this.settings.ModeCustom)
            {
                if (this.settings.TagsLinkChk)
                {
                    this.settings.CacheItems = false;
                }

                list.AddRange(this.GetItemsFromCustom());
            }

            if (this.settings.ModeTax)
            {
                this.settings.TagsLinkChk = false;
                list.AddRange(this.GetItemsFromTax());
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

            // Exclude Words
            if (this.settings.exlusionWords.Count > 0)
            {
                foreach (var exWord in this.settings.exlusionWords)
                {
                    var value = exWord.Word.ToLower();

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
                DataCache.SetCache($"CloudItems{this.TabModuleId}", list);
            }

            return list;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
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
        /// The std dev.
        /// </param>
        /// <returns>
        /// Returns the normalized weight.
        /// </returns>
        private static int NormalizeWeight(double weight, double mean, double stdDev)
        {
            var factor = weight - mean;

            if (factor != 0 && stdDev != 0)
            {
                factor /= stdDev;
            }

            return factor > 2
                       ? 7
                       : factor > 1
                             ? 6
                             : factor > 0.5 ? 5 : factor > -0.5 ? 4 : factor > -1 ? 3 : factor > -2 ? 2 : 1;
        }

        /// <summary>
        /// Check if the User can see the Page
        /// </summary>
        /// <param name="portalId">
        /// Portal ID
        /// </param>
        /// <param name="tabId">
        /// Tab to Check
        /// </param>
        /// <returns>
        /// Returns if the User can see the Page or not.
        /// </returns>
        private static bool UserSeeTag(int portalId, int tabId)
        {
            var canSee = false;

            var tabPermissions = TabPermissionController.GetTabPermissions(tabId, portalId);

            // Get The Tab
            if (PortalSecurity.IsInRoles(tabPermissions.ToString("VIEW")))
            {
                canSee = true;
            }

            return canSee;
        }

        /// <summary>
        /// Build Entire Script Link and Block for the HTML5 Canvas Tag Cloud
        /// </summary>
        private void AddCanvasScript()
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);

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
                $"CanvasScript{this.tagCloudDiv.ClientID}",
                canvasScript.ToString(),
                true);
        }

        /// <summary>
        /// Build Entire Script Link and Block for the HTML5 Canvas Tag Cloud
        /// </summary>
        /// <param name="cloudItems">The item list.</param>
        private void AddWordCloudScript(IEnumerable<CloudItem> cloudItems)
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);

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
            var itemCount = enumerable.Count;

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
                $"CanvasScript{this.tagCloudDiv.ClientID}",
                canvasScript.ToString(),
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
            var sNewUrl = sInputUrl;

            if (string.IsNullOrEmpty(sNewUrl) || sNewUrl.StartsWith("http://"))
            {
                return sNewUrl;
            }

            var iTabId = int.Parse(sNewUrl);

            var objTabController = new TabController();

            var objTabInfo = objTabController.GetTab(iTabId, PortalSettings.PortalId, true);

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
            var list = new List<CloudItem>();

            try
            {
                ////////////////////////
                foreach (var tag in
                    DataControl.TagCloudActiveForumsTags(PortalSettings.PortalId, this.settings.AfModule, this.settings.TagCount).Where(
                        tag => tag.Weight >= this.settings.OccurCount))
                {
                    CloudItem entry;

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(tag.Text),
                            tag.Weight,
                            Globals.NavigateURL(this.settings.AfTab, string.Empty, "afv=search", $"aftg={tag.Text}"),
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
            var list = new List<CloudItem>();

            try
            {
                ////////////////////////
                foreach (var tag in
                    DataControl.TagCloudItemsGetByModule(this.ModuleId).Where(tag => tag.Weight >= this.settings.OccurCount))
                {
                    if (Utility.IsNumeric(tag.Url) && this.settings.TagsLinkChk)
                    {
                        if (!UserSeeTag(PortalSettings.PortalId, int.Parse(tag.Url)))
                        {
                            continue;
                        }
                    }

                    CloudItem entry;

                    // Has Localized Value?
                    var currentCulture = Thread.CurrentThread.CurrentUICulture;

                    tag.LocalTags = DataControl.TagCloudItemsGetByLocale(this.ModuleId, tag.TagId);

                    string sTag = null, sTagUrl = null;

                    foreach (var locales in
                        tag.LocalTags.Where(locales => locales.Locale.Equals(currentCulture.ToString())))
                    {
                        sTag = locales.TagMl;
                        sTagUrl = locales.UrlMl;

                        break;
                    }

                    if (string.IsNullOrEmpty(sTag))
                    {
                        sTag = tag.Tag;
                    }

                    if (string.IsNullOrEmpty(sTagUrl))
                    {
                        sTagUrl = tag.Url;
                    }

                    if (this.settings.TagsLink)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(sTag), tag.Weight, this.FormatUrl(sTagUrl), sTag);
                    }
                    else
                    {
                        entry = new CloudItem(Utility.RemoveIllegalCharecters(sTag), tag.Weight, null, sTag);
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
            var list = new List<CloudItem>();

            var tc = new Ventrian.NewsArticles.TagController();

            try
            {
                ////////////////////////
                foreach (var tag in
                    tc.List(this.settings.VentrianModuleNews, this.settings.TagCount).Cast<Ventrian.NewsArticles.TagInfo>().Where(
                        tag => tag.Usages >= this.settings.OccurCount))
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
                                $"articletype=tagview&tag={this.Server.UrlEncode(tag.NameLowered)}"),
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
            var list = new List<CloudItem>();

            var tc = new TagController();

            var simpleGalleryInstance = new ModuleController().GetModule(this.settings.VentrianModuleSimple, this.settings.VentrianTabSimple);

            try
            {
                ////////////////////////
                foreach (var tag in
                    tc.List(this.settings.VentrianModuleSimple, -1, this.settings.TagCount, true).Cast<TagInfo>().Where(
                        tag => tag.Usages >= this.settings.OccurCount))
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
                                $"Tag={tag.Name}",
                                $"Tags={simpleGalleryInstance.TabModuleID}"),
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
        /// Get Custom Tag Items From the DNN Taxonomy
        /// </summary>
        /// <returns>
        /// List of Tags
        /// </returns>
        private IEnumerable<CloudItem> GetItemsFromTax()
        {
            var list = new List<CloudItem>();

            try
            {
                ////////////////////////
                foreach (var term in this.GetTerms())
                {
                    // if (term.ParentTermId != -1) continue;
                    CloudItem entry;

                    if (term.Weight > 0)
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(term.Name),
                            term.Weight,
                            string.Format(
                                "{0}?{2}{1}",
                                Globals.NavigateURL(this.settings.searchTabId),
                                Utility.RemoveIllegalCharecters(term.Name),
                                this.settings.SearchTaxQueryString),
                            term.Description);
                    }
                    else
                    {
                        entry = new CloudItem(
                            Utility.RemoveIllegalCharecters(term.Name),
                            term.TermId,
                            string.Format(
                                "{0}?{2}{1}",
                                Globals.NavigateURL(this.settings.searchTabId),
                                Utility.RemoveIllegalCharecters(term.Name),
                                this.settings.SearchTaxQueryString),
                            term.Description);
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
        /// Get Dnn Taxonomy Terms
        /// </summary>
        /// <returns>
        /// The Item List
        /// </returns>
        private IEnumerable<Term> GetTerms()
        {
            var terms = new List<Term>();

            switch (this.settings.TaxMode)
            {
                case "tab":
                    {
                        terms = this.PortalSettings.ActiveTab.Terms;
                    }

                    break;
                case "all":
                    {
                        var termRep = Util.GetTermController();

                        var vocabRep = Util.GetVocabularyController();

                        var vs = from v in vocabRep.GetVocabularies()
                                                    where
                                                        v.ScopeType.ScopeType == "Application" ||
                                                        v.ScopeType.ScopeType == "Portal" && v.ScopeId == this.PortalSettings.PortalId
                                                    select v;

                        foreach (var v in vs)
                        {
                            foreach (var t in termRep.GetTermsByVocabulary(v.VocabularyId))
                            {
                                if (v.Type == VocabularyType.Simple)
                                {
                                    t.ParentTermId = -v.VocabularyId;
                                }

                                terms.Add(t);
                            }
                        }
                    }

                    break;
                case "custom":
                    {
                        if (this.vocabularies != null)
                        {
                            var termRep = Util.GetTermController();

                            foreach (var sVocabularyId in this.vocabularies)
                            {
                                terms.AddRange(termRep.GetTermsByVocabulary(int.Parse(sVocabularyId)));
                            }
                        }
                    }

                    break;
            }

            return terms;
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
            var objModuleController = new ModuleController();

            var moduleSettings = objModuleController.GetTabModuleSettings(this.TabModuleId);

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
                this.settings.renderMode = RenderMode.HTML5;
            }

            // Setting Render Item Weight
            if (!string.IsNullOrEmpty((string)moduleSettings["RenderItemWeight"]))
            {
                bool.TryParse((string)moduleSettings["RenderItemWeight"], out var renderItemWeight);

                this.settings.RenderItemWeight = renderItemWeight;
            }

            // Get Search Tab
            if (!string.IsNullOrEmpty((string)this.TabModuleSettings["CustomSearchPage"]))
            {
                this.settings.searchTabId = Convert.ToInt32((string)this.TabModuleSettings["CustomSearchPage"]);
            }
            else
            {
                var portal = new PortalController().GetPortal(PortalSettings.PortalId);

                this.settings.searchTabId = portal.SearchTabId;
            }

            this.settings.SearchQueryString =
                !string.IsNullOrEmpty((string)this.TabModuleSettings["SearchPageQueryString"])
                    ? (string)this.TabModuleSettings["SearchPageQueryString"]
                    : "Search=";

            this.settings.SearchTaxQueryString =
               !string.IsNullOrEmpty((string)this.TabModuleSettings["SearchPageTaxQueryString"])
                   ? (string)this.TabModuleSettings["SearchPageTaxQueryString"]
                   : "Tag=";

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

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeCustom"]))
            {
                bool.TryParse((string)moduleSettings["ModeCustom"], out var modeCustom);

                this.settings.ModeCustom = modeCustom;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeTax"]))
            {
                bool.TryParse((string)moduleSettings["ModeTax"], out var modeTax);

                this.settings.ModeTax = modeTax;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeNewsarticles"]))
            {
                bool.TryParse((string)moduleSettings["ModeNewsarticles"], out var modenews);

                this.settings.ModeNewsarticles = modenews;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeSimplegallery"]))
            {
                bool.TryParse((string)moduleSettings["ModeSimplegallery"], out var modesimple);

                this.settings.ModeSimplegallery = modesimple;
            }

            if (!string.IsNullOrEmpty((string)moduleSettings["ModeActiveforums"]))
            {
                bool.TryParse((string)moduleSettings["ModeActiveforums"], out var modeactive);

                this.settings.ModeActiveforums = modeactive;
            }

            // Load old Setting
            if (!string.IsNullOrEmpty((string)moduleSettings["TagMode"]))
            {
                var sTagMode = (string)moduleSettings["TagMode"];

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
                    case "tax":
                        this.settings.ModeTax = true;
                        break;
                }
            }

            // Check if all false, set search
            if (!this.settings.ModeCustom && !this.settings.ModeTax && !this.settings.ModeNewsarticles &&
                !this.settings.ModeSimplegallery && !this.settings.ModeActiveforums)
            {
               // this.settings.ModeSearch = true;
               // TODO
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

            if (!string.IsNullOrEmpty((string)moduleSettings["TaxMode"]))
            {
                try
                {
                    this.settings.TaxMode = (string)moduleSettings["TaxMode"];
                }
                catch (Exception)
                {
                    this.settings.ModeTax = false;
                }
            }
            else
            {
                this.settings.ModeTax = false;
            }

            // Setting TaxMode Vocabularies
            if (!string.IsNullOrEmpty((string)moduleSettings["TaxVocabularies"]))
            {
                var sVocabularies = (string)moduleSettings["TaxVocabularies"];

                this.vocabularies = sVocabularies.Split(';');
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
                ClientResourceManager.RegisterStyleSheet(this.Page,
                    $"{this.ResolveUrl("Skins")}/{this.settings.SkinName}.css");

                //PageBase.RegisterStyleSheet(Page,  string.Format("{0}/{1}.css", this.ResolveUrl("Skins"), this.settings.SkinName));

                this.tagCloudDiv.CssClass += $"-{this.settings.SkinName}";
                this.c1.ItemCssClassPrefix += $"-{this.settings.SkinName}";
            }

            this.settings.OccurCount = !string.IsNullOrEmpty((string)moduleSettings["occurcount"])
                                   ? int.Parse((string)moduleSettings["occurcount"])
                                   : 1;

            this.settings.TagCount = !string.IsNullOrEmpty((string)moduleSettings["tagscount"])
                                 ? int.Parse((string)moduleSettings["tagscount"])
                                 : 30;

            if (!string.IsNullOrEmpty((string)moduleSettings["ExcludeCommon"]))
            {
                bool.TryParse((string)moduleSettings["ExcludeCommon"], out var excludeCommon);

                this.settings.ExcludeCommon = excludeCommon;
            }

            if (this.settings.ExcludeCommon)
            {
                // Load Common Words, current Language
                var commonWords = Localization.GetString("CommonWords.Text", this.LocalResourceFile);

                if (!string.IsNullOrEmpty(commonWords))
                {
                    var comnmonWordList = commonWords.Split(',');

                    foreach (var word in comnmonWordList)
                    {
                        this.settings.exlusionWords.Add(new ExcludeWord { Word = word, ExcludeWordType = ExcludeType.Equals });
                    }
                }
            }

            // Import Old List
            var strExlusionList = (string)moduleSettings["exlusLst"];

            if (!string.IsNullOrEmpty(strExlusionList))
            {
                var oldList = strExlusionList.Split(',');

                foreach (var word in oldList)
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

            var sRenderUl = (string)moduleSettings["RenderUl"];

            this.settings.RenderUl = !string.IsNullOrEmpty(sRenderUl) && bool.Parse(sRenderUl);

            if (this.settings.renderMode.Equals(RenderMode.HTML5))
            {
                this.settings.RenderUl = true;
                this.c1.CanvasEnabled = true;
            }

            this.c1.Skin = this.settings.SkinName;

            // Fix for sliding theme
            if (this.settings.SkinName.Contains("Sliding"))
            {
                this.settings.RenderUl = true;
            }

            this.c1.RenderItemWeight = this.settings.RenderItemWeight;

            var strTagsLink = (string)moduleSettings["tagslink"];

            this.settings.TagsLink = string.IsNullOrEmpty(strTagsLink) || bool.Parse(strTagsLink);

            var strTagsLinkChk = (string)moduleSettings["tagslinkChk"];

            this.settings.TagsLinkChk = string.IsNullOrEmpty(strTagsLinkChk) || bool.Parse(strTagsLinkChk);

            var sCacheItems = (string)moduleSettings["CacheItems"];

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

            /*try
            {
                sTagCloudHeight = ((string)moduleSettings["tagcloudheight"]);
            }
            finally
            {
                if (string.IsNullOrEmpty(sTagCloudHeight))
                {
                    sTagCloudHeight = "300";
                    sTagCloudHValue = "px";
                }

                if (sTagCloudHeight.Contains("px"))
                {
                    sTagCloudHeight = sTagCloudHeight.Replace("px", "");
                    sTagCloudHValue = "px";
                }

                if (sTagCloudHeight.Contains("%"))
                {
                    sTagCloudHeight = sTagCloudHeight.Replace("%", "");
                    sTagCloudHValue = "%";
                }
            }

            if (sTagCloudHeight.Equals("-1"))
            {
                sTagCloudHeight = "auto";
                sTagCloudHValue = "";
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
                this.settings.Tcolor = string.IsNullOrEmpty(this.settings.Tcolor) ? "0x000000" : $"0x{this.settings.Tcolor}";
            }

            try
            {
                this.settings.Tcolor2 = (string)moduleSettings["tcolor2"];
            }
            finally
            {
                this.settings.Tcolor2 = string.IsNullOrEmpty(this.settings.Tcolor2) ? "0x000000" : $"0x{this.settings.Tcolor2}";
            }

            try
            {
                this.settings.Hicolor = (string)moduleSettings["hicolor"];
            }
            finally
            {
                this.settings.Hicolor = string.IsNullOrEmpty(this.settings.Hicolor) ? "0x42a5ff" : $"0x{this.settings.Hicolor}";
            }

            try
            {
                this.settings.Bgcolor = (string)moduleSettings["bgcolor"];
            }
            finally
            {
                this.settings.Bgcolor = string.IsNullOrEmpty(this.settings.Bgcolor)
                                    ? "transparent"
                                    : $"#{this.settings.Bgcolor}";
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
                $"width:{this.settings.TagCloudWidth}{this.settings.TagCloudWValue};height:auto;";
        }

        #endregion
    }
}