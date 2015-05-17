namespace Trirand.Web.Mvc
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;

    public class TrirandNamespace
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool Equals(object value)
        {
            return base.Equals(value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private Type GetType()
        {
            return base.GetType();
        }

        public MvcHtmlString JQAutoComplete(Trirand.Web.Mvc.JQAutoComplete autoComplete, string id)
        {
            JQAutoCompleteRenderer renderer = new JQAutoCompleteRenderer(autoComplete);
            autoComplete.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQChart(Trirand.Web.Mvc.JQChart chart, string id)
        {
            JQChartRenderer renderer = new JQChartRenderer(chart);
            chart.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQComboBox(Trirand.Web.Mvc.JQComboBox comboBox, string id)
        {
            JQComboBoxRenderer renderer = new JQComboBoxRenderer(comboBox);
            comboBox.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQDatePicker(Trirand.Web.Mvc.JQDatePicker datePicker, string id)
        {
            JQDatePickerRenderer renderer = new JQDatePickerRenderer(datePicker);
            datePicker.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQDropDownList(Trirand.Web.Mvc.JQDropDownList dropDownList, string id)
        {
            JQDropDownListRenderer renderer = new JQDropDownListRenderer(dropDownList);
            dropDownList.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQGrid(Trirand.Web.Mvc.JQGrid grid, string id)
        {
            JQGridRenderer renderer = new JQGridRenderer();
            grid.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml(grid));
        }

        public MvcHtmlString JQMultiSelect(Trirand.Web.Mvc.JQMultiSelect multiSelect, string id)
        {
            JQMultiSelectRenderer renderer = new JQMultiSelectRenderer(multiSelect);
            multiSelect.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString JQTreeView(Trirand.Web.Mvc.JQTreeView tree, string id)
        {
            JQTreeViewRenderer renderer = new JQTreeViewRenderer(tree);
            tree.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string ToString()
        {
            return base.ToString();
        }
    }
}

