namespace Trirand.Web.Mvc
{
    using System;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class JQDropDownListRenderer
    {
        private JQDropDownList _model;

        public JQDropDownListRenderer(JQDropDownList model)
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
            builder.AppendFormat("jQuery('#{0}').jqDropDownList({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            StringBuilder builder = new StringBuilder();
            JQDropDownList list = this._model;
            DropDownListClientSideEvents clientSideEvents = this._model.ClientSideEvents;
            builder.AppendFormat("id:'{0}'", list.ID);
            builder.AppendFormat(",width:{0}", list.Width.ToString());
            builder.AppendFormat(",height:{0}", list.Height.ToString());
            if (list.DropDownWidth.HasValue)
            {
                builder.AppendFormat(",dropDownWidth:{0}", list.DropDownWidth.ToString());
            }
            if (list.TabIndex != 0)
            {
                builder.AppendFormat(",tabIndex:{0}", list.TabIndex.ToString());
            }
            if (list.Items.Count > 0)
            {
                builder.AppendFormat(",items:{0}", new JavaScriptSerializer().Serialize(list.SerializeItems(list.Items)));
            }
            if (!string.IsNullOrEmpty(list.ItemTemplateID))
            {
                builder.AppendFormat(",itemTemplateID:'{0}'", list.ItemTemplateID);
            }
            if (!string.IsNullOrEmpty(list.HeaderTemplateID))
            {
                builder.AppendFormat(",headerTemplateID:'{0}'", list.HeaderTemplateID);
            }
            if (!string.IsNullOrEmpty(list.FooterTemplateID))
            {
                builder.AppendFormat(",footerTemplateID:'{0}'", list.FooterTemplateID);
            }
            if (!string.IsNullOrEmpty(list.ToggleImageCssClass))
            {
                builder.AppendFormat(",toggleImageCssClass:'{0}'", list.ToggleImageCssClass);
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
            Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQDropDownList instance.");
            return this.GetStandaloneJavascript();
        }
    }
}

