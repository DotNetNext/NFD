//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Trirand.Web.Mvc;
//using NFD.Entities.Data;
//using System.Web.UI.WebControls;
//namespace NFD.Controllers
//{
//    public class JqGridController : Controller
//    {
//        //
//        // GET: /JqGrid/

//        public ActionResult Index()
//        {
//            JQGrid model = GetGridModel();
//            return View(model);
//        }

//        public JsonResult GridData()
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                JQGrid model = GetGridModel();
//                return model.DataBind(BLL.HomeManager.GetHome(db));
//            }
//        }
//        public ActionResult EditGrid(Home home)
//        {
//            var gridModel = GetGridModel();
//            //修改或新增
//            if (gridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || gridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
//            {
//                BLL.HomeManager.Save(home);
//            }
//            //删除
//            else if (gridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
//            {
//                //批量删除
//                if (home.Id == 0)
//                {
//                    BLL.HomeManager.Delete(Request["id"].Split(','));
//                }
//                else
//                {
//                    BLL.HomeManager.Delete(home.Id);
//                }
//            }
//            return RedirectToAction("Index", "JqGrid");
//        }

//        //获取grid model
//        private JQGrid GetGridModel()
//        {
//            return new JQGrid
//            {

//                Height = 400,
//                DataUrl = "/JqGrid/GridData",
//                EditUrl = "/JqGrid/EditGrid",
//                MultiSelect = true,
//                ToolBarSettings = new ToolBarSettings()
//                {
//                    ShowAddButton = true,
//                    ShowEditButton = true,
//                    ShowRefreshButton = true,
//                    ShowSearchToolBar = true,
//                    ShowDeleteButton = true,
//                },

//                EditDialogSettings = new EditDialogSettings()
//                {
//                    CloseAfterEditing = true
//                },
//                AddDialogSettings = new AddDialogSettings()
//                {
//                    CloseAfterAdding = true
//                },

//                SortSettings = new SortSettings()
//                {
//                    InitialSortColumn = "id desc"

//                },

//                AutoWidth = true,
                
//                Columns = new List<JQGridColumn>()
//                                 {
//                                     new JQGridColumn { DataField = "Id", 
//                                                        PrimaryKey = true,
//                                                        Editable = false,
//                                                         HeaderText="编号",
//                                                         Searchable=false,
                                               
//                                                        Width = 50 },
//                                     new JQGridColumn { DataField = "Name", 
//                                                        Editable = true,
//                                                         HeaderText="姓名",
//                                                         Width=100,
//                                                         Searchable=true,
                                                         
//                                                         DataType=typeof(string),
//                                                         SearchToolBarOperation=SearchOperation.Contains,
//                                                         SearchType=SearchType.TextBox,
//                                                         EditType=EditType.DropDown,
//                                                         EditList=new List<SelectListItem>(){
//                                                           new SelectListItem(){ Value="1", Text="选项一"},
//                                                           new SelectListItem(){ Value="2", Text="选项二"}
//                                                         },
//                                                         EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
//                                                           new JQGridEditFieldAttribute(){
//                                                              Name="height",
//                                                              Value="50"
//                                                           }
//                                                         }
                                                   
//                                     },
//                                     new JQGridColumn{
//                                                          DataField="Content",
//                                                          Editable=true    ,
//                                                          HeaderText="内容",
//                                                          Searchable=false
//                                     },
//                                     new JQGridColumn{
 
//                                                          HeaderText="编辑",
//                                                          Searchable=false,
//                                                          Sortable=false
//                                     }
//                                                      },



//                Width = Unit.Pixel(640)
//            };
//        }

//    }
//}
