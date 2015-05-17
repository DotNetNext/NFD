namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class JQGridRenderer
    {
        private string GetAddOptions(JQGrid grid)
        {
            JsonAddDialog dialog = new JsonAddDialog(grid);
            return dialog.Process();
        }

        private string GetChildSubGridJavaScript(JQGrid grid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type='text/javascript'>\n");
            builder.AppendFormat("function showSubGrid_{0}(subgrid_id, row_id, message, suffix) {{", grid.ID);
            builder.Append("var subgrid_table_id, pager_id;\r\n\t\t                subgrid_table_id = subgrid_id+'_t';\r\n\t\t                pager_id = 'p_'+ subgrid_table_id;\r\n                        if (suffix) { subgrid_table_id += suffix; pager_id += suffix;  }\r\n                        if (message) jQuery('#'+subgrid_id).append(message);                        \r\n\t\t                jQuery('#'+subgrid_id).append('<table id=' + subgrid_table_id + ' class=scroll></table><div id=' + pager_id + ' class=scroll></div>');\r\n                ");
            builder.Append(this.GetStartupOptions(grid, true));
            builder.Append("}");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetColModel(JQGrid grid)
        {
            Hashtable[] hashtableArray = new Hashtable[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                JsonColModel model = new JsonColModel(grid.Columns[i], grid);
                hashtableArray[i] = model.JsonValues;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer {
                MaxJsonLength = 0x7fffffff
            };
            return JsonColModel.RemoveQuotesForJavaScriptMethods(serializer.Serialize(hashtableArray), grid);
        }

        private string GetColNames(JQGrid grid)
        {
            string[] strArray = new string[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                JQGridColumn column = grid.Columns[i];
                strArray[i] = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
            }
            return new JavaScriptSerializer().Serialize(strArray);
        }

        private string GetDelOptions(JQGrid grid)
        {
            JsonDelDialog dialog = new JsonDelDialog(grid);
            return dialog.Process();
        }

        private string GetEditOptions(JQGrid grid)
        {
            JsonEditDialog dialog = new JsonEditDialog(grid);
            return dialog.Process();
        }

        private string GetFirstVisibleDataField(JQGrid grid)
        {
            foreach (JQGridColumn column in grid.Columns)
            {
                if (column.Visible)
                {
                    return column.DataField;
                }
            }
            return grid.Columns[0].DataField;
        }

        private string GetJQuerySubmit(JQGrid grid)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("\r\n                        var _theForm = document.getElementsByTagName('FORM')[0];\r\n                        jQuery(_theForm).submit( function() \r\n                        {{  \r\n                            jQuery('#{0}').attr('value', jQuery('#{1}').getGridParam('selrow'));                            \r\n                        }});\r\n                       ", grid.ID + "_SelectedRow", grid.ID, grid.ID + "_CurrentPage");
            return builder.ToString();
        }

        private string GetLoadErrorHandler()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n");
            builder.Append("function jqGrid_aspnet_loadErrorHandler(xht, st, handler) {");
            builder.Append("jQuery(document.body).css('font-size','100%'); jQuery(document.body).html(xht.responseText);");
            builder.Append("}");
            return builder.ToString();
        }

        private string GetMultiKeyString(MultiSelectKey key)
        {
            switch (key)
            {
                case MultiSelectKey.Shift:
                    return "shiftKey";

                case MultiSelectKey.Ctrl:
                    return "ctrlKey";

                case MultiSelectKey.Alt:
                    return "altKey";
            }
            throw new Exception("Should not be here.");
        }

        private string GetSearchOptions(JQGrid grid)
        {
            JsonSearchDialog dialog = new JsonSearchDialog(grid);
            return dialog.Process();
        }

        private string GetStartupJavascript(JQGrid grid, bool subgrid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.Append(this.GetStartupOptions(grid, subgrid));
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions(JQGrid grid, bool subGrid)
        {
            string str12;
            StringBuilder sb = new StringBuilder();
            string str = subGrid ? "jQuery('#' + subgrid_table_id)" : string.Format("jQuery('#{0}')", grid.ID);
            string str2 = subGrid ? "jQuery('#' + pager_id)" : string.Format("jQuery('#{0}')", grid.ID + "_pager");
            string pagerSelectorID = subGrid ? "'#' + pager_id" : string.Format("'#{0}'", grid.ID + "_pager");
            string str4 = subGrid ? "&parentRowID=' + row_id + '" : string.Empty;
            string str5 = (grid.DataUrl.IndexOf("?") > 0) ? "&" : "?";
            string str6 = (grid.EditUrl.IndexOf("?") > 0) ? "&" : "?";
            string str7 = string.Format("{0}{1}jqGridID={2}{3}", new object[] { grid.DataUrl, str5, grid.ID, str4 });
            string str8 = string.Format("{0}{1}jqGridID={2}&editMode=1{3}", new object[] { grid.EditUrl, str6, grid.ID, str4 });
            if ((grid.Columns.Count > 0) && grid.Columns[0].Frozen)
            {
                grid.AppearanceSettings.ShrinkToFit = false;
            }
            string str9 = string.Format("{0}.jqGrid({{", str);
            if (grid.PivotSettings.IsPivotEnabled())
            {
                string str10 = grid.PivotSettings.ToJSON();
                str9 = string.Format("{0}.jqGrid('jqPivot','{1}',{2},{{", str, str7, str10);
            }
            sb.Append(str9);
            sb.AppendFormat("url: '{0}'", str7);
            sb.AppendFormat(",editurl: '{0}'", str8);
            sb.AppendFormat(",mtype: 'GET'", new object[0]);
            if (!string.IsNullOrEmpty(grid.IDPrefix))
            {
                sb.AppendFormat(",idPrefix: '{0}'", grid.IDPrefix);
            }
            if (!grid.PivotSettings.IsPivotEnabled())
            {
                sb.AppendFormat(",datatype: 'json'", new object[0]);
            }
            sb.AppendFormat(",page: {0}", grid.PagerSettings.CurrentPage);
            if (!grid.PivotSettings.IsPivotEnabled())
            {
                sb.AppendFormat(",colNames: {0}", this.GetColNames(grid));
                sb.AppendFormat(",colModel: {0}", this.GetColModel(grid));
            }
            sb.AppendFormat(",viewrecords: true", new object[0]);
            sb.AppendFormatIfTrue(grid.AutoEncode, ",autoencode: true", new object[0]);
            sb.AppendFormat(",scrollrows: {0}", grid.ScrollToSelectedRow.ToString().ToLower());
            if (grid.SortSettings.MultiColumnSorting)
            {
                sb.AppendFormat(",multiSort: true", new object[0]);
            }
            sb.AppendFormat(",prmNames: {{ id: \"{0}\" }}", Util.GetPrimaryKeyField(grid));
            if (grid.AppearanceSettings.ShowFooter)
            {
                sb.Append(",footerrow: true");
                sb.Append(",userDataOnFooter: true");
            }
            if (!grid.AppearanceSettings.ShrinkToFit)
            {
                sb.Append(",shrinkToFit: false");
            }
            sb.Append(",headertitles: true");
            if (grid.ColumnReordering)
            {
                sb.Append(",sortable: true");
            }
            if (grid.AppearanceSettings.ScrollBarOffset != 0x12)
            {
                sb.AppendFormat(",scrollOffset: {0}", grid.AppearanceSettings.ScrollBarOffset);
            }
            if (grid.AppearanceSettings.RightToLeft)
            {
                sb.Append(",direction: 'rtl'");
            }
            if (grid.AutoWidth)
            {
                sb.Append(",autowidth: true");
            }
            if (!grid.ShrinkToFit)
            {
                sb.Append(",shrinkToFit: false");
            }
            if ((grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Bottom) || (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom))
            {
                sb.AppendFormat(",pager: {0}", str2);
            }
            if ((grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Top) || (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom))
            {
                sb.Append(",toppager: true");
            }
            if (grid.RenderingMode == RenderingMode.Optimized)
            {
                if (grid.HierarchySettings.HierarchyMode != HierarchyMode.None)
                {
                    throw new Exception("Optimized rendering is not compatible with hierarchy.");
                }
                sb.Append(",gridview: true");
            }
            if ((grid.HierarchySettings.HierarchyMode == HierarchyMode.Parent) || (grid.HierarchySettings.HierarchyMode == HierarchyMode.ParentAndChild))
            {
                sb.Append(",subGrid: true");
                sb.AppendFormat(",subGridOptions: {0}", grid.HierarchySettings.ToJSON());
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.SubGridRowExpanded))
            {
                sb.AppendFormat(",subGridRowExpanded: {0}", grid.ClientSideEvents.SubGridRowExpanded);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ServerError))
            {
                sb.AppendFormat(",errorCell: {0}", grid.ClientSideEvents.ServerError);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowSelect))
            {
                sb.AppendFormat(",onSelectRow: {0}", grid.ClientSideEvents.RowSelect);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ColumnSort))
            {
                sb.AppendFormat(",onSortCol: {0}", grid.ClientSideEvents.ColumnSort);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowDoubleClick))
            {
                sb.AppendFormat(",ondblClickRow: {0}", grid.ClientSideEvents.RowDoubleClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowRightClick))
            {
                sb.AppendFormat(",onRightClickRow: {0}", grid.ClientSideEvents.RowRightClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.LoadDataError))
            {
                sb.AppendFormat(",loadError: {0}", grid.ClientSideEvents.LoadDataError);
            }
            else
            {
                sb.AppendFormat(",loadError: {0}", "jqGrid_aspnet_loadErrorHandler");
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.GridInitialized))
            {
                sb.AppendFormat(",gridComplete: {0}", grid.ClientSideEvents.GridInitialized);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.BeforeAjaxRequest))
            {
                sb.AppendFormat(",beforeRequest: {0}", grid.ClientSideEvents.BeforeAjaxRequest);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.AfterAjaxRequest))
            {
                sb.AppendFormat(",loadComplete: {0}", grid.ClientSideEvents.AfterAjaxRequest);
            }
            if (grid.TreeGridSettings.Enabled)
            {
                sb.AppendFormat(",treeGrid: true", new object[0]);
                sb.AppendFormat(",treedatatype: 'json'", new object[0]);
                sb.AppendFormat(",treeGridModel: 'adjacency'", new object[0]);
                string str11 = "{ level_field: 'tree_level', parent_id_field: 'tree_parent', leaf_field: 'tree_leaf', expanded_field: 'tree_expanded', loaded: 'tree_loaded', icon_field: 'tree_icon' }";
                sb.AppendFormat(",treeReader: {0}", str11);
                sb.AppendFormat(",ExpandColumn: '{0}'", this.GetFirstVisibleDataField(grid));
                Hashtable hashtable = new Hashtable();
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.CollapsedIcon))
                {
                    hashtable.Add("plus", grid.TreeGridSettings.CollapsedIcon);
                }
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.ExpandedIcon))
                {
                    hashtable.Add("minus", grid.TreeGridSettings.ExpandedIcon);
                }
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.LeafIcon))
                {
                    hashtable.Add("leaf", grid.TreeGridSettings.LeafIcon);
                }
                if (hashtable.Count > 0)
                {
                    sb.AppendFormat(",treeIcons: {0}", new JavaScriptSerializer().Serialize(hashtable));
                }
            }
            if (grid.LoadOnce)
            {
                sb.Append(",loadonce: true");
                sb.Append(",ignoreCase: true");
            }
            if (!grid.AppearanceSettings.HighlightRowsOnHover)
            {
                sb.Append(",hoverrows: false");
            }
            if (grid.AppearanceSettings.AlternateRowBackground)
            {
                sb.Append(",altRows: true");
            }
            if (grid.AppearanceSettings.ShowRowNumbers)
            {
                sb.Append(",rownumbers: true");
            }
            if (grid.AppearanceSettings.RowNumbersColumnWidth != 0x19)
            {
                sb.AppendFormat(",rownumWidth: {0}", grid.AppearanceSettings.RowNumbersColumnWidth.ToString());
            }
            if (grid.PagerSettings.ScrollBarPaging)
            {
                sb.AppendFormat(",scroll: 1", new object[0]);
            }
            sb.AppendFormat(",rowNum: {0}", grid.PagerSettings.PageSize.ToString());
            sb.AppendFormat(",rowList: {0}", grid.PagerSettings.PageSizeOptions.ToString());
            if (!string.IsNullOrEmpty(grid.PagerSettings.NoRowsMessage))
            {
                sb.AppendFormat(",emptyrecords: '{0}'", grid.PagerSettings.NoRowsMessage.ToString());
            }
            sb.AppendFormat(",editDialogOptions: {0}", this.GetEditOptions(grid));
            sb.AppendFormat(",addDialogOptions: {0}", this.GetAddOptions(grid));
            sb.AppendFormat(",delDialogOptions: {0}", this.GetDelOptions(grid));
            sb.AppendFormat(",searchDialogOptions: {0}", this.GetSearchOptions(grid));
            sb.AppendFormat(",viewRowDialogOptions: {0}", this.GetViewRowOptions(grid));
            if (grid.TreeGridSettings.Enabled)
            {
                str12 = ",jsonReader: {{ id: \"{0}\", repeatitems:false,subgrid:{{repeatitems:false}} }}";
            }
            else
            {
                str12 = ",jsonReader: {{ id: \"{0}\" }}";
            }
            if (grid.PivotSettings.IsPivotEnabled())
            {
                sb.Append(",jsonReader: { repeatitems:false,subgrid:{repeatitems:false} }");
            }
            else
            {
                sb.AppendFormat(str12, grid.Columns[Util.GetPrimaryKeyIndex(grid)].DataField);
            }
            if (!string.IsNullOrEmpty(grid.SortSettings.InitialSortColumn))
            {
                sb.AppendFormat(",sortname: '{0}'", grid.SortSettings.InitialSortColumn);
            }
            sb.AppendFormat(",sortorder: '{0}'", grid.SortSettings.InitialSortDirection.ToString().ToLower());
            if (grid.MultiSelect)
            {
                sb.Append(",multiselect: true");
                if (grid.MultiSelectMode == MultiSelectMode.SelectOnCheckBoxClickOnly)
                {
                    sb.AppendFormat(",multiboxonly: true", grid.MultiSelect.ToString().ToLower());
                }
                if (grid.MultiSelectKey != MultiSelectKey.None)
                {
                    sb.AppendFormat(",multikey: '{0}'", this.GetMultiKeyString(grid.MultiSelectKey));
                }
            }
            if (!string.IsNullOrEmpty(grid.AppearanceSettings.Caption))
            {
                sb.AppendFormat(",caption: '{0}'", grid.AppearanceSettings.Caption);
            }
            if (grid.AppearanceSettings.HiddenGrid)
            {
                sb.Append(",hiddengrid:true");
            }
            if (!grid.AppearanceSettings.ShowHideGridCaptionButton)
            {
                sb.Append(",hidegrid:false");
            }
            if (!grid.Width.IsEmpty)
            {
                sb.AppendFormat(",width: '{0}'", grid.Width.ToString().Replace("px", ""));
            }
            if (!grid.Height.IsEmpty)
            {
                sb.AppendFormat(",height: '{0}'", grid.Height.ToString().Replace("px", ""));
            }
            if (!string.IsNullOrEmpty(grid.PostData))
            {
                sb.AppendFormat(",postData: {0}", grid.PostData);
            }
            if (grid.GroupSettings.GroupFields.Count > 0)
            {
                sb.Append(grid.GroupSettings.ToJSON());
            }
            sb.AppendFormat(",viewsortcols: [{0},'{1}',{2}]", "false", grid.SortSettings.SortIconsPosition.ToString().ToLower(), (grid.SortSettings.SortAction == SortAction.ClickOnHeader) ? "true" : "false");
            sb.AppendFormat("}})\r", new object[0]);
            sb.Append(";");
            sb.Append(this.GetLoadErrorHandler());
            sb.Append(";");
            if (grid.PivotSettings.IsPivotEnabled())
            {
                sb.AppendFormat("{0}.bind('jqGridInitGrid.pivotGrid',(function(){{", str);
            }
            if (!grid.PagerSettings.ScrollBarPaging && grid.EnableKeyboardNavigation)
            {
                sb.AppendFormat("{0}.bindKeys();", str);
            }
            sb.Append(this.GetToolBarOptions(grid, subGrid, pagerSelectorID, str));
            if (grid.PivotSettings.IsPivotEnabled())
            {
                sb.Append("}));");
            }
            if (grid.HeaderGroups.Count > 0)
            {
                List<Hashtable> list = new List<Hashtable>();
                foreach (JQGridHeaderGroup group in grid.HeaderGroups)
                {
                    list.Add(group.ToHashtable());
                }
                sb.AppendFormat("{0}.setGroupHeaders( {{ useColSpanStyle:true,groupHeaders:{1} }});", str, new JavaScriptSerializer().Serialize(list));
            }
            if (grid.ToolBarSettings.ShowSearchToolBar)
            {
                sb.AppendFormat("{0}.filterToolbar({1});", str, new JsonSearchToolBar(grid).Process());
            }
            if ((grid.Columns.Count > 0) && grid.Columns[0].Frozen)
            {
                sb.AppendFormat("{0}.setFrozenColumns();", str);
            }
            return this.PreProcessJSON(grid, sb.ToString());
        }

        private string GetToolBarOptions(JQGrid grid, bool subGrid, string pagerSelectorID, string tableSelector)
        {
            StringBuilder builder = new StringBuilder();
            if (!grid.ShowToolBar)
            {
                return string.Empty;
            }
            JsonToolBar bar = new JsonToolBar(grid.ToolBarSettings);
            if (!subGrid)
            {
                builder.AppendFormat("{7}.navGrid('#{0}',{1},{2},{3},{4},{5},{6} );", new object[] { grid.ID + "_pager", new JavaScriptSerializer().Serialize(bar), string.Format("jQuery('#{0}').getGridParam('editDialogOptions')", grid.ID), string.Format("jQuery('#{0}').getGridParam('addDialogOptions')", grid.ID), string.Format("jQuery('#{0}').getGridParam('delDialogOptions')", grid.ID), string.Format("jQuery('#{0}').getGridParam('searchDialogOptions')", grid.ID), string.Format("jQuery('#{0}').getGridParam('viewRowDialogOptions')", grid.ID), tableSelector });
            }
            else
            {
                builder.AppendFormat("{6}.navGrid('#' + pager_id,{0},{1},{2},{3},{4} );", new object[] { new JavaScriptSerializer().Serialize(bar), "jQuery('#' + subgrid_table_id).getGridParam('editDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('addDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('delDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('searchDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('viewRowDialogOptions')", tableSelector });
            }
            foreach (JQGridToolBarButton button in grid.ToolBarSettings.CustomButtons)
            {
                if ((grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Bottom) || (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom))
                {
                    builder.AppendFormat("{2}.navButtonAdd({0},{1});", pagerSelectorID, new JsonCustomButton(button).Process(), tableSelector);
                }
                if ((grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom) || (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Top))
                {
                    builder.AppendFormat("{2}.navButtonAdd({0},{1});", pagerSelectorID.Replace("_pager", "_toppager"), new JsonCustomButton(button).Process(), tableSelector);
                }
            }
            return builder.ToString();
        }

        private string GetViewRowOptions(JQGrid grid)
        {
            JsonViewRowDialog dialog = new JsonViewRowDialog(grid);
            return dialog.Process();
        }

        private string PreProcessJSON(JQGrid grid, string gridJSON)
        {
            foreach (string str in grid.FunctionsHash.Keys)
            {
                string oldValue = string.Format("\"{0}\":\"{1}\"", str, grid.FunctionsHash[str]);
                string newValue = string.Format("\"{0}\":{1}", str, grid.FunctionsHash[str]);
                gridJSON = gridJSON.Replace(oldValue, newValue);
            }
            foreach (string str4 in grid.ReplacementsHash.Keys)
            {
                gridJSON = gridJSON.Replace(str4, grid.ReplacementsHash[str4].ToString());
            }
            return gridJSON;
        }

        public string RenderHtml(JQGrid grid)
        {
            string format = "<table id='{0}'></table>";
            if (grid.ToolBarSettings.ToolBarPosition != ToolBarPosition.Hidden)
            {
                format = format + "<div id='{0}_pager'></div>";
            }
            if (DateTime.Now > CompiledOn.CompilationDate.AddDays(45.0))
            {
                return "This is a trial version of jqGrid for ASP.NET MVC which has expired.<br> Please, contact sales@trirand.net for purchasing the product or for trial extension.";
            }
            if (string.IsNullOrEmpty(grid.ID))
            {
                throw new Exception("You need to set ID for this grid.");
            }
            format = string.Format(format, grid.ID);
            if ((grid.HierarchySettings.HierarchyMode == HierarchyMode.Child) || (grid.HierarchySettings.HierarchyMode == HierarchyMode.ParentAndChild))
            {
                return (format + this.GetChildSubGridJavaScript(grid));
            }
            return (format + this.GetStartupJavascript(grid, false));
        }
    }
}

