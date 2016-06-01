using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;

namespace FlightChecker
{
    public class ResponseFromWebsite
    {
        public DateTime? FurthestDate { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public static class WebHelper
    {
        /// <summary>Gets the message fro mthe website</summary>
        /// <value>The returned message</value>
        public static ResponseFromWebsite MessageFromWebsite
        {
            get
            {
                string message;
                bool success = false;
                DateTime? furthestDate = null;

                WebResponse webResponse = GetHtml(new Uri(ConfigurationManager.AppSettings["url"]), ConfigurationManager.AppSettings["useragent"]);

                if (webResponse.StatusCode == HttpStatusCode.OK && string.IsNullOrEmpty(webResponse.Html) == false)
                {
                    MatchCollection matches = Regex.Matches(webResponse.Html, ConfigurationManager.AppSettings["dateregex"], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (matches.Count == 1)
                    {
                        string value = matches[0].Groups[ConfigurationManager.AppSettings["matchgroupname"]].Value;
                        if (string.IsNullOrEmpty(value) == false)
                        {
                            // we have a value

                            DateTime parsedDateTime;
                            if (DateTime.TryParse(value, out parsedDateTime))
                            {
                                message = "Furthest date available is " + parsedDateTime.ToString("dd MMM yyyy");
                                furthestDate = parsedDateTime;
                                success = true;
                            }
                            else
                            {
                                // unable to parse value
                                message = "ERROR: Unable to parse date value '" + value + "'";
                            }
                        }
                        else
                        {
                            // no value retrieved
                            message = "ERROR: Empty match value";
                        }
                    }
                    else
                    {
                        message = "ERROR: " + (matches.Count == 0 ? "No match made using Regex" : "More than 1 match return by Regex");
                    }
                }
                else
                {
                    message = "ERROR: No html returned. Error code " + webResponse.StatusCode;
                }

                return new ResponseFromWebsite
                           {
                               Success = success,
                               Message = message,
                               FurthestDate = furthestDate
                           };
            }
        }

        /// <summary>Gets the HTML.</summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <returns>The retrieved Html</returns>
        private static WebResponse GetHtml(Uri uri, string userAgent)
        {
            WebResponse httpResponse = new WebResponse();

            HttpWebRequest.DefaultCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.UserAgent = userAgent;
            httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                httpResponse.StatusCode = httpWebResponse.StatusCode;

                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            httpResponse.Html = streamReader.ReadToEnd();
                            streamReader.Close();
                        }
                    }
                }
            }

            return httpResponse;
        }

        private struct WebResponse
        {
            public string Html;
            public HttpStatusCode StatusCode;
        }
    }
}