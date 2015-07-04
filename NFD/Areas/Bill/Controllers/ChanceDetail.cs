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
namespace NFD.Areas.Bill.Controllers
{
    public partial class ChanceController : Controller
    {

  
        public ActionResult Save(int id = 0, int htId = 0)
        {
            JQGrid jqGridModel = GetGridModel();
            jqGridModel.DataUrl += "?billId=" + id;
            ViewBag.jsGridModel = jqGridModel;

            ChanceBill model = new ChanceBill();
            if (id > 0)
            {
                TempData["billId"] = id;
                model = ChanceBillManager.GetChanceBillById(id);
            }
            else if (htId > 0)
            {
                var ht = HandmadeThingsManager.GetSingle(htId);
                model.ht_no = ht.pro_no;
                model.ht_id = ht.ht_id;
                model.trader_id = ht.trader_id;
                model.ht_specifications = ht.specifications;
            }

            return View(model);
        }

        public ActionResult EditGrid(ChanceBillDetail bill)
        {
            TempData["billId"]=bill.bill_id = TempData["billId"].ToInt();

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

        [HttpPost]
        public ActionResult Save(ChanceBill bill)
        {
            ChanceBillManager.SaveChanceBill(bill);
            return RedirectToAction("Save", new { id = bill.bill_id });
        }


        public JsonResult GridData(int billId)
        {

            JQGrid model = GetGridModel();
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                var reval = ChanceBillManager.GetChanceBillDetailListByBillId(billId, db);
                return model.DataBind(reval);
            }

        }

        //获取grid model
        private JQGrid GetGridModel()
        {
            //供应商
            var providerList = DictManager.GetProvider().Select(c => new SelectListItem() { Value = c.d_name + "", Text = c.d_name }).ToList();
            return new JQGrid
            {
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = BLL.AppstringManager.GetPageSize
                },
                Height = BLL.PubMethod.IsWindow ? 100 : 150,
                DataUrl = Url.Action("GridData"),
                EditUrl = Url.Action("EditGrid"),
                //MultiSelect = true,
                ToolBarSettings = new ToolBarSettings()
                {
                    ShowAddButton = true,
                    ShowEditButton = true,
                    ShowDeleteButton = true,
                },

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
                    InitialSortColumn = "billd_id"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "billd_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Width = 50 } ,
                                             new JQGridColumn { DataField = "bill_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                Visible=false,
                                                                HeaderText="报价单编号",
                                                                Searchable=false,
                                                                Width = 50 } ,

                                              new JQGridColumn { DataField = "name", 
                                                                Editable = true,
                                                                HeaderText="辅料",
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new RequiredValidator()
                                                                } 
                                                               },
                                                                     new JQGridColumn { DataField = "specifications", 
                                                                Editable = true,
                                                                HeaderText="规格",
                                                                EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                  new RequiredValidator()
                                                                } 
                                                               },

                                             //new JQGridColumn { DataField = "cost_price", 
                                             //                   Editable = true,
                                             //                   HeaderText="成本价",
                                             //                   EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                             //                    new NumberValidator()
                                             //                   },
                                             //                      Formatter=new  CustomFormatter(){
                                             //                    FormatFunction="ToRound"
                                             //                   },
                                             //                   Width=100,
                                             //                   Searchable=true,
                                             //                   DataType=typeof(string)},
                                             new JQGridColumn { DataField = "cost_price", 
                                                                Editable = true,
                                                                HeaderText="单价(元)",
                                                                  EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                 new NumberValidator()
                                                                },
                                                                   Formatter=new  CustomFormatter(){
                                                                 FormatFunction="ToRound"
                                                                },
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string)},
                                             //new JQGridColumn { DataField = "market_price", 
                                             //                   Editable = true,
                                             //                      Formatter=new  CustomFormatter(){
                                             //                    FormatFunction="ToRound"
                                             //                   },
                                             //                   HeaderText="市场价",
                                             //                     EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                             //                    new NumberValidator()
                                             //                   },
                                             //                   Width=100,
                                             //                   Searchable=true,
                                             //                   DataType=typeof(string)},               
                                                          
                                             new JQGridColumn { DataField = "num", 
                                                                Editable = true,
                                                                   Formatter=new  CustomFormatter(){
                                                                 FormatFunction="ToRound"
                                                                },
                                                                HeaderText="单耗",
                                                                Width=100,
                                                                  EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                 new NumberValidator()
                                                                },
                                                                Searchable=true,
                                                                DataType=typeof(string)},  
     
                                        new JQGridColumn()
                {
                    DataField = "supplier_name",
                    Editable = true,
                    HeaderText = "供应商",
                    EditList = providerList,
                    EditType = EditType.DropDown

                }

                                             
               },



                Width = Unit.Pixel(640)
            };
        }
    }
}
