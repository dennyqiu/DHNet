using DHNet.Components.Extensions.Html;
using DHNet.Components.Security;
using DHNet.Resources;
using DHNet.Resources.Table;
using DHNet.Tests.Objects;
using NonFactors.Mvc.Grid;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Xunit;

namespace DHNet.Tests.Unit.Components.Extensions.Html
{
    public class MvcGridExtensionsTests : IDisposable
    {
        private IGridColumns<AllTypesView> columns;
        private IHtmlGrid<AllTypesView> html;

        public MvcGridExtensionsTests()
        {
            Grid<AllTypesView> grid = new Grid<AllTypesView>(new AllTypesView[0]);
            HtmlHelper htmlHelper = HtmlHelperFactory.CreateHtmlHelper();
            html = new HtmlGrid<AllTypesView>(htmlHelper, grid);
            columns = new GridColumns<AllTypesView>(grid);
        }
        public void Dispose()
        {
            Authorization.Provider = null;
        }

        #region AddActionLink<T>(this IGridColumns<T> columns, String action, String iconClass)

        [Fact]
        public void AddActionLink_Unauthorized_Empty()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();

            IGridColumn<AllTypesView> actual = columns.AddActionLink("Edit", "fa fa-pencil");

            Assert.Empty(actual.ValueFor(null).ToString());
            Assert.Empty(columns);
        }

        [Fact]
        public void AddActionLink_Authorized_Renders()
        {
            AllTypesView view = new AllTypesView();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();
            UrlHelper urlHelper = new UrlHelper(columns.Grid.HttpContext.Request.RequestContext);
            Authorization.Provider.IsAuthorizedFor(Arg.Any<Int32?>(), Arg.Any<String>(), Arg.Any<String>(), "Details").Returns(true);

            IGridColumn<AllTypesView> column = columns.AddActionLink("Details", "fa fa-info");

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_NullAuthorization_Renders()
        {
            Authorization.Provider = null;
            AllTypesView view = new AllTypesView();
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();
            UrlHelper urlHelper = new UrlHelper(columns.Grid.HttpContext.Request.RequestContext);

            IGridColumn<AllTypesView> column = columns.AddActionLink("Details", "fa fa-info");

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_NoKey_Throws()
        {
            IGrid<Object> grid = new Grid<Object>(new Object[0]);
            IGridColumns<Object> columns = new GridColumns<Object>(grid);
            columns.Grid.HttpContext = HttpContextFactory.CreateHttpContextBase();

            IGridColumn<Object> column = columns.AddActionLink("Delete", "fa fa-times");

            String actual = Assert.Throws<Exception>(() => column.ValueFor(new GridRow(new Object()))).Message;
            String expected = "Object type does not have a key property.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_Column()
        {
            IGridColumn<AllTypesView> actual = columns.AddActionLink("Edit", "fa fa-pencil");

            Assert.Equal("action-cell", actual.CssClasses);
            Assert.False(actual.IsEncoded);
            Assert.Single(columns);
        }

        #endregion

        #region AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> expression)

        [Fact]
        public void AddDateProperty_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime>(model => model.DateTimeField);
            Expression<Func<AllTypesView, DateTime>> expression = (model) => model.DateTimeField;

            IGridColumn<AllTypesView> actual = columns.AddDateProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal("{0:d}", actual.Format);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        #endregion

        #region AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> expression)

        [Fact]
        public void AddDateProperty_Nullable_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            IGridColumn<AllTypesView> actual = columns.AddDateProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal("{0:d}", actual.Format);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        #endregion

        #region AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean>> expression)

        [Fact]
        public void AddBooleanProperty_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean>(model => model.BooleanField);
            Expression<Func<AllTypesView, Boolean>> expression = (model) => model.BooleanField;

            IGridColumn<AllTypesView> actual = columns.AddBooleanProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        [Fact]
        public void AddBooleanProperty_RendersYes()
        {
            AllTypesView view = new AllTypesView { BooleanField = true };
            IGridColumn<AllTypesView> column = columns.AddBooleanProperty(model => model.BooleanField);

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = TableResources.Yes;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_RendersNo()
        {
            AllTypesView view = new AllTypesView { BooleanField = false };
            IGridColumn<AllTypesView> column = columns.AddBooleanProperty(model => model.BooleanField);

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = TableResources.No;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean?>> expression)

        [Fact]
        public void AddBooleanProperty_Nullable_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean?>(model => model.NullableBooleanField);
            Expression<Func<AllTypesView, Boolean?>> expression = (model) => model.NullableBooleanField;

            IGridColumn<AllTypesView> actual = columns.AddBooleanProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersYes()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = true };
            IGridColumn<AllTypesView> column = columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = TableResources.Yes;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersNo()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = false };
            IGridColumn<AllTypesView> column = columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = column.ValueFor(new GridRow(view)).ToString();
            String expected = TableResources.No;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersEmpty()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = null };
            IGridColumn<AllTypesView> column = columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = column.ValueFor(new GridRow(view)).ToString();

            Assert.Empty(actual);
        }

        #endregion

        #region AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> expression)

        [Fact]
        public void AddDateTimeProperty_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime>(model => model.DateTimeField);
            Expression<Func<AllTypesView, DateTime>> expression = (model) => model.DateTimeField;

            IGridColumn<AllTypesView> actual = columns.AddDateTimeProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal("{0:g}", actual.Format);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        #endregion

        #region AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> expression)

        [Fact]
        public void AddDateTimeProperty_Nullable_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            IGridColumn<AllTypesView> actual = columns.AddDateTimeProperty(expression);

            Assert.Equal("text-center", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal("{0:g}", actual.Format);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        #endregion

        #region AddProperty<T, TProperty>(this IGridColumns<T> columns, Expression<Func<T, TProperty>> expression)

        [Fact]
        public void AddProperty_Column()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, AllTypesView>(model => model);
            Expression<Func<AllTypesView, AllTypesView>> expression = (model) => model;

            IGridColumn<AllTypesView> actual = columns.AddProperty(expression);

            Assert.Equal("text-left", actual.CssClasses);
            Assert.Equal(expression, actual.Expression);
            Assert.Equal(title, actual.Title);
            Assert.Single(columns);
        }

        [Fact]
        public void AddProperty_SetsCssClassForEnum()
        {
            AssertCssClassFor(model => model.EnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSByte()
        {
            AssertCssClassFor(model => model.SByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForByte()
        {
            AssertCssClassFor(model => model.ByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt16()
        {
            AssertCssClassFor(model => model.Int16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt16()
        {
            AssertCssClassFor(model => model.UInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt32()
        {
            AssertCssClassFor(model => model.Int32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt32()
        {
            AssertCssClassFor(model => model.UInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForInt64()
        {
            AssertCssClassFor(model => model.Int64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForUInt64()
        {
            AssertCssClassFor(model => model.UInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForSingle()
        {
            AssertCssClassFor(model => model.SingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDouble()
        {
            AssertCssClassFor(model => model.DoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDecimal()
        {
            AssertCssClassFor(model => model.DecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForBoolean()
        {
            AssertCssClassFor(model => model.BooleanField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForDateTime()
        {
            AssertCssClassFor(model => model.DateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableEnum()
        {
            AssertCssClassFor(model => model.NullableEnumField, "text-left");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSByte()
        {
            AssertCssClassFor(model => model.NullableSByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableByte()
        {
            AssertCssClassFor(model => model.NullableByteField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt16()
        {
            AssertCssClassFor(model => model.NullableInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt16()
        {
            AssertCssClassFor(model => model.NullableUInt16Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt32()
        {
            AssertCssClassFor(model => model.NullableInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt32()
        {
            AssertCssClassFor(model => model.NullableUInt32Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableInt64()
        {
            AssertCssClassFor(model => model.NullableInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableUInt64()
        {
            AssertCssClassFor(model => model.NullableUInt64Field, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableSingle()
        {
            AssertCssClassFor(model => model.NullableSingleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDouble()
        {
            AssertCssClassFor(model => model.NullableDoubleField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDecimal()
        {
            AssertCssClassFor(model => model.NullableDecimalField, "text-right");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableBoolean()
        {
            AssertCssClassFor(model => model.NullableBooleanField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForNullableDateTime()
        {
            AssertCssClassFor(model => model.NullableDateTimeField, "text-center");
        }

        [Fact]
        public void AddProperty_SetsCssClassForOtherTypes()
        {
            AssertCssClassFor(model => model.StringField, "text-left");
        }

        #endregion

        #region ApplyDefaults<T>(this IHtmlGrid<T> grid)

        [Fact]
        public void ApplyDefaults_Values()
        {
            IGridColumn column = html.Grid.Columns.Add(model => model.ByteField);
            column.IsFilterable = null;
            column.IsSortable = null;

            IGrid actual = html.ApplyDefaults().Grid;

            Assert.Equal(TableResources.NoDataFound, actual.EmptyText);
            Assert.Equal("table-hover", actual.CssClasses);
            Assert.Equal(true, column.IsFilterable);
            Assert.Equal(true, column.IsSortable);
            Assert.Equal("AllTypes", actual.Name);
            Assert.NotEmpty(actual.Columns);
        }

        #endregion

        #region Test helpers

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, String cssClasses)
        {
            IGridColumn<AllTypesView> column = columns.AddProperty(property);

            String actual = column.CssClasses;
            String expected = cssClasses;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
