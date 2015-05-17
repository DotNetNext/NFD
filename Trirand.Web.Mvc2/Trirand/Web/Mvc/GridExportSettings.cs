namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class GridExportSettings
    {
        public GridExportSettings()
        {
            this.ExportUrl = "";
            this.CSVSeparator = ",";
            this.ExportHeaders = true;
            this.ExportDataRange = Trirand.Web.Mvc.ExportDataRange.All;
        }

        public string CSVSeparator { get; set; }

        public Trirand.Web.Mvc.ExportDataRange ExportDataRange { get; set; }

        public bool ExportHeaders { get; set; }

        public string ExportUrl { get; set; }
    }
}

