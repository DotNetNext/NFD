using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace COM.Extension
{
    public static class ExtJqGrid
    {

        /// <summary>
        /// 在jqgrid后面追加js代码
        /// </summary>
        /// <param name="html"></param>
        /// <param name="appendJs"></param>
        /// <returns></returns>
        public static MvcHtmlString JqGridAppendJs(this MvcHtmlString html, string appendJs)
        {
            var gridhtml = html.ToString();
            gridhtml = System.Text.RegularExpressions.Regex.Replace(gridhtml, @"\}\)\;<\/script>", "");
            gridhtml += @"
              " +
               appendJs
            + @"

             ";
            gridhtml += "});</script>";
            return MvcHtmlString.Create(gridhtml);

        }

        /// <summary>
        /// 在jqgrid中添加option
        /// 调用如下：
        ///  @Html.Trirand().JQGrid((JQGrid)Model, "JQGrid1").JqGridAppendOption(@"gridComplete:function(){ alert(2)} ");
        /// </summary>
        /// <param name="html"></param>
        /// <param name="appendOption"></param>
        /// <returns></returns>
        public static MvcHtmlString JqGridAppendOption(this MvcHtmlString html, string appendOption)
        {
            var gridhtml = html.ToString();
            gridhtml = gridhtml.Replace("jqGrid({", "jqGrid({" + appendOption + ",");
            return MvcHtmlString.Create(gridhtml);

        }
        /// <summary>
        /// 在jqgrid前面追加js代码
        /// </summary>
        /// <param name="html"></param>
        /// <param name="appendJs"></param>
        /// <returns></returns>
        public static MvcHtmlString JqGridPrependJs(this MvcHtmlString html, string prependJs)
        {
            var gridhtml = html.ToString();
            gridhtml = gridhtml.Replace("jQuery(document).ready(funct", prependJs + "\njQuery(document).ready(funct");
            return MvcHtmlString.Create(gridhtml);
        }
    }
}
