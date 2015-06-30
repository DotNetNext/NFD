using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
using NFD.BLL.Bill;
using NFD.BLL;
using NFD.Entities.Common;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.Objects.SqlClient;
namespace NFD.Areas.Bill.Controllers
{
    public partial class ChanceController : Controller
    {

        public ActionResult Index()
        {
            Tuple<JQGrid, JQGrid> model = Tuple.Create(GetChanceGridModel(), GetChanceDetailGridModel());
            return View(model);
        }
        //报价单
        public JsonResult GridChanceData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid model = GetChanceGridModel();
                return model.DataBind(ChanceBillManager.GetV_ChanceList(db).Select(c => new
                {
                    clothing_number = c.clothing_number,
                    bill_id = c.bill_id,
                    create_time = c.create_time,
                    creator_id = c.creator_id,
                    creator_name = c.creator_name,
                    cost_price = c.cost_price,
                    market_price = c.market_price,
                    custome_price = c.custome_price,
                    singlePrice = (SqlFunctions.StringConvert(c.cost_price == null ? 0 : c.cost_price, 10, 2).Trim()),
                    ht_id = c.ht_id,
                    ht_no = c.ht_no,
                    modified_time = c.modified_time,
                    is_del = c.is_del,
                    door = c.door,
                    pricessing_fee = c.pricessing_fee,
                    paper = c.paper,
                    ht_specifications = c.ht_specifications,
                    num = SqlFunctions.StringConvert(c.num == null ? 0 : c.num, 10, 2),
                    postage = SqlFunctions.StringConvert(c.postage == null ? 0 : c.postage, 10, 2),
                    tolcostPrice = c.cost_price * c.num,
                    tolMarketPrice = c.market_price * c.num,
                    tolCustomePrice = c.market_price * c.num,
                    trader_id = c.trader_id,
                    smallTol = (SqlFunctions.StringConvert(c.cost_price == null ? 0 : c.cost_price * c.num, 10, 2).Trim() ),
                    allTol = (SqlFunctions.StringConvert(+(c.detailCostPrice), 10, 2).Trim()),
                    usdTol = (SqlFunctions.StringConvert(+(c.detailCostPrice / c.rate), 10, 2).Trim()),
                    status = c.status
                }));
            }
        }

        public string DelBill(int id)
        {
            ChanceBillManager.DelBill(id);
            return "删除成功！";

        }

        //报价单明细grid数据 二级
        public JsonResult GridChanceDetailData(string parentRowID)
        {
            using (NFDEntities db = new NFDEntities())
            {
                int cbd_id = parentRowID.ToInt();
                JQGrid model = GetChanceDetailGridModel();
                return model.DataBind(BLL.Bill.ChanceBillManager.GetChanceBillDetailListByBillId(cbd_id, db).Select(c => new
                {
                    billd_id = c.billd_id,
                    name = c.name,
                    num = SqlFunctions.StringConvert(c.num, 10, 2).Trim(),
                    cost_price = SqlFunctions.StringConvert(c.cost_price, 10, 2).Trim(),
                    //market_price = SqlFunctions.StringConvert(c.market_price, 10, 2).Trim(),
                    //custome_price = SqlFunctions.StringConvert(c.custome_price, 10, 2).Trim(),
                    tolcostPrice = SqlFunctions.StringConvert(c.cost_price * c.num, 10, 2).Trim(),
                    //tolMarketPrice = SqlFunctions.StringConvert(c.market_price * c.num, 10, 2).Trim(),
                    //tolCustomePrice = SqlFunctions.StringConvert(c.market_price * c.num, 10, 2).Trim(),
                    tol = ( SqlFunctions.StringConvert(c.cost_price == null ? 0 : c.cost_price * c.num, 10, 2).Trim() )


                }));
            }
        }

        //统一字段
        public void UpdateField(int id, string filed, DateTime? date)
        {
            BLL.HandmadeThingsDetailManager.UpdateField(id, filed, date);

        }
        public ActionResult EditChanceDetailGrid(ChanceBillDetail bill)
        {


            var gridModel = GetGridModel();
            if (gridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || gridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                ChanceBillManager.SaveChanceBillDetail(bill);
            }
            //删除
            else if (gridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                ChanceBillManager.DeleteChanceBillDetail(bill.billd_id);
            }
            return this.Redirect(Url.Action("Save"));
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
        private JQGrid GetChanceGridModel()
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
                       Text="发送邮件",
                        OnClick="sendMail"
                      },
                        new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exExcel"
                      },
                        new JQGridToolBarButton(){
                       Text="打印PDF",
                        OnClick="exPdf"
                      },new JQGridToolBarButton(){
                       Text="添加报价",
                        OnClick="addChanceBill"
                      },
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
                DataUrl = Url.Action("GridChanceData"),

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
                    InitialSortColumn = "bill_id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "bill_id", 
                                                                PrimaryKey = true,
                                                         Searchable=false,
                                                                HeaderText="编号",
                                                                Width = 30 } ,
                                                                  new JQGridColumn { DataField="trader_id", Width=50,HeaderText="客户",Frozen = true,SearchToolBarOperation=SearchOperation.IsEqualTo,DataType=typeof(int),SearchType=SearchType.DropDown,SearchList=traderList,Formatter= new  CustomFormatter(){FormatFunction="ToTrader"} },
                                               new JQGridColumn { DataField = "clothing_number", 
                                                       
                                                                HeaderText="款号",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                               new JQGridColumn { DataField = "ht_specifications", 
                                                       
                                                                HeaderText="规格",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             } 
                                             ,
                                               new JQGridColumn { DataField = "num", 
                                                       
                                                                HeaderText="用料",
                                                                Width=50,
                                                              Searchable=false,
                                                                DataType=typeof(string),
                                                              
                                                            
                                                          

                                             } 
                                                      ,
                                               new JQGridColumn { DataField = "singlePrice", 
                                                       
                                                                HeaderText="单价",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                               Searchable=false,
                                                            
                                                          

                                              } ,
                                                      new JQGridColumn { DataField = "smallTol", 
                                                       
                                                                HeaderText="小计（单价 × 用料）",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                              } 
                                              ,

                                              
                                                      new JQGridColumn { DataField = "pricessing_fee", 
                                                       
                                                                HeaderText="加工费",
                                                                Width=50,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             } ,
                                              
                                                      new JQGridColumn { DataField = "postage", 
                                                       
                                                                HeaderText=" 通关费",
                                                                Width=50,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             } 
                                                ,
                                                              new JQGridColumn { DataField = "paper", 
                                                       
                                                                HeaderText="纸型",
                                                                Width=50,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             } ,
                                                                  new JQGridColumn { DataField = "door", 
                                                       
                                                                HeaderText="面料门幅",
                                                                Width=50,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             } ,
                                                      new JQGridColumn { DataField = "allTol", 
                                                       
                                                                HeaderText="总计(运费+辅料+小计+加工费)",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             } ,
                                                    new JQGridColumn { DataField = "usdTol", 
                                                       
                                                                HeaderText="USD总计",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                            
                                                          

                                             }
                                             ,
                                                    new JQGridColumn { DataField = "bill_id", 
                                                       
                                                                HeaderText="操作",
                                                                Width=120,
                                                                DataType=typeof(string),
                                                                Searchable=false,
                                                                Formatter=new CustomFormatter(){
                                                                   FormatFunction="action"
                                                                }
                                                            
                                                          

                                             },
                                             new JQGridColumn{
                                             DataField="status",
                                              Visible=false
                                              
                                             }
                                             


                                             

                                             
               },



                Width = Unit.Pixel(640)
            };
        }

        /// <summary>
        /// 获取二级grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetChanceDetailGridModel()
        {

            return new JQGrid
            {
                Height = Unit.Percentage(100),
                DataUrl = Url.Action("GridChanceDetailData"),
                EditUrl = Url.Action("EditChanceDetailGrid"),
                HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Child
                },
                SortSettings = new SortSettings()
                {
                    InitialSortColumn = "billd_id"

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
                                             new JQGridColumn { DataField = "billd_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Width = 50 } ,
                                            

                                               new JQGridColumn { DataField = "name", 
                                                                Editable = true,
                                                                HeaderText="辅料",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                                    new JQGridColumn { DataField = "num", 
                                                                Editable = true,
                                                                HeaderText="用量",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator(),
                                                                     new NumberValidator()

                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                                    new JQGridColumn { DataField = "cost_price", 
                                                                Editable = true,
                                                                HeaderText="单价",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator(),
                                                                        new NumberValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                             //       new JQGridColumn { DataField = "market_price", 
                                             //                   Editable = true,
                                             //                   HeaderText="市场价",
                                             //                   Width=100,
                                             //                   Searchable=true,
                                             //                       EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                             //                        new RequiredValidator(),
                                             //                           new NumberValidator()
                                             //                    },
                                             //                    DataType=typeof(string)
                                                          

                                             //},
                                             //       new JQGridColumn { DataField = "custome_price", 
                                             //                   Editable = true,
                                             //                   HeaderText="客户价",
                                             //                   Width=100,
                                             //                   Searchable=true,
                                             //                       EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                             //                        new RequiredValidator(),
                                             //                           new NumberValidator()
                                             //                    },
                                             //                    DataType=typeof(string)
                                                          

                                             //},
                                                    new JQGridColumn { DataField = "tol", 
                                                                Editable = false,
                                                                HeaderText="合计(单价 × 用料)",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             }

                                             
                                             


                                             
               },



                Width = Unit.Pixel(640)
            };
        }

        //导出报价单
        public void Export(DateTime? dateMin, DateTime? dateMax, int trader_id = 0, int typeId = 0/*0为excel 1为pdf*/, int priceType = 1)
        {
            DateTime bt;
            DateTime et;
            if (dateMin == null)
            {
                bt = DateTime.Now.AddYears(-100);
            }
            else
            {
                bt = (DateTime)dateMin;
            }

            if (dateMax == null)
            {
                et = DateTime.Now.AddDays(1);
            }
            else
            {
                et = ((DateTime)dateMax).AddDays(1);
            }
            ChanceBillExportManager.Export(trader_id, bt, et, priceType, typeId);
        }

        //发送邮件
        public JsonResult SendMail(Mail mail, DateTime? dateMin, DateTime? dateMax, bool? isAtta, int trader_id = 0, int priceType = 1)
        {

            DateTime bt;
            DateTime et;
            if (dateMin == null)
            {
                bt = DateTime.Now.AddYears(-100);
            }
            else
            {
                bt = (DateTime)dateMin;
            }

            if (dateMax == null)
            {
                et = DateTime.Now.AddDays(1);
            }
            else
            {
                et = ((DateTime)dateMax).AddDays(1);
            }
            using (NFDEntities db = new NFDEntities())
            {
                COM.Utility.SendMail sm = new COM.Utility.SendMail(BLL.AppstringManager.GetMailSmtp, BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailPassword);

                var reJson = new ReJson();
                if (isAtta == true)
                {
                    string fileName = ChanceBillExportManager.SaveExport(trader_id, bt, et, priceType, 1);
                    reJson.IsSuccess = sm.Send(BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailName, mail.MailName, mail.Title, mail.Content, fileName);
                }
                else
                {
                    reJson.IsSuccess = sm.Send(BLL.AppstringManager.GetMailUserName, BLL.AppstringManager.GetMailName, mail.MailName, mail.Title, mail.Content);
                }
                return Json(reJson);
            }
        }
    }
}
