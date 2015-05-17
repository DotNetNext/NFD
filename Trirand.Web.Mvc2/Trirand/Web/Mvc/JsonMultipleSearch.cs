namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class JsonMultipleSearch
    {
        public string groupOp { get; set; }

        public List<MultipleSearchRule> rules { get; set; }
    }
}

