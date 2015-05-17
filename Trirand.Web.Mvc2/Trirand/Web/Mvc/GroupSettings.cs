namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.Script.Serialization;

    public sealed class GroupSettings
    {
        public GroupSettings()
        {
            this.CollapseGroups = false;
            this.GroupFields = new List<GroupField>();
        }

        private string GetDataFields()
        {
            List<string> list = new List<string>();
            foreach (GroupField field in this.GroupFields)
            {
                list.Add(field.DataField);
            }
            return new JavaScriptSerializer().Serialize(list);
        }

        private string GetGroupColumnShow()
        {
            List<bool> list = new List<bool>();
            foreach (GroupField field in this.GroupFields)
            {
                list.Add(field.ShowGroupColumn);
            }
            return new JavaScriptSerializer().Serialize(list);
        }

        private string GetGroupShowGroupSummary()
        {
            List<bool> list = new List<bool>();
            foreach (GroupField field in this.GroupFields)
            {
                list.Add(field.ShowGroupSummary);
            }
            return new JavaScriptSerializer().Serialize(list);
        }

        private string GetGroupSortDirection()
        {
            List<string> list = new List<string>();
            foreach (GroupField field in this.GroupFields)
            {
                list.Add(field.GroupSortDirection.ToString().ToLower());
            }
            return new JavaScriptSerializer().Serialize(list);
        }

        private string GetHeaderText()
        {
            List<string> list = new List<string>();
            foreach (GroupField field in this.GroupFields)
            {
                list.Add(field.HeaderText);
            }
            return new JavaScriptSerializer().Serialize(list);
        }

        internal string ToJSON()
        {
            StringBuilder builder = new StringBuilder();
            if (this.GroupFields.Count > 0)
            {
                builder.Append(",grouping:true");
                builder.Append(",groupingView: {");
                builder.AppendFormat("groupField: {0}", this.GetDataFields());
                builder.AppendFormat(",groupColumnShow: {0}", this.GetGroupColumnShow());
                builder.AppendFormat(",groupText: {0}", this.GetHeaderText());
                builder.AppendFormat(",groupOrder: {0}", this.GetGroupSortDirection());
                builder.AppendFormat(",groupSummary: {0}", this.GetGroupShowGroupSummary());
                builder.AppendFormat(",groupCollapse: {0}", this.CollapseGroups.ToString().ToLower());
                builder.AppendFormat(",groupDataSorted: true", new object[0]);
                builder.Append("}");
            }
            return builder.ToString();
        }

        public bool CollapseGroups { get; set; }

        public List<GroupField> GroupFields { get; set; }
    }
}

