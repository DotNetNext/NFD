namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal class JsonPivotResponse
    {
        public JsonPivotResponse(int actualRows)
        {
            this.rows = new Hashtable[actualRows];
        }

        public Hashtable[] rows { get; set; }
    }
}

