using System.Web.Mvc;

namespace NFD.Areas.Dict
{
    public class DictAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dict";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Dict_default",
                "Dict/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
