using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.BLL.Bill;
using NFD.Entities.Data;
using COM.Extension;
using Trirand.Web.Mvc;
using COM.Utility;
using NFD.BLL;
using System.Data;
namespace NFD.Areas.Bill.Controllers
{
    public partial class OrderController : Controller
    {


        public ActionResult Save(int id = 0, int chanceId = 0, int htId = 0)
        {

            var model = new OrderBill();
            if (id > 0)
            {
                model = OrderBillManager.GetOrderById(id);
            }
            else if (chanceId > 0)
            {
                model.chance_id = chanceId;
                var chance = ChanceBillManager.GetV_ChanceBillById(chanceId);
                model.cost_price = chance.detailCostPrice;
                model.market_price = chance.market_price;
                model.custome_price = chance.custome_price;
                model.trader_id = chance.trader_id;
                model.rate = chance.rate;
                model.ht_id = chance.ht_id;
                if (chance.ht_id > 0)
                {
                    var ht = BLL.HandmadeThingsManager.GetSingle(model.ht_id.ToInt());
                    model.ht_id = ht.ht_id;
                    model.no = ht.pro_no;
                    model.trader_id = ht.trader_id;
                }
            }
            else if (htId > 0)
            {
                var ht = BLL.HandmadeThingsManager.GetSingle(htId);
                model.ht_id = ht.ht_id;
                model.no = ht.pro_no;
                model.trader_id = ht.trader_id;
            }
            if (id > 0)
            {
                var fdGrid = GetFabricDetailGridModel;
                fdGrid.DataUrl += "?id=" + id;
                ViewBag.fdGrid = fdGrid;

                var adGrid = GetAccessoriesDetailGridModel;
                adGrid.DataUrl += "?id=" + id;
                ViewBag.adGrid = adGrid;

                var fdoGrid = GetFabricOrderGridModel;
                fdoGrid.DataUrl += "?id=" + id;
                ViewBag.fdoGrid = fdoGrid;

                var ppGrid = GetProductionPlanGridModel;
                ppGrid.DataUrl += "?id=" + id;
                ViewBag.ppGrid = ppGrid;
            }



            return View(model);
        }



        [HttpPost]
        public ActionResult Save(OrderBill bill)
        {
            OrderBillManager.SaveOrderBill(bill);
            return Redirect(Url.Action("Save") + "?id=" + bill.o_id);
        }


        #region 面料
        public JsonResult GetFDGridData(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridModel = GetFabricDetailGridModel;
                var modelData = FabricDetailManager.GetFabricDetailByOrderId(id, db);
                return gridModel.DataBind(modelData);
            }
        }

        public ActionResult EditFDGridData(FabricDetail fd)
        {
            if (GetFabricDetailGridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                FabricDetailManager.Del(fd.fd_id);
            }
            else
            {
                var orderId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id").ToInt();
                if (orderId > 0)
                {
                    fd.order_id = orderId;
                    FabricDetailManager.Save(fd);
                }
            }
            return RedirectToAction("Save");
        }

        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetFabricDetailGridModel
        {
            get
            {
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridWindowHeight;
                reval.Height = height;
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 1100,
                    LeftOffset = 400

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
                    InitialSortColumn = "fd_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowAddButton = true,
                    ShowDeleteButton = true
                };
                reval.DataUrl = Url.Action("GetFDGridData");
                reval.EditUrl = Url.Action("EditFDGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列
                reval.Columns.Add(new JQGridColumn()
                       {
                           DataField = "fd_id",
                           PrimaryKey = true,
                           Editable = false,
                           HeaderText = "编号"

                       });
                reval.Columns.Add(new JQGridColumn()
                    {
                        DataField = "color_foreign",
                        Editable = true,
                        HeaderText = "品番",
                        EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                                     new  RequiredValidator()
                                    }

                    });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "color_name",
                    Editable = true,
                    HeaderText = "色号",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  RequiredValidator()
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_quantity",
                    Editable = true,
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
                    Editable = true,
                    HeaderText = "单价(元)",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new NumberValidator(),
                     new RequiredValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                //reval.Columns.Add(new JQGridColumn()
                // {
                //     DataField = "subtotal",
                //     Editable = true,
                //     HeaderText = "小计",
                //     EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                //             new NumberValidator()
                //            },
                //     Formatter = new CustomFormatter()
                //     {
                //         FormatFunction = "ToRound"
                //     }

                // });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fabric_arrival",
                    Editable = true,
                    HeaderText = "面料到货（米）",
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
                    Editable = true,
                    HeaderText = "面料增减（米）",
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
                    Editable = true,
                    HeaderText = "成衣订单数（套）",
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
                    Editable = true,
                    HeaderText = "单耗（米）",
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
                    Editable = true,
                    HeaderText = "有效门幅（厘米）",
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
                    Editable = true,
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
                    HeaderText = "创建人"



                });
                #endregion
                return reval;
            }

        }
        #endregion


        #region 辅料
        public void ExportADExcel(int id=0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var adGrid = GetAccessoriesDetailGridModel;
                adGrid.ExportSettings.ExportDataRange = ExportDataRange.FilteredAndPaged;
                var modelData = AccessoriesDetailManager.GetV_AccessoriesDetail(db).Where(c=>c.order_id==id);
                adGrid.ExportToExcel(modelData, " 辅料.xls", dt => {
                    DataTable newDt = new DataTable();
                    PubMethod.CopyDataTable(dt,newDt);
                    var dr = newDt.NewRow();
                    dr["clothing_number"] = "合计:";
                    dr["tol_price"] = newDt.AsEnumerable().Select(c => Convert.ToDecimal(c["tol_price"])).Sum().ToMoneyString();
                    newDt.Rows.Add(dr);
                    return newDt;
                });
            }
        }

        public JsonResult GetADGridData(int id = 0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridModel = GetAccessoriesDetailGridModel;
                var modelData = AccessoriesDetailManager.GetV_AccessoriesDetail(db).Where(c => c.order_id == id);
                return gridModel.DataBind(modelData);
            }
        }

        public ActionResult EditADGridData(AccessoriesDetail ad)
        {
            if (GetAccessoriesDetailGridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                AccessoriesDetailManager.Del(ad.ad_id);
            }
            else
            {
                var orderId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id").ToInt();
                if (orderId > 0)
                {
                    ad.order_id = orderId;
                    AccessoriesDetailManager.Save(ad);
                }
            }
            return RedirectToAction("Save");
        }

        /// <summary>
        /// 获取面料详情
        /// </summary>
        public JQGrid GetAccessoriesDetailGridModel
        {
            get
            {
                //供应商
                var providerList = DictManager.GetProvider().Select(c => new SelectListItem() { Value = c.dd_id + "", Text = c.d_name }).ToList();
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                reval.Height = 400;

                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "ad_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    CustomButtons = new List<JQGridToolBarButton>() { 
                         new JQGridToolBarButton(){
                       Text="导出EXCEL",
                        OnClick="exportAdExcel"
                      } 
                    },
                    ShowEditButton = true,
                    ShowAddButton = true,
                    ShowDeleteButton = true
                    
                };
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 1450,
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
                reval.EditUrl = Url.Action("EditADGridData");
                #endregion
                #region 列
                reval.Columns = new List<JQGridColumn>();
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "ad_id",
                    PrimaryKey = true,
                    Editable = false,
                    HeaderText = "编号",
                    Visible=false,

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "clothing_number",
                     Visible=true,
                     HeaderText="款号"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "ad_name",
                    Editable = true,
                    HeaderText = "名称",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new RequiredValidator() }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "supplier_name",
                    Editable = true,
                    HeaderText = "供应商",
                    EditList = providerList,
                    EditType = EditType.DropDown

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "specifications",
                    Editable = true,
                    HeaderText = "规格"

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "unit",
                    Editable = true,
                    HeaderText = "单位"

                });

                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "order_num",
                    Editable = true,
                    HeaderText = "订单数",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "get_num",
                    Editable = true,
                    HeaderText = "到货数量",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    }

                });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "use_num",
                    Editable = true,
                    HeaderText = "使用数量",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator()
                    }

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
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "price",
                    Editable = true,
                    HeaderText = "单价(元)",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator(),
                     new RequiredValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                             reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "tol_price",
                    Editable = true,
                    HeaderText = "小计（元）",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                     new  NumberValidator(),
                     new RequiredValidator()
                    },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToRound"
                    }

                });
                
                reval.Columns.Add(new JQGridColumn()
            {
                DataField = "get_date",
                Editable = true,
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
                    HeaderText = "创建人"


                });
                return reval;
            }
                #endregion

        }
        #endregion


        #region 面料订购

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
                var orderId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id").ToInt();
                if (orderId > 0)
                {
                    fd.order_id = orderId;
                    FabricOrderBillManager.SaveFabricOrderBillAll(fd);
                }
            }
            return RedirectToAction("Save");
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
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridWindowHeight;
                reval.Height = 300;
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 500,
                    LeftOffset = 300

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
                    ShowEditButton = true,
                    ShowAddButton = true,
                    ShowDeleteButton = true
                };
                reval.DataUrl = Url.Action("GetFabricOrderGridData");
                reval.EditUrl = Url.Action("EditFabricOrderGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "fob_id",
                    PrimaryKey = true,
                    Editable = false,
                    HeaderText = "编号"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "no",
                    Editable = true,
                    HeaderText = "品番",
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
                    HeaderText = "单价（元）",
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
                    HeaderText = "生地幅"



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
                    HeaderText = "数量（米）",
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
                #endregion
                return reval;
            }

        }
        #endregion


        #region 生产计划

        public JsonResult GetProductionPlanGridData(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var gridDataList = ProductionPlanManager.GetProductionPlanBillList(db, id);
                var gridModel = GetProductionPlanGridModel;
                return gridModel.DataBind(gridDataList);
            }
        }

        public ActionResult EditProductionPlanGridData(ProductionPlan fd)
        {
            if (GetProductionPlanGridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                ProductionPlanManager.DelBill(fd.pp_id);
            }
            else
            {
                var orderId = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id").ToInt();
                if (orderId > 0)
                {
                    fd.order_id = orderId;
                    ProductionPlanManager.SaveProductionPlanAll(fd);
                }
            }
            return RedirectToAction("Save");
        }
        /// <summary>
        /// 获取面料订购
        /// </summary>
        public JQGrid GetProductionPlanGridModel
        {
            get
            {
                //供应商
                var providerList = DictManager.GetFactory().Select(c => new SelectListItem() { Value = c.d_name + "", Text = c.d_name }).ToList();
                #region 设置
                var reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridWindowHeight;
                reval.Height = 300;
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    Width = 400,
                    CloseAfterAdding = true,
                    TopOffset = 1600,
                    LeftOffset = 300

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
                    InitialSortColumn = "pp_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowAddButton = true,
                    ShowDeleteButton = true
                };
                reval.DataUrl = Url.Action("GetProductionPlanGridData");
                reval.EditUrl = Url.Action("EditProductionPlanGridData");
                reval.Columns = new List<JQGridColumn>();
                #endregion
                #region 列
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "pp_id",
                    PrimaryKey = true,
                    Editable = false,
                    HeaderText = "编号"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "trader_name",
                    Editable = false,
                    HeaderText = "客户"

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "style_name",
                    Editable = true,
                    HeaderText = "款式",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                                     new  RequiredValidator()
                                    }

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "style_no",
                    Editable = true,
                    HeaderText = "款号",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                                     new  RequiredValidator()
                                    }

                });
                reval.Columns.Add(new JQGridColumn()
                  {
                      DataField = "no",
                      Editable = true,
                      HeaderText = "番号",
                      EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                                             new  RequiredValidator()
                                            }

                  });


                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "num",
                    Editable = true,
                    HeaderText = "制品数",
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
                    DataField = "factory_name",
                    Editable = true,
                    HeaderText = "裁缝厂",
                    EditList = providerList,
                    EditType = EditType.DropDown,

                });



                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "rely_date",
                    Editable = true,
                    HeaderText = "产前样衣依赖日",
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
                    DataField = "send_date",
                    Editable = true,
                    HeaderText = "产前寄出日",
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
                    DataField = "confirm_date",
                    Editable = true,
                    HeaderText = "产前确认日",
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
                    DataField = "f_get_date",
                    Editable = true,
                    HeaderText = "面料交期",
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
                    DataField = "a_get_date",
                    Editable = true,
                    HeaderText = "辅料交期",
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
                    DataField = "c_start_date",
                    Editable = true,
                    HeaderText = "裁剪开始",
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
                    DataField = "c_report_date",
                    Editable = true,
                    HeaderText = "裁剪报告日",
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
                    DataField = "c_end_date",
                    Editable = true,
                    HeaderText = "裁剪结束日",
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
                    DataField = "send_check_date",
                    Editable = true,
                    HeaderText = "送检日期",
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
                    DataField = "ship_date",
                    Editable = true,
                    HeaderText = "船期",
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
                #endregion
                return reval;
            }

        }
        #endregion

    }
}
