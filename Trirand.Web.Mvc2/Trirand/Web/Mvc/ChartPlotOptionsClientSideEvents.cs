namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartPlotOptionsClientSideEvents
    {
        public ChartPlotOptionsClientSideEvents()
        {
            this.Click = "";
            this.CheckBoxClick = "";
            this.Hide = "";
            this.LegendItemClick = "";
            this.MouseOver = "";
            this.MouseOut = "";
            this.Show = "";
        }

        internal Hashtable ToHashtable()
        {
            return new Hashtable();
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public string CheckBoxClick { get; set; }

        public string Click { get; set; }

        public string Hide { get; set; }

        public string LegendItemClick { get; set; }

        public string MouseOut { get; set; }

        public string MouseOver { get; set; }

        public string Show { get; set; }
    }
}

