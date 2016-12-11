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
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Statistical functions
    /// </summary>
    public static class Statistics
    {
        #region Public Methods

        /// <summary>
        /// The mean.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The mean.
        /// </returns>
        public static double Mean(IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// The std dev.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="mean">
        /// The mean.
        /// </param>
        /// <returns>
        /// The std dev.
        /// </returns>
        public static double StdDev(IEnumerable<double> values, out double mean)
        {
            mean = Mean(values);
            double sumOfDiffSquares = 0;
            int count = 0;

            foreach (double d in values)
            {
                double diff = d - mean;
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return Math.Sqrt(sumOfDiffSquares / count);
        }

        /// <summary>
        /// The std dev.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The std dev.
        /// </returns>
        public static double StdDev(IEnumerable<double> values)
        {
            double mean;
            return StdDev(values, out mean);
        }

        #endregion
    }
}