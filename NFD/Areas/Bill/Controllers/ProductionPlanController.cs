using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.BLL;
using NFD.BLL.Bill;
using NFD.Entities.Data;
using COM.Utility;
using COM.Extension;
namespace NFD.Areas.Bill.Controllers
{
    public class ProductionPlanController : Controller
    {
        //
        // GET: /Bill/ProductionPlan/

        public ActionResult Index()
        {
            var model = GetProductionPlanGridModel;
            model.DataUrl += "?id=0";
            return View(model);
        }


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
                    //ShowAddButton = true,
                    //ShowDeleteButton = true
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

    }
}
