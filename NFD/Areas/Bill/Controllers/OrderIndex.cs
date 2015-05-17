using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.BLL.Bill;
using NFD.Entities.Data;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;
using COM.Extension;
using NFD.Entities.Common;
namespace NFD.Areas.Bill.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    public partial class OrderController : Controller
    {


        public ActionResult Index(int chanceId = 0)
        {
            var model = GetGridModel();
            if (chanceId > 0)
            {
                model.DataUrl = model.DataUrl + "?chanceId=" + chanceId;
            }
            return View(model);
        }

        public ActionResult CreateCut(int id)
        {
            OrderBillManager.CreateCutBill(id);
            return this.Redirect("/Bill/Cut/Index");
        }

        //获取统计信息
        public PartialViewResult _GetTolInfo(OrderBill bill)
        {
            var model = OrderBillManager.GetTolByPars(bill);
            return PartialView(model);
        }

        public JsonResult GridOrderData(int chanceId = 0)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                TempData["chanceId"] = TempData["chanceId"];
                var model = GetGridModel();
                var list = OrderBillManager.GetOrderList(db);
                if (chanceId > 0)
                {
                    list = list.Where(c => c.chance_id == chanceId);
                }
                return model.DataBind(list);
            }
        }

        /// <summary>
        /// 获取一级 grid model
        /// </summary>
        /// <returns></returns>
        private JQGrid GetGridModel()
        {
            var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
            traderList.Insert(0, new SelectListItem());
            var factoryList = BLL.DictManager.GetFactory().Select(c => new SelectListItem() { Value = c.dd_id + "", Text = c.d_name }).ToList();
            factoryList.Insert(0, new SelectListItem());
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
                        OnClick="sendMail",
                               ButtonIcon="ui-icon-mail-open"
                      },
                        new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exExcel",
                         ButtonIcon="ui-icon-bookmark"
                      },
                        new JQGridToolBarButton(){
                       Text="打印PDF",
                        OnClick="exPdf",
                         ButtonIcon="ui-icon-print"
                      },
                      new JQGridToolBarButton(){
                        Text="删除",
                         OnClick="openDel",
                          ButtonIcon="ui-icon-trash"
                      },

                        new JQGridToolBarButton(){
                       Text="编辑查看",
                        OnClick="openEditBySelected",
                         ButtonIcon="ui-icon-pencil"
                      },new JQGridToolBarButton(){
                       Text="添加定单",
                        OnClick="addOrderBill",
                         ButtonIcon="ui-icon-plus"
                      },

                    }
                },
                HierarchySettings = new HierarchySettings()
                {

                    ReloadOnExpand = false,
                    SelectOnExpand = false,
                    ExpandOnLoad = false
                },
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = 10
                },
                Height = BLL.AppstringManager.GetGridHeight,
                DataUrl = Url.Action("GridOrderData"),

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
                    InitialSortColumn = "o_id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "o_id", 
                                                                PrimaryKey = true,
                                                                Frozen = true,
                                                         Searchable=false,
                                                                HeaderText="编号",
                                                                Width = 30 } ,
                                                                 
                                                                  new JQGridColumn { DataField="trader_id", Width=90,HeaderText="客户",Frozen = true,SearchToolBarOperation=SearchOperation.IsEqualTo,DataType=typeof(int),SearchType=SearchType.DropDown,SearchList=traderList,Formatter= new  CustomFormatter(){FormatFunction="ToTrader"} },
                                               
                                                                      new JQGridColumn { DataField = "factory_name", 
                                                       
                                                                HeaderText="工厂名",
                                                                Frozen = true,
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToFactoryName"
                                                                 },
                                                                  
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.DropDown,
                                                                SearchList=factoryList
                                                            
                                                          

                                             },
                                                                  new JQGridColumn { DataField = "clothing_number", 
                                                       
                                                                HeaderText="款号",
                                                                Frozen = true,
                                                                Width=150,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox
                                                            
                                                          

                                             },
                                         
                                               new JQGridColumn { DataField = "no", 
                                                       
                                                                HeaderText="面料品番",
                                                                Width=150,
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
                                                                DataType=typeof(int),
                                                                Visible=false
                                                               
                                                            
                                                          

                                             } 
                                                      ,
                                               new JQGridColumn { DataField = "styles", 
                                                       
                                                                HeaderText="面料款号",
                                                                Width=150,
                                                                DataType=typeof(string),
                                                               Searchable=false,
                                                            
                                                          

                                              } ,
                                                      new JQGridColumn { DataField = "num", 
                                                       
                                                                HeaderText="数量",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,

                                                            
                                                          

                                              } 
                                              ,
                                                 new JQGridColumn { DataField = "cost_price", 
                                                       
                                                                HeaderText="合同价",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Visible=false
                                                            
                                                          

                                              } 
                                              ,
                                                         new JQGridColumn { DataField = "market_price", 
                                                       
                                                                HeaderText="市场价",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Visible=false
                                                            
                                                          

                                              } 
                                              ,
                                                        new JQGridColumn { DataField = "custome_price", 
                                                       
                                                                HeaderText="客户价",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Visible=false
                                                            
                                                          

                                              } 
                                              ,
                                                           new JQGridColumn { DataField = "o_id", 
                                                       
                                                                HeaderText="合同价",
                                                                Width=80,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Formatter=new CustomFormatter(){
                                                                  FormatFunction="singlePrice"
                                                                 } 
                                                            
                                                          

                                              },
                                             
                                                                  new JQGridColumn { DataField = "rate", 
                                                       
                                                                HeaderText="USD汇率",
                                                                Width=50,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToRound"
                                                                 } 
                                                            
                                                          

                                              },
                                            
                                              
                                                      new JQGridColumn { DataField = "order_date", 
                                                       
                                                                HeaderText="面料下单日",
                                                                Width=100,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                  Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToDate"
                                                                 }
                                                            
                                                          

                                             } ,
                                              
                                                      new JQGridColumn { DataField = "get_date", 
                                                       
                                                                HeaderText="面料交期日",
                                                                Width=100,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                  Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToDate"
                                                                 }
                                                            
                                                          

                                             } 
                                                ,
                                                              new JQGridColumn { DataField = "submission_date", 
                                                       
                                                                HeaderText="送检日",
                                                                Width=100,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                  Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToDate"
                                                                 }
                                                            
                                                          

                                             } ,
                                                                  new JQGridColumn { DataField = "creator_name", 
                                                       
                                                                HeaderText="创建人",
                                                                Width=100,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,

                                                            
                                                          

                                             } ,
                                                      new JQGridColumn { DataField = "create_time", 
                                                       
                                                                HeaderText="创建时间",
                                                                Width=100,
                                                                DataType=typeof(string),
                                                                SearchToolBarOperation=SearchOperation.Contains,
                                                                SearchType=SearchType.TextBox,
                                                                 Searchable=false,
                                                                 Formatter=new CustomFormatter(){
                                                                  FormatFunction="ToDate"
                                                                 }
                                              
                                                  
                                                          

                                             } 
                                             , new JQGridColumn{
                                             
                                              DataField="is_cut",
                                               HeaderText="操作",
                                               Searchable=false,
                                               Width=250,
                                               Formatter=new CustomFormatter(){
                                                FormatFunction="action"
                                               }
                                             }
                                             


                                             

                                             
               },



                Width = Unit.Pixel(640)
            };
        }

        //删除订单
        public void DelOrder(int id)
        {
            OrderBillManager.DelOrder(id);
        }

        //导出报价单
        public void Export(DateTime? dateMin, DateTime? dateMax, int trader_id = 0, int typeId = 0/*0为excel 1为pdf*/, int priceType = 1, int dd_id = 0)
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
            OrderBillManager.Export(trader_id, bt, et, priceType, typeId, dd_id);

        }

        //发送邮件
        public JsonResult SendMail(Mail mail, DateTime? dateMin, DateTime? dateMax, bool? isAtta, int trader_id = 0, int priceType = 1, int dd_id = 0)
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
                    string fileName = OrderBillManager.SaveExport(trader_id, bt, et, priceType, 1, dd_id);
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
