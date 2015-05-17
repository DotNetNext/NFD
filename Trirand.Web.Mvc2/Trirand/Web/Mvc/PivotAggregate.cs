namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public class PivotAggregate
    {
        public PivotAggregate()
        {
            this.Member = "";
            this.Label = "";
            this.Aggregator = PivotAggregator.None;
            this.Width = null;
            this.GroupSummaryType = Trirand.Web.Mvc.GroupSummaryType.None;
            this.Formatter = PivotFormatter.None;
            this.Align = TextAlign.Left;
        }

        internal Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable();
            if (!string.IsNullOrEmpty(this.Member))
            {
                hashtable.Add("member", this.Member);
            }
            if (!string.IsNullOrEmpty(this.Label))
            {
                hashtable.Add("label", this.Label);
            }
            if (this.Aggregator != PivotAggregator.None)
            {
                hashtable.Add("aggregator", this.Aggregator.ToString().ToLower());
            }
            if (this.Width.HasValue)
            {
                hashtable.Add("width", this.Width);
            }
            if (this.GroupSummaryType != Trirand.Web.Mvc.GroupSummaryType.None)
            {
                hashtable.Add("summaryType", this.GroupSummaryType.ToString().ToLower());
            }
            if (this.Formatter != PivotFormatter.None)
            {
                hashtable.Add("formatter", this.Formatter.ToString().ToLower());
            }
            if (this.Align != TextAlign.Left)
            {
                hashtable.Add("align", this.Align.ToString().ToLower());
            }
            return hashtable;
        }

        public PivotAggregator Aggregator { get; set; }

        public TextAlign Align { get; set; }

        public PivotFormatter Formatter { get; set; }

        public Trirand.Web.Mvc.GroupSummaryType GroupSummaryType { get; set; }

        public string Label { get; set; }

        public string Member { get; set; }

        public int? Width { get; set; }
    }
}

