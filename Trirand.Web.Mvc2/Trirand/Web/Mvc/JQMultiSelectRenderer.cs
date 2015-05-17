namespace Trirand.Web.Mvc
{
    using System;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class JQMultiSelectRenderer
    {
        private JQMultiSelect _model;

        public JQMultiSelectRenderer(JQMultiSelect model)
        {
            this._model = model;
        }

        private string GetButtonText()
        {
            string text = "";
            foreach (JQListItem item in this._model.Items)
            {
                if (item.Selected)
                {
                    text = item.Text;
                    break;
                }
            }
            if (string.IsNullOrEmpty(text) && (this._model.Items.Count > 0))
            {
                text = this._model.Items[0].Text;
            }
            return text;
        }

        private string GetStandaloneJavascript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<span id='{0}'></span>", this._model.ID);
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.AppendFormat("jQuery('#{0}').jqMultiSelect({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            StringBuilder builder = new StringBuilder();
            JQMultiSelect select = this._model;
            MultiSelectClientSideEvents clientSideEvents = this._model.ClientSideEvents;
            builder.AppendFormat("id:'{0}'", select.ID);
            builder.AppendFormat(",width:{0}", select.Width.ToString());
            builder.AppendFormat(",height:{0}", select.Height.ToString());
            if (select.DropDownWidth.HasValue)
            {
                builder.AppendFormat(",dropDownWidth:{0}", select.DropDownWidth.ToString());
            }
            if (select.TabIndex != 0)
            {
                builder.AppendFormat(",tabIndex:{0}", select.TabIndex.ToString());
            }
            if (select.Items.Count > 0)
            {
                builder.AppendFormat(",items:{0}", new JavaScriptSerializer().Serialize(select.SerializeItems(select.Items)));
            }
            if (!string.IsNullOrEmpty(select.ItemTemplateID))
            {
                builder.AppendFormat(",itemTemplateID:'{0}'", select.ItemTemplateID);
            }
            if (!string.IsNullOrEmpty(select.HeaderTemplateID))
            {
                builder.AppendFormat(",headerTemplateID:'{0}'", select.HeaderTemplateID);
            }
            if (!string.IsNullOrEmpty(select.FooterTemplateID))
            {
                builder.AppendFormat(",footerTemplateID:'{0}'", select.FooterTemplateID);
            }
            if (!string.IsNullOrEmpty(select.ToggleImageCssClass))
            {
                builder.AppendFormat(",toggleImageCssClass:'{0}'", select.ToggleImageCssClass);
            }
            if (select.Filter != Trirand.Web.Mvc.Filter.None)
            {
                builder.AppendFormat(",filter:'{0}'", select.Filter.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Show))
            {
                builder.AppendFormat(",onShow:{0}", clientSideEvents.Show);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Hide))
            {
                builder.AppendFormat(",onHide:{0}", clientSideEvents.Hide);
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
            if (!string.IsNullOrEmpty(clientSideEvents.Select))
            {
                builder.AppendFormat(",onInitialized:{0}", clientSideEvents.Initialized);
            }
            if (!string.IsNullOrEmpty(clientSideEvents.Select))
            {
                builder.AppendFormat(",onKeyDown:{0}", clientSideEvents.KeyDown);
            }
            return builder.ToString();
        }

        public string RenderHtml()
        {
            if (DateTime.Now > CompiledOn.CompilationDate.AddDays(45.0))
            {
                return "This is a 30-day trial version of jqSuite for ASP.NET MVC which has expired.<br> Please, contact sales@trirand.net for purchasing the product or for trial extension.";
            }
            Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQComboBox instance.");
            return this.GetStandaloneJavascript();
        }
    }
}

