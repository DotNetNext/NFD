namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    internal static class Util
    {
        internal static string ConstructLinqFilterExpression(JQAutoComplete autoComplete, SearchArguments args)
        {
            Guard.IsNotNull(autoComplete.DataField, "DataField", "must be set in order to perform search operations. If you get this error from search/export method, make sure you setup(initialize) the grid again prior to filtering/exporting.");
            string filterExpressionCompare = "{0} {1} \"{2}\"";
            return GetLinqExpression(filterExpressionCompare, args, false, typeof(string));
        }

        private static string ConstructLinqFilterExpression(JQGrid grid, SearchArguments args)
        {
            JQGridColumn column = grid.Columns.Find(c => c.DataField == args.SearchColumn);
            if (column.DataType == null)
            {
                throw new DataTypeNotSetException("JQGridColumn.DataType must be set in order to perform search operations.");
            }
            string filterExpressionCompare = (column.DataType == typeof(string)) ? "{0} {1} \"{2}\"" : "{0} {1} {2}";
            if (column.DataType == typeof(DateTime))
            {
                DateTime time = DateTime.Parse(args.SearchString);
                string str2 = string.Format("({0},{1},{2})", time.Year, time.Month, time.Day);
                filterExpressionCompare = "{0} {1} DateTime" + str2;
            }
            return (string.Format("{0} != null AND ", args.SearchColumn) + GetLinqExpression(filterExpressionCompare, args, column.SearchCaseSensitive, column.DataType));
        }

        internal static JsonResult ConvertToJson(JsonResponse response, JQGrid grid, DataTable dt)
        {
            JsonResult result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            if (response.records == 0)
            {
                if (grid.AppearanceSettings.ShowFooter)
                {
                    result.Data = PrepareJsonResponse(response, grid, dt);
                    return result;
                }
                result.Data = new object[0];
                return result;
            }
            result.Data = PrepareJsonResponse(response, grid, dt);
            return result;
        }

        internal static JsonResult ConvertToPivotJson(JQGrid grid, DataTable dt)
        {
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        internal static JsonResult ConvertToPivotJson(JsonPivotResponse response, JQGrid grid, DataTable dt)
        {
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = PrepareJsonPivotResponse(response, grid, dt) };
        }

        internal static JsonResult ConvertToTreeJson(JsonTreeResponse response, JQGrid grid, DataTable dt)
        {
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = PrepareJsonTreeResponse(response, grid, dt) };
        }

        public static Hashtable GetFooterInfo(JQGrid grid)
        {
            Hashtable hashtable = new Hashtable();
            if (grid.AppearanceSettings.ShowFooter)
            {
                foreach (JQGridColumn column in grid.Columns)
                {
                    hashtable[column.DataField] = column.FooterValue;
                }
            }
            return hashtable;
        }

        private static string GetLinqExpression(string filterExpressionCompare, SearchArguments args, bool caseSensitive, Type dataType)
        {
            string str = caseSensitive ? args.SearchString : args.SearchString.ToLower();
            string searchColumn = args.SearchColumn;
            if (((dataType != null) && (dataType == typeof(string))) && !caseSensitive)
            {
                searchColumn = string.Format("{0}.ToLower()", args.SearchColumn);
            }
            switch (args.SearchOperation)
            {
                case SearchOperation.IsEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "=", str);

                case SearchOperation.IsNotEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "<>", str);

                case SearchOperation.IsLessThan:
                    return string.Format(filterExpressionCompare, searchColumn, "<", str);

                case SearchOperation.IsLessOrEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "<=", str);

                case SearchOperation.IsGreaterThan:
                    return string.Format(filterExpressionCompare, searchColumn, ">", str);

                case SearchOperation.IsGreaterOrEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, ">=", str);

                case SearchOperation.BeginsWith:
                    return string.Format("{0}.StartsWith(\"{1}\")", searchColumn, str);

                case SearchOperation.DoesNotBeginWith:
                    return string.Format("!{0}.StartsWith(\"{1}\")", searchColumn, str);

                case SearchOperation.EndsWith:
                    return string.Format("{0}.EndsWith(\"{1}\")", searchColumn, str);

                case SearchOperation.DoesNotEndWith:
                    return string.Format("!{0}.EndsWith(\"{1}\")", searchColumn, str);

                case SearchOperation.Contains:
                    return string.Format("{0}.Contains(\"{1}\")", searchColumn, str);

                case SearchOperation.DoesNotContain:
                    return string.Format("!{0}.Contains(\"{1}\")", searchColumn, str);
            }
            throw new Exception("Invalid search operation.");
        }

        public static string GetPrimaryKeyField(JQGrid grid)
        {
            int primaryKeyIndex = GetPrimaryKeyIndex(grid);
            return grid.Columns[primaryKeyIndex].DataField;
        }

        public static int GetPrimaryKeyIndex(JQGrid grid)
        {
            foreach (JQGridColumn column in grid.Columns)
            {
                if (column.PrimaryKey)
                {
                    return grid.Columns.IndexOf(column);
                }
            }
            return 0;
        }

        private static SearchOperation GetSearchOperationFromString(string searchOperation)
        {
            switch (searchOperation)
            {
                case "eq":
                    return SearchOperation.IsEqualTo;

                case "ne":
                    return SearchOperation.IsNotEqualTo;

                case "lt":
                    return SearchOperation.IsLessThan;

                case "le":
                    return SearchOperation.IsLessOrEqualTo;

                case "gt":
                    return SearchOperation.IsGreaterThan;

                case "ge":
                    return SearchOperation.IsGreaterOrEqualTo;

                case "in":
                    return SearchOperation.IsIn;

                case "ni":
                    return SearchOperation.IsNotIn;

                case "bw":
                    return SearchOperation.BeginsWith;

                case "bn":
                    return SearchOperation.DoesNotBeginWith;

                case "ew":
                    return SearchOperation.EndsWith;

                case "en":
                    return SearchOperation.DoesNotEndWith;

                case "cn":
                    return SearchOperation.Contains;

                case "nc":
                    return SearchOperation.DoesNotContain;
            }
            throw new Exception("Search operation not known: " + searchOperation);
        }

        public static string GetWhereClause(JQGrid grid, NameValueCollection queryString)
        {
            string str = " && ";
            string str2 = "";
            new Hashtable();
            foreach (JQGridColumn column in grid.Columns)
            {
                string str3 = queryString[column.DataField];
                if (!string.IsNullOrEmpty(str3))
                {
                    SearchArguments args = new SearchArguments
                    {
                        SearchColumn = column.DataField,
                        SearchString = str3,
                        SearchOperation = column.SearchToolBarOperation
                    };
                    string str4 = (str2.Length > 0) ? str : "";
                    string str5 = ConstructLinqFilterExpression(grid, args);
                    str2 = str2 + str4 + str5;
                }
            }
            return str2;
        }

        public static string GetWhereClause(JQGrid grid, string filters)
        {
            JsonMultipleSearch search = new JavaScriptSerializer().Deserialize<JsonMultipleSearch>(filters);
            string str = "";
            foreach (MultipleSearchRule rule in search.rules)
            {
                SearchArguments args = new SearchArguments
                {
                    SearchColumn = rule.field,
                    SearchString = rule.data,
                    SearchOperation = GetSearchOperationFromString(rule.op)
                };
                string str2 = (str.Length > 0) ? (" " + search.groupOp + " ") : "";
                str = str + str2 + ConstructLinqFilterExpression(grid, args);
            }
            return str;
        }

        public static string GetWhereClause(JQGrid grid, string searchField, string searchString, string searchOper)
        {
            string str = " && ";
            string str2 = "";
            new Hashtable();
            SearchArguments args = new SearchArguments
            {
                SearchColumn = searchField,
                SearchString = searchString,
                SearchOperation = GetSearchOperationFromString(searchOper)
            };
            string str3 = (str2.Length > 0) ? str : "";
            string str4 = ConstructLinqFilterExpression(grid, args);
            return (str2 + str3 + str4);
        }

        internal static JsonPivotResponse PrepareJsonPivotResponse(JsonPivotResponse response, JQGrid grid, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hashtable hashtable = new Hashtable();
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    JQGridColumn column = grid.Columns[j];
                    string str = "";
                    if (!string.IsNullOrEmpty(column.DataField))
                    {
                        int ordinal = dt.Columns[column.DataField].Ordinal;
                        str = string.IsNullOrEmpty(column.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : column.FormatDataValue(dt.Rows[i].ItemArray[ordinal], column.HtmlEncode);
                    }
                    hashtable.Add(column.DataField, str);
                }
                response.rows[i] = hashtable;
            }
            return response;
        }

        internal static JsonResponse PrepareJsonResponse(JsonResponse response, JQGrid grid, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] strArray = new string[grid.Columns.Count];
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    JQGridColumn column = grid.Columns[j];
                    string str = "";
                    if (!string.IsNullOrEmpty(column.DataField))
                    {
                        int ordinal = 0;
                        try
                        {
                            ordinal = dt.Columns[column.DataField].Ordinal;
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(column.DataField + "不存在于dataTable中" + "\r" + ex.Message);
                        }
                        str = string.IsNullOrEmpty(column.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : column.FormatDataValue(dt.Rows[i].ItemArray[ordinal], column.HtmlEncode);
                    }
                    strArray[j] = str;
                }
                string str2 = strArray[GetPrimaryKeyIndex(grid)];
                JsonRow row = new JsonRow
                {
                    id = str2,
                    cell = strArray
                };
                response.rows[i] = row;
            }
            return response;
        }

        internal static JsonTreeResponse PrepareJsonTreeResponse(JsonTreeResponse response, JQGrid grid, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                response.rows[i] = new Hashtable();
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    JQGridColumn column = grid.Columns[j];
                    string str = "";
                    if (!string.IsNullOrEmpty(column.DataField))
                    {
                        int ordinal = dt.Columns[column.DataField].Ordinal;
                        str = string.IsNullOrEmpty(column.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : column.FormatDataValue(dt.Rows[i].ItemArray[ordinal], column.HtmlEncode);
                    }
                    response.rows[i].Add(column.DataField, str);
                }
                try
                {
                    response.rows[i].Add("tree_level", dt.Rows[i]["tree_level"] as int?);
                }
                catch
                {
                }
                try
                {
                    string str2 = "";
                    object obj2 = dt.Rows[i]["tree_parent"];
                    if (obj2 is DBNull)
                    {
                        str2 = "null";
                    }
                    else
                    {
                        str2 = Convert.ToString(dt.Rows[i]["tree_parent"]);
                    }
                    response.rows[i].Add("tree_parent", str2);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_leaf", dt.Rows[i]["tree_leaf"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_expanded", dt.Rows[i]["tree_expanded"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_loaded", dt.Rows[i]["tree_loaded"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_icon", dt.Rows[i]["tree_icon"] as string);
                }
                catch
                {
                }
            }
            return response;
        }

        internal class SearchArguments
        {
            public string SearchColumn { get; set; }

            public Trirand.Web.Mvc.SearchOperation SearchOperation { get; set; }

            public string SearchString { get; set; }
        }
    }
}

