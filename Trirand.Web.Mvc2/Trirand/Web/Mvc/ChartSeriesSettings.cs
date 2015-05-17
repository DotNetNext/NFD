namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ChartSeriesSettings
    {
        public ChartSeriesSettings()
        {
            this.Name = "";
            this.Color = "";
            this.Data = null;
            this.Stack = "";
            this.Type = ChartType.Line;
            this.Marker = new ChartMarkerSettings();
            this.DataLabels = new ChartLabelSettings();
            this.Size = Unit.Empty;
            this.InnerSize = Unit.Empty;
        }

        internal Hashtable ToHashtable(JQChart chart)
        {
            Hashtable hashtable = new Hashtable();
            if (!string.IsNullOrEmpty(this.Name))
            {
                hashtable.Add("name", this.Name);
            }
            if (this.Type != ChartType.Line)
            {
                hashtable.Add("type", this.Type.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(this.Color))
            {
                hashtable.Add("color", this.Color);
            }
            if (this.Marker.IsSet())
            {
                hashtable.Add("marker", this.Marker.ToHashtable());
            }
            if (!string.IsNullOrEmpty(this.Stack))
            {
                hashtable.Add("stack", this.Stack);
            }
            if (this.DataLabels.IsSet(chart))
            {
                hashtable.Add("dataLabels", this.DataLabels.ToHashtable(chart));
            }
            if (!this.Size.IsEmpty)
            {
                if (this.Size.Type != UnitType.Percentage)
                {
                    hashtable.Add("size", this.Size.Value.ToString());
                }
                else
                {
                    hashtable.Add("size", this.Size.ToString());
                }
            }
            if (!this.InnerSize.IsEmpty)
            {
                if (this.InnerSize.Type != UnitType.Percentage)
                {
                    hashtable.Add("innerSize", this.InnerSize.Value.ToString());
                    return hashtable;
                }
                hashtable.Add("innerSize", this.InnerSize.ToString());
            }
            return hashtable;
        }

        public string Color { get; set; }

        public IEnumerable<ChartPoint> Data { get; set; }

        public ChartLabelSettings DataLabels { get; set; }

        public Unit InnerSize { get; set; }

        public ChartMarkerSettings Marker { get; set; }

        public string Name { get; set; }

        public Unit Size { get; set; }

        public string Stack { get; set; }

        public ChartType Type { get; set; }
    }
}

