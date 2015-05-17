namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class HierarchySettings
    {
        public HierarchySettings()
        {
            this.HierarchyMode = Trirand.Web.Mvc.HierarchyMode.None;
            this.PlusIcon = "ui-icon-plus";
            this.MinusIcon = "ui-icon-minus";
            this.OpenIcon = "ui-icon-carat-1-sw";
            this.ExpandOnLoad = false;
            this.SelectOnExpand = false;
            this.ReloadOnExpand = true;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if ((this.PlusIcon != null) && (this.PlusIcon != "ui-icon-plus"))
            {
                hashtable.Add("plusicon", this.PlusIcon);
            }
            if ((this.MinusIcon != null) && (this.MinusIcon != "ui-icon-minus"))
            {
                hashtable.Add("minusicon", this.MinusIcon);
            }
            if ((this.OpenIcon != null) && (this.OpenIcon != "ui-icon-carat-1-sw"))
            {
                hashtable.Add("openicon", this.OpenIcon);
            }
            if (this.ExpandOnLoad)
            {
                hashtable.Add("expandOnLoad", true);
            }
            if (this.SelectOnExpand)
            {
                hashtable.Add("selectOnExpand", true);
            }
            if (!this.ReloadOnExpand)
            {
                hashtable.Add("reloadOnExpand", false);
            }
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public bool ExpandOnLoad { get; set; }

        public Trirand.Web.Mvc.HierarchyMode HierarchyMode { get; set; }

        public string MinusIcon { get; set; }

        public string OpenIcon { get; set; }

        public string PlusIcon { get; set; }

        public bool ReloadOnExpand { get; set; }

        public bool SelectOnExpand { get; set; }
    }
}

