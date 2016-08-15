using DHNet.Components.Extensions.Html;
using Xunit;

namespace DHNet.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeNodeTests
    {
        #region JsTreeNode(String title)

        [Fact]
        public void JsTreeNode_SetsTitle()
        {
            JsTreeNode actual = new JsTreeNode("Title");

            Assert.Equal("Title", actual.Title);
            Assert.Empty(actual.Nodes);
            Assert.Null(actual.Id);
        }

        #endregion

        #region JsTreeNode(Int32? id, String title)

        [Fact]
        public void JsTreeNode_SetsIdAndTitle()
        {
            JsTreeNode actual = new JsTreeNode(1, "Title");

            Assert.Equal("Title", actual.Title);
            Assert.Equal(1, actual.Id);
            Assert.Empty(actual.Nodes);
        }

        #endregion
    }
}
