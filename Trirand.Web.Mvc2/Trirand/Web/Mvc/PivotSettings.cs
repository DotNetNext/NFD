namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    public class PivotSettings
    {
        public bool FrozenStaticCols;

        public PivotSettings()
        {
            this.XDimension = new List<PivotDimension>();
            this.YDimension = new List<PivotDimension>();
            this.Aggregates = new List<PivotAggregate>();
            this.FrozenStaticCols = false;
            this.RowTotals = false;
            this.RowTotalsText = "Total";
            this.ColTotals = false;
            this.GroupSummary = true;
            this.GroupSummaryPosition = Trirand.Web.Mvc.GroupSummaryPosition.Header;
        }

        internal bool IsPivotEnabled()
        {
            return (this.XDimension.Count > 0);
        }

        internal List<Hashtable> SerializePivotAggregate(List<PivotAggregate> aggregates)
        {
            List<Hashtable> list = new List<Hashtable>();
            foreach (PivotAggregate aggregate in aggregates)
            {
                list.Add(aggregate.ToHashtable());
            }
            return list;
        }

        internal List<Hashtable> SerializePivotDimension(List<PivotDimension> dimensions)
        {
            List<Hashtable> list = new List<Hashtable>();
            foreach (PivotDimension dimension in dimensions)
            {
                list.Add(dimension.ToHashtable());
            }
            return list;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("xDimension", this.SerializePivotDimension(this.XDimension));
            hashtable.Add("yDimension", this.SerializePivotDimension(this.YDimension));
            hashtable.Add("aggregates", this.SerializePivotAggregate(this.Aggregates));
            if (this.FrozenStaticCols)
            {
                hashtable.Add("frozenStaticCols", true);
            }
            if (this.RowTotals)
            {
                hashtable.Add("rowTotals", true);
            }
            if (this.ColTotals)
            {
                hashtable.Add("colTotals", true);
            }
            if (this.RowTotalsText != "Total")
            {
                hashtable.Add("rowTotalsText", this.RowTotalsText);
            }
            if (!this.GroupSummary)
            {
                hashtable.Add("groupSummary", false);
            }
            if (this.GroupSummaryPosition != Trirand.Web.Mvc.GroupSummaryPosition.Header)
            {
                hashtable.Add("groupSummaryPos", this.GroupSummaryPosition.ToString().ToLower());
            }
            return hashtable;
        }

        internal string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this.ToHashtable());
        }

        public List<PivotAggregate> Aggregates { get; set; }

        public bool ColTotals { get; set; }

        public bool GroupSummary { get; set; }

        public Trirand.Web.Mvc.GroupSummaryPosition GroupSummaryPosition { get; set; }

        public bool RowTotals { get; set; }

        public string RowTotalsText { get; set; }

        public List<PivotDimension> XDimension { get; set; }

        public List<PivotDimension> YDimension { get; set; }
    }
}

