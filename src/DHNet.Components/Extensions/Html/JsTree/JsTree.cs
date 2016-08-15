using System;
using System.Collections.Generic;

namespace DHNet.Components.Extensions.Html
{
    public class JsTree
    {
        public IList<JsTreeNode> Nodes { get; set; }
        public IList<Int32> SelectedIds { get; set; }

        public JsTree()
        {
            Nodes = new List<JsTreeNode>();
            SelectedIds = new List<Int32>();
        }
    }
}
