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

namespace WatchersNET.DNN.Modules.TagCloud
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Helper Class
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Sort a List Ascending
        /// </summary>
        /// <param name="source">
        /// The source List
        /// </param>
        /// <param name="selector">
        /// The selector.
        /// </param>
        /// <typeparam name="TSource">
        /// The TSource List Item
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The TValue List item
        /// </typeparam>
        public static void SortAscending<TSource, TValue>(List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
        }

        /// <summary>
        /// Sort a List Descending
        /// </summary>
        /// <param name="source">
        /// The source List
        /// </param>
        /// <param name="selector">
        /// The selector.
        /// </param>
        /// <typeparam name="TSource">
        /// The TSource List Item
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The TValue List item
        /// </typeparam>
        public static void SortDescending<TSource, TValue>(List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
        }

        /// <summary>
        /// Check if Object is a Number
        /// </summary>
        /// <param name="valueToCheck">Object to Check</param>
        /// <returns>
        ///   <c>true</c> if the specified value to check is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(object valueToCheck)
        {
            var inputValue = Convert.ToString(valueToCheck);

            var numeric = double.TryParse(inputValue, System.Globalization.NumberStyles.Any, null, out var dummy);

            return numeric;
        }

        /// <summary>
        /// Remove some Characters from the Word otherwise Flash will be not rendered correctly
        /// </summary>
        /// <param name="sString">the String to Check</param>
        /// <returns>Cleaned String</returns>
        public static string RemoveIllegalCharecters(string sString)
        {
            var sNewComposed = sString;

            sNewComposed = sNewComposed.Replace("'", string.Empty);
            sNewComposed = sNewComposed.Replace("\"", string.Empty);
            sNewComposed = sNewComposed.Replace("%", string.Empty);
            sNewComposed = sNewComposed.Replace(":", string.Empty);
            sNewComposed = sNewComposed.Replace("/", string.Empty);
            sNewComposed = sNewComposed.Replace("\\", string.Empty);
            sNewComposed = sNewComposed.Replace("„", string.Empty);
            sNewComposed = sNewComposed.Replace(",", string.Empty);
            sNewComposed = sNewComposed.Replace("+", string.Empty);
            sNewComposed = sNewComposed.Replace("=", string.Empty);
            sNewComposed = sNewComposed.Replace("]", " ");
            sNewComposed = sNewComposed.Replace("[", " ");
            sNewComposed = sNewComposed.Replace("}", " ");
            sNewComposed = sNewComposed.Replace("{", " ");
            sNewComposed = sNewComposed.Replace(")", " ");
            sNewComposed = sNewComposed.Replace("(", " ");
            sNewComposed = sNewComposed.Replace("#", string.Empty);
            sNewComposed = sNewComposed.Replace(">", string.Empty);
            sNewComposed = sNewComposed.Replace("<", string.Empty);

            if (sNewComposed.Contains("&"))
            {
                sNewComposed = sNewComposed.Remove(sNewComposed.IndexOf("&", StringComparison.Ordinal));
            }

            if (sNewComposed.EndsWith("."))
            {
                sNewComposed = sNewComposed.Remove(sNewComposed.Length - 1);
            }

            return sNewComposed;
        }

        /// <summary>
        /// Check if Ventrian Simply Gallery is Installed
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is simply gallery installed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSimplyGalleryInstalled()
        {
            return File.Exists(HttpContext.Current.Server.MapPath("~/bin/Ventrian.SimpleGallery.dll"));
        }

        /// <summary>
        /// Check if Ventrian News Articles is Installed
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is news articles installed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNewsArticlesInstalled()
        {
            return File.Exists(HttpContext.Current.Server.MapPath("~/bin/Ventrian.NewsArticles.dll"));
        }

        /// <summary>
        /// Check if Active Forums Module is Installed
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is active forums installed]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsActiveForumsInstalled()
        {
            return File.Exists(HttpContext.Current.Server.MapPath("~/bin/Active.Modules.Forums.40.dll"))
                   || File.Exists(HttpContext.Current.Server.MapPath("~/bin/DotNetNuke.Modules.ActiveForums.dll"));
        }
    }
}
