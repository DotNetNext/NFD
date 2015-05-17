namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal class JsonResponse
    {
        public JsonResponse(int currentPage, int totalPagesCount, int totalRowCount, int pageSize, int actualRows, Hashtable userData)
        {
            this.page = currentPage;
            this.total = totalPagesCount;
            this.records = totalRowCount;
            this.rows = new JsonRow[actualRows];
            this.userdata = userData;
        }

        public int page { get; set; }

        public int records { get; set; }

        public JsonRow[] rows { get; set; }

        public int total { get; set; }

        public Hashtable userdata { get; set; }
    }
}

