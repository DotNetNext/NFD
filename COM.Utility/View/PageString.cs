using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace COM.Utility
{

    /// 功能：分页算法
    /// 作者：孙凯旋
    /// 时间：2011-1-29
    /// </summary>
    public class PageStringHelper
    {
        /// <summary>
        /// 分页算法＜一＞共20页 首页 上一页  1  2  3  4  5  6  7  8  9  10  下一页  末页 
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// <returns></returns>
        public static string PageString(int total, int pageSize, int pageIndex, string query_string)
        {
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (pageIndex < 1) { pageIndex = 1; }
            //计算总页数
            if (pageSize != 0)
            {
                allpage = (total / pageSize);
                allpage = ((total % pageSize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = pageIndex + 1;
            pre = pageIndex - 1;
            startcount = (pageIndex + 5) > allpage ? allpage - 9 : pageIndex - 4;//中间页起始序号
            //中间页终止序号
            endcount = pageIndex < 5 ? 10 : pageIndex + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页   当前:" + pageIndex + "/" + allpage + "总共" + total + "条   ";

            pagestr += pageIndex > 1 ? "<a href=\"" + query_string + "pageIndex=1\">首页</a>  <a href=\"" + query_string + "pageIndex=" + pre + "\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += pageIndex == i ? "  <font color=\"#ff0000\">" + i + "</font>" : "  <a href=\"" + query_string + "pageIndex=" + i + "\">" + i + "</a>";
            }
            pagestr += pageIndex != allpage ? "  <a href=\"" + query_string + "pageIndex=" + next + "\">下一页</a>  <a href=\"" + query_string + "pageIndex=" + allpage + "\">末页</a>" : " 下一页 末页";

            return pagestr;


        }

        /// <summary>
        /// 分页算法＜一＞共20页 首页 上一页  1  2  3  4  5  6  7  8  9  10  下一页  末页 
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// <returns></returns>
        public static string PageAjaxString(int total, int pageSize, int pageIndex)
        {
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (pageIndex < 1) { pageIndex = 1; }
            //计算总页数
            if (pageSize != 0)
            {
                allpage = (total / pageSize);
                allpage = ((total % pageSize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = pageIndex + 1;
            pre = pageIndex - 1;
            startcount = (pageIndex + 5) > allpage ? allpage - 9 : pageIndex - 4;//中间页起始序号
            //中间页终止序号
            endcount = pageIndex < 5 ? 10 : pageIndex + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页   当前:" + pageIndex + "/" + allpage + "总共" + total + "条   ";

            pagestr += pageIndex > 1 ? "<a onclick=\"AjaxPage(1)\">首页</a>  <a onclick=\"AjaxPage( " + pre + ")\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += pageIndex == i ? "  <font class='checked' color=\"#ff0000\">" + i + "</font>" : "  <a onclick=\"AjaxPage(" + i + ")\">" + i + "</a>";
            }
            pagestr += pageIndex != allpage ? "  <a onclick=\"AjaxPage(" + next + ")\">下一页</a>  <a onclick=\"AjaxPage(" + allpage + ")\">末页</a>" : " 下一页 末页";

            return pagestr;


        }
        /// <summary>
        /// 分页算法＜一＞共20页 首页 上一页  1  2  3  4  5  6  7  8  9  10  下一页  末页 
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// <returns></returns>
        public static string PageAjaxString(int total, int pageSize, int pageIndex, int p1, int P2)
        {
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (pageIndex < 1) { pageIndex = 1; }
            //计算总页数
            if (pageSize != 0)
            {
                allpage = (total / pageSize);
                allpage = ((total % pageSize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = pageIndex + 1;
            pre = pageIndex - 1;
            startcount = (pageIndex + 5) > allpage ? allpage - 9 : pageIndex - 4;//中间页起始序号
            //中间页终止序号
            endcount = pageIndex < 5 ? 10 : pageIndex + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页   当前:" + pageIndex + "/" + allpage + "总共<span class='tol'>" + total + "</span>条   ";

            pagestr += pageIndex > 1 ? "<a onclick=\"AjaxPage(1," + p1 + "," + P2 + ")\">首页</a>  <a onclick=\"AjaxPage( " + pre + "," + p1 + "," + P2 + ")\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += pageIndex == i ? "  <font class='checked' color=\"#ff0000\">" + i + "</font>" : "  <a onclick=\"AjaxPage(" + i + "," + p1 + "," + P2 + ")\">" + i + "</a>";
            }
            pagestr += pageIndex != allpage ? "  <a onclick=\"AjaxPage(" + next + "," + p1 + "," + P2 + ")\">下一页</a>  <a onclick=\"AjaxPage(" + allpage + "," + p1 + "," + P2 + ")\">末页</a>" : " 下一页 末页";

            return pagestr;


        }

    }
}
