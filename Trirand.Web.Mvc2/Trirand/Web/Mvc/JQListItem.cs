namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class JQListItem
    {
        public JQListItem()
        {
            this.Text = "";
            this.Value = "";
            this.Selected = false;
            this.Enabled = true;
            this.Url = "";
            this.ImageUrl = "";
            this.Options = new NameValueCollection();
            this.ItemTemplateID = "";
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if (!string.IsNullOrEmpty(this.Text))
            {
                hashtable.Add("text", this.Text);
            }
            if (!string.IsNullOrEmpty(this.Value))
            {
                hashtable.Add("value", this.Value);
            }
            if (!this.Enabled)
            {
                hashtable.Add("enabled", false);
            }
            if (this.Selected)
            {
                hashtable.Add("selected", true);
            }
            if (!string.IsNullOrEmpty(this.Url))
            {
                hashtable.Add("url", this.Url);
            }
            if (!string.IsNullOrEmpty(this.ImageUrl))
            {
                hashtable.Add("imageUrl", this.ImageUrl);
            }
            if (!string.IsNullOrEmpty(this.ExpandedImageUrl))
            {
                hashtable.Add("expandedImageUrl", this.ExpandedImageUrl);
            }
            if (!string.IsNullOrEmpty(this.ItemTemplateID))
            {
                hashtable.Add("itemTemplateID", this.ItemTemplateID);
            }
            foreach (string str in this.Options.AllKeys)
            {
                string str2 = this.Options[str];
                if (str2 != null)
                {
                    hashtable.Add(str, str2);
                }
            }
            return hashtable;
        }

        public string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public bool Enabled { get; set; }

        public string ExpandedImageUrl { get; set; }

        public string ImageUrl { get; set; }

        public string ItemTemplateID { get; set; }

        public NameValueCollection Options { get; set; }

        public bool Selected { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public string Value { get; set; }
    }
}

