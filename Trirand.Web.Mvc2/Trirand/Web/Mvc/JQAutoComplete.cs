namespace Trirand.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Mvc;

    public class JQAutoComplete
    {
        public JQAutoComplete()
        {
            this.AutoCompleteMode = Trirand.Web.Mvc.AutoCompleteMode.BeginsWith;
            this.DataField = "";
            this.DataSource = null;
            this.DataUrl = "";
            this.DisplayMode = AutoCompleteDisplayMode.Standalone;
            this.ID = "";
            this.AutoFocus = false;
            this.Delay = 300;
            this.Enabled = true;
            this.MinLength = 1;
        }

        public JsonResult DataBind()
        {
            return this.GetJsonResponse();
        }

        public JsonResult DataBind(object dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }

        private JsonResult GetJsonResponse()
        {
            Guard.IsNotNull(this.DataSource, "DataSource");
            IQueryable dataSource = this.DataSource as IQueryable;
            Guard.IsNotNull(dataSource, "DataSource", "should implement the IQueryable interface.");
            Guard.IsNotNullOrEmpty(this.DataField, "DataField", "should be set to the datafield (column) of the datasource to search in.");
            SearchOperation isEqualTo = SearchOperation.IsEqualTo;
            if (this.AutoCompleteMode == Trirand.Web.Mvc.AutoCompleteMode.BeginsWith)
            {
                isEqualTo = SearchOperation.BeginsWith;
            }
            else
            {
                isEqualTo = SearchOperation.Contains;
            }
            string str = HttpContext.Current.Request.QueryString["term"];
            if (!string.IsNullOrEmpty(str))
            {
                Util.SearchArguments args = new Util.SearchArguments {
                    SearchColumn = this.DataField,
                    SearchOperation = isEqualTo,
                    SearchString = str
                };
                dataSource = dataSource.Where(Util.ConstructLinqFilterExpression(this, args), new object[0]);
            }
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = dataSource.ToListOfString(this) };
        }

        public Trirand.Web.Mvc.AutoCompleteMode AutoCompleteMode { get; set; }

        public bool AutoFocus { get; set; }

        public string DataField { get; set; }

        public object DataSource { get; set; }

        public string DataUrl { get; set; }

        public int Delay { get; set; }

        public AutoCompleteDisplayMode DisplayMode { get; set; }

        public bool Enabled { get; set; }

        public string ID { get; set; }

        public int MinLength { get; set; }
    }
}

