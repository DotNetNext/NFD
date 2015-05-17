namespace Trirand.Web.Mvc
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class JQChartExportData
    {
        public JQChartExportData()
        {
            this.Width = 300;
            this.Type = "";
            this.SvgStream = null;
            this.FileName = "";
            this.ExportActive = false;
        }

        public bool ExportActive { get; set; }

        public string FileName { get; set; }

        public MemoryStream SvgStream { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }
    }
}

