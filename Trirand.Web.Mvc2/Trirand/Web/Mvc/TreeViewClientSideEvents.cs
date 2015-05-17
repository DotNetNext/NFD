namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    public class TreeViewClientSideEvents
    {
        public TreeViewClientSideEvents()
        {
            this.Expand = "";
            this.Collapse = "";
            this.Check = "";
            this.Select = "";
            this.MouseOver = "";
            this.MouseOut = "";
            this.NodesDragged = "";
            this.NodesDropped = "";
            this.NodesMoved = "";
        }

        public string Check { get; set; }

        public string Collapse { get; set; }

        public string Expand { get; set; }

        public string MouseOut { get; set; }

        public string MouseOver { get; set; }

        public string NodesDragged { get; set; }

        public string NodesDropped { get; set; }

        public string NodesMoved { get; set; }

        public string Select { get; set; }
    }
}

