namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    internal class JsonSearchToolBar
    {
        private JQGrid _grid;
        private Hashtable _jsonValues = new Hashtable();

        public JsonSearchToolBar(JQGrid grid)
        {
            this._grid = grid;
        }

        public string Process()
        {
            if (this._grid.SearchToolBarSettings.SearchToolBarAction == SearchToolBarAction.SearchOnKeyPress)
            {
                this._jsonValues["searchOnEnter"] = false;
            }
            return new JavaScriptSerializer().Serialize(this._jsonValues);
        }
    }
}

