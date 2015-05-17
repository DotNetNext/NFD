using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.Entities.Data;
using Trirand.Web.Mvc;
using NFD.BLL;
 

namespace NFD.Areas.User.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            var model = GetUserGridModel;
            return View(model);
        }
        public ActionResult EditUserGrid(UserInfo dd)
        {

            //修改或新增
            if (GetUserGridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || GetUserGridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                UserManager.Save(dd);
            }
            else
            {

            }
            return RedirectToAction("Index");
        }

        public JsonResult GetUserGridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                var jqModel = GetUserGridModel;
                var gridSource = UserManager.GetUserList(db);
                return jqModel.DataBind(gridSource);
            }
        }


        public JQGrid GetUserGridModel
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
                    InitialSortColumn = "user_id desc",


                };
                reval.ToolBarSettings = new ToolBarSettings()
                {
                    ShowEditButton = true,
                    ShowAddButton = true
                };
                reval.DataUrl = Url.Action("GetUserGridData");
                reval.EditUrl = Url.Action("EditUserGrid");
                reval.Columns = new List<JQGridColumn>();
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "user_id",
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
                    DataField = "userName",
                    HeaderText = "名称",
                    Editable = true,
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>(){
                      new RequiredValidator()
                     },

                });
                reval.Columns.Add(new JQGridColumn()
                {
                    DataField = "password",
                    HeaderText = "密码",
                    EditType = EditType.Password,
                    EditClientSideValidators = new List<JQGridEditClientSideValidator>(){
                      new RequiredValidator()
                     },
                    Editable = true

                });

                reval.Columns.Add(new JQGridColumn()
               {
                   DataField = "user_id",
                   HeaderText = "操作",
                   Formatter = new CustomFormatter()
                   {
                       FormatFunction = "EditPower"
                   }

               });

                return reval;

            }
        }
    }
}
