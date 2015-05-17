namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ExportSettings
    {
        public ExportSettings()
        {
            this.Url = "";
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if (!string.IsNullOrEmpty(this.Url))
            {
                hashtable.Add("url", this.Url);
            }
            hashtable.Add("enableImages", true);
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public string Url { get; set; }
    }
}

