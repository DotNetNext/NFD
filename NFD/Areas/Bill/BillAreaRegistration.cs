using System.Web.Mvc;

namespace NFD.Areas.Bill
{
    public class BillAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Bill";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Bill_default",
                "Bill/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
