using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COM.Extension;
using COM.Utility;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL;
namespace NFD.Areas.Dict.Controllers
{
    /// <summary>
    /// 字典配制
    /// </summary>
    public class IndexController : Controller
    {

        public ActionResult Index(int key)
        {
            var jqModel = GetDictGridModel;
            ViewBag.dt = DictManager.GetDictionariesById(key);
            jqModel.DataUrl += "?key=" + key;
            if (key == 2)
            {
                jqModel.Columns.Add(new JQGridColumn()
                {
                    DataField = "value2",
                    HeaderText = "本公司",
                    EditType = EditType.DropDown,
                    EditList = new List<SelectListItem>(){
                         new SelectListItem(){ Text="", Selected=true},
                       new SelectListItem(){ Text="是", Value="是"}
                     },
                    Editable = true

                });
            }
            return View(jqModel);
        }

        public ActionResult EditDictGrid(DictionariesDetail dd)
        {
            dd.d_key = RequestHelper.QueryStringByUrlString(Request.UrlReferrer.ToString(), "key").ToInt();
            //修改或新增
            if (GetDictGridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || GetDictGridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                DictManager.Save(dd);
            }
            else
            {

            }
            return RedirectToAction("Index", new { key = dd.d_key });
        }

        public JsonResult GetDictGridData(int key)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var jqModel = GetDictGridModel;
                var gridSource = DictManager.GetDictDetail(db).Where(c => c.d_key == key);
                if (key == 2)
                {
                    jqModel.Columns.Add(new JQGridColumn()
                    {
                        DataField = "value2",
                        HeaderText = "本公司"
                     }

                   );
                }
                return jqModel.DataBind(gridSource);
            }
        }


        public JQGrid GetDictGridModel
        {
            get
            {
                JQGrid reval = new JQGrid();
                reval.AutoWidth = true;
                int height = BLL.PubMethod.IsWindow ? BLL.AppstringManager.GetGridWindowHeight : BLL.AppstringManager.GetGridHeight;
                reval.Height = height;
                reval.SortSettings = new SortSettings()
                {
                    MultiColumnSorting = true,
                    InitialSortColumn = "sort desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowAddButton = true
                };
                reval.DataUrl = Url.Action("GetDictGridData");
                reval.EditUrl = Url.Action("EditDictGrid");
                reval.Columns = new List<JQGridColumn>();
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "dd_id",
                    HeaderText = "编号",
                    Width = 50,
                    PrimaryKey = true
                });
                reval.EditDialogSettings = new EditDialogSettings()
                {
                    CloseAfterEditing = true
                };
                reval.AddDialogSettings = new AddDialogSettings()
                {
                    CloseAfterAdding = true
                };
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "d_name",
                    HeaderText = "名称",
                    Editable = true,
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>(){
                      new RequiredValidator()
                     },

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "sort",
                    HeaderText = "排序号",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>(){
                      new NumberValidator()
                     },
                    Editable = true

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "value1",
                    HeaderText = "备注",
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>()
                    {

                    },
                    Editable = true

                });
                return reval;

            }

        }


    }
}
