using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Trirand.Web.Mvc;
using NFD.Entities.Data;

namespace NFD.Areas.Crm.Controllers
{
    public class TraderController : Controller
    {

        //
        // GET: /JqGrid/

        public ActionResult Index()
        {
            JQGrid model = GetGridModel();
            return View(model);
        }

        public JsonResult GridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                JQGrid model = GetGridModel();
                return model.DataBind(BLL.TraderManager.GetList(db));
            }
        }
        public ActionResult EditGrid(Trader trader)
        {
            var gridModel = GetGridModel();
            //修改或新增
            if (gridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || gridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                BLL.TraderManager.Save(trader);
            }
            //删除
            else if (gridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                //批量删除
                if (trader.trader_id == 0)
                {
                    BLL.TraderManager.Del(Request["trader_id"].Split(','));
                }
                else
                {
                    BLL.TraderManager.Del(trader.trader_id);
                }
            }
            return this.Redirect(Url.Action("Index"));
        }

        //获取grid model
        private JQGrid GetGridModel()
        {

            return new JQGrid
            {
                PagerSettings = new Trirand.Web.Mvc.PagerSettings()
                {
                    PageSize = BLL.AppstringManager.GetPageSize
                },
                Height = BLL.PubMethod.IsWindow ? BLL.AppstringManager.GetGridWindowHeight : BLL.AppstringManager.GetGridHeight,
                DataUrl = Url.Action("GridData"),
                EditUrl = Url.Action("EditGrid"),
                //MultiSelect = true,
                ToolBarSettings = new ToolBarSettings()
                {
                    ShowAddButton = true,
                    ShowEditButton = true,
                    ShowRefreshButton = true,
                    ShowSearchToolBar = true,
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
                    InitialSortColumn = "trader_id desc"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
                                         {
                                             new JQGridColumn { DataField = "trader_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Width = 50 },
                                             new JQGridColumn { DataField = "name", 
                                                                Editable = true,
                                                                HeaderText="姓名",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string),
                                                                 SearchToolBarOperation=SearchOperation.Contains,
                                                                 SearchType=SearchType.TextBox
                                                          

                                             },
                                                new JQGridColumn{
                                                                 

                                                                  DataField="tel",
                                                                  Editable=true    ,
                                                                  HeaderText="电话",
                                                                  Searchable=false
                                             },
                                                    new JQGridColumn{
                                                                 

                                                                  DataField="fax_number",
                                                                  Editable=true    ,
                                                                  HeaderText="传真",
                                                                  Searchable=false
                                             },
                                             new JQGridColumn{
                                                                  DataField="mobile",
                                                                  Editable=true    ,
                                                                  HeaderText="手机",
                                                                  Searchable=false,
                                                                  EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new   Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isMobile"}
                                                                  }
                                             },
                                          
                                              new JQGridColumn{
                                                                 

                                                                  DataField="qq",
                                                                  Editable=true    ,
                                                                  HeaderText="qq",
                                                                  Searchable=false,
                                                                  Visible=false
                                             },
                                              new JQGridColumn{
                                                                 

                                                                  DataField="email",
                                                                  Editable=true    ,
                                                                  HeaderText="邮箱",
                                                                  Searchable=false,
                                                                  EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new Trirand.Web.Mvc.CustomValidator(){ ValidationFunction="isMail"}
                                                                  }
                                             },
                                                       new JQGridColumn{
                                                                 

                                                                  DataField="url",
                                                                  Editable=true    ,
                                                                  HeaderText="公司地址",
                                                                  Searchable=false,
                                                                  
                                             },
                                                      new JQGridColumn{
                                                                 

                                                                  DataField="code",
                                                                  Editable=true    ,
                                                                  HeaderText="邮编",
                                                                  Searchable=false,
                                                                 
                                             },
                                      
                                                new JQGridColumn{
                                                                 

                                                                  DataField="remark",
                                                                  EditType=EditType.TextArea,
                                                                  Editable=true    ,
                                                                  HeaderText="备注",
                                                                  Searchable=false
                                             },
                                                  new JQGridColumn{
                                                                 

                                                                  DataField="creator_name",
                                                                  Editable=false    ,
                                                                  HeaderText="创建人",
                                                                  Searchable=false
                                             }
                                             ,
                                                  new JQGridColumn{
                                                                 

                                                                  DataField="create_time",
                                                                  Editable=false    ,
                                                                  HeaderText="创建时间",
                                                                  Searchable=false
                                             }


                                             
                                                              }



                
            };
        }

    }
}
