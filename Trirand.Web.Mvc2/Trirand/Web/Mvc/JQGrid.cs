namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class JQGrid
    {
        private EventHandlerList _events;
        private static readonly object EventDataResolved = new object();

        public event JQGridDataResolvedEventHandler DataResolved
        {
            add
            {
                this.Events.AddHandler(EventDataResolved, value);
            }
            remove
            {
                this.Events.RemoveHandler(EventDataResolved, value);
            }
        }

        public JQGrid()
        {
            this.AutoEncode = false;
            this.AutoWidth = false;
            this.ShrinkToFit = true;
            this.LoadOnce = false;
            this.ScrollToSelectedRow = false;
            this.EnableKeyboardNavigation = true;
            this.EditDialogSettings = new Trirand.Web.Mvc.EditDialogSettings();
            this.AddDialogSettings = new Trirand.Web.Mvc.AddDialogSettings();
            this.DeleteDialogSettings = new Trirand.Web.Mvc.DeleteDialogSettings();
            this.SearchDialogSettings = new Trirand.Web.Mvc.SearchDialogSettings();
            this.SearchToolBarSettings = new Trirand.Web.Mvc.SearchToolBarSettings();
            this.ViewRowDialogSettings = new Trirand.Web.Mvc.ViewRowDialogSettings();
            this.PagerSettings = new Trirand.Web.Mvc.PagerSettings();
            this.ToolBarSettings = new Trirand.Web.Mvc.ToolBarSettings();
            this.SortSettings = new Trirand.Web.Mvc.SortSettings();
            this.AppearanceSettings = new Trirand.Web.Mvc.AppearanceSettings();
            this.HierarchySettings = new Trirand.Web.Mvc.HierarchySettings();
            this.GroupSettings = new Trirand.Web.Mvc.GroupSettings();
            this.TreeGridSettings = new Trirand.Web.Mvc.TreeGridSettings();
            this.ExportSettings = new GridExportSettings();
            this.ClientSideEvents = new Trirand.Web.Mvc.ClientSideEvents();
            this.PivotSettings = new Trirand.Web.Mvc.PivotSettings();
            this.Columns = new List<JQGridColumn>();
            this.HeaderGroups = new List<JQGridHeaderGroup>();
            this.DataUrl = "";
            this.EditUrl = "";
            this.ColumnReordering = false;
            this.RenderingMode = Trirand.Web.Mvc.RenderingMode.Default;
            this.MultiSelect = false;
            this.MultiSelectMode = Trirand.Web.Mvc.MultiSelectMode.SelectOnRowClick;
            this.MultiSelectKey = Trirand.Web.Mvc.MultiSelectKey.None;
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
            this.ID = "";
            this.IDPrefix = "";
            this.PostData = "";
            this.FunctionsHash = new Hashtable();
            this.ReplacementsHash = new Hashtable();
        }

        private DataTable ConvertDataGridToDataTable(DataGrid grid)
        {
            DataTable table = new DataTable();
            foreach (DataGridColumn column in grid.Columns)
            {
                table.Columns.Add(column.HeaderText);
            }
            foreach (DataGridItem item in grid.Items)
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    row[i] = item.Cells[i].Text;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public JsonResult DataBind()
        {
            if (this.AjaxCallBackMode == Trirand.Web.Mvc.AjaxCallBackMode.RequestData)
            {
            }
            return this.GetJsonResponse();
        }

        public JsonResult DataBind(object dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }

        public void ExportToCSV(object dataSource, string fileName)
        {
            DataGrid exportGrid = this.GetExportGrid();
            exportGrid.DataSource = dataSource;
            exportGrid.DataBind();
            this.RenderCSVToStream(exportGrid, fileName);
        }

        public void ExportToCSV(object dataSource, string fileName, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            this.ExportToCSV(filteredDataSource, fileName);
        }

        public void ExportToExcel(object dataSource, string fileName)
        {
            DataGrid exportGrid = this.GetExportGrid();
            IQueryable en = dataSource as IQueryable;
            if (en != null)
            {
                exportGrid.DataSource = en.ToDataTable(this);
            }
            else
            {
                exportGrid.DataSource = dataSource;
            }
            exportGrid.DataBind();
            this.RenderExcelToStream(exportGrid, fileName);
        }
        public void ExportToExcel(object dataSource, string fileName,Func<DataTable,DataTable> addTolRow)
        {
            DataGrid exportGrid = this.GetExportGrid();
            IQueryable en = dataSource as IQueryable;
            if (en != null)
            {
                var dt= en.ToDataTable(this);;
                exportGrid.DataSource = addTolRow(dt);
            }
            else
            {
                exportGrid.DataSource = dataSource;
            }
            exportGrid.DataBind();
            this.RenderExcelToStream(exportGrid, fileName);
        }


        public void ExportToExcel(object dataSource, string fileName, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            this.ExportToExcel(filteredDataSource, fileName);
        }

        private JsonResult FilterDataSource(object dataSource, NameValueCollection queryString, out IQueryable iqueryable)
        {
            iqueryable = dataSource as IQueryable;
            Guard.IsNotNull(iqueryable, "DataSource", "should implement the IQueryable interface.");
            int pageIndex = this.GetPageIndex(queryString["page"]);
            int count = Convert.ToInt32(queryString["rows"]);
            string primaryKeyField = queryString["sidx"];
            string sortDirection = queryString["sord"];
            string text1 = queryString["parentRowID"];
            string str3 = queryString["_search"];
            string str4 = queryString["filters"];
            string str5 = queryString["searchField"];
            string searchString = queryString["searchString"];
            string searchOper = queryString["searchOper"];
            this.PagerSettings.CurrentPage = pageIndex;
            if (count > 0)
            {
                this.PagerSettings.PageSize = count;
            }
            if ((!string.IsNullOrEmpty(str3) && (str3 != "false")) || !string.IsNullOrEmpty(str4))
            {
                try
                {
                    if (string.IsNullOrEmpty(str4) && !string.IsNullOrEmpty(str5))
                    {
                        iqueryable = iqueryable.Where(Trirand.Web.Mvc.Util.GetWhereClause(this, str5, searchString, searchOper), new object[0]);
                    }
                    else if (!string.IsNullOrEmpty(str4))
                    {
                        iqueryable = iqueryable.Where(Trirand.Web.Mvc.Util.GetWhereClause(this, str4), new object[0]);
                    }
                    else if (this.ToolBarSettings.ShowSearchToolBar || (str3 == "true"))
                    {
                        iqueryable = iqueryable.Where(Trirand.Web.Mvc.Util.GetWhereClause(this, queryString), new object[0]);
                    }
                }
                catch (DataTypeNotSetException exception)
                {
                    throw exception;
                }
                catch (Exception)
                {
                    return new JsonResult { Data = new object(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            int totalRowCount = iqueryable.Count();
            if (this.PivotSettings.IsPivotEnabled())
            {
                count = totalRowCount;
            }
            int totalPagesCount = (int) Math.Ceiling((double) (((float) totalRowCount) / ((float) count)));
            if (string.IsNullOrEmpty(primaryKeyField) && this.SortSettings.AutoSortByPrimaryKey)
            {
                if (this.Columns.Count == 0)
                {
                    throw new Exception("JQGrid must have at least one column defined.");
                }
                primaryKeyField = Trirand.Web.Mvc.Util.GetPrimaryKeyField(this);
                sortDirection = "asc";
            }
            if (!string.IsNullOrEmpty(primaryKeyField))
            {
                iqueryable = iqueryable.OrderBy(this.GetSortExpression(primaryKeyField, sortDirection), new object[0]);
            }
            if (!this.LoadOnce && !this.PivotSettings.IsPivotEnabled())
            {
                iqueryable = iqueryable.Skip(((pageIndex - 1) * count)).Take(count);
            }
            DataTable dt = iqueryable.ToDataTable(this);
            this.OnDataResolved(new JQGridDataResolvedEventArgs(this, iqueryable, this.DataSource as IQueryable));
            if (this.TreeGridSettings.Enabled)
            {
                JsonTreeResponse response = new JsonTreeResponse(pageIndex, totalPagesCount, totalRowCount, count, dt.Rows.Count, Trirand.Web.Mvc.Util.GetFooterInfo(this));
                return Trirand.Web.Mvc.Util.ConvertToTreeJson(response, this, dt);
            }
            if (this.PivotSettings.IsPivotEnabled())
            {
                return Trirand.Web.Mvc.Util.ConvertToPivotJson(new JsonPivotResponse(dt.Rows.Count), this, dt);
            }
            JsonResponse response2 = new JsonResponse(pageIndex, totalPagesCount, totalRowCount, count, dt.Rows.Count, Trirand.Web.Mvc.Util.GetFooterInfo(this));
            return Trirand.Web.Mvc.Util.ConvertToJson(response2, this, dt);
        }

        public JQGridEditData GetEditData()
        {
            NameValueCollection values = new NameValueCollection();
            foreach (string str in HttpContext.Current.Request.Form.Keys)
            {
                if (str != "oper")
                {
                    values[str] = HttpContext.Current.Request.Form[str];
                }
            }
            string dataField = string.Empty;
            foreach (JQGridColumn column in this.Columns)
            {
                if (column.PrimaryKey)
                {
                    dataField = column.DataField;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(dataField) && !string.IsNullOrEmpty(values["id"]))
            {
                values[dataField] = values["id"];
            }
            JQGridEditData data = new JQGridEditData {
                RowData = values,
                RowKey = values["id"]
            };
            string str3 = HttpContext.Current.Request.QueryString["parentRowID"];
            if (!string.IsNullOrEmpty(str3))
            {
                data.ParentRowKey = str3;
            }
            return data;
        }

        public DataTable GetExportData(object dataSource)
        {
            DataGrid exportGrid = this.GetExportGrid();
            exportGrid.DataSource = dataSource;
            exportGrid.DataBind();
            return this.ConvertDataGridToDataTable(exportGrid);
        }

        public DataTable GetExportData(object dataSource, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            return this.GetExportData(filteredDataSource);
        }

        private DataGrid GetExportGrid()
        {
            DataGrid grid = new DataGrid {
                AutoGenerateColumns = false,
                ID = this.ID + "_exportGrid"
            };
            foreach (JQGridColumn column in this.Columns)
            {
                if (column.Visible)
                {
                    BoundColumn column2 = new BoundColumn {
                        DataField = column.DataField
                    };
                    string str = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
                    column2.HeaderText = str;
                    column2.DataFormatString = column.DataFormatString;
                    column2.FooterText = column.FooterValue;
                    grid.Columns.Add(column2);
                }
            }
            return grid;
        }

        private IQueryable GetFilteredDataSource(object dataSource, JQGridState gridState)
        {
            IQueryable queryable;
            if (this.ExportSettings.ExportDataRange != ExportDataRange.FilteredAndPaged)
            {
                gridState.QueryString["page"] = "1";
                gridState.QueryString["rows"] = "1000000";
            }
            this.FilterDataSource(dataSource, gridState.QueryString, out queryable);
            return queryable;
        }

        private JsonResult GetJsonResponse()
        {
            IQueryable queryable;
            Guard.IsNotNull(this.DataSource, "DataSource");
            return this.FilterDataSource(this.DataSource, HttpContext.Current.Request.QueryString, out queryable);
        }

        private int GetPageIndex(string value)
        {
            int num = 1;
            try
            {
                num = Convert.ToInt32(value);
            }
            catch (Exception)
            {
            }
            return num;
        }

        private string GetSortExpression(string sortExpression, string sortDirection)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in sortExpression.Split(new char[] { ',' }).ToList<string>())
            {
                string str2 = sortDirection;
                if (str.Trim().Length == 0)
                {
                    break;
                }
                List<string> list2 = str.Trim().Split(new char[] { ' ' }).ToList<string>();
                string str3 = list2[0];
                if (list2.Count > 1)
                {
                    str2 = list2[1];
                }
                if (list2.Count > 1)
                {
                    string local1 = list2[1];
                }
                if (builder.Length > 0)
                {
                    builder.Append(",");
                }
                builder.AppendFormat("{0} {1}", str3, str2);
            }
            return builder.ToString();
        }

        public JQGridState GetState()
        {
            NameValueCollection values = new NameValueCollection();
            foreach (string str in HttpContext.Current.Request.QueryString.Keys)
            {
                values.Add(str, HttpContext.Current.Request.QueryString[str]);
            }
            return new JQGridState { QueryString = values };
        }

        public JQGridTreeExpandData GetTreeExpandData()
        {
            JQGridTreeExpandData data = new JQGridTreeExpandData();
            if (HttpContext.Current.Request["nodeid"] != null)
            {
                data.ParentID = HttpContext.Current.Request["nodeid"];
            }
            if (HttpContext.Current.Request["n_level"] != null)
            {
                data.ParentLevel = Convert.ToInt32(HttpContext.Current.Request["n_level"]);
            }
            return data;
        }

        protected internal virtual void OnDataResolved(JQGridDataResolvedEventArgs e)
        {
            JQGridDataResolvedEventHandler handler = (JQGridDataResolvedEventHandler) this.Events[EventDataResolved];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private string QuoteText(string input)
        {
            return string.Format("\"{0}\"", input.Replace("\"", "\"\"").Replace("&nbsp;", ""));
        }

        private void RenderCSVToStream(DataGrid grid, string fileName)
        {
            StringBuilder builder = new StringBuilder();
            if (this.ExportSettings.ExportHeaders)
            {
                foreach (BoundColumn column in grid.Columns)
                {
                    builder.AppendFormat("{0}{1}", this.QuoteText(column.HeaderText), this.ExportSettings.CSVSeparator);
                }
            }
            builder.Append("\n");
            foreach (DataGridItem item in grid.Items)
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.Columns[i].Visible)
                    {
                        builder.AppendFormat("{0}{1}", this.QuoteText(item.Cells[i].Text), this.ExportSettings.CSVSeparator);
                    }
                }
                builder.Append("\n");
            }
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.ContentType = "application/excel";
            response.ContentEncoding = Encoding.Default;
            response.Clear();
            response.Write(builder.ToString());
            response.Flush();
            response.SuppressContent = true;
        }

        private void RenderExcelToStream(DataGrid grid, string fileName)
        {
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            grid.RenderControl(writer2);
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.ContentType = "application/ms-excel";
            response.ContentEncoding = Encoding.Unicode;
            response.BinaryWrite(Encoding.Unicode.GetPreamble());
            response.Write(writer.ToString());
            response.Flush();
            response.SuppressContent = true;
        }

        public ActionResult ShowEditValidationMessage(string errorMessage)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.StatusCode = 500;
            return new ContentResult { Content = errorMessage };
        }

        public Trirand.Web.Mvc.AddDialogSettings AddDialogSettings { get; set; }

        public Trirand.Web.Mvc.AjaxCallBackMode AjaxCallBackMode
        {
            get
            {
                string str4;
                string str = HttpContext.Current.Request.Form["oper"];
                string str2 = HttpContext.Current.Request.QueryString["editMode"];
                string str3 = HttpContext.Current.Request.QueryString["_search"];
                Trirand.Web.Mvc.AjaxCallBackMode requestData = Trirand.Web.Mvc.AjaxCallBackMode.RequestData;
                if (!string.IsNullOrEmpty(str) && ((str4 = str) != null))
                {
                    if (str4 == "add")
                    {
                        return Trirand.Web.Mvc.AjaxCallBackMode.AddRow;
                    }
                    if (str4 == "edit")
                    {
                        return Trirand.Web.Mvc.AjaxCallBackMode.EditRow;
                    }
                    if (str4 == "del")
                    {
                        return Trirand.Web.Mvc.AjaxCallBackMode.DeleteRow;
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    requestData = Trirand.Web.Mvc.AjaxCallBackMode.EditRow;
                }
                if (!string.IsNullOrEmpty(str3) && Convert.ToBoolean(str3))
                {
                    requestData = Trirand.Web.Mvc.AjaxCallBackMode.Search;
                }
                return requestData;
            }
        }

        public Trirand.Web.Mvc.AppearanceSettings AppearanceSettings { get; set; }

        public bool AutoEncode { get; set; }

        public bool AutoWidth { get; set; }

        public Trirand.Web.Mvc.ClientSideEvents ClientSideEvents { get; set; }

        public bool ColumnReordering { get; set; }

        public List<JQGridColumn> Columns { get; set; }

        public object DataSource { get; set; }

        public string DataUrl { get; set; }

        public Trirand.Web.Mvc.DeleteDialogSettings DeleteDialogSettings { get; set; }

        public Trirand.Web.Mvc.EditDialogSettings EditDialogSettings { get; set; }

        public string EditUrl { get; set; }

        public bool EnableKeyboardNavigation { get; set; }

        private EventHandlerList Events
        {
            get
            {
                if (this._events == null)
                {
                    this._events = new EventHandlerList();
                }
                return this._events;
            }
        }

        public GridExportSettings ExportSettings { get; set; }

        internal Hashtable FunctionsHash { get; set; }

        public Trirand.Web.Mvc.GroupSettings GroupSettings { get; set; }

        public List<JQGridHeaderGroup> HeaderGroups { get; set; }

        public Unit Height { get; set; }

        public Trirand.Web.Mvc.HierarchySettings HierarchySettings { get; set; }

        public string ID { get; set; }

        public string IDPrefix { get; set; }

        public bool LoadOnce { get; set; }

        public bool MultiSelect { get; set; }

        public Trirand.Web.Mvc.MultiSelectKey MultiSelectKey { get; set; }

        public Trirand.Web.Mvc.MultiSelectMode MultiSelectMode { get; set; }

        public Trirand.Web.Mvc.PagerSettings PagerSettings { get; set; }

        public Trirand.Web.Mvc.PivotSettings PivotSettings { get; set; }

        public string PostData { get; set; }

        public Trirand.Web.Mvc.RenderingMode RenderingMode { get; set; }

        internal Hashtable ReplacementsHash { get; set; }

        public bool ScrollToSelectedRow { get; set; }

        public Trirand.Web.Mvc.SearchDialogSettings SearchDialogSettings { get; set; }

        public Trirand.Web.Mvc.SearchToolBarSettings SearchToolBarSettings { get; set; }

        internal bool ShowToolBar
        {
            get
            {
                if (((!this.ToolBarSettings.ShowAddButton && !this.ToolBarSettings.ShowDeleteButton) && (!this.ToolBarSettings.ShowEditButton && !this.ToolBarSettings.ShowRefreshButton)) && (!this.ToolBarSettings.ShowSearchButton && !this.ToolBarSettings.ShowViewRowDetailsButton))
                {
                    return (this.ToolBarSettings.CustomButtons.Count > 0);
                }
                return true;
            }
        }

        public bool ShrinkToFit { get; set; }

        public Trirand.Web.Mvc.SortSettings SortSettings { get; set; }

        public Trirand.Web.Mvc.ToolBarSettings ToolBarSettings { get; set; }

        public Trirand.Web.Mvc.TreeGridSettings TreeGridSettings { get; set; }

        public Trirand.Web.Mvc.ViewRowDialogSettings ViewRowDialogSettings { get; set; }

        public Unit Width { get; set; }
    }
}

