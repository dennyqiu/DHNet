using DHNet.Components.Extensions.Html;
using Xunit;

namespace DHNet.Tests.Unit.Components.Extensions.Html
{
    public class JsTreeTests
    {
        #region JsTree()

        [Fact]
        public void JsTree_CreatesEmpty()
        {
            JsTree actual = new JsTree();

            Assert.Empty(actual.Nodes);
            Assert.Empty(actual.SelectedIds);
        }

        #endregion
    }
}
