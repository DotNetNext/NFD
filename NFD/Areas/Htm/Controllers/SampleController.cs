using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
using System.Web.UI.WebControls;
using NFD.BLL;
using System.Data;
using NFD.Entities.Common;
namespace NFD.Areas.Htm.Controllers
{
    //产样衣
    public class SampleController : Controller, IDisposable
    {


        //产样衣首页
        public ActionResult Index()
        {
            Tuple<JQGrid, JQGrid> model = Tuple.Create(JqGridModel, JqGridDetailModel);
            return View(model);
        }

        //编辑样衣
        public ActionResult Edit(Sample s)
        {
            return RedirectToAction("index");
        }

        //编辑样衣明细
        public ActionResult EditDetail(SampleDetail s)
        {
            SampleManager.Save(s);
            return RedirectToAction("index");
        }


        //编定样衣grid
        public JsonResult Grid_DataBind()
        {
            using (NFDEntities db = new NFDEntities())
            {
                var jqGridModel = JqGridModel;
                var data = BLL.SampleManager.GetVList(db);
                return jqGridModel.DataBind(data);
            };
        }

        //编定样衣明细 grid
        public JsonResult GridDatail_DataBind(string parentRowID)
        {

            using (NFDEntities db = new NFDEntities())
            {
                int sid = parentRowID.ToInt();
                var jqGridModel = JqGridDetailModel;
                var data = BLL.SampleManager.GetVDetailList(db).Where(c => c.sam_id == sid);
                return jqGridModel.DataBind(data);
            }
        }
        //获取样衣明细历史
        public JsonResult GridDatailHistory_DataBind()
        {
            using (NFDEntities db = new NFDEntities())
            {
                int samdId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "samdId").ToInt();
                var gridDataModel = JqGridDetailHistoryModel;
                return gridDataModel.DataBind(SampleManager.GetSampleListBySamdId(db, samdId));
            }

        }

        //更改状态
        public void ChangeStatus(int htId, int status)
        {
            BLL.SampleManager.ChangeStatus(htId, status);
        }

        //更新时间
        public void UpdateField(int id, string filed, DateTime? date)
        {
            BLL.SampleManager.UpdateField(id, filed, date);
        }

        //更新时间
        public void UpdateFieldChar(int id, string filed, string val)
        {
            BLL.SampleManager.UpdateFieldByVarchar(id, filed, val);
        }



        //样衣grid  model
        public JQGrid JqGridModel
        {
            get
            {
                var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
                return new JQGrid()
                {
                    Height = AppstringManager.GetGridHeight,
                    SearchDialogSettings = new SearchDialogSettings()
                    {


                    },
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
                    DataUrl = Url.Action("Grid_DataBind"),

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
                        InitialSortColumn = "sam_id desc"

                    },

                    AutoWidth = true,

                    Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "sam_id", 
                                                                PrimaryKey = true,
                                                         Searchable=false,
                                                                HeaderText="编号",
                                                                Width = 50 } ,
                                                                  new JQGridColumn{

                                                DataField="sam_name",
                                                HeaderText="样品番号",
                                                  DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                           
                                             },
                                               new JQGridColumn { DataField = "pro_no", 
                                                       
                                                                HeaderText="手织样番号",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                             new JQGridColumn { DataField = "specifications", 
                                                       
                                                                HeaderText="手织样规格"   ,
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
                                                                  Searchable=false,
                                                                  Width=100
                                                         
                                             }
                                             ,
                                                  new JQGridColumn{
                                                                 

                                                                  DataField="create_time",
                                                                  Editable=false    ,
                                                                  HeaderText="创建时间",
                                                                  Searchable=false ,
                                                                  Width=100,
                                                                Formatter=new CustomFormatter(){
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
        }
        //样衣明细 grid model
        public JQGrid JqGridDetailModel
        {
            get
            {
                return new JQGrid()
                {

                    SearchDialogSettings = new SearchDialogSettings()
                    {


                    },
                    ToolBarSettings = new ToolBarSettings()
                    {
                        ShowEditButton = true,
                        ShowRefreshButton = true
                    },
                    HierarchySettings = new HierarchySettings()
                    {
                        HierarchyMode = HierarchyMode.Child
                    },


                    PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                    {
                        PageSize = 10
                    },
                    DataUrl = Url.Action("GridDatail_DataBind"),
                    EditUrl = Url.Action("EditDetail"),

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
                        InitialSortColumn = "samd_id desc"

                    },

                    AutoWidth = true,

                    Columns = new List<JQGridColumn>()
               {
                   new JQGridColumn{ DataField="is_next", Visible=false},
                                             new JQGridColumn { DataField = "samd_id", 
                                                                PrimaryKey = true,
                                                                HeaderText="编号",
                                                                Width = 50
                                             } ,
                                              new JQGridColumn{

                                                DataField="bill_code",
                                                HeaderText="快件单号",
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,

                                                                 
                                             },
                                             new JQGridColumn { DataField = "color_foreign", 
                                                                HeaderText="色番",
                                                                Width = 50 } ,
                                              new JQGridColumn { DataField = "color_name", 
                                                                HeaderText="色号",
                                                                Width = 50 } ,
                                                                         new JQGridColumn { DataField = "paper_type", 
                                                       
                                                                HeaderText="纸型",
                                                                Width=100,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                                        new JQGridColumn { DataField = "fabric_num", 
                                              
                                                                HeaderText="面料米数" ,
                                                                Editable=true,
                                                                DataType=typeof(int),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new NumberValidator()
                                                                },
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToRound"
                                                                }
                                                          
                                                          

                                             },
                                                                
                                              new JQGridColumn { DataField = "indication_date", 
                                              
                                                                HeaderText="样品指示日" ,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                               new JQGridColumn { DataField = "no", 
                                                       
                                                                HeaderText="尺码",
                                                                Width=100,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                             new JQGridColumn { DataField = "necessary_number", 
                                              
                                                                HeaderText="必要枚数" ,
                                                                Editable=true,
                                                                DataType=typeof(int),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new NumberValidator()
                                                                }
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "hope_date", 
                                                       
                                                                HeaderText="样衣寄出日"   ,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                           
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             },
                                             new JQGridColumn{

                                                DataField="factory_date",
                                                HeaderText="工厂工作日",
                                                         SearchToolBarOperation=SearchOperation.IsEqualTo,
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.DropDown ,
                                                                    EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } , 
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             },
                                                   new JQGridColumn{

                                                DataField="actual_date",
                                                HeaderText="实际工作日",
                                                         SearchToolBarOperation=SearchOperation.IsEqualTo,
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,
                                                                   EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                 
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                                                 
                                             },
                                                           new JQGridColumn{

                                                DataField="remark",
                                                HeaderText="备注",
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,

                                                                 
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
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             },
                                                 new JQGridColumn { DataField = "samd_id", 
                                                       
                                                                Editable = false,
                                                                HeaderText="操作",
                                                                Formatter=new  CustomFormatter(){
                                                                  FormatFunction="childAction"
                                                                },
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             }
                                          //new JQGridColumn { DataField = "samd_id", 
                                          //                      Searchable=false,
                                          //                      HeaderText="操作",
                                          //                      Width = 190 ,
                                          //        Formatter= new  CustomFormatter(){
                                          //                         FormatFunction="action"
                                          //                      }
                                          //} ,

                                             

                                             
               },



                    Width = Unit.Pixel(640)


                };

            }
        }

        //样衣明细 grid model
        public JQGrid JqGridDetailHistoryModel
        {
            get
            {
                return new JQGrid()
                {

                    SearchDialogSettings = new SearchDialogSettings()
                    {


                    },
                    ToolBarSettings = new ToolBarSettings()
                    {

                    },


                    PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                    {
                        PageSize = 10
                    },
                    DataUrl = Url.Action("GridDatailHistory_DataBind"),

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
                        InitialSortColumn = "create_time desc"

                    },

                    AutoWidth = true,

                    Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "samd_id", 
                                                                PrimaryKey = true,
                                                                HeaderText="编号",
                                                                Width = 50,
                                                                Visible=false
                                             } ,
                                              new JQGridColumn{

                                                DataField="bill_code",
                                                HeaderText="快件单号",
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,

                                                                 
                                             },
                                             new JQGridColumn { DataField = "color_foreign", 
                                                                HeaderText="色番",
                                                                Width = 50 } ,
                                              new JQGridColumn { DataField = "color_name", 
                                                                HeaderText="色号",
                                                                Width = 50 } ,
                                                                         new JQGridColumn { DataField = "paper_type", 
                                                       
                                                                HeaderText="纸型",
                                                                Width=100,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                                            new JQGridColumn { DataField = "fabric_num", 
                                              
                                                                HeaderText="面料米数" ,
                                                                Editable=true,
                                                                DataType=typeof(int),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new NumberValidator()
                                                                }
                                                          
                                                          

                                             },
                                                                
                                              new JQGridColumn { DataField = "indication_date", 
                                              
                                                                HeaderText="样品指示日" ,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                               new JQGridColumn { DataField = "no", 
                                                       
                                                                HeaderText="尺码",
                                                                Width=100,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                             new JQGridColumn { DataField = "necessary_number", 
                                              
                                                                HeaderText="必要枚数" ,
                                                                Editable=true,
                                                                DataType=typeof(int),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new NumberValidator()
                                                                }
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "hope_date", 
                                                       
                                                                HeaderText="样衣寄出日"   ,
                                                                Editable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                           
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             },
                                             new JQGridColumn{

                                                DataField="factory_date",
                                                HeaderText="工厂工作日",
                                                         SearchToolBarOperation=SearchOperation.IsEqualTo,
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.DropDown ,
                                                                    EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } , 
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             },
                                                   new JQGridColumn{

                                                DataField="actual_date",
                                                HeaderText="实际工作日",
                                                         SearchToolBarOperation=SearchOperation.IsEqualTo,
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,
                                                                   EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                 
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                                                 
                                             },
                                                           new JQGridColumn{

                                                DataField="remark",
                                                HeaderText="备注",
                                                                  DataType=typeof(int),
                                                                  Editable=true,
                                                                  SearchType=SearchType.TextBox,

                                                                 
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
                                                                Formatter=new CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                }
                                             }
                                          //new JQGridColumn { DataField = "samd_id", 
                                          //                      Searchable=false,
                                          //                      HeaderText="操作",
                                          //                      Width = 190 ,
                                          //        Formatter= new  CustomFormatter(){
                                          //                         FormatFunction="action"
                                          //                      }
                                          //} ,

                                             

                                             
               },



                    Width = Unit.Pixel(640)


                };

            }
        }



        //大样记录
        [OutputCache(Duration = 0)]
        public PartialViewResult SampleHistory()
        {
            var model = JqGridDetailHistoryModel;
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult ReSample(int samdId)
        {

            var model = SampleManager.GetSampleDetailById(samdId);
            return PartialView(model);
        }

        //重新产样衣
        [HttpPost]
        public void ReSample(SampleDetailHistory h)
        {
            SampleManager.ReSample(h);

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
                    string fileName = AsposeExcel.MergeCellSavePdf(dt, "样品总览.pdf", 0, 1, 2, 3, 19);
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
                    AsposeExcel.MergeCellExportToPdf(dt, "样品总览.pdf", 0, 1, 2, 3, 19, 20);
                }
                else
                {
                    AsposeExcel.MergeCellExport(dt, "样品总览.xls", 0, 1, 2, 3, 19, 20);
                }
            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {
            var dbList = BLL.HandmadeThingsManager.GetV_Ht_Htd_A_S_Sd(db).Where(c => c.status == 2);
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
