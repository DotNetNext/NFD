namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class ChartClientSideEvents
    {
        public ChartClientSideEvents()
        {
            this.AddSeries = "";
            this.Click = "";
            this.Load = "";
            this.Redraw = "";
            this.Selection = "";
        }

        internal Hashtable ToHashtable(JQChart chart)
        {
            Hashtable h = new Hashtable();
            if (!string.IsNullOrEmpty(this.AddSeries))
            {
                h.AddLiteral("addSeries", this.AddSeries, chart);
            }
            if (!string.IsNullOrEmpty(this.Click))
            {
                h.AddLiteral("click", this.Click, chart);
            }
            if (!string.IsNullOrEmpty(this.Load))
            {
                h.AddLiteral("load", this.Load, chart);
            }
            if (!string.IsNullOrEmpty(this.Redraw))
            {
                h.AddLiteral("redraw", this.Redraw, chart);
            }
            if (!string.IsNullOrEmpty(this.Selection))
            {
                h.AddLiteral("selection", this.Selection, chart);
            }
            return h;
        }

        internal string ToJSON(JQChart chart)
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable(chart));
        }

        public string AddSeries { get; set; }

        public string Click { get; set; }

        public string Load { get; set; }

        public string Redraw { get; set; }

        public string Selection { get; set; }
    }
}

