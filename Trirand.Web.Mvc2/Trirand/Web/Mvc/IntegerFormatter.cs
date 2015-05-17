namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class IntegerFormatter : JQGridColumnFormatter
    {
        public string DefaultValue { get; set; }

        public string ThousandsSeparator { get; set; }
    }
}

