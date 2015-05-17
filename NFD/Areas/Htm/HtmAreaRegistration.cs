using System.Web.Mvc;

namespace NFD.Areas.Htm
{
    public class HtmAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Htm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Htm_default",
                "Htm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
