using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using NFD.Entities.Data;
using System.Web.UI.WebControls;
using COM.Utility;
using NFD.Entities.Common;
using COM.Extension;
using NFD.BLL;
using System.Data;
namespace NFD.Areas.Htm.Controllers
{
    //登录记手织样
    public class RegistrationController : Controller
    {


        //手织样登录页
        [HttpGet]
        public ActionResult SavePage(int id = 0)
        {
            HandmadeThings ht = new HandmadeThings();
            if (id > 0)
            {
                ht = BLL.HandmadeThingsManager.GetSingle(id);
                CookiesHelper.AddCookie(PubConst.COOKIES_HT_ID, id + "", DateTime.Now.AddYears(1));
            }
           
            Tuple<HandmadeThings, JQGrid> model = Tuple.Create(ht, GetGridModel());
            return View(model);
        }
        //手织样登录页-提交
        [HttpPost]
        public ActionResult SavePage(HandmadeThings ht,string [] filePath)
        {
            var newHt = BLL.HandmadeThingsManager.Save(ht,filePath);
            CookiesHelper.AddCookie(PubConst.COOKIES_HT_ID, ht.ht_id + "", DateTime.Now.AddYears(1));
            if (PubMethod.IsWindow)
            {
                return this.Redirect(Url.Action("SavePage") + "?id=" + ht.ht_id+"&isWindow=true");
            }
            else
            {
                return this.Redirect(Url.Action("SavePage") + "?id=" + ht.ht_id);
            }
        }
        //手织样grid数据
        public JsonResult GridData()
        {
            using (NFDEntities db = new NFDEntities())
            {
                int ht_id = CookiesHelper.GetCookieValue(PubConst.COOKIES_HT_ID).ToInt();
                JQGrid model = GetGridModel();
                return model.DataBind(BLL.HandmadeThingsDetailManager.GetList(db).Where(c => c.ht_id == ht_id));
            }
        }
        public ActionResult EditGrid(HandmadeThingsDetail htd)
        {
            int ht_id = CookiesHelper.GetCookieValue(PubConst.COOKIES_HT_ID).ToInt();
            htd.ht_id = ht_id;
            var gridModel = GetGridModel();
            //修改或新增
            if (gridModel.AjaxCallBackMode == AjaxCallBackMode.EditRow || gridModel.AjaxCallBackMode == AjaxCallBackMode.AddRow)
            {
                BLL.HandmadeThingsDetailManager.Save(htd);
            }
            //删除
            else if (gridModel.AjaxCallBackMode == AjaxCallBackMode.DeleteRow)
            {
                //批量删除
                if (htd.htd_id == 0)
                {
                    BLL.HandmadeThingsDetailManager.Del(Request["htd_id"].Split(','));
                }
                else
                {
                    BLL.HandmadeThingsDetailManager.Del(htd.htd_id);
                }
            }
            return this.Redirect(Url.Action("SavePage"));
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
                Height = BLL.PubMethod.IsWindow?100:150,
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
                    InitialSortColumn = "color_foreign"

                },

                AutoWidth = true,

                Columns = new List<JQGridColumn>()
               {
                                             new JQGridColumn { DataField = "htd_id", 
                                                                PrimaryKey = true,
                                                                Editable = false,
                                                                HeaderText="编号",
                                                                Searchable=false,
                                                                Width = 50 } ,
                                               new JQGridColumn { DataField = "color_foreign", 
                                                                Editable = true,
                                                                HeaderText="色番",
                                                                Width=100,
                                                                Searchable=true,
                                                                    EditClientSideValidators=new List<JQGridEditClientSideValidator>(){
                                                                     new RequiredValidator()
                                                                 },
                                                                 DataType=typeof(string)
                                                          

                                             },
                                             new JQGridColumn { DataField = "color_name", 
                                                                Editable = true,
                                                                HeaderText="色名",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string)
                                                          

                                             },
                                              new JQGridColumn { DataField = "Indicate_day", 
                                                                Editable = true,
                                                                HeaderText="指示日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                               
                                                          

                                             },
                                              new JQGridColumn { DataField = "confirm_day", 
                                                                Editable = true,
                                                                HeaderText="确认日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                  Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          

                                             },
                                              new JQGridColumn { DataField = "confirm_day", 
                                                                Editable = true,
                                                                HeaderText="预定寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                  Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },
                                             new JQGridColumn { DataField = "actual_sending_date", 
                                                                Editable = true,
                                                                HeaderText="实际寄出日",
                                                                Width=100,
                                                                Searchable=true,
                                                                DataType=typeof(string),
                                                                 EditFieldAttributes=new List<JQGridEditFieldAttribute>(){
                                                                 new JQGridEditFieldAttribute(){ Name="class", Value="Wadate"},
                                                                 new JQGridEditFieldAttribute(){Name="onclick",Value="WdatePicker()"}
                                                                } ,
                                                                  Formatter= new  CustomFormatter(){
                                                                   FormatFunction="ToDate"
                                                                }
                                                          
                                                          

                                             },


                                             
               },



                Width = Unit.Pixel(640)
            };
        }


    }
}
