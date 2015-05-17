namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartCrossHairSettings
    {
        public ChartCrossHairSettings()
        {
            this.Width = 1;
            this.Color = "blue";
            this.DashStyle = ChartLineDashStyle.Solid;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if (this.Width != 1)
            {
                hashtable.Add("width", this.Width);
            }
            if (this.Color != "blue")
            {
                hashtable.Add("color", this.Color);
            }
            if (this.DashStyle != ChartLineDashStyle.Solid)
            {
                hashtable.Add("dashStyle", this.DashStyle.ToString().ToLower());
            }
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public string Color { get; set; }

        public ChartLineDashStyle DashStyle { get; set; }

        public int Width { get; set; }
    }
}

