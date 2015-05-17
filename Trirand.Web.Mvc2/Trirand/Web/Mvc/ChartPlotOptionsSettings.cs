namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartPlotOptionsSettings
    {
        public ChartPlotOptionsSettings()
        {
            this.Area = new ChartAreaSettings();
            this.AreaSpline = new ChartAreaSplineSettings();
            this.Bar = new ChartBarSettings();
            this.Column = new ChartColumnSettings();
            this.Line = new ChartLineSettings();
            this.Pie = new ChartPieSettings();
            this.Series = new ChartSeriesPlotSettings();
            this.Scatter = new ChartScatterSettings();
            this.Spline = new ChartSplineSettings();
        }

        internal Hashtable ToHashtable(JQChart chart)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("area", this.Area.ToHashtable(chart));
            hashtable.Add("areaspline", this.AreaSpline.ToHashtable(chart));
            hashtable.Add("bar", this.Bar.ToHashtable(chart));
            hashtable.Add("column", this.Column.ToHashtable(chart));
            hashtable.Add("line", this.Line.ToHashtable(chart));
            hashtable.Add("pie", this.Pie.ToHashtable(chart));
            hashtable.Add("series", this.Series.ToHashtable());
            hashtable.Add("scatter", this.Scatter.ToHashtable(chart));
            hashtable.Add("spline", this.Spline.ToHashtable(chart));
            return hashtable;
        }

        internal string ToJSON(JQChart chart)
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable(chart));
        }

        public ChartAreaSettings Area { get; set; }

        public ChartAreaSplineSettings AreaSpline { get; set; }

        public ChartBarSettings Bar { get; set; }

        public ChartColumnSettings Column { get; set; }

        public ChartLineSettings Line { get; set; }

        public ChartPieSettings Pie { get; set; }

        public ChartScatterSettings Scatter { get; set; }

        public ChartSeriesPlotSettings Series { get; set; }

        public ChartSplineSettings Spline { get; set; }
    }
}

