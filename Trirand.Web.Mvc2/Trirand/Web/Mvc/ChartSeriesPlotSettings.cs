namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartSeriesPlotSettings
    {
        public ChartSeriesPlotSettings()
        {
            this.Stacking = ChartSeriesStacking.None;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if (this.Stacking != ChartSeriesStacking.None)
            {
                hashtable.Add("stacking", this.Stacking.ToString().ToLower());
            }
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public ChartSeriesStacking Stacking { get; set; }
    }
}

