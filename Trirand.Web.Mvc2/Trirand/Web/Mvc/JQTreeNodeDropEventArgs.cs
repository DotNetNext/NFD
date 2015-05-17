namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class JQTreeNodeDropEventArgs
    {
        public JQTreeNode DestinationNode { get; set; }

        public string DestinationTreeViewID { get; set; }

        public List<JQTreeNode> DraggedNodes { get; set; }

        public Hashtable PostData { get; set; }

        public string SourceTreeViewID { get; set; }
    }
}

