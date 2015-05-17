namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class JQGridTreeExpandData
    {
        public JQGridTreeExpandData()
        {
            this.ParentLevel = -1;
            this.ParentID = null;
        }

        public string ParentID { get; set; }

        public int ParentLevel { get; set; }
    }
}

