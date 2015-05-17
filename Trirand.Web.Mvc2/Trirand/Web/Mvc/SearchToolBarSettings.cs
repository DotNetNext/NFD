namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class SearchToolBarSettings
    {
        public SearchToolBarSettings()
        {
            this.SearchToolBarAction = Trirand.Web.Mvc.SearchToolBarAction.SearchOnEnter;
        }

        public Trirand.Web.Mvc.SearchToolBarAction SearchToolBarAction { get; set; }
    }
}

