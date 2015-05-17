namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class CurrencyFormatter : JQGridColumnFormatter
    {
        public int DecimalPlaces { get; set; }

        public string DecimalSeparator { get; set; }

        public string DefaultValue { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string ThousandsSeparator { get; set; }
    }
}

