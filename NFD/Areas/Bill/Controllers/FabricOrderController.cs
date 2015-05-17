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
                    //ShowAddButton = true,
                    //ShowDeleteButton = true
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
                    HeaderText = "单价",
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
                    HeaderText = "数量",
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
                #endregion
                return reval;
            }

        }

    }
}
