using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL.Report;
using COM.Utility;
using COM.Extension;
namespace NFD.Areas.Report.Controllers
{
    public class OrderTotalController : Controller
    {
        //
        // GET: /Report/OrderTotal/

        public ActionResult Index()
        {
            var model = GetReportOrderGridModel;
            return View(model);
        }


        public JsonResult GetReportOrderGridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridData = ReportManager.GetV_Report_OrderList(db);
                var gridModel = GetReportOrderGridModel;
                return gridModel.DataBind(gridData);
            }
        }
        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetReportOrderGridModel
        {
            get
            {
                var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
                traderList.Insert(0, new SelectListItem());

                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridHeight;
                reval.Height = height;

                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowSearchToolBar = true,
                    CustomButtons = new List<JQGridToolBarButton>() { 
                     new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exExcel",
                         ButtonIcon="ui-icon-bookmark"
                      },
                    }
                };

                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "create_time desc",


                };

                reval.DataUrl = Url.Action("GetReportOrderGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列
                reval.Columns.Add(new JQGridColumn { Frozen = true, Searchable = true, DataField = "trader_id", HeaderText = "客户", SearchToolBarOperation = SearchOperation.IsEqualTo, DataType = typeof(int), SearchType = SearchType.DropDown, SearchList = traderList, Formatter = new CustomFormatter() { FormatFunction = "ToTrader" } });
                reval.Columns.Add(new JQGridColumn()
                {
                    Frozen = true,
                    DataField = "clothing_number",
                    Editable = false,
                    HeaderText = "款号",
                    DataType = typeof(string)

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "num",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "订单数量(套)",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_order_num",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "面料订货数（米）",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "consumption",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "单秏",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
            {
                DataField = "arrival_num",
                Editable = false,
                Searchable = false,
                HeaderText = "面料到货数(米)",
                Formatter = new CustomFormatter()
                {
                    FormatFunction = "ToRound"
                }

            });
     

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_price",
                    Searchable = false,
                    Editable = false,
                    HeaderText = "面料单价(元)",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_total_price",
                    Editable = false,
                    HeaderText = "面料金额(元)",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                     reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "actual_num_tol",
                    Editable = false,
                    HeaderText = "裁剪数(套)",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                     //reval.Columns.Add(new JQGridColumn()
                     //{
                     //    DataField = "actual_num_tol",
                     //    Editable = false,
                     //    HeaderText = "送检数(套)",
                     //    Searchable = false,
                     //    Formatter = new CustomFormatter()
                     //    {
                     //        FormatFunction = "ToRound"
                     //    }

                     //});
                

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "delivers_num",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "出货数（套）",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "bnum",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "B品数量",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "pricessing_fee",
                    Editable = false,
                    HeaderText = "加工费单价",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "tatol_pricessing_fee",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "服装加工费",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "reality_total_price",
                    Editable = false,
                    HeaderText = "成品金额（面料+加工费+辅料）",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });





                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "my_accesssor",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "辅料金额",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                //reval.Columns.Add(new JQGridColumn()
                //{
                //    DataField = "factory_accesssor",
                //    Editable = false,
                //    HeaderText = "服装厂辅料金额",
                //    Searchable = false,
                //    Formatter = new CustomFormatter()
                //    {
                //        FormatFunction = "ToRound"
                //    }

                //});
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "inspection_fee",
                    Editable=false,
                    HeaderText = "检品费(元)",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }
                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "contract_price",
                    Editable = false,
                    HeaderText = "合同单价（$）",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "contract_price_total",
                    Editable = false,
                    HeaderText = "金额($)",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_price_date",
                    Editable = false,
                    HeaderText = "收款日期",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToDate"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "rate",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "汇率",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "single_fee",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "快件费",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "wellhead_price",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "进口报关费(元)",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "export_price",
                    Searchable = false,
                    Editable = false,
                    HeaderText = "出口报关费（元）",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "ship_date",
                    Editable = false,
                    HeaderText = "出货日期",
                    Searchable = false,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToDate"
                    }

                });

                #endregion

                return reval;

            }
        }

        [OutputCache(Duration=0)]
        public void Export()
        {
            ReportManager.Export();

        }
    }
}
