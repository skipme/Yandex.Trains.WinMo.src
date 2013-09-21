// -----------------------------------------------------------------------
// <copyright file="WebPageData.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace htmlRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPageData
    {
        public WebPageData()
        {
            Parameters = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Parameters { get; set; }
        public WebPage Page { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> sv in Parameters)
            {
                sb.AppendFormat("'{0}'-'{1}'", sv.Key, sv.Value);
            }
            return string.Format("{0} : ({1})", ""/*Page.Address*/, sb.ToString());
        }
    }
}
