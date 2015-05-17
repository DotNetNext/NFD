namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomFormatter : JQGridColumnFormatter
    {
        public string FormatFunction { get; set; }

        public string SetAttributesFunction { get; set; }

        public string UnFormatFunction { get; set; }
    }
}

