using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using System.Data;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Common;
namespace NFD.Areas.Htm.Controllers
{
    //手织样汇总
    public class HtSummaryController : Controller
    {


        public ActionResult Index()
        {
            JQGrid gridModel = GetHtGridModel();
            return View(gridModel);
        }


        public JsonResult JqGridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid gridModel = GetHtGridModel();
                var list = BLL.HandmadeThingsManager.GetV_Ht_Htd_A_S_Sd(db);
                return gridModel.DataBind(list);
            }
        }

        /// <summary>
        /// 获取一级 grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetHtGridModel()
        {
            var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
            traderList.Insert(0, new SelectListItem());
            var dateFormat = new CustomFormatter() { FormatFunction = "ToDate" };
            List<SelectListItem> searchList = new List<SelectListItem>();
            searchList.Add(new SelectListItem { Text = "", Value = "" });
            searchList.Add(new SelectListItem { Text = "近3天", Value = DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd") });
            searchList.Add(new SelectListItem { Text = "近1个月", Value = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd") });
            searchList.Add(new SelectListItem { Text = "近3个月", Value = DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd") });
            searchList.Add(new SelectListItem { Text = "近半年", Value = DateTime.Now.AddMonths(-6).ToString("yyyy/MM/dd") });
            searchList.Add(new SelectListItem { Text = "近一年", Value = DateTime.Now.AddMonths(-12).ToString("yyyy/MM/dd") });
            return new JQGrid
            {
                ToolBarSettings = new ToolBarSettings()
                {
                    ShowSearchToolBar = true,
                    CustomButtons = new List<JQGridToolBarButton>() { 
                      new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exExcel"
                      },
                        new JQGridToolBarButton(){
                       Text="打印PDF",
                        OnClick="exPdf"
                      },
                      new JQGridToolBarButton(){
                       Text="发送邮件",
                        OnClick="sendMail"
                      }
                    }
                },

                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = 20
                },
                Height = 600,
                DataUrl = Url.Action("JqGridData"),


                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                         new JQGridColumn { DataField="trader_id",HeaderText="客户",Frozen = true,SearchToolBarOperation=SearchOperation.IsEqualTo,DataType=typeof(int),SearchType=SearchType.DropDown,SearchList=traderList,Formatter= new  CustomFormatter(){FormatFunction="ToTrader"} },
                    new JQGridColumn { DataField = "id", PrimaryKey = true,Searchable=false, HeaderText="编号",Width = 100, Visible=false ,  Frozen = true} ,
                      new JQGridColumn { DataField = "ht_id",Searchable=false, HeaderText="手织样id",Width = 100, Visible=false ,  Frozen = true} ,
                    new JQGridColumn { DataField = "pro_no",DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="品番",  Frozen = true, Width=150,Formatter= new  CustomFormatter(){FormatFunction="ToNo"}},
                    new JQGridColumn { DataField = "production_place",DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="生产地名",  Frozen = true, Width=150},
                    new JQGridColumn { DataField = "specifications",DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains,  HeaderText="规格", Width=150, Frozen = true,},

                    new JQGridColumn { DataField = "color_foreign", DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="色番", Width=50, Frozen = true,},
                    new JQGridColumn { DataField = "color_name", DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="色名", Width=50, Frozen = true,},
                    new JQGridColumn { DataField = "htdIndicate_day",  HeaderText="手织样指示日", Width=150,Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "htdpredetermined_sent_day",  HeaderText="预定寄出日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "htdactual_sending_date",  HeaderText="实际寄出日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "htdconfirm_day",  HeaderText="确认日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                   
                    
                    new JQGridColumn { DataField = "aIndicate_day",  HeaderText="放大样指示日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "ahope_data",  HeaderText="希望成品日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "afactory_date",  HeaderText="工厂成品日", Width=150,     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "aactual_date",  HeaderText="实际成品日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "am_num", DataType=typeof(decimal),SearchToolBarOperation=SearchOperation.IsEqualTo,  HeaderText="放样M数"},
                    new JQGridColumn { DataField = "afinished_num", DataType=typeof(decimal),SearchToolBarOperation=SearchOperation.IsEqualTo,  HeaderText="放样成品数"},
                    new JQGridColumn { DataField = "afactory_date2",  HeaderText="送工厂日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "afactory_num",  HeaderText="送工厂量",DataType=typeof(decimal),SearchToolBarOperation=SearchOperation.IsEqualTo, },
                    new JQGridColumn { DataField = "awarehouse_num",  HeaderText="在仓米数",DataType=typeof(decimal),SearchToolBarOperation=SearchOperation.IsEqualTo, },
                    new JQGridColumn { DataField = "aremark", DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="备注"},
                    new JQGridColumn { DataField = "sdindication_date",  HeaderText="样品指示日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},
                    new JQGridColumn { DataField = "sam_name", DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="样品品番"},
                    new JQGridColumn { DataField = "sdpaper_type", DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains, HeaderText="型纸"},
                    new JQGridColumn { DataField = "sdno",  DataType=typeof(string),SearchToolBarOperation=SearchOperation.Contains,HeaderText="尺码"},
                    new JQGridColumn { DataField = "sdnecessary_number", DataType=typeof(int),SearchToolBarOperation=SearchOperation.IsEqualTo,  HeaderText="必要枚数"},
                    new JQGridColumn { DataField = "sdhope_date",  HeaderText="希望寄送日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},  
                    new JQGridColumn { DataField = "sdfactory_date",  HeaderText="工厂寄送日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},      
                    new JQGridColumn { DataField = "sdactual_date",  HeaderText="实际寄送日",     Formatter=dateFormat,SearchToolBarOperation=SearchOperation.IsGreaterOrEqualTo,SearchType = SearchType.DropDown,SearchList=searchList,DataType=typeof(DateTime)},      
                    new JQGridColumn{ DataField="register_time", HeaderText="创建日期",Formatter=dateFormat}
               }
            };
        }

        //发送邮件
        public JsonResult SendMail(Mail mail, DateTime? dateMin, DateTime? dateMax, bool? isAtta, int trader_id = 0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                COM.Utility.SendMail sm = new COM.Utility.SendMail(BLL.AppstringManager.GetMailSmtp, BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailPassword);
               
                var reJson = new ReJson();
                if (isAtta == true)
                {
                    DataTable dt = GetExportDataTable(dateMin, dateMax, trader_id, db);
                    string fileName = AsposeExcel.MergeCellSavePdf(dt, "手织样总览.pdf", 0, 1, 2, 3, 19);
                    reJson.IsSuccess = sm.Send(BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailName, mail.MailName, mail.Title, mail.Content, fileName);
                }
                else
                {
                    reJson.IsSuccess = sm.Send(BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailName, mail.MailName, mail.Title, mail.Content);
                }
                return Json(reJson);
            }
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="trader_id"></param>
        public void ExportExcel(DateTime? dateMin, DateTime? dateMax, int trader_id = 0, int typeId = 0)/* 0、excel 1、pdf*/
        {
            using (NFDEntities db = new NFDEntities())
            {
                DataTable dt = GetExportDataTable(dateMin, dateMax, trader_id, db);
                if (typeId == 1)
                {
                    AsposeExcel.MergeCellExportToPdf(dt, "手织样总览.pdf", 0, 1, 2, 3, 19,20);
                }
                else
                {
                    AsposeExcel.MergeCellExport(dt, "手织样总览.xls", 0, 1, 2, 3, 19,20);
                }
            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {
            var dbList = BLL.HandmadeThingsManager.GetV_Ht_Htd_A_S_Sd(db).Where(c => true);
            if (trader_id > 0)
            {
                dbList = dbList.Where(c => c.trader_id == trader_id);
            }
            if (dateMin != null)
            {
                dbList = dbList.Where(c => c.register_time >= dateMin);
            }
            if (dateMax != null)
            {
                dateMax = ((DateTime)dateMax).AddDays(1);
                dbList = dbList.Where(c => c.register_time <= dateMax);
            }
            var data = dbList.OrderByDescending(c=>c.id).ToList();
            var traderList = BLL.TraderManager.GetList();
            DataTable dt = new DataTable();
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("产地番号", typeof(string));
            dt.Columns.Add("生产地名称", typeof(string));
            dt.Columns.Add("规格", typeof(string));
            dt.Columns.Add("色番", typeof(string));
            dt.Columns.Add("色号", typeof(string));
            dt.Columns.Add("手织样指示日", typeof(string));
            dt.Columns.Add("预定寄出日", typeof(string));
            dt.Columns.Add("确认日", typeof(string));
            dt.Columns.Add("放大样指示日", typeof(string));
            dt.Columns.Add("希望成品日", typeof(string));
            dt.Columns.Add("工厂成品日", typeof(string));
            dt.Columns.Add("实际成品日", typeof(string));
            dt.Columns.Add("放样M数", typeof(string));
            dt.Columns.Add("放样成品数", typeof(string));
            dt.Columns.Add("送工厂日", typeof(string));
            dt.Columns.Add("送工厂量", typeof(string));
            dt.Columns.Add("在仓米数", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            dt.Columns.Add("样品指示日", typeof(string));
            dt.Columns.Add("样品品番", typeof(string));
            dt.Columns.Add("型纸", typeof(string));
            dt.Columns.Add("尺码", typeof(string));
            dt.Columns.Add("必要枚数", typeof(string));
            dt.Columns.Add("希望寄送日", typeof(string));
            dt.Columns.Add("工厂寄送日", typeof(string));
            dt.Columns.Add("实际寄送日", typeof(string));
            foreach (var r in data)
            {
                DataRow dr = dt.NewRow();
                dr["客户"] = traderList.Single(c => c.trader_id == r.trader_id).name;
                dr["产地番号"] = r.pro_no;
                dr["生产地名称"] = r.production_place;
                dr["规格"] = r.specifications;
                dr["色番"] = r.color_foreign;
                dr["色名"] = r.color_name;
                dr["手织样指示日"] = r.htdIndicate_day.ToDateStr("yyyy-MM-dd");
                dr["预定寄出日"] = r.htdpredetermined_sent_day.ToDateStr("yyyy-MM-dd");
                dr["确认日"] = r.htdconfirm_day.ToDateStr("yyyy-MM-dd");
                dr["放大样指示日"] = r.aIndicate_day.ToDateStr("yyyy-MM-dd");
                dr["希望成品日"] = r.ahope_data.ToDateStr("yyyy-MM-dd");
                dr["工厂成品日"] = r.afactory_date.ToDateStr("yyyy-MM-dd");
                dr["实际成品日"] = r.aactual_date.ToDateStr("yyyy-MM-dd");
                dr["放样M数"] = r.am_num;
                dr["放样成品数"] = r.afinished_num;
                dr["送工厂日"] = r.afactory_date2.ToDateStr("yyyy-MM-dd"); ;
                dr["送工厂量"] = r.afactory_num;
                dr["在仓米数"] = r.awarehouse_num;
                dr["备注"] = r.aremark;
                dr["样品指示日"] = r.sdindication_date.ToDateStr("yyyy-MM-dd");
                dr["样品品番"] = r.sam_name;
                dr["型纸"] = r.sdpaper_type;
                dr["尺码"] = r.sdno;
                dr["必要枚数"] = r.sdnecessary_number;
                dr["希望寄送日"] = r.sdhope_date.ToDateStr("yyyy-MM-dd");
                dr["工厂寄送日"] = r.sdfactory_date.ToDateStr("yyyy-MM-dd");
                dr["实际寄送日"] = r.sdactual_date.ToDateStr("yyyy-MM-dd");

                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
