namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;

    public class JQGridEditData
    {
        public string ParentRowKey { get; set; }

        public NameValueCollection RowData { get; set; }

        public string RowKey { get; set; }
    }
}

