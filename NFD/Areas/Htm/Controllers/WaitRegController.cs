using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.Entities.Data;
using Trirand.Web.Mvc;
using COM.Utility;
using NFD.Entities.Common;
using COM.Extension;
using System.Web.UI.WebControls;
using System.Data;
using NFD.BLL;
namespace NFD.Areas.Htm.Controllers
{
    //待登录手织样
    public class WaitRegController : Controller
    {

        public ActionResult Index()
        {
            Tuple<JQGrid, JQGrid> model = Tuple.Create(GetHtGridModel(), GetHtdGridModel());
            return View(model);
        }
        //手织样grid数据 一级
        public JsonResult GridHtData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid model = GetHtGridModel();
                return model.DataBind(BLL.HandmadeThingsManager.GetList(db).Where(c=>c.status==null||c.status>0));
            }
        }
        //手织样明细grid数据 二级
        public JsonResult GridHtdData(string parentRowID)
        {
            using (NFDEntities db = new NFDEntities())
            {
                int ht_id = parentRowID.ToInt();
                JQGrid model = GetHtdGridModel();
                return model.DataBind(BLL.HandmadeThingsDetailManager.GetList(db).Where(c => c.ht_id == ht_id));
            }
        }

        public JsonResult GridHtdHData()
        {
            //htdId 手织样明细ID
            int id = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id").ToInt();
            var gridModel = GetHtdHGridModel();
            using (NFDEntities db = new NFDEntities())
            {
                var GridModelData = BLL.HandmadeThingsDetailManager.GetHtdHistoryByHtdId(db, id);
                return gridModel.DataBind(GridModelData);
            }
        }

        //统一字段
        public void UpdateField(int id, string filed, DateTime? date)
        {
            BLL.HandmadeThingsDetailManager.UpdateField(id, filed, date);

        }

        //放大样
        public void AmplifierSample(int id)
        {
            BLL.HandmadeThingsManager.AmplifierSample(id);
        }

        /// <summary>
        /// 获取一级 grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetHtGridModel()
        {
            var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
            traderList.Insert(0, new SelectListItem());
            return new JQGrid
            {
                SearchDialogSettings = new SearchDialogSettings()
                {


                },
                ToolBarSettings = new ToolBarSettings()
                {
                    ShowSearchToolBar = true,
                    ShowRefreshButton=true,
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
                HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Parent,
                    ReloadOnExpand = false,
                    SelectOnExpand = false,
                    ExpandOnLoad = false
                },

                ClientSideEvents = new ClientSideEvents()
                {
                    SubGridRowExpanded = "showSubGrid"
                },
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = 10
                },
                Height = BLL.AppstringManager.GetGridHeight,
                DataUrl = Url.Action("GridHtData"),

                EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true
                },
                AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true
                },

                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "ht_id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "status",  Visible=false } ,
                                             new JQGridColumn { DataField = "ht_id", 
                                                                PrimaryKey = true,
                                                         Searchable=false,
                                                                HeaderText="编号",
                                                                Width = 50 } ,
                                               new JQGridColumn { DataField = "pro_no", 
                                                       
                                                                HeaderText="品番",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                             new JQGridColumn { DataField = "production_place", 
                                              
                                                                HeaderText="生产地" ,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "specifications", 
                                                       
                                                                HeaderText="规格"   ,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox

                                             },
                                             new JQGridColumn{

                                                DataField="trader_id",
                                                HeaderText="客户",
                                                         SearchToolBarOperation=SearchOperation.IsEqualTo,
                                                                  DataType=typeof(int),
                                                                  SearchType=SearchType.DropDown,
                                                                  SearchList=traderList,
                                                                  Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToTrader"
                                                                }
                                             },
                                             new JQGridColumn{
                                                                 

                                                                  DataField="creator_name",
                                                                  Editable=false    ,
                                                                  HeaderText="创建人",
                                                                  Searchable=false
                                                         
                                             }
                                             ,
                                                  new JQGridColumn{
                                                                 

                                                                  DataField="create_time",
                                                                  Editable=false    ,
                                                                  HeaderText="创建时间",
                                                                  Searchable=false,
                                                                      Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                             },
                                          new JQGridColumn { DataField = "ht_id", 
                                                                Searchable=false,
                                                                HeaderText="操作",
                                                                Width = 190 ,
                                                  Formatter= new  CustomFormatter(){
                                                                   FormatFunction="action"
                                                                }
                                          } ,

                                             

                                             
               },



                Width = Unit.Pixel(640)
            };
        }

        /// <summary>
        /// 获取二级grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetHtdGridModel()
        {

            return new JQGrid
            {
                Height = Unit.Percentage(100),
                DataUrl = Url.Action("GridHtdData"),
                EditUrl = ("/Htm/Registration/EditGrid"),
                HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Child
                },
                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "color_foreign"

                },
                ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowDeleteButton = true,
                    ShowRefreshButton = true
                },

                EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true,
                    TopOffset = 0
                },
                AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true
                },

                Columns = new List<JQGridColumn>()
               {

                                             new JQGridColumn { DataField = "is_next", Visible=false} ,
                                             new JQGridColumn { DataField = "htd_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Width = 50 } ,
                                               new JQGridColumn { DataField = "color_foreign", 
                                                                Editable = true,
                                                                HeaderText="色番",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                             new JQGridColumn { DataField = "color_name", 
                                                                Editable = true,
                                                                HeaderText="色名",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             },
                                              new JQGridColumn { DataField = "Indicate_day", 
                                                                Editable = true,
                                                                HeaderText="指示日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },

                                                               
                                                          

                                             },
                                    
                                              new JQGridColumn { DataField = "predetermined_sent_day", 
                                                                Editable = true,
                                                                HeaderText="预定寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "actual_sending_date", 
                                                                Editable = true,
                                                                HeaderText="实际寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                                       new JQGridColumn { DataField = "confirm_day", 
                                                                Editable = true,
                                                                HeaderText="确认日期",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          

                                             },
                                                     new JQGridColumn { DataField = "confirm_content", 
                                                                Editable = true,
                                                                HeaderText="确认意见",
                                                                Width=150,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                                

                                             },
                                            new JQGridColumn { DataField = "remark", 
                                                                Editable = true,
                                                                HeaderText="备注",
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                                

                                             },
                                                   new JQGridColumn { DataField = "htd_id", 
                                                       
                                                                Editable = false,
                                                                HeaderText="操作",
                                                                Formatter=new  CustomFormatter(){
                                                                  FormatFunction="childAction"
                                                                },
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             }



                                             
               },



                Width = Unit.Pixel(840)
            };
        }


        /// <summary>
        /// 获取三级grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetHtdHGridModel()
        {

            return new JQGrid
            {
                Height = Unit.Percentage(100),
                DataUrl = Url.Action("GridHtdHData"),
                //EditUrl = ("/Htm/Registration/EditGrid"),
                //HierarchySettings = new HierarchySettings()
                //{
                //    HierarchyMode = HierarchyMode.Child
                //},
                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "create_time desc"

                },
                ToolBarSettings = new ToolBarSettings()
                {

                },

                EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true,
                    TopOffset = 0
                },
                AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true
                },

                Columns = new List<JQGridColumn>()
               {

                                             //new JQGridColumn { DataField = "is_next", Visible=false} ,
                                             new JQGridColumn { DataField = "htd_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                 Visible=false,
                                                                Width = 50 } ,
                                               new JQGridColumn { DataField = "color_foreign", 
                                                                Editable = true,
                                                                HeaderText="色番",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                             new JQGridColumn { DataField = "color_name", 
                                                                Editable = true,
                                                                HeaderText="色号",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             },
                                              new JQGridColumn { DataField = "Indicate_day", 
                                                                Editable = true,
                                                                HeaderText="指示日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },

                                                               
                                                          

                                             },
                                    
                                              new JQGridColumn { DataField = "predetermined_sent_day", 
                                                                Editable = true,
                                                                HeaderText="预定寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "actual_sending_date", 
                                                                Editable = true,
                                                                HeaderText="实际寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                                       new JQGridColumn { DataField = "confirm_day", 
                                                                Editable = true,
                                                                HeaderText="确认日期",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          

                                             },
                                                     new JQGridColumn { DataField = "confirm_content", 
                                                                Editable = true,
                                                                HeaderText="确认意见",
                                                                Width=150,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                                

                                             },
                                            new JQGridColumn { DataField = "remark", 
                                                                Editable = true,
                                                                HeaderText="备注",
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                                

                                             } ,
                                                           new JQGridColumn { DataField = "create_time", 
                                                                Editable = true,
                                                                HeaderText="创建时间",
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                                

                                             } 
                                             



                                             
               },



                Width = Unit.Pixel(840)
            };
        }

        //重新打手织样
        [HttpPost]
        public void ReHtm(HandmadeThingsDetailHistory h)
        {
            HandmadeThingsDetailManager.ReHtm(h);
        }
        [HttpGet]
        [OutputCache(Duration = 0)]
        public PartialViewResult ReHtm(int id/*htdId*/)
        {
            var model = HandmadeThingsDetailManager.GetHtdById(id);
            return PartialView(model);
        }

        //打样记录
        [OutputCache(Duration = 0)]
        public PartialViewResult Htdh(int id/*htdId*/)
        {
            var model = GetHtdHGridModel();
            return PartialView(model);
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
                    AsposeExcel.MergeCellExportToPdf(dt, "手织样总览.pdf", 0, 1, 2, 3, 19, 20);
                }
                else
                {
                    AsposeExcel.MergeCellExport(dt, "手织样总览.xls", 0, 1, 2, 3, 19, 20);
                }
            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {
            var dbList = BLL.HandmadeThingsManager.GetV_Ht_Htd_A_S_Sd(db).Where(c => c.status == 0 || c.status == null);
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
            var data = dbList.OrderByDescending(c => c.id).ToList();
            var traderList = BLL.TraderManager.GetList();
            DataTable dt = new DataTable();
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("产地番号", typeof(string));
            dt.Columns.Add("生产地名称", typeof(string));
            dt.Columns.Add("规格", typeof(string));
            dt.Columns.Add("色番", typeof(string));
            dt.Columns.Add("色名", typeof(string));
            dt.Columns.Add("手织样指示日", typeof(string));
            dt.Columns.Add("预定寄出日", typeof(string));
            dt.Columns.Add("确认日", typeof(string));
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
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
