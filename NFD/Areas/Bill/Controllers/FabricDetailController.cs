using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL.Bill;
namespace NFD.Areas.Bill.Controllers
{
    /// <summary>
    /// 面料管理
    /// </summary>
    public class FabricDetailController : Controller
    {

        public ActionResult Index()
        {
            var gridModel = GetFabricDetailGridModel;
            return View(gridModel);
        }

        public JsonResult GetFDGridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridModel = GetFabricDetailGridModel;
                var gridData = FabricDetailManager.GetV_FabricDetailByOrderId(db);
                return gridModel.DataBind(gridData);
            }
        }

        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetFabricDetailGridModel
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
                       OnClick="edit",
                       Text="编辑该定单",
                        ButtonIcon="ui-icon-pencil"
                      }
                    }
                  
                };

                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "fd_id desc",


                };

                reval.DataUrl = Url.Action("GetFDGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列
                reval.Columns.Add( new JQGridColumn { DataField = "trader_id", HeaderText = "客户", Frozen = true, SearchToolBarOperation = SearchOperation.IsEqualTo, DataType = typeof(int), SearchType = SearchType.DropDown, SearchList = traderList, Formatter = new CustomFormatter() { FormatFunction = "ToTrader" } });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fd_id",
                    PrimaryKey = true,
                    Editable = false,
                    Visible=false,
                    HeaderText = "编号"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "clothing_number",
                    Editable = true,
                    HeaderText = "款号",
                    DataType=typeof(string),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "no",
                    Editable = true,
                    HeaderText = "面料番号",
                    DataType = typeof(string),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "color_name",
                    Editable = true,
                    HeaderText = "色号",
                    DataType = typeof(string),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_quantity",
                    Searchable = false,
                    HeaderText = "订货数量(米)",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new NumberValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "price",
                    Searchable = false,
                    HeaderText = "单价",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "tatol",
                    Searchable = false,
                    HeaderText = "小计",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_arrival",
                    Searchable = false,
                    HeaderText = "面料到货",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_add_reduction",
                    Searchable = false,
                    HeaderText = "面料增减",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "clothes_orders_num",
                    Searchable = false,
                    HeaderText = "成衣订单数",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "consumption",
                    Searchable = false,
                    HeaderText = "单耗",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "mf",
                    Searchable = false,
                    HeaderText = "有效门幅",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                             new NumberValidator()
                            },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_date",
                    Searchable = false,
                    HeaderText = "交货时间",
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToDate"
                    },
                    EditFieldAttributes = new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                },

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "creator_name",
                    Editable = false,
                    Searchable = false,
                    HeaderText = "创建人"



                });
                reval.Columns.Add(new JQGridColumn() { Visible = false, DataField = "order_id" });
                #endregion
                return reval;
            }

        }

    }
}
