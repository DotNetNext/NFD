namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class GroupField
    {
        public GroupField()
        {
            this.DataField = "";
            this.HeaderText = "<b>{0}</b>";
            this.ShowGroupColumn = true;
            this.GroupSortDirection = SortDirection.Asc;
            this.ShowGroupSummary = false;
        }

        public string DataField { get; set; }

        public SortDirection GroupSortDirection { get; set; }

        public string HeaderText { get; set; }

        public bool ShowGroupColumn { get; set; }

        public bool ShowGroupSummary { get; set; }
    }
}

