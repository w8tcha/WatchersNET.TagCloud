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

namespace WatchersNET.DNN.Modules.TagCloud.Objects
{
    using System;
    using System.Collections.Generic;

    using WatchersNET.DNN.Modules.TagCloud.Constants;

    /// <summary>
    /// The TagCloud Settings
    /// </summary>
    public class TagCloudSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether cache items.
        /// </summary>
        public bool CacheItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blogs all.
        /// </summary>
        public bool DnnBlogsAll { get; set; }

        /// <summary>
        /// Gets or sets The Render Mode
        /// </summary>
        public RenderMode renderMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Render the Item Weight
        /// </summary>
        public bool RenderItemWeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mode activeforums.
        /// </summary>
        public bool ModeActiveforums { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mode custom.
        /// </summary>
        public bool ModeCustom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  dnnblog.
        /// </summary>
        public bool ModeDnnblog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mode newsarticles.
        /// </summary>
        public bool ModeNewsarticles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mode simplegallery.
        /// </summary>
        public bool ModeSimplegallery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mode tax.
        /// </summary>
        public bool ModeTax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether render ul.
        /// </summary>
        public bool RenderUl { get; set; }

        /// <summary>
        /// Gets or sets The Sort Type for the Cloud
        /// </summary>
        public SortType CloudSortType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether The tags link.
        /// </summary>
        public bool TagsLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether The tags link chk.
        /// </summary>
        public bool TagsLinkChk { get; set; }

        /// <summary>
        /// Gets or sets The start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets The exclusion words.
        /// </summary>
        public List<ExcludeWord> exlusionWords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Exclude Common Words.
        /// </summary>
        public bool ExcludeCommon { get; set; }

        /// <summary>
        /// Gets or sets The Active Forums Module Instance.
        /// </summary>
        public int AfModule { get; set; }

        /// <summary>
        /// Gets or sets Gets or sets The tab of ActiveForums.
        /// </summary>
        public int AfTab { get; set; }

        /// <summary>
        /// Gets or sets Gets or sets The dnn blog tab.
        /// </summary>
        public int DnnBlogTab { get; set; }

        /// <summary>
        /// Gets or sets The occur count.
        /// </summary>
        public int OccurCount { get; set; }

        /// <summary>
        /// Gets or sets The tag count.
        /// </summary>
        public int TagCount { get; set; }

        /// <summary>
        /// Gets or sets The ventrian module news.
        /// </summary>
        public int VentrianModuleNews { get; set; }

        /// <summary>
        /// Gets or sets The ventrian module simple.
        /// </summary>
        public int VentrianModuleSimple { get; set; }

        /// <summary>
        /// Gets or sets The ventrian tab news.
        /// </summary>
        public int VentrianTabNews { get; set; }

        /// <summary>
        /// Gets or sets The ventrian tab simple.
        /// </summary>
        public int VentrianTabSimple { get; set; }

        /// <summary>
        /// Gets or sets The bgcolor.
        /// </summary>
        public string Bgcolor { get; set; }

        /// <summary>
        /// Gets or sets The dnn blog blogs.
        /// </summary>
        public string[] DnnBlogBlog { get; set; }

        /// <summary>
        /// Gets or sets The flash height.
        /// </summary>
        public string FlashHeight { get; set; }

        /// <summary>
        /// Gets or sets The flash width.
        /// </summary>
        public string FlashWidth { get; set; }

        /// <summary>
        /// Gets or sets The hicolor.
        /// </summary>
        public string Hicolor { get; set; }

        /// <summary>
        /// Gets or sets The skin name.
        /// </summary>
        public string SkinName { get; set; }

        /// <summary>
        /// Gets or sets The tag cloud w value.
        /// </summary>
        public string TagCloudWValue { get; set; }

        /// <summary>
        /// Gets or sets The tag cloud width.
        /// </summary>
        public string TagCloudWidth { get; set; }

        /// <summary>
        /// Gets or sets tax mode.
        /// </summary>
        public string TaxMode { get; set; }

        /// <summary>
        /// Gets or sets The tag separator.
        /// </summary>
        public string TagSeparator { get; set; }

        /// <summary>
        /// Gets or sets The  Flash tcolor.
        /// </summary>
        public string Tcolor { get; set; }

        /// <summary>
        /// Gets or sets The Flash tcolor 2.
        /// </summary>
        public string Tcolor2 { get; set; }

        /// <summary>
        /// Gets or sets The Flash tspeed.
        /// </summary>
        public string Tspeed { get; set; }

        /// <summary>
        /// Gets or sets The search tab id.
        /// </summary>
        public int searchTabId { get; set; }

        /// <summary>
        /// Gets or sets the search query string.
        /// </summary>
        /// <value>
        /// The search query string.
        /// </value>
        public string SearchQueryString { get; set; }

        /// <summary>
        /// Gets or sets the search tax query string.
        /// </summary>
        /// <value>
        /// The search tax query string.
        /// </value>
        public string SearchTaxQueryString { get; set; }

        /// <summary>
        /// Gets or sets weightMode for the Canvas Cloud
        /// </summary>
        public WeightMode weightMode { get; set; }

        /// <summary>
        /// Gets or sets Font Family for the Canvas Cloud.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the word cloud (wordle) settings.
        /// </summary>
        /// <value>
        /// The word cloud settings.
        /// </value>
        public WordCloudSettings WordCloudSettings { get; set; }
    }
}