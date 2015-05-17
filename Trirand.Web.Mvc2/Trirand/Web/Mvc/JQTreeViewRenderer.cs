namespace Trirand.Web.Mvc
{
    using System;
    using System.Text;

    internal class JQTreeViewRenderer
    {
        private JQTreeView _model;

        public JQTreeViewRenderer(JQTreeView model)
        {
            this._model = model;
        }

        private string GetStandaloneJavascript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<div id='{0}'/>", this._model.ID);
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.AppendFormat("jQuery('#{0}').jqTreeView({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            StringBuilder builder = new StringBuilder();
            JQTreeView view = this._model;
            TreeViewClientSideEvents clientSideEvents = view.ClientSideEvents;
            builder.AppendFormat("id: '{0}'", view.ID);
            if (!view.Height.IsEmpty)
            {
                builder.AppendFormat(",height: '{0}'", view.Height);
            }
            if (!view.Width.IsEmpty)
            {
                builder.AppendFormat(",width: '{0}'", view.Height);
            }
            if (!string.IsNullOrEmpty(view.DataUrl))
            {
                builder.AppendFormat(",dataUrl: '{0}'", view.DataUrl);
            }
            if (!string.IsNullOrEmpty(view.DragAndDropUrl))
            {
                builder.AppendFormat(",dragAndDropUrl: '{0}'", view.DragAndDropUrl);
            }
            if (!view.HoverOnMouseOver)
            {
                builder.AppendFormat(",hoverOnMouseOver:false", new object[0]);
            }
            if (view.CheckBoxes)
            {
                builder.Append(",checkBoxes:true");
            }
            if (view.MultipleSelect)
            {
                builder.Append(",multipleSelect:true");
            }
            if (view.DragAndDrop)
            {
                builder.Append(",dragAndDrop:true");
            }
            if (!string.IsNullOrEmpty(view.NodeTemplateID))
            {
                builder.AppendFormat(",nodeTemplateID:'{0}'", view.NodeTemplateID);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Check))
            {
                builder.AppendFormat(",onCheck:{0}", clientSideEvents.Check);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Collapse))
            {
                builder.AppendFormat(",onCollapse:{0}", clientSideEvents.Collapse);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Expand))
            {
                builder.AppendFormat(",onExpand:{0}", clientSideEvents.Expand);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.MouseOut))
            {
                builder.AppendFormat(",onMouseOut:{0}", clientSideEvents.MouseOut);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.MouseOver))
            {
                builder.AppendFormat(",onMouseOver:{0}", clientSideEvents.MouseOver);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Select))
            {
                builder.AppendFormat(",onSelect:{0}", clientSideEvents.Select);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.NodesDragged))
            {
                builder.AppendFormat(",onNodesDragged:{0}", clientSideEvents.NodesDragged);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.NodesMoved))
            {
                builder.AppendFormat(",onNodesMoved:{0}", clientSideEvents.NodesMoved);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.NodesDropped))
            {
                builder.AppendFormat(",onNodesDropped:{0}", clientSideEvents.NodesDropped);
            }
            return builder.ToString();
        }

        public string RenderHtml()
        {
            if (DateTime.Now > CompiledOn.CompilationDate.AddDays(45.0))
            {
                return "This is a 30-day trial version of jqSuite for ASP.NET MVC which has expired.<br> Please, contact sales@trirand.net for purchasing the product or for trial extension.";
            }
            Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQTreeView instance.");
            Guard.IsNotNullOrEmpty(this._model.DataUrl, "DataUrl", "You need to set DataUrl to the Action of the tree returning nodes.");
            return this.GetStandaloneJavascript();
        }
    }
}

