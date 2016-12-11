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
    using System.Data;
    using System.Reflection;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Modules.ActiveForums;
    using DotNetNuke.Modules.ActiveForums.Data;
    using DotNetNuke.Services.Log.SiteLog;

    using VRK.Controls;

    using WatchersNET.DNN.Modules.TagCloud.Constants;
    using WatchersNET.DNN.Modules.TagCloud.Objects;

    using DataProvider = DotNetNuke.Data.DataProvider;
    using Exceptions = DotNetNuke.Services.Exceptions.Exceptions;

    #endregion

    /// <summary>
    /// The data control.
    /// </summary>
    public class DataControl : PortalModuleBase
    {
        #region Public Methods

        /// <summary>
        /// Get Tag Cloud Items From Active Forums SQL
        /// </summary>
        /// <param name="iPortalId">
        /// The PortalId of the Forums
        /// </param>
        /// <param name="iModuleId">
        /// The Module Instance of the Active Forums
        /// </param>
        /// <returns>
        /// The Tags
        /// </returns>
        public static List<CloudItem> TagCloudActiveForumsTags(int iPortalId, int iModuleId, int tagCount)
        {
            string forumIds;

            // Get Current Forum User
            var forumUser = new UserController().GetUser(iPortalId, iModuleId);

            forumIds = !string.IsNullOrEmpty(forumUser.UserForums)
                           ? forumUser.UserForums
                           : GetForumsForUser(
                               forumUser.UserRoles, iPortalId, iModuleId);
            
            List<CloudItem> afTagsList = new List<CloudItem>();

            using (IDataReader dr = new Common().TagCloud_Get(iPortalId, iModuleId, forumIds, tagCount))
            {
                while (dr.Read())
                {
                    CloudItem item = new CloudItem
                        {
                           Weight = Convert.ToInt32(dr["Priority"]), Text = dr["TagName"].ToString() 
                        };

                    afTagsList.Add(item);
                }
            }

            return afTagsList;
        }

        /// <summary>
        /// Gets the forums for user.
        /// </summary>
        /// <param name="UserRoles">The user roles.</param>
        /// <param name="PortalId">The portal id.</param>
        /// <param name="ModuleId">The module id.</param>
        /// <param name="PermissionType">Type of the permission.</param>
        /// <returns></returns>
        internal static string GetForumsForUser(string UserRoles, int PortalId, int ModuleId, string PermissionType = "CanView")
        {
            ForumsDB forumsDb = new ForumsDB();
            string str = string.Empty;
            foreach (Forum forum in forumsDb.Forums_List(PortalId, ModuleId))
            {
                string AuthorizedRoles;
                switch (PermissionType)
                {
                    case "CanView":
                        AuthorizedRoles = forum.Security.View;
                        break;
                    case "CanRead":
                        AuthorizedRoles = forum.Security.Read;
                        break;
                    case "CanApprove":
                        AuthorizedRoles = forum.Security.ModApprove;
                        break;
                    case "CanEdit":
                        AuthorizedRoles = forum.Security.ModEdit;
                        break;
                    default:
                        AuthorizedRoles = forum.Security.View;
                        break;
                }

                if ((Permissions.HasPerm(AuthorizedRoles, UserRoles)
                     || !forum.Hidden && (PermissionType == "CanView" || PermissionType == "CanRead")) && forum.Active)
                {
                    str = string.Format("{0}{1};", str, forum.ForumID);
                }
            }
            return str;
        }

        /// <summary>
        /// The tag cloud items add.
        /// </summary>
        /// <param name="objTag">
        /// The obj tag.
        /// </param>
        /// <returns>
        /// Returns the Tag ID of the new Tag
        /// </returns>
        public static int TagCloudItemsAdd(CustomTags objTag)
        {
            return
                Convert.ToInt32(
                    DataProvider.Instance().ExecuteScalar(
                        "TagCloudItemsAdd", objTag.iTagId, objTag.iWeight, objTag.sTag, objTag.iModulId, objTag.sUrl));
        }

        /// <summary>
        /// The tag cloud items add ml.
        /// </summary>
        /// <param name="iTagId">
        /// The i tag id.
        /// </param>
        /// <param name="sLocale">
        /// The s locale.
        /// </param>
        /// <param name="sTag">
        /// The s tag.
        /// </param>
        /// <param name="iModuleId">
        /// The i module id.
        /// </param>
        /// <param name="sTagUrl">
        /// The s tag url.
        /// </param>
        public static void TagCloudItemsAddMl(int iTagId, string sLocale, string sTag, int iModuleId, string sTagUrl)
        {
            DataProvider.Instance().ExecuteScalar("TagCloudItemsAddMl", iTagId, sLocale, sTag, iModuleId, sTagUrl);
        }

        /// <summary>
        /// The tag cloud items delete.
        /// </summary>
        /// <param name="iTagId">
        /// The i tag id.
        /// </param>
        /// <param name="iModulId">
        /// The i modul id.
        /// </param>
        public static void TagCloudItemsDelete(int iTagId, int iModulId)
        {
            DataProvider.Instance().ExecuteNonQuery("TagCloudItemsDelete", iTagId, iModulId);
        }

        /// <summary>
        /// The tag cloud items delete ml.
        /// </summary>
        /// <param name="iTagId">
        /// The i tag id.
        /// </param>
        /// <param name="iModulId">
        /// The i modul id.
        /// </param>
        /// <param name="sLocale">
        /// The s locale.
        /// </param>
        public static void TagCloudItemsDeleteMl(int iTagId, int iModulId, string sLocale)
        {
            DataProvider.Instance().ExecuteNonQuery("TagCloudItemsDeleteMl", iTagId, iModulId, sLocale);
        }

        /// <summary>
        ///  Get All Locales of the Tag by TagID and ModuleId
        /// </summary>
        /// <param name="iModulId">
        ///  Module Id that is uses
        /// </param>
        /// <param name="iTagId">
        ///  the Tag id
        /// </param>
        /// <returns>
        /// All Locales of the Tag
        /// </returns>
        public static List<Locales> TagCloudItemsGetByLocale(int iModulId, int iTagId)
        {
            List<Locales> localesList = new List<Locales>();

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("TagCloudItemsGetByLocale", iModulId, iTagId))
            {
                while (dr.Read())
                {
                    Locales locales = new Locales
                        {
                            Locale = Convert.ToString(dr["Locale"]), 
                            TagMl = Convert.ToString(dr["Tag"]), 
                            UrlMl = Convert.ToString(dr["TagUrl"])
                        };

                    localesList.Add(locales);
                }
            }

            return localesList;
        }

        /// <summary>
        /// The tag cloud items get by module.
        /// </summary>
        /// <param name="iModulId">
        /// The module id.
        /// </param>
        /// <returns>
        /// TagCloud Item List
        /// </returns>
        public static List<CustomTags> TagCloudItemsGetByModule(int iModulId)
        {
            List<CustomTags> tagsList = new List<CustomTags>();

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("TagCloudItemsGetByModule", iModulId))
            {
                while (dr.Read())
                {
                    CustomTags tag = new CustomTags
                        {
                            iTagId = Convert.ToInt32(dr["TagID"]), 
                            iWeight = Convert.ToInt32(dr["Weight"]), 
                            sTag = Convert.ToString(dr["Tag"]), 
                            sUrl = Convert.ToString(dr["TagUrl"])
                        };

                    tagsList.Add(tag);
                }
            }

            return tagsList;
        }

        /// <summary>
        /// The tag cloud items update.
        /// </summary>
        /// <param name="objTag">
        /// The obj tag.
        /// </param>
        public static void TagCloudItemsUpdate(CustomTags objTag)
        {
            DataProvider.Instance().ExecuteNonQuery(
                "TagCloudItemsUpdate", objTag.iTagId, objTag.iWeight, objTag.sTag, objTag.iModulId, objTag.sUrl);
        }

        /// <summary>
        /// Get the Occurrence Count for the selected Words Id
        /// </summary>
        /// <param name="searchWordsId">the Id of the Word</param>
        /// <returns>
        /// The Count of Occurrences
        /// </returns>
        public static int TagCloudSearchWordsOccur(int searchWordsId)
        {
            using (IDataReader dr = DataProvider.Instance().ExecuteReader("TagCloud_SearchWords", searchWordsId))
            {
                while (dr.Read())
                {
                    searchWordsId = Convert.ToInt32(dr["Occurrences"]);
                }
            }

            return searchWordsId;
        }

        /// <summary>
        /// Get Referrers from Sitelog
        /// </summary>
        /// <param name="portalId">
        /// The Portal ID that is used
        /// </param>
        /// <param name="sPortalAlias">
        /// The Current Portal Alias
        /// </param>
        /// <param name="dtStartDate">
        /// Startdate to get Referres from
        /// </param>
        /// <param name="dtEndDate">
        /// Enddate to get Referres from
        /// </param>
        /// <returns>
        /// Item List with Referrers
        /// </returns>
        public static List<ReferrerItems> TagCloudSiteLogInfo(
            int portalId, string sPortalAlias, DateTime dtStartDate, DateTime dtEndDate)
        {
            List<ReferrerItems> siteLogInfoList = new List<ReferrerItems>();

            SiteLogController objSiteLog = new SiteLogController();

            try
            {
                using (IDataReader dr = objSiteLog.GetSiteLog(portalId, sPortalAlias, 4, dtStartDate, dtEndDate))
                {
                    try
                    {
                        while (dr.Read())
                        {
                            ReferrerItems item = new ReferrerItems();

                            string sReferrer = Convert.ToString(dr["Referrer"]);

                            if (((((!sReferrer.Contains("search?q=") && !sReferrer.Contains("&q=")) &&
                                   !sReferrer.Contains("bing")) && !sReferrer.Contains(@"Search=")) &&
                                 !sReferrer.Contains("yahoo")) && !sReferrer.Contains(@"ask.com"))
                            {
                                continue;
                            }

                            item.Referrer = sReferrer;

                            siteLogInfoList.Add(item);
                        }
                    }
                    finally
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
            catch (Exception /*exc*/)
            {
                // Exceptions.LogException(exc);
                return siteLogInfoList;
            }

            return siteLogInfoList;
        }

        /// <summary>
        /// Add ne Exclude Word
        /// </summary>
        /// <param name="addWord">
        /// The add Word.
        /// </param>
        /// <returns>
        /// The New Word ID
        /// </returns>
        public static int TagCloudExcludeWordAdd(ExcludeWord addWord)
        {
            return
                Convert.ToInt32(
                    DataProvider.Instance().ExecuteScalar(
                        "TagCloudExcludeWordAdd", addWord.Word, addWord.ExcludeWordType, addWord.ModuleID, addWord.WordID));
        }

        /// <summary>
        /// Update the Exclude Word
        /// </summary>
        /// <param name="updateWord">
        /// The update Word.
        /// </param>
        public static void TagCloudExcludeWordUpdate(ExcludeWord updateWord)
        {
            DataProvider.Instance().ExecuteNonQuery(
                "TagCloudExcludeWordUpdate", updateWord.Word, updateWord.ExcludeWordType, updateWord.ModuleID, updateWord.WordID);
        }

        /// <summary>
        /// Delete the Exclude Word
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="wordId">
        /// The word id.
        /// </param>
        public static void TagCloudExcludeWordDelete(int moduleId, int wordId)
        {
            DataProvider.Instance().ExecuteNonQuery("TagCloudExcludeWordDelete", moduleId, wordId);
        }

        /// <summary>
        /// Get All Exclude Words by Module
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// List with Exclude Words
        /// </returns>
        public static List<ExcludeWord> TagCloudExcludeWordsGetByModule(int moduleId)
        {
            var wordList = new List<ExcludeWord>();

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("TagCloudExcludeWordsGetByModule", moduleId))
            {
                while (dr.Read())
                {
                    var word = new ExcludeWord
                    {
                        Word = Convert.ToString(dr["Word"]),
                        ExcludeWordType =
                            (ExcludeType)Enum.Parse(typeof(ExcludeType), Convert.ToString(dr["ExcludeWordType"])),
                        WordID = Convert.ToInt32(dr["WordID"]),
                        ModuleID = moduleId
                    };

                    wordList.Add(word);
                }
            }

            return wordList;
        }

        /// <summary>
        /// Get Exclude Word by moduleId and wordId
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="wordId">
        /// The word id.
        /// </param>
        /// <returns>
        /// Returns a specific Exclude Word
        /// </returns>
        public static ExcludeWord TagCloudExcludeWordsGetWord(int moduleId, int wordId)
        {
            ExcludeWord word = null;

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("TagCloudExcludeWordsGetWord", moduleId, wordId))
            {
                while (dr.Read())
                {
                    word = new ExcludeWord
                    {
                        Word = Convert.ToString(dr["Word"]),
                        ExcludeWordType =
                            (ExcludeType)Enum.Parse(typeof(ExcludeType), Convert.ToString(dr["ExcludeWordType"])),
                        WordID = wordId,
                        ModuleID = moduleId
                    };
                }
            }

            return word;
        }

        #endregion
    }
}