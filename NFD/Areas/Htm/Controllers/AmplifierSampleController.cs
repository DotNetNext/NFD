using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL;
using COM.Extension;
using System.Web.UI.WebControls;
using NFD.Entities.Common;
using System.Data;
using COM.Utility;
namespace NFD.Areas.Htm.Controllers
{
    //放大样
    public class AmplifierSampleController : Controller
    {
        public ActionResult Index()
        {
            Tuple<JQGrid, JQGrid> model = Tuple.Create(GetHtGridModel(), GetGridAsModel());
            return View(model);
        }
        //手织样grid数据 一级
        public JsonResult GridHtData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid model = GetHtGridModel();
                return model.DataBind(HandmadeThingsManager.GetList(db).Where(c => c.status != null && c.status>0));
            }
        }
        //放大样数据 二级
        public JsonResult GridAsData(string parentRowID)
        {
            using (NFDEntities db = new NFDEntities())
            {
                int parentId = parentRowID.ToInt();
                JQGrid model = GetGridAsModel();
                return model.DataBind(AmplifierSampleManager.GetVList(db).Where(c => c.ht_id == parentId));
            }
        }
        //放大样数据 三级
        public JsonResult GridAsHistoryData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid model = GetAsHistoryGridModel();
                int asId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "asId").ToInt();
                return model.DataBind(AmplifierSampleManager.GetAmplifierSampleHistoryList(db).Where(c => c.as_id == asId));
            }
        }
        //更新时间
        public void UpdateField(int id, string filed, DateTime? date)
        {
            BLL.AmplifierSampleManager.UpdateField(id, filed, date);
        }

        public ActionResult EditAsGrid(AmplifierSample afs)
        {
            var gridModel = GetGridAsModel();
            //修改或新增
            if (gridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || gridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                BLL.AmplifierSampleManager.Save(afs);
            }
            //删除
            else if (gridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {

            }
            return this.Redirect(Url.Action("Index"));

        }

        //放大样
        public void Sample(int id, string sam_name, DateTime? indication_date, string paper_type)
        {
            BLL.AmplifierSampleManager.Sample(id, sam_name, indication_date, paper_type);

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
                                             new JQGridColumn { DataField = "ht_id", 
                                                                PrimaryKey = true,
                                                         Searchable=false,
                                                                HeaderText="编号",
                                                                Width = 50 } ,
                                               new JQGridColumn { DataField = "pro_no", 
                                                       
                                                                HeaderText="番号",
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
                                                                   Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                  Searchable=false
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

        //获取二级 grid model
        private JQGrid GetGridAsModel()
        {

            return new JQGrid
            {
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = BLL.AppstringManager.GetPageSize
                },
                HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Child
                },
                Height = Unit.Percentage(100),
                Width = 740,
                DataUrl = Url.Action("GridAsData"),
                EditUrl = Url.Action("EditAsGrid"),
                //MultiSelect = true,
                ToolBarSettings = new ToolBarSettings()
                {

                    ShowEditButton = true,
                    ShowRefreshButton = true

                },

                EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true,
                    Width = 350
                },
                AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true,
                    Width = 350
                },

                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "as_id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
                                         {
                                                              new JQGridColumn{ DataField="is_next",Visible=false},
                                                                new JQGridColumn { DataField = "as_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Visible=false,
                                                                Width = 50 },

                                                               new JQGridColumn { DataField = "pro_no", 
                                                                Editable = false,
                                                                HeaderText="番号",
                                                                Searchable=false,
                                                                Visible=false,
                                                                
                                                               } ,
                                                      

                                                                    new JQGridColumn { DataField = "specifications", 
                                                                Editable = false,
                                                                HeaderText="规格",
                                                                Searchable=false,
                                                                 Visible=false
                                                                
                                                                } ,
                                                                   new JQGridColumn { DataField = "color_foreign", 
                                                                Editable = false,
                                                                HeaderText="色号",
                                                                Searchable=false,
                                                                
                                                                } ,
                                                                    new JQGridColumn { DataField = "color_name", 
                                                                Editable = false,
                                                                HeaderText="色名",
                                                                Searchable=false,
                                                                
                                                                } ,
                                                                                      new JQGridColumn { DataField = "binfan", 
                                                                   Editable = true,
                                                                DataType=typeof(string),
                                                                HeaderText="面料品番",
                                                                Searchable=false,
                                                                 
                                                                
                                                                } ,
                                                                             new JQGridColumn { DataField = "binname", 
                                                                     Editable = true,
                                                                DataType=typeof(string),
                                                                HeaderText="面料品名",
                                                                Searchable=false,
                                                               
                                                                
                                                                } ,
                                                                 new JQGridColumn { DataField = "Indicate_day", 
                                                                Editable = true,
                                                                HeaderText="大样交期",
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                             
                                                                  
                                                                    new JQGridColumn { DataField = "factory_date2", 
                                                                Editable = true,
                                                                HeaderText="工厂交期",
                                                                Searchable=false,
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                        Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                EditType=EditType.TextBox
                                                                
                                                                } ,
                                                                 new JQGridColumn { DataField = "actual_date", 
                                                                Editable = true,
                                                                HeaderText="实际交期",
                                                                 
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                new JQGridColumn { DataField = "m_num", 
                                                                Editable = true,
                                                                HeaderText="数量（M）",
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                        EditType=EditType.TextBox
                                                                } ,

                                                                new JQGridColumn { DataField = "finished_num", 
                                                                Editable = true,
                                                                HeaderText="样衣数量（枚）",
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                        EditType=EditType.TextBox
                                                                } ,
                                                                
                                                            
                                                                
                                                                          new JQGridColumn { DataField = "factory_num", 
                                                                Editable = true,
                                                                HeaderText="送货数量",
                                                                Searchable=false,
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                EditType=EditType.TextBox
                                                                
                                                                } ,
                                                                      new JQGridColumn { DataField = "hope_data", 
                                                                Editable = true,
                                                                HeaderText="送货日期",
                                                                Visible=false,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                       new JQGridColumn { DataField = "factory_date", 
                                                                Editable = true,
                                                                HeaderText="样衣寄出日",
                                                                Visible=false,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                new JQGridColumn { DataField = "warehouse_num", 
                                                                Editable = true,
                                                                HeaderText="在仓米数",
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                EditType=EditType.TextBox
                                                                
                                                                
                                                                },
                                                                new JQGridColumn { 
                                                                DataField = "remark", 
                                                                Editable = true,
                                                                HeaderText="备注",
                                                                EditType=EditType.TextArea,
                                                                Searchable=false
                                                                
                                                                } ,
                                                                    new JQGridColumn { DataField = "as_id", 
                                                       
                                                                Editable = false,
                                                                HeaderText="操作",
                                                                Formatter=new  CustomFormatter(){
                                                                  FormatFunction="childAction"
                                                                },
                                                                Width=200,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             }
                                         
                                                        }




            };
        }

        //获取三级 grid model
        private JQGrid GetAsHistoryGridModel()
        {

            return new JQGrid
            {
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = BLL.AppstringManager.GetPageSize
                },
                HierarchySettings = new HierarchySettings()
                {
                    //HierarchyMode = HierarchyMode.Child
                },
                Height = Unit.Percentage(100),
                Width = 740,
                DataUrl = Url.Action("GridAsHistoryData"),
                //EditUrl = Url.Action("EditAsGrid"),
                //MultiSelect = true,
                ToolBarSettings = new ToolBarSettings()
                {



                },

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
                                                        
                                                                   new JQGridColumn { DataField = "color_foreign", 
                                                                Editable = false,
                                                                HeaderText="色番",
                                                                Searchable=false,
                                                                
                                                                } ,
                                                                    new JQGridColumn { DataField = "color_name", 
                                                                Editable = false,
                                                                HeaderText="色名",
                                                                Searchable=false,
                                                                
                                                                } ,
                                                     
                                                                 new JQGridColumn { DataField = "Indicate_day", 
                                                                Editable = true,
                                                                HeaderText="大样交期",
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                             
                                                                  
                                                                    new JQGridColumn { DataField = "factory_date2", 
                                                                Editable = true,
                                                                HeaderText="工厂交期",
                                                                Searchable=false,
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                        Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                EditType=EditType.TextBox
                                                                
                                                                } ,
                                                                 new JQGridColumn { DataField = "actual_date", 
                                                                Editable = true,
                                                                HeaderText="实际交期",
                                                                 
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                new JQGridColumn { DataField = "m_num", 
                                                                Editable = true,
                                                                HeaderText="数量（M）",
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                        EditType=EditType.TextBox
                                                                } ,

                                                                new JQGridColumn { DataField = "finished_num", 
                                                                Editable = true,
                                                                HeaderText="样衣数量（枚）",
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                        EditType=EditType.TextBox
                                                                } ,
                                                                
                                                            
                                                                
                                                                          new JQGridColumn { DataField = "factory_num", 
                                                                Editable = true,
                                                                HeaderText="送货数量",
                                                                Searchable=false,
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                EditType=EditType.TextBox
                                                                
                                                                } ,
                                                                      new JQGridColumn { DataField = "hope_data", 
                                                                Editable = true,
                                                                HeaderText="送货日期",
                                                                Visible=false,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                       new JQGridColumn { DataField = "factory_date", 
                                                                Editable = true,
                                                                HeaderText="样衣寄出日",
                                                                Visible=false,
                                                                  EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                },
                                                                Searchable=false,
                                                                   EditType=EditType.TextBox
                                                                } ,
                                                                new JQGridColumn { DataField = "warehouse_num", 
                                                                Editable = true,
                                                                HeaderText="在仓米数",
                                                                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isDecimal"}},
                                                                Searchable=false,
                                                                EditType=EditType.TextBox
                                                                
                                                                
                                                                },
                                                                new JQGridColumn { 
                                                                DataField = "remark", 
                                                                Editable = true,
                                                                HeaderText="备注",
                                                                EditType=EditType.TextArea,
                                                                Searchable=false
                                                                
                                                                } ,
                                                                  new JQGridColumn { 
                                                                DataField = "create_time", 
                                                                Editable = true,
                                                                HeaderText="创建时间",
                                                                EditType=EditType.TextArea,
                                                                Formatter=new  CustomFormatter(){
                                                                 FormatFunction="ToDate"
                                                                },
                                                                Searchable=false
                                                                
                                                                } 
                                         
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
                    string fileName = AsposeExcel.MergeCellSavePdf(dt, "放大样总览.pdf", 0, 1, 2, 3, 19);
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
                    AsposeExcel.MergeCellExportToPdf(dt, "放大样总览.pdf", 0, 1, 2, 3, 19, 20);
                }
                else
                {
                    AsposeExcel.MergeCellExport(dt, "放大样总览.xls", 0, 1, 2, 3, 19, 20);
                }
            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {

            var dbList = BLL.HandmadeThingsManager.GetV_Ht_Htd_A_S_Sd(db).Where(c => c.status == 1);
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
            dt.Columns.Add("面料品番", typeof(string));
            dt.Columns.Add("面料品名", typeof(string));

            dt.Columns.Add("面料规格", typeof(string));
            dt.Columns.Add("色号", typeof(string));
            dt.Columns.Add("色名", typeof(string));

            dt.Columns.Add("大样交期", typeof(string));
            dt.Columns.Add("工厂交期", typeof(string));
            dt.Columns.Add("实际交期", typeof(string));
            dt.Columns.Add("数量（M)", typeof(string));
            dt.Columns.Add("样衣数量（枚）", typeof(string));
            dt.Columns.Add("货数量", typeof(string));
            dt.Columns.Add("送货日期", typeof(string));
            dt.Columns.Add("样衣寄出日", typeof(string));
            dt.Columns.Add("在仓米数", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            foreach (var r in data)
            {
                DataRow dr = dt.NewRow();
                dr["客户"] = traderList.Single(c => c.trader_id == r.trader_id).name;
                dr["面料品番"] = r.binfan;
                dr["面料品名"] = r.binname;
                dr["面料规格"] = r.specifications;
                dr["色号"] = r.color_foreign;
                dr["色名"] = r.color_name;

                dr["大样交期"] = r.htdIndicate_day.ToDateStr("yyyy-MM-dd");
                dr["工厂交期"] = r.afactory_date2.ToDateStr("yyyy-MM-dd");
                dr["实际交期"] = r.aactual_date.ToDateStr("yyyy-MM-dd");
                dr["数量（M)"] = r.am_num.ToMoney();
                dr["样衣数量（枚）"] = r.afinished_num;
                dr["货数量"] = r.afactory_num;
                dr["送货日期"] = r.ahope_data.ToDateStr("yyyy-MM-dd"); ;
                dr["样衣寄出日"] = r.afactory_date;
                dr["在仓米数"] = r.awarehouse_num;
                dr["备注"] = r.aremark;
                dt.Rows.Add(dr);
            }
            return dt;
        }


        //大样记录
        [OutputCache(Duration = 0)]
        public PartialViewResult AsHistory()
        {
            var model = GetAsHistoryGridModel();
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult ReAs(int asId)
        {

            var model = AmplifierSampleManager.GetAmplifierSampleById(asId);
            return PartialView(model);
        }

        //重新放大样
        [HttpPost]
        public void ReAs(AmplifierSampleHistory h)
        {
            AmplifierSampleManager.ReAs(h);

        }
    }
}
