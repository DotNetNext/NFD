namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class SortSettings
    {
        public SortSettings()
        {
            this.AutoSortByPrimaryKey = true;
            this.InitialSortColumn = "";
            this.InitialSortDirection = SortDirection.Asc;
            this.SortIconsPosition = Trirand.Web.Mvc.SortIconsPosition.Vertical;
            this.SortAction = Trirand.Web.Mvc.SortAction.ClickOnHeader;
            this.MultiColumnSorting = false;
        }

        public bool AutoSortByPrimaryKey { get; set; }

        public string InitialSortColumn { get; set; }

        public SortDirection InitialSortDirection { get; set; }

        public bool MultiColumnSorting { get; set; }

        public Trirand.Web.Mvc.SortAction SortAction { get; set; }

        public Trirand.Web.Mvc.SortIconsPosition SortIconsPosition { get; set; }
    }
}

