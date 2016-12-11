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
    using System.Text;
    using System.Xml;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Search;

    using WatchersNET.DNN.Modules.TagCloud.Constants;

    #endregion

    /// <summary>
    /// The controller.
    /// </summary>
    public class Controller : ModuleSettingsBase, IPortable, ISearchable
    {
        #region Implemented Interfaces

        #region IPortable

        /// <summary>
        /// Export Module Settings
        /// </summary>
        /// <param name="moduleId">
        ///  Current Module Instance id
        /// </param>
        /// <returns>
        /// The export module.
        /// </returns>
        public string ExportModule(int moduleId)
        {
            ModuleController objModules = new ModuleController();

            Hashtable moduleSettings = objModules.GetTabModuleSettings(TagCloud.CurrentTabModuleId);

            StringBuilder sBXml = new StringBuilder();

            try
            {
                string sTagsCount,
                       sExcludeType,
                       sExcludeCommon,
                       sSortTags,
                       sRenderUl,
                       sTagsLink,
                       sTagsLinkChk,
                       sCacheItems,
                       sCustomSearchPage,
                       sSearchPageQueryString,
                       sFontFamily,
                       sWeightMode,
                       sRenderMode,
                       sRenderItemWeight,
                       sTagCloudWidth,
                       sFlashWidth,
                       sFlashHeight,
                       sTcolor = string.Empty,
                       sTcolor2 = string.Empty,
                       sHicolor = string.Empty,
                       sBgcolor = string.Empty,
                       sTspeed = string.Empty,
                       sOccurCount,
                       sTagSeparator,
                       sStartDate,
                       sSkinName,
                       sNewsArcticleTab,
                       sNewsArcticleModule,
                       sSimpleGalleryTab,
                       sSimpleGalleryModule,
                       sActiveForumsTab,
                       sActiveForumsModule,
                       sModeCustom,
                       sModeReferrals,
                       sModeNewsarticles,
                       sModeSimplegallery,
                       sModeActiveforums,
                       sModeSearch,
                       sEllipticity = string.Empty,
                       sGridSize = string.Empty,
                       sMinSize = string.Empty,
                       sShape = string.Empty,
                       sWeightFactor = string.Empty,
                       sFillBox = string.Empty;

                sBXml.AppendLine("<TagCloud>");
                sBXml.AppendLine("<Settings>");

                if (!string.IsNullOrEmpty((string)moduleSettings["SkinName"]))
                {
                    try
                    {
                        sSkinName = (string)moduleSettings["SkinName"];
                    }
                    catch (Exception)
                    {
                        sSkinName = "Standard";
                    }
                }
                else
                {
                    sSkinName = "Standard";
                }

                try
                {
                    sTagSeparator = (string)moduleSettings["TagSeparator"];
                }
                catch
                {
                    sTagSeparator = string.Empty;
                }

                try
                {
                    sStartDate = (string)moduleSettings["ReferralStart"];
                }
                catch
                {
                    sStartDate = "01.01.1999";
                }

                try
                {
                    sModeCustom = (string)moduleSettings["ModeCustom"];
                }
                catch
                {
                    sModeCustom = "False";
                }

                try
                {
                    sModeReferrals = (string)moduleSettings["ModeReferrals"];
                }
                catch
                {
                    sModeReferrals = "False";
                }

                try
                {
                    sModeNewsarticles = (string)moduleSettings["ModeNewsarticles"];
                }
                catch
                {
                    sModeNewsarticles = "False";
                }

                try
                {
                    sModeSimplegallery = (string)moduleSettings["ModeSimplegallery"];
                }
                catch
                {
                    sModeSimplegallery = "False";
                }

                try
                {
                    sModeActiveforums = (string)moduleSettings["ModeActiveforums"];
                }
                catch
                {
                    sModeActiveforums = "False";
                }

                try
                {
                    sModeSearch = (string)moduleSettings["ModeSearch"];
                }
                catch
                {
                    sModeSearch = "True";
                }

                try
                {
                    sNewsArcticleTab = (string)moduleSettings["NewsArticlesTab"];
                }
                catch
                {
                    sNewsArcticleTab = "-1";
                }

                try
                {
                    sNewsArcticleModule = (string)moduleSettings["NewsArticlesModule"];
                }
                catch
                {
                    sNewsArcticleModule = "-1";
                }

                try
                {
                    sSimpleGalleryTab = (string)moduleSettings["SimpleGalleryTab"];
                }
                catch
                {
                    sSimpleGalleryTab = "-1";
                }

                try
                {
                    sSimpleGalleryModule = (string)moduleSettings["SimpleGalleryModule"];
                }
                catch
                {
                    sSimpleGalleryModule = "-1";
                }

                try
                {
                    sActiveForumsTab = (string)moduleSettings["ActiveForumsTab"];
                }
                catch
                {
                    sActiveForumsTab = "-1";
                }

                try
                {
                    sActiveForumsModule = (string)moduleSettings["ActiveForumsModule"];
                }
                catch
                {
                    sActiveForumsModule = "-1";
                }

                try
                {
                    sOccurCount = (string)moduleSettings["occurcount"];
                }
                catch
                {
                    sOccurCount = "1";
                }

                try
                {
                    sTagsCount = (string)moduleSettings["tagscount"];
                }
                catch
                {
                    sTagsCount = "60";
                }

                try
                {
                    sExcludeType = (string)moduleSettings["ExcludeType"];
                }
                catch (Exception)
                {
                    sExcludeType = ExcludeType.Equals.ToString();
                }

                try
                {
                    sExcludeCommon = (string)moduleSettings["ExcludeCommon"];
                }
                catch (Exception)
                {
                    sExcludeCommon = "True";
                }

                try
                {
                    sSortTags = (string)moduleSettings["SortType"];
                }
                catch (Exception)
                {
                    sSortTags = SortType.AlphabeticAsc.ToString();
                }

                try
                {
                    sRenderUl = (string)moduleSettings["RenderUl"];
                }
                catch (Exception)
                {
                    sRenderUl = "False";
                }

                try
                {
                    sTagsLink = (string)moduleSettings["tagslink"];
                }
                catch (Exception)
                {
                    sTagsLink = "True";
                }

                try
                {
                    sTagsLinkChk = (string)moduleSettings["tagslinkChk"];
                }
                catch (Exception)
                {
                    sTagsLinkChk = "False";
                }

                try
                {
                    sCacheItems = (string)moduleSettings["CacheItems"];
                }
                catch (Exception)
                {
                    sCacheItems = "true";
                }

                try
                {
                    sCustomSearchPage = (string)moduleSettings["CustomSearchPage"];
                }
                catch (Exception)
                {
                    sCustomSearchPage = "-1";
                }

                try
                {
                    sSearchPageQueryString = (string)moduleSettings["SearchPageQueryString"];
                }
                catch (Exception)
                {
                    sSearchPageQueryString = "Search=";
                }

                try
                {
                    sFontFamily = (string)moduleSettings["FontFamily"];
                }
                catch (Exception)
                {
                    sFontFamily = "Georgia, Arial, sans-serif";
                }

                try
                {
                    sWeightMode = (string)moduleSettings["WeightMode"];
                }
                catch (Exception)
                {
                    sWeightMode = WeightMode.size.ToString();
                }

                try
                {
                    sRenderMode = (string)moduleSettings["RenderMode"];
                }
                catch (Exception)
                {
                    sRenderMode = RenderMode.HTML5.ToString();
                }

                try
                {
                    sRenderItemWeight = (string)moduleSettings["RenderItemWeight"];
                }
                catch (Exception)
                {
                    sRenderItemWeight = "False";
                }

                try
                {
                    sFlashWidth = (string)moduleSettings["flashwidth"];
                }
                catch (Exception)
                {
                    sFlashWidth = "500";
                }

                try
                {
                    sFlashHeight = (string)moduleSettings["flashheight"];
                }
                catch (Exception)
                {
                    sFlashHeight = "300";
                }

                try
                {
                    sTagCloudWidth = (string)moduleSettings["tagcloudwidth"];
                }
                catch (Exception)
                {
                    sTagCloudWidth = "500px";
                }

                /*try
                {
                    sTagCloudHeight = ((string)moduleSettings["tagcloudheight"]);
                }
                catch (Exception)
                {
                    sTagCloudHeight = "300px";
                }*/
                try
                {
                    sTcolor = string.Format("0x{0}", moduleSettings["tcolor"]);
                }
                finally
                {
                    if (string.IsNullOrEmpty(sTcolor))
                    {
                        sTcolor = "0x000000";
                    }
                }

                try
                {
                    sTcolor2 = string.Format("0x{0}", moduleSettings["tcolor2"]);
                }
                finally
                {
                    if (string.IsNullOrEmpty(sTcolor2))
                    {
                        sTcolor2 = "0x000000";
                    }
                }

                try
                {
                    sHicolor = string.Format("0x{0}", moduleSettings["hicolor"]);
                }
                finally
                {
                    if (string.IsNullOrEmpty(sHicolor))
                    {
                        sHicolor = "0x42a5ff";
                    }
                }

                try
                {
                    sBgcolor = string.Format("#{0}", moduleSettings["bgcolor"]);
                }
                finally
                {
                    if (string.IsNullOrEmpty(sBgcolor))
                    {
                        sBgcolor = "transparent";
                    }
                    else
                    {
                        if (sBgcolor.Equals("#transparent"))
                        {
                            sBgcolor = "transparent";
                        }
                    }
                }

                try
                {
                    sTspeed = (string)moduleSettings["tspeed"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sTspeed))
                    {
                        sTspeed = "75";
                    }
                }

                try
                {
                    sEllipticity = (string)moduleSettings["Ellipticity"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sEllipticity))
                    {
                        sEllipticity = "0.85";
                    }
                }

                try
                {
                    sGridSize = (string)moduleSettings["GridSize"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sGridSize))
                    {
                        sGridSize = "8";
                    }
                }

                try
                {
                    sMinSize = (string)moduleSettings["MinSize"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sMinSize))
                    {
                        sMinSize = "0";
                    }
                }

                try
                {
                    sShape = (string)moduleSettings["Shape"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sShape))
                    {
                        sShape = "circle";
                    }
                }

                try
                {
                    sWeightFactor = (string)moduleSettings["WeightFactor"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sWeightFactor))
                    {
                        sWeightFactor = "2.1";
                    }
                }

                try
                {
                    sFillBox = (string)moduleSettings["FillBox"];
                }
                finally
                {
                    if (string.IsNullOrEmpty(sFillBox))
                    {
                        sFillBox = "False";
                    }
                }

                sBXml.AppendFormat("<occurcount>{0}</occurcount>", XmlUtils.XMLEncode(sOccurCount));
                sBXml.AppendFormat("<skinname>{0}</skinname>", XmlUtils.XMLEncode(sSkinName));
                sBXml.AppendFormat("<tagseparator>{0}</tagseparator>", XmlUtils.XMLEncode(sTagSeparator));
                sBXml.AppendFormat("<referralstart>{0}</referralstart>", XmlUtils.XMLEncode(sStartDate));

                sBXml.AppendFormat("<ModeCustom>{0}</ModeCustom>", XmlUtils.XMLEncode(sModeCustom));
                sBXml.AppendFormat("<ModeReferrals>{0}</ModeReferrals>", XmlUtils.XMLEncode(sModeReferrals));
                sBXml.AppendFormat("<ModeSimplegallery>{0}</ModeSimplegallery>", XmlUtils.XMLEncode(sModeSimplegallery));
                sBXml.AppendFormat("<ModeNewsarticles>{0}</ModeNewsarticles>", XmlUtils.XMLEncode(sModeNewsarticles));
                sBXml.AppendFormat("<ModeActiveforums>{0}</ModeActiveforums>", XmlUtils.XMLEncode(sModeActiveforums));
                sBXml.AppendFormat("<ModeSearch>{0}</ModeSearch>", XmlUtils.XMLEncode(sModeSearch));

                sBXml.AppendFormat("<newsarcticletab>{0}</newsarcticletab>", XmlUtils.XMLEncode(sNewsArcticleTab));
                sBXml.AppendFormat(
                    "<newsarcticlemodule>{0}</newsarcticlemodule>", XmlUtils.XMLEncode(sNewsArcticleModule));
                sBXml.AppendFormat("<simplegallerytab>{0}</simplegallerytab>", XmlUtils.XMLEncode(sSimpleGalleryTab));
                sBXml.AppendFormat(
                    "<simplegallerymodule>{0}</simplegallerymodule>", XmlUtils.XMLEncode(sSimpleGalleryModule));
                sBXml.AppendFormat("<activeforumstab>{0}</activeforumstab>", XmlUtils.XMLEncode(sActiveForumsTab));
                sBXml.AppendFormat(
                    "<activeforumsmodule>{0}</activeforumsmodule>", XmlUtils.XMLEncode(sActiveForumsModule));
                sBXml.AppendFormat("<tagscount>{0}</tagscount>", XmlUtils.XMLEncode(sTagsCount));
                sBXml.AppendFormat("<SortType>{0}</SortType>", XmlUtils.XMLEncode(sSortTags));
                sBXml.AppendFormat("<tagslink>{0}</tagslink>", XmlUtils.XMLEncode(sTagsLink));
                sBXml.AppendFormat("<renderul>{0}</renderul>", XmlUtils.XMLEncode(sRenderUl));
                sBXml.AppendFormat("<tagslinkChk>{0}</tagslinkChk>", XmlUtils.XMLEncode(sTagsLinkChk));
                sBXml.AppendFormat("<cacheitems>{0}</cacheitems>", XmlUtils.XMLEncode(sCacheItems));
                sBXml.AppendFormat("<CustomSearchPage>{0}</CustomSearchPage>", XmlUtils.XMLEncode(sCustomSearchPage));
                sBXml.AppendFormat(
                    "<SearchPageQueryString>{0}</SearchPageQueryString>", XmlUtils.XMLEncode(sSearchPageQueryString));
                sBXml.AppendFormat("<FontFamily>{0}</FontFamily>", XmlUtils.XMLEncode(sFontFamily));
                sBXml.AppendFormat("<WeightMode>{0}</WeightMode>", XmlUtils.XMLEncode(sWeightMode));
                sBXml.AppendFormat("<rendermode>{0}</rendermode>", XmlUtils.XMLEncode(sRenderMode));
                sBXml.AppendFormat("<RenderItemWeight>{0}</RenderItemWeight>", XmlUtils.XMLEncode(sRenderItemWeight));
                sBXml.AppendFormat("<flashwidth>{0}</flashwidth>", XmlUtils.XMLEncode(sFlashWidth));
                sBXml.AppendFormat("<flashheight>{0}</flashheight>", XmlUtils.XMLEncode(sFlashHeight));
                sBXml.AppendFormat("<tagcloudwidth>{0}</tagcloudwidth>", XmlUtils.XMLEncode(sTagCloudWidth));

                // sBXml.AppendLine("<tagcloudheight>" + XmlUtils.XMLEncode(sTagCloudHeight) + "</tagcloudheight>");
                sBXml.AppendFormat("<excludeType>{0}</excludeType>", XmlUtils.XMLEncode(sExcludeType));
                sBXml.AppendFormat("<ExcludeCommon>{0}</ExcludeCommon>", XmlUtils.XMLEncode(sExcludeCommon));
                sBXml.AppendFormat("<tcolor>{0}</tcolor>", XmlUtils.XMLEncode(sTcolor));
                sBXml.AppendFormat("<tcolor2>{0}</tcolor2>", XmlUtils.XMLEncode(sTcolor2));
                sBXml.AppendFormat("<hicolor>{0}</hicolor>", XmlUtils.XMLEncode(sHicolor));
                sBXml.AppendFormat("<bgcolor>{0}</bgcolor>", XmlUtils.XMLEncode(sBgcolor));
                sBXml.AppendFormat("<tspeed>{0}</tspeed>", XmlUtils.XMLEncode(sTspeed));

                sBXml.AppendFormat("<Ellipticity>{0}</Ellipticity>", XmlUtils.XMLEncode(sEllipticity));
                sBXml.AppendFormat("<GridSize>{0}</GridSize>", XmlUtils.XMLEncode(sGridSize));
                sBXml.AppendFormat("<MinSize>{0}</MinSize>", XmlUtils.XMLEncode(sMinSize));
                sBXml.AppendFormat("<Shape>{0}</Shape>", XmlUtils.XMLEncode(sShape));
                sBXml.AppendFormat("<WeightFactor>{0}</WeightFactor>", XmlUtils.XMLEncode(sWeightFactor));
                sBXml.AppendFormat("<FillBox>{0}</FillBox>", XmlUtils.XMLEncode(sFillBox));

                sBXml.AppendLine("</Settings>");
                sBXml.AppendLine("</TagCloud>");

                ////////////////////////
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

            return sBXml.ToString();
        }

        /// <summary>
        /// Import Module Settings
        /// </summary>
        /// <param name="moduleId">
        ///  The Module Id
        /// </param>
        /// <param name="content">
        ///  The Content
        /// </param>
        /// <param name="version">
        ///  The Version
        /// </param>
        /// <param name="userId">
        ///  The User ID
        /// </param>
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            try
            {
                XmlNode xmlTagCloud = Globals.GetContent(content, "TagCloud");

                ModuleController objModules = new ModuleController();

                foreach (XmlNode xmlContent in xmlTagCloud.SelectNodes("Settings"))
                {
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "occurcount", xmlContent["occurcount"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "SkinName", xmlContent["skinname"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "TagSeparator", xmlContent["tagseparator"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ReferralStart", xmlContent["referralstart"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ModeCustom", xmlContent["ModeCustom"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ModeReferrals", xmlContent["ModeReferrals"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ModeSimplegallery", xmlContent["ModeSimplegallery"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ModeNewsarticles", xmlContent["ModeNewsarticles"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ModeActiveforums", xmlContent["ModeActiveforums"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "NewsArticlesTab", xmlContent["newsarticlestab"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "NewsArticlesModule", xmlContent["newsarticlesmodule"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "SimpleGalleryTab", xmlContent["simplegallerytab"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "SimpleGalleryModule", xmlContent["simplegallerymodule"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ActiveForumsTab", xmlContent["activeforumstab"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ActiveForumsModule", xmlContent["activeforumsmodule"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tagscount", xmlContent["tagscount"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "SortType", xmlContent["SortType"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "RenderUl", xmlContent["renderul"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tagslink", xmlContent["tagslink"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tagslinkChk", xmlContent["tagslinkChk"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "CacheItems", xmlContent["cacheitems"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "CustomSearchPage", xmlContent["CustomSearchPage"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId,
                        "SearchPageQueryString",
                        xmlContent["SearchPageQueryString"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "FontFamily", xmlContent["FontFamily"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "WeightMode", xmlContent["WeightMode"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "rendermode", xmlContent["RenderMode"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "RenderItemWeight", xmlContent["RenderItemWeight"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "flashwidth", xmlContent["flashwidth"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "flashheight", xmlContent["flashheight"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tagcloudwidth", xmlContent["tagcloudwidth"].InnerText);

                    /* objModules.UpdateTabModuleSetting(TagCloud.tabModuleId, "tagcloudheight",
                                                       xmlContent["tagcloudheight"].InnerText);*/
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ExcludeType", xmlContent["excludeType"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "ExcludeCommon", xmlContent["ExcludeCommon"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tcolor", xmlContent["tcolor"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tcolor2", xmlContent["tcolor2"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "hicolor", xmlContent["hicolor"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "bgcolor", xmlContent["bgcolor"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "tspeed", xmlContent["tspeed"].InnerText);

                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "Ellipticity", xmlContent["Ellipticity"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "GridSize", xmlContent["GridSize"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "MinSize", xmlContent["MinSize"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "Shape", xmlContent["Shape"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "WeightFactor", xmlContent["WeightFactor"].InnerText);
                    objModules.UpdateTabModuleSetting(
                        TagCloud.CurrentTabModuleId, "FillBox", xmlContent["FillBox"].InnerText);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region ISearchable

        /// <summary>
        /// Included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
        /// </summary>
        /// <param name="modInfo">
        /// The mod Info.
        /// </param>
        /// <returns>
        /// Returns the Search Items
        /// </returns>
        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            return null;
        }

        #endregion

        #endregion
    }
}