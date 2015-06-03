using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using COM.Utility;
using System.Web;
using NFD.Entities.Common;
namespace NFD.BLL
{
    public class PubMethod
    {
        public static bool IsWindow
        {
            get
            {
                return RequestHelper.QueryString("isWindow").ToConvertString().ToLower() == "true";

            }
        }

        public static HtmlString GetLeftMenuJson
        {
            get
            {
                var myMenuIds = UserManager.GetUserInfoMenuByUserId(UserManager.GetCurrentUserInfo.user_id);
                List<Ztree> List = new List<Ztree>();
                List.Add(new Ztree() { id = 100000, name = "首页", open = "true", pId = 0, target = "_self", url = "/Home/Index" });
                //客户管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 1,
                    name = "客户管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Crm/Trader/Index"
                });
                List.Add(new Ztree() { id = 11, name = "总览", open = "true", pId = 1, target = "_self", url = "/Crm/Trader/Index" });
                //手织样管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 2,
                    name = "手织样管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Htm/HtSummary/Index"
                });
                List.Add(new Ztree() { id = 22, name = "总览", open = "true", pId = 2, target = "_self", url = "/Htm/HtSummary/Index" });
                List.Add(new Ztree() { id = 23, name = "1、登记", open = "true", pId = 2, target = "_self", url = "/Htm/Registration/SavePage" });
                List.Add(new Ztree() { id = 24, name = "2、手织样", open = "true", pId = 2, target = "_self", url = "/Htm/WaitReg/Index" });
                List.Add(new Ztree() { id = 25, name = "3、放大样", open = "true", pId = 2, target = "_self", url = "/Htm/AmplifierSample/Index" });
                List.Add(new Ztree() { id = 26, name = "4、产样衣", open = "true", pId = 2, target = "_self", url = "/Htm/Sample/Index" });




                //报价管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 3,
                    name = "报价管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Bill/Chance/Index"
                });
                List.Add(new Ztree() { id = 33, name = "报价单总览", open = "true", pId = 3, target = "_self", url = "/Bill/Chance/Index" });


                //生产计划
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 201541401,
                    name = "生产计划",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Bill/Order/Index"
                });

                List.Add(new Ztree() { id = 34, name = "订单总览", open = "true", pId = 201541401, target = "_self", url = "/Bill/Order/Index" });
                List.Add(new Ztree() { id = 36, name = "面料订购", open = "true", pId = 201541401, target = "_self", url = "/Bill/FabricOrder/Index" });
                List.Add(new Ztree() { id = 37, name = "计划", open = "true", pId = 201541401, target = "_self", url = "/Bill/ProductionPlan/Index" });
                List.Add(new Ztree() { id = 35, name = "裁剪总览", open = "true", pId = 201541401, target = "_self", url = "/Bill/Cut/Index" });


                //面料管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 4,
                    name = "面料管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Bill/FabricDetail/Index"
                });
                List.Add(new Ztree() { id = 44, name = "面料总览", open = "true", pId = 4, target = "_self", url = "/Bill/FabricDetail/Index" });



                //辅料管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 5,
                    name = "辅料管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Bill/AccessoriesDetail/Index"
                });
                List.Add(new Ztree() { id = 55, name = "辅料管理", open = "true", pId = 5, target = "_self", url = "/Bill/AccessoriesDetail/Index" });


                //所表中心
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 110,
                    name = "报表中心",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Report/OrderTotal/Index"
                });
                List.Add(new Ztree() { id = 270, name = "数据汇总", open = "true", pId = 110, target = "_self", url = "/Report/OrderTotal/Index" });



                //配置管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 10,
                    name = "其它",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/Dict/Index/Index?key=1"
                });
                List.Add(new Ztree() { id = 27, name = "裁缝厂总览", open = "true", pId = 10, target = "_self", url = "/Dict/Index/Index?key=1" });
                List.Add(new Ztree() { id = 28, name = "供应商总览", open = "true", pId = 10, target = "_self", url = "/Dict/Index/Index?key=2" });



                //配置管理
                List.Add(new Ztree()
                {
                    iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                    iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                    id = 500,
                    name = "系统管理",
                    open = "true",
                    pId = 0,
                    target = "_self",
                    url = "/UserPower/Account/Index"
                });
                List.Add(new Ztree() { id = 501, name = "账号管理", open = "true", pId = 500, target = "_self", url = "/UserPower/Account/Index" });
                List = List.Where(r => myMenuIds.Any(c => r.id == c.menu_id)).ToList();
                var json = List.ToJson();
                var redata = "var zNodes={0}".ToFormat(json).ToJavaScript();
                return redata;
            }
        }

        public static HtmlString GetLeftMenuJson2(int userId)
        {

            var myMenuIds = UserManager.GetUserInfoMenuByUserId(userId);
            List<Ztree> List = new List<Ztree>();
            List.Add(new Ztree() { id = 100000, name = "首页", open = "true", pId = 0, target = "_self", url = "/Home/Index" });
            //客户管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 1,
                name = "客户管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Crm/Trader/Index"
            });
            List.Add(new Ztree() { id = 11, name = "总览", open = "true", pId = 1, target = "_self", url = "/Crm/Trader/Index" });
            //手织样管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 2,
                name = "手织样管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Htm/HtSummary/Index"
            });
            List.Add(new Ztree() { id = 22, name = "总览", open = "true", pId = 2, target = "_self", url = "/Htm/HtSummary/Index" });
            List.Add(new Ztree() { id = 23, name = "1、登记", open = "true", pId = 2, target = "_self", url = "/Htm/Registration/SavePage" });
            List.Add(new Ztree() { id = 24, name = "2、手织样", open = "true", pId = 2, target = "_self", url = "/Htm/WaitReg/Index" });
            List.Add(new Ztree() { id = 25, name = "3、放大样", open = "true", pId = 2, target = "_self", url = "/Htm/AmplifierSample/Index" });
            List.Add(new Ztree() { id = 26, name = "4、产样衣", open = "true", pId = 2, target = "_self", url = "/Htm/Sample/Index" });



            //报价管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 3,
                name = "报价管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Bill/Chance/Index"
            });
            List.Add(new Ztree() { id = 33, name = "报价单总览", open = "true", pId = 3, target = "_self", url = "/Bill/Chance/Index" });


            //生产计划
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 201541401,
                name = "生产计划",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Bill/Order/Index"
            });

            List.Add(new Ztree() { id = 34, name = "订单总览", open = "true", pId = 201541401, target = "_self", url = "/Bill/Order/Index" });
            List.Add(new Ztree() { id = 36, name = "面料订购", open = "true", pId = 201541401, target = "_self", url = "/Bill/FabricOrder/Index" });
            List.Add(new Ztree() { id = 37, name = "计划", open = "true", pId = 201541401, target = "_self", url = "/Bill/ProductionPlan/Index" });
            List.Add(new Ztree() { id = 35, name = "裁剪总览", open = "true", pId = 201541401, target = "_self", url = "/Bill/Cut/Index" });




            //配置管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 4,
                name = "面料管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Bill/FabricDetail/Index"
            });
            List.Add(new Ztree() { id = 44, name = "面料总览", open = "true", pId = 4, target = "_self", url = "/Bill/FabricDetail/Index" });



            //配置管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 5,
                name = "辅料管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Bill/AccessoriesDetail/Index"
            });
            List.Add(new Ztree() { id = 55, name = "辅料管理", open = "true", pId = 5, target = "_self", url = "/Bill/AccessoriesDetail/Index" });


            //所表中心
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 110,
                name = "报表中心",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Report/OrderTotal/Index"
            });
            List.Add(new Ztree() { id = 270, name = "数据汇总", open = "true", pId = 110, target = "_self", url = "/Report/OrderTotal/Index" });



            //配置管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 10,
                name = "其它",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/Dict/Index/Index?key=1"
            });
            List.Add(new Ztree() { id = 27, name = "裁缝厂总览", open = "true", pId = 10, target = "_self", url = "/Dict/Index/Index?key=1" });
            List.Add(new Ztree() { id = 28, name = "供应商总览", open = "true", pId = 10, target = "_self", url = "/Dict/Index/Index?key=2" });



            //配置管理
            List.Add(new Ztree()
            {
                iconClose = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_close.png",
                iconOpen = "/Content/Scripts/Plugs/Multiple/zTree-zTree_v3-master/zTree_v3/css/zTreeStyle/img/diy/1_open.png",
                id = 500,
                name = "系统管理",
                open = "true",
                pId = 0,
                target = "_self",
                url = "/UserPower/Account/Index"
            });
            List.Add(new Ztree() { id = 501, name = "账号管理", open = "true", pId = 500, target = "_self", url = "/UserPower/Account/Index" });
            List.FillEach(c =>
            {

                c.@checked = myMenuIds.Any(x => x.menu_id == c.id);
            });
            var json = List.ToJson();
            var redata = "var zNodesPower={0}".ToFormat(json).ToJavaScript();
            return redata;

        }


        public static HtmlString GetSelectSize(string id, string selectVal = "")
        {
            var sizeArray = new string[] { "S", "M", "L", "XL", "XXL" };
            string selectHTML = "<select id='{0}' name='{0}'><option></option>".ToFormat(id);
            sizeArray.FillEach(c =>
            {
                if (c == selectVal)
                {
                    selectHTML += "<option selected=selected value=" + c + " >" + c + " </option>";
                }
                else
                {
                    selectHTML += "<option   value=" + c + "  >" + c + " </option>";
                }

            });
            selectHTML += "</select>";
            return selectHTML.ToHtmlString();
        }
    }
}
