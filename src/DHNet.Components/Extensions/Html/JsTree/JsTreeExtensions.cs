using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace DHNet.Components.Extensions.Html
{
    public static class JsTreeExtensions
    {
        public static MvcHtmlString JsTreeFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, JsTree>> expression)
        {
            return JsTreeFor(ExpressionHelper.GetExpressionText(expression) + ".SelectedIds",
                ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model as JsTree);
        }

        private static MvcHtmlString JsTreeFor(String name, JsTree jsTree)
        {
            return new MvcHtmlString(FormIdsDiv(name, jsTree.SelectedIds) + FormTree(name, jsTree.Nodes));
        }
        private static String FormIdsDiv(String name, IEnumerable<Int32> selectedIds)
        {
            TagBuilder idsDiv = new TagBuilder("div");
            idsDiv.AddCssClass("js-tree-view-ids");

            StringBuilder hiddenInputs = new StringBuilder();
            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "hidden");
            input.MergeAttribute("name", name);

            foreach (Int32 id in selectedIds)
            {
                input.MergeAttribute("value", id.ToString(), true);
                hiddenInputs.Append(input.ToString(TagRenderMode.SelfClosing));
            }

            idsDiv.InnerHtml = hiddenInputs.ToString();

            return idsDiv.ToString();
        }
        private static String FormTree(String name, IList<JsTreeNode> nodes)
        {
            TagBuilder tree = new TagBuilder("div");
            tree.MergeAttribute("for", name);
            tree.AddCssClass("js-tree-view");

            AddNodes(tree, nodes);

            return tree.ToString();
        }
        private static void AddNodes(TagBuilder root, IList<JsTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            StringBuilder leafBuilder = new StringBuilder();
            TagBuilder branch = new TagBuilder("ul");

            foreach (JsTreeNode treeNode in nodes)
            {
                TagBuilder node = new TagBuilder("li");
                String id = treeNode.Id.ToString();
                node.InnerHtml = treeNode.Title;
                node.MergeAttribute("id", id);

                AddNodes(node, treeNode.Nodes);
                leafBuilder.Append(node);
            }

            branch.InnerHtml = leafBuilder.ToString();
            root.InnerHtml += branch.ToString();
        }
    }
}
