using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
using NFD.BLL.Bill;
using System.Web.UI.WebControls;
namespace NFD.Areas.Bill.Controllers
{
    /// <summary>
    /// 裁剪发货单
    /// </summary>
    public class CutController : Controller
    {


        public ActionResult Index()
        {

            var model = Tuple.Create(GetCutModel, GetCutModelChild);
            return View(model);
        }
        //获取裁剪
        public JsonResult GetCutGridData(int id = 0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var cutDataList = CutBillManager.GetV_CutBill(db);
                string oidstr = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "id");
                if (oidstr.ToInt() > 0)
                {
                    int oid = oidstr.ToInt();
                    cutDataList = cutDataList.Where(c => c.order_id == oid);
                }
                var gridModel = GetCutModel;
                return gridModel.DataBind(cutDataList);
            }
        }
        //获取送货
        public JsonResult GetCutGridChildData(string parentRowID)
        {
            int parentId = parentRowID.ToInt();
            using (NFDEntities db = new NFDEntities())
            {
                var cutDataList = CutBillManager.GetCutBillShipment(db).Where(c => c.c_id == parentId);
                var gridModel = GetCutModelChild;
                return gridModel.DataBind(cutDataList);
            }

        }
        //编辑裁剪
        public ActionResult EditCutGridData(CutBill bill)
        {
            if (GetCutModel.AjaxCallBackMode != AjaxCallBackMode.DeleteRow)
            {
                CutBillManager.SaveCuiBillAll(bill);
            }
            else
            {
                CutBillManager.DelBill(bill.c_id);
            }
            return RedirectToAction("index");
        }

        public ActionResult EditCutGridChildData(CutBillShipment bill)
        {
            if (GetCutModel.AjaxCallBackMode != AjaxCallBackMode.DeleteRow)
            {
                CutBillManager.SaveCutBillShipment(bill);
            }
            else
            {
                CutBillManager.DelCutBillShipment(bill.c_id);
            }
            return RedirectToAction("index");

        }


        public JQGrid GetCutModel
        {
            get
            {
                JQGrid reval = new JQGrid();

                var traderList = BLL.TraderManager.GetList().Select(c => new SelectListItem() { Value = c.trader_id + "", Text = c.name }).ToList();
                traderList.Insert(0, new SelectListItem());

                #region 设置
                reval.DataUrl = "GetCutGridData";
                reval.EditUrl = "EditCutGridData";
                reval.AutoWidth = true;
                int height = BLL.AppstringManager.GetGridHeight;
                reval.Height = height;
                reval.ClientSideEvents = new ClientSideEvents()
               {
                   SubGridRowExpanded = "showSubGrid"
               };
                reval.HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Parent,
                    ReloadOnExpand = false,
                    SelectOnExpand = false,
                    ExpandOnLoad = false
                };
                reval.ToolBarSettings = new ToolBarSettings()
                {

                    //ShowDeleteButton = true,
                    //ShowAddButton = true,
                    ShowSearchToolBar = true,
                    ShowEditButton = true,
                };
                reval.EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true
                };

                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "c_id desc",



                };
                #endregion

                #region 款号
                reval.Columns.Add(
                    new JQGridColumn()
                    {
                        PrimaryKey = true,
                        HeaderText = "编号",
                        DataField = "c_id",
                        Visible = true,
                        Frozen=true
                    }
                    );
                reval.Columns.Add(new JQGridColumn
                {
                    Editable = false,
                    DataField = "trader_id",
                    HeaderText = "客户",
                    SearchToolBarOperation = SearchOperation.IsEqualTo,
                    SearchType = SearchType.DropDown,
                    DataType = typeof(int),
                    Width=150,
                    SearchList = traderList,
                    EditType = EditType.DropDown,
                    EditList = traderList,
                    Formatter = new CustomFormatter() { FormatFunction = "ToTrader" },
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator()
                        }
                });
                reval.Columns.Add(
                  new JQGridColumn()
                  {

                      HeaderText = "款号",
                      DataField = "clothing_number",
                      Editable = true,
                      DataType = typeof(string),
                      EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator()
                        }

                  }
                  );
                reval.Columns.Add(
       new JQGridColumn()
       {

           HeaderText = "番号",
           DataField = "no",
           Editable = true,
           DataType = typeof(string),
           EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator()
                        }

       }
       );
                reval.Columns.Add(
                new JQGridColumn()
                {

                    HeaderText = "色番",
                    DataField = "color_foreign",
                    Editable = true,
                    DataType = typeof(string),
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator()
                        }

                }
                );
                reval.Columns.Add(
                new JQGridColumn()
                {

                    HeaderText = "色名",
                    DataField = "color_name",
                    Searchable = false,
                    Editable = true,

                }
                );
                reval.Columns.Add(
                    new JQGridColumn()
                    {

                        HeaderText = "面料订单数（米）",
                        DataField = "order_quantity",
                        Editable = false,
                        Searchable = false,
                        Formatter = new CustomFormatter()
                        {
                            FormatFunction = "ToRound"
                        }

                    }
                    );
                reval.Columns.Add(
                    new JQGridColumn()
                    {

                        HeaderText = "到货数量(米)",
                        DataField = "arrival_num",
                        Editable = true,
                        Searchable = false,
                        EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new NumberValidator()
                        }, Formatter = new CustomFormatter() { FormatFunction = "ToRound"}

                    }
                    );
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "尺码", DataField = "size" });
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "発注数（套）", DataField = "note_num", Formatter = new CustomFormatter() { FormatFunction = "ToRound"},EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new NumberValidator() } });
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "预裁数（套）", DataField = "will_num", Formatter = new CustomFormatter() { FormatFunction = "ToRound" }, EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new NumberValidator() } });
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "实裁数（套）", DataField = "actual_num", Formatter = new CustomFormatter() { FormatFunction = "ToRound" }, EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new NumberValidator() } });
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "出货数（套）", DataField = "delivers_num", Formatter = new CustomFormatter() { FormatFunction = "ToRound" }, EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new NumberValidator() } });
                reval.Columns.Add(new JQGridColumn() { Editable = true, Searchable = false, HeaderText = "B品数（套）", DataField = "bnum", Formatter = new CustomFormatter() {  FormatFunction="ToRound"}, EditClientSideValidators = new List<JQGridEditClientSideValidator>() { new NumberValidator() } });
                reval.Columns.Add(new JQGridColumn()
               {
                   Editable = true,
                   Searchable = false,
                   HeaderText = "送检日期",
                   DataField = "check_date",
                   Formatter = new CustomFormatter()
                   {
                       FormatFunction = "ToDate"
                   },
                   EditFieldAttributes = new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                },
               });
                reval.Columns.Add(
 new JQGridColumn()
 {

     HeaderText = "进口费用(元)",
     DataField = "wellhead_price",
     Editable = true,
     Formatter = new CustomFormatter() { FormatFunction = "ToRound" },
     Searchable = false,
     EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new NumberValidator()
                        }

 }
 );
                reval.Columns.Add(
            new JQGridColumn()
            {

                HeaderText = "出口费用（元）",
                Formatter = new CustomFormatter() { FormatFunction = "ToRound" },
                DataField = "export_price",
                Editable = true,
                Searchable = false,
                EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new NumberValidator()
                        }

            }
            );
                reval.Columns.Add(
       new JQGridColumn()
       {

           HeaderText = "备注",
           DataField = "remark",
           Searchable = false,
           Editable = true,

       }
       );
                #endregion
                return reval;
            }

        }

        public JQGrid GetCutModelChild
        {
            get
            {
                JQGrid reval = new JQGrid();
                reval.Height = Unit.Percentage(100);
                reval.DataUrl = "GetCutGridChildData";
                reval.EditUrl = "EditCutGridChildData";
                reval.HierarchySettings = new HierarchySettings()
                {
                    HierarchyMode = HierarchyMode.Child
                };
                reval.GroupSettings = new GroupSettings()
                {

                    GroupFields = new List<GroupField>() {
                      new GroupField(){
                        DataField="c_id",  ShowGroupColumn=true, ShowGroupSummary=true , HeaderText="裁剪编号：{0}"
                      }
                    }
                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowDeleteButton = true,
                    ShowRefreshButton = true,
                    ShowAddButton = true
                };
                reval.ClientSideEvents.AfterAddDialogShown = "showDialog";

                reval.EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true,
                    TopOffset = 0
                };
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true
                };
                reval.Columns.Add(new JQGridColumn
                {
                    DataField = "cs_id",
                    PrimaryKey = true,
                    HeaderText = "编号",
                    Visible = false,
                    Width = 50
                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "c_id",
                    Visible = false,
                    HeaderText = "编号",
                    Width = 40,
                    Editable = true
                });
                reval.Columns.Add(new JQGridColumn
                {
                    DataField = "shipment_date",
                    Editable = true,
                    HeaderText = "送货日期",
                    DataType = typeof(string),
                    Searchable = false,
                    EditFieldAttributes = new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                },
                    Formatter = new CustomFormatter()
                    {
                        FormatFunction = "ToDate"
                    },
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator()
                        },
                    Width = 200
                });
                reval.Columns.Add(new JQGridColumn
                {
                    DataField = "num",
                    Editable = true,
                    HeaderText = "送货数量",
                    DataType = typeof(int),
                    Searchable = false,
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>() { 
                         new RequiredValidator(),
                         new NumberValidator()
                        },
                    Width = 100,
                    GroupSummaryType = GroupSummaryType.Sum,
                    GroupTemplate = "合计:{0}"
                });
                return reval;

            }
        }

    }
}
