using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.BLL;
using NFD.Entities.Data;
using NFD.BLL.Bill;
using System.Data;
using COM.Utility;
using COM.Extension;
namespace NFD.Areas.Bill.Controllers
{
    /// <summary>
    /// 辅料管理
    /// </summary>
    public class AccessoriesDetailController : Controller
    {

        public ActionResult Index()
        {
            var gridModel = GetAccessoriesDetailGridModel;
            return View(gridModel);
        }



        public JsonResult GetADGridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                var model = GetAccessoriesDetailGridModel;
                var gridData = AccessoriesDetailManager.GetV_AccessoriesDetail(db);
                return model.DataBind(gridData);
            }
        }


        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetAccessoriesDetailGridModel
        {
            get
            {
                List<SelectListItem> dateParamList = new List<SelectListItem>();
                dateParamList.Add(new SelectListItem { Text = "", Value = "" });
                dateParamList.Add(new SelectListItem { Text = "近3天", Value = DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd") });
                dateParamList.Add(new SelectListItem { Text = "近1个月", Value = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/dd") });
                dateParamList.Add(new SelectListItem { Text = "近3个月", Value = DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd") });
                dateParamList.Add(new SelectListItem { Text = "近半年", Value = DateTime.Now.AddMonths(-6).ToString("yyyy/MM/dd") });
                dateParamList.Add(new SelectListItem { Text = "近一年", Value = DateTime.Now.AddMonths(-12).ToString("yyyy/MM/dd") });
                var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
                traderList.Insert(0, new SelectListItem());
                //供应商
                var providerList = DictManager.GetProvider().Select(c => new SelectListItem() { Value = c.d_name + "", Text = c.d_name }).ToList();
                providerList.Insert(0, new SelectListItem());
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                reval.Height = 500;

                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "ad_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowSearchToolBar = true,
                    CustomButtons = new List<JQGridToolBarButton>() { 
                         new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exExcel"
                      } ,
                    new JQGridToolBarButton(){
                       OnClick="edit",
                       Text="编辑该定单",
                        ButtonIcon="ui-icon-pencil"
                      }
                      
                    }
                  
                };
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 850,
                    LeftOffset = 400

                };
                reval.EditDialogSettings = new EditDialogSettings()
                {
                    Width = 400,
                    CloseAfterEditing = true,
                    TopOffset = reval.AddDialogSettings.TopOffset,
                    LeftOffset = reval.AddDialogSettings.LeftOffset
                };
                reval.DataUrl = Url.Action("GetADGridData");

                #endregion
                #region 列
                reval.Columns = new List<JQGridColumn>();
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "ad_id",
                    PrimaryKey = true,
                    Editable = false,
                    HeaderText = "编号",
                    Visible = false

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_id",
                    PrimaryKey = true,
                    Editable = false,
                    Visible = false,
                    HeaderText = "编号"

                });
                reval.Columns.Add(new JQGridColumn { DataField = "trader_id", HeaderText = "客户", SearchToolBarOperation = SearchOperation.IsEqualTo, DataType = typeof(int), SearchType = SearchType.DropDown, SearchList = traderList, Formatter = new CustomFormatter() { FormatFunction = "ToTrader" } });
    
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "ad_name",
                    Editable = true,
                    HeaderText = "辅料名称",
                    DataType = typeof(string),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new RequiredValidator() }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "specifications",
                    Editable = true,
                    HeaderText = "规格",
                    DataType = typeof(string),

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "price",
                    Editable = true,
                    HeaderText = "单价",
                    DataType = typeof(string),
                    Formatter = new CustomFormatter() {
                        FormatFunction = "ToRound"
                    }

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "unit",
                    Editable = true,
                    HeaderText = "单位",
                    Searchable = false

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_num",
                    Editable = true,
                    HeaderText = "订单数",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    },
                    Searchable = false

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_num",
                    Editable = true,
                    HeaderText = "到货数量",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    },
                    Searchable = false

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "use_num",
                    Editable = true,
                    HeaderText = "使用数量",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    },
                    Searchable = false

                });

                //reval.Columns.Add(new JQGridColumn()
                //{
                //    DataField = "surplus_num",
                //    Editable = true,
                //    HeaderText = "应剩余",
                //    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                //     new  NumberValidator()
                //    }

                //});
                //reval.Columns.Add(new JQGridColumn()
                //{
                //    DataField = "price",
                //    Editable = true,
                //    HeaderText = "单价",
                //    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                //     new  NumberValidator()
                //    },
                //    Searchable = false,
                //    Formatter = new CustomFormatter()
                //    {
                //        FormatFunction = "ToRound"
                //    }

                //});
                reval.Columns.Add(new JQGridColumn()
             {
                 DataField = "tol_price",
                 Editable = true,
                 HeaderText = "辅料总价",
                 EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    },
                 Formatter = new CustomFormatter()
                 {
                     FormatFunction = "ToRound"
                 },
                 Searchable = false

             });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_date",
                    Editable = true,
                    HeaderText = "交货时间",
                    SearchToolBarOperation = SearchOperation.IsGreaterOrEqualTo,
                    SearchType = SearchType.DropDown,
                    SearchList = dateParamList,
                    Formatter=new  CustomFormatter(){
                     FormatFunction="ToDate"
                    },
                    DataType=typeof(DateTime),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    },
                    Searchable = true

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "supplier_name",
                    Editable = true,
                    HeaderText = "供应商",
                    EditList = providerList,
                    EditType = EditType.DropDown,
                    SearchList = providerList,
                    SearchType = SearchType.DropDown,
                    Searchable = true,
                    DataType = typeof(string)

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "creator_name",
                    Searchable = false,
                    Editable = false,
                    HeaderText = "创建人"


                });
                return reval;
            }
                #endregion

        }


        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="trader_id"></param>
        [OutputCache(Duration = 0)]
        public void ExportExcel(DateTime? dateMin, DateTime? dateMax, int trader_id = 0, int typeId = 0)/* 0、excel 1、pdf*/
        {
            using (NFDEntities db = new NFDEntities())
            {
                DataTable dt = GetExportDataTable(dateMin, dateMax, trader_id, db);
                AsposeExcel.MergeCellExport(dt, "新丝路国际贸易公司辅料单.xls");

            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {
            var dbList = AccessoriesDetailManager.GetV_AccessoriesDetail(db);
            if (trader_id > 0)
            {
                dbList = dbList.Where(c => c.trader_id == trader_id);
            }
            if (dateMin != null)
            {
                dbList = dbList.Where(c => c.create_time >= dateMin);
            }
            if (dateMax != null)
            {
                dateMax = ((DateTime)dateMax).AddDays(1);
                dbList = dbList.Where(c => c.create_time <= dateMax);
            }
            var data = dbList.OrderByDescending(c => c.ad_id).ToList();
            var traderList = BLL.TraderManager.GetList();
            DataTable dt = new DataTable();
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("辅料名称", typeof(string));
            dt.Columns.Add("规格", typeof(string));
            dt.Columns.Add("单价", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("订单数", typeof(string));
            dt.Columns.Add("到货数量", typeof(string));
            dt.Columns.Add("使用数量", typeof(string));
            dt.Columns.Add("辅料总价", typeof(string));
            dt.Columns.Add("交货时间", typeof(string));
            dt.Columns.Add("供应商", typeof(string));
            dt.Columns.Add("创建人", typeof(string));
            foreach (var r in data)
            {
                DataRow dr = dt.NewRow();
                dr["客户"] = traderList.Single(c => c.trader_id == r.trader_id).name;
                dr["辅料名称"]=r.ad_name;
                dr["规格"] = r.specifications;
                dr["单价"] = r.price.ToMoney();
                dr["单位"] = r.unit;
                dr["订单数"] = r.order_num;
                dr["到货数量"] = r.get_num;
                dr["使用数量"] = r.use_num;
                dr["辅料总价"] = r.tol_price.ToMoney();
                dr["供应商"] = r.supplier_name;
                dr["创建人"]=r.creator_name;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
