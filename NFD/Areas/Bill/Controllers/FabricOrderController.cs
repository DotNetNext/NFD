using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL;
using NFD.BLL.Bill;
using COM.Utility;
using COM.Extension;
using System.Data;
namespace NFD.Areas.Bill.Controllers
{
    public class FabricOrderController : Controller
    {
        /// <summary>
        /// 面料订物单
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            var model = GetFabricOrderGridModel;
            model.DataUrl += "?id=0";
            return View(model);
        }

        public JsonResult GetFabricOrderGridData(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridDataList = FabricOrderBillManager.GetFabricOrderBillList(db, id);
                var gridModel = GetFabricOrderGridModel;
                return gridModel.DataBind(gridDataList);
            }
        }

        public ActionResult EditFabricOrderGridData(FabricOrderBill fd)
        {
            if (GetFabricOrderGridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                FabricOrderBillManager.DelBill(fd.fob_id);
            }
            else
            {
                if (fd.fob_id > 0)
                {
                    FabricOrderBillManager.SaveFabricOrderBillAll(fd);
                }
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 获取面料订购
        /// </summary>
        public JQGrid GetFabricOrderGridModel
        {
            get
            {
                //供应商
                var providerList = DictManager.GetProvider().Select(c => new SelectListItem() { Value = c.dd_id + "", Text = c.d_name }).ToList();
                var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
                traderList.Insert(0, new SelectListItem());
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridWindowHeight;
                reval.Height = 600;
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,


                };
                reval.EditDialogSettings = new EditDialogSettings()
                {
                    Width = 400,
                    CloseAfterEditing = true,
                    TopOffset = reval.AddDialogSettings.TopOffset,
                    LeftOffset = reval.AddDialogSettings.LeftOffset
                };
                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "fob_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = false,
                    //ShowAddButton = true,
                    //ShowDeleteButton = true
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
                reval.DataUrl = Url.Action("GetFabricOrderGridData");
                reval.EditUrl = Url.Action("EditFabricOrderGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列


                reval.Columns.Add(new JQGridColumn { DataField = "trader_id", HeaderText = "客户", Frozen = true, SearchToolBarOperation = SearchOperation.IsEqualTo, DataType = typeof(int), SearchType = SearchType.DropDown, SearchList = traderList, Formatter = new CustomFormatter() { FormatFunction = "ToTrader" } });
                reval.Columns.Add(new JQGridColumn()
                {
                    Frozen = true,
                    DataField = "no",
                    Editable = true,
                    HeaderText = "面料品番",
                    Searchable = true,
                    SearchType = SearchType.TextBox,
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                                     new  RequiredValidator()
                                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "specifications",
                    Editable = true,
                    HeaderText = "规格",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "price",
                    Editable = true,
                    HeaderText = "面料单价(元)",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator(),
                     new NumberValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "sdf",
                    Editable = true,
                    HeaderText = "生地幅(厘米)"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "element",
                    Editable = true,
                    HeaderText = "成份"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "color_foreign",
                    Editable = true,
                    HeaderText = "色号"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "color_name",
                    Editable = true,
                    HeaderText = "色名"



                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "num",
                    Editable = true,
                    HeaderText = "面料数量",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator(),
                     new NumberValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound()"
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_date",
                    Editable = true,
                    HeaderText = "面料厂交期",
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
                    DataField = "area",
                    Editable = true,
                    HeaderText = "出货范围"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "datum",
                    Editable = true,
                    HeaderText = "面料测试基准"



                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "supplier_id",
                    Editable = true,
                    HeaderText = "供应商",
                    EditList = providerList,
                    EditType = EditType.DropDown,
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToSupplier"
                    }


                });



                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "address",
                    Editable = true,
                    HeaderText = "送货地址"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "remark",
                    Editable = true,
                    HeaderText = "备注"



                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "creator_name",
                    Editable = false,
                    HeaderText = "创建人"



                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fob_id",
                    PrimaryKey = true,
                    Editable = false,
                    Visible = true,
                    HeaderText = "编号",
                    Frozen = true

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_id",
                    Visible = false
                });
                #endregion
                return reval;
            }

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
                AsposeExcel.MergeCellExport(dt, "新丝路国际贸易公司面料订购单.xls");

            }
        }

        private static DataTable GetExportDataTable(DateTime? dateMin, DateTime? dateMax, int trader_id, NFDEntities db)
        {
            var dbList = FabricOrderBillManager.GetFabricOrderBillList(db);
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
            var data = dbList.OrderByDescending(c => c.fob_id).ToList();
            var traderList = BLL.TraderManager.GetList();
            DataTable dt = new DataTable();
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("面料品番", typeof(string));
            dt.Columns.Add("规格", typeof(string));
            dt.Columns.Add("面料单价(元)", typeof(string));
            dt.Columns.Add("生地幅(厘米)", typeof(string));
            dt.Columns.Add("成份", typeof(string));
            dt.Columns.Add("色号", typeof(string));
            dt.Columns.Add("色名", typeof(string));
            dt.Columns.Add("面料数量(米)", typeof(string));
            dt.Columns.Add("面料厂交期", typeof(string));
            dt.Columns.Add("出货范围", typeof(string));
            dt.Columns.Add("面料测试基准", typeof(string));
            dt.Columns.Add("供应商", typeof(string));
            dt.Columns.Add("送货地址", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            dt.Columns.Add("创建人", typeof(string));
            foreach (var r in data)
            {
                DataRow dr = dt.NewRow();
                dr["客户"] = traderList.Single(c => c.trader_id == r.trader_id).name;
                dr["面料品番"] = r.no;
                dr["规格"] = r.specifications;
                dr["面料单价(元)"] = r.price.ToMoney();
                dr["生地幅(厘米)"] = r.sdf; ;
                dr["成份"] = r.element;
                dr["色号"] = r.color_foreign;
                dr["色名"] = r.color_name;
                dr["面料数量(米)"] = r.num;
                dr["面料厂交期"] = r.get_date.ToDateStr("yyyy-MM-dd");
                dr["出货范围"] = r.area;
                dr["面料测试基准"] = r.datum;
                dr["供应商"] = r.supplier_id;
                dr["送货地址"] = r.address;
                dr["备注"] = r.remark;
                dr["创建人"] = r.creator_name;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
