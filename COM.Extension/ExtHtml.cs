using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public static class ExtHml
    {
        /// <summary>
        /// 获取value为 url Referrer 的 hidden
        /// 后台用 Request["urlReferrer"]获取
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenToUrlReferrer(this HtmlHelper helper)
        {
            TagBuilder inputBuilder = new TagBuilder("input");
            inputBuilder.Attributes.Add("type", "hidden");
            inputBuilder.Attributes.Add("name", "urlReferrer");
            inputBuilder.Attributes.Add("value", System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
            return MvcHtmlString.Create(inputBuilder.ToString(TagRenderMode.SelfClosing));

        }
    }
}
