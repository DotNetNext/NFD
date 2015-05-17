namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class TreeGridSettings
    {
        public TreeGridSettings()
        {
            this.Enabled = false;
            this.CollapsedIcon = "";
            this.ExpandedIcon = "";
            this.LeafIcon = "";
        }

        public string CollapsedIcon { get; set; }

        public bool Enabled { get; set; }

        public string ExpandedIcon { get; set; }

        public string LeafIcon { get; set; }
    }
}

