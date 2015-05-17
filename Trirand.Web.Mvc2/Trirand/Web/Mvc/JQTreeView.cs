namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;
    using System.Collections;

    public class JQTreeView
    {
        private JQTreeNode _nodeByValue;

        public JQTreeView()
        {
            this.ID = "";
            this.DataUrl = "";
            this.DragAndDropUrl = "";
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
            this.HoverOnMouseOver = true;
            this.CheckBoxes = false;
            this.MultipleSelect = false;
            this.NodeTemplateID = "";
            this.ClientSideEvents = new TreeViewClientSideEvents();
            this.DragAndDrop = false;
        }

        public JsonResult DataBind(List<JQTreeNode> nodes)
        {
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new JavaScriptSerializer().Serialize(this.SerializeNodes(nodes)) };
        }

        public JQTreeNode FindNodeByValue(List<JQTreeNode> nodes, string value)
        {
            this._nodeByValue = null;
            this.FindNodeByValueInternal(nodes, value);
            return this._nodeByValue;
        }

        private void FindNodeByValueInternal(List<JQTreeNode> nodes, string value)
        {
            foreach (JQTreeNode node in nodes)
            {
                if (node.Value == value)
                {
                    this._nodeByValue = node;
                    break;
                }
                if (node.Nodes.Count > 0)
                {
                    this.FindNodeByValueInternal(node.Nodes, value);
                }
            }
        }

        public List<JQTreeNode> GetAllNodesFlat(List<JQTreeNode> nodes)
        {
            List<JQTreeNode> result = new List<JQTreeNode>();
            foreach (JQTreeNode node in nodes)
            {
                result.Add(node);
                if (node.Nodes.Count > 0)
                {
                    this.GetNodesFlat(node.Nodes, result);
                }
            }
            return result;
        }

        public JQTreeNodeDropEventArgs GetDragDropInfo()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string input = HttpContext.Current.Request["args"];
            JQTreeNodeDropEventArgs args = new JQTreeNodeDropEventArgs();
            return serializer.Deserialize<JQTreeNodeDropEventArgs>(input);
        }

        private void GetNodesFlat(List<JQTreeNode> nodes, List<JQTreeNode> result)
        {
            foreach (JQTreeNode node in nodes)
            {
                result.Add(node);
                if (node.Nodes.Count > 0)
                {
                    this.GetNodesFlat(node.Nodes, result);
                }
            }
        }

        private List<Hashtable> SerializeNodes(List<JQTreeNode> nodes)
        {
            List<Hashtable> list = new List<Hashtable>();
            foreach (JQTreeNode node in nodes)
            {
                list.Add(node.ToHashtable());
            }
            return list;
        }

        public bool CheckBoxes { get; set; }

        public TreeViewClientSideEvents ClientSideEvents { get; set; }

        public string DataUrl { get; set; }

        public bool DragAndDrop { get; set; }

        public string DragAndDropUrl { get; set; }

        public Unit Height { get; set; }

        public bool HoverOnMouseOver { get; set; }

        public string ID { get; set; }

        public bool MultipleSelect { get; set; }

        public string NodeTemplateID { get; set; }

        public Unit Width { get; set; }
    }
}

