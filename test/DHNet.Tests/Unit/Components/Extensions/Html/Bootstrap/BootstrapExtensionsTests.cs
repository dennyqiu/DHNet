using DHNet.Components.Extensions.Html;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Xunit;

namespace DHNet.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapExtensionsTests
    {
        private HtmlHelper<BootstrapModel> html;
        private BootstrapModel model;

        public BootstrapExtensionsTests()
        {
            html = HtmlHelperFactory.CreateHtmlHelper(new BootstrapModel());
            model = html.ViewData.Model;
        }

        #region FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Boolean? required = null)

        [Fact]
        public void FormLabelFor_NotMemberExpression_Throws()
        {
            Exception exception = Assert.Throws<InvalidOperationException>(() => html.FormLabelFor(expression => expression.GetType(), required: true));

            String expected = "Expression must be a member expression.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_OverwrittenNotRequiredExpression()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequired, required: true).ToString();
            String expected =
                "<label for=\"Relation_NotRequired\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_OverwrittenRequiredExpression()
        {
            String actual = html.FormLabelFor(x => x.Relation.Required, required: false).ToString();
            String expected =
                "<label for=\"Relation_Required\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_RequiredExpression()
        {
            String actual = html.FormLabelFor(x => x.Relation.Required).ToString();
            String expected =
                "<label for=\"Relation_Required\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_ValueType()
        {
            String actual = html.FormLabelFor(x => x.Relation.Number).ToString();
            String expected =
                "<label for=\"Relation_Number\">" +
                    "<span class=\"require\">*</span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_NotRequiredExpression()
        {
            String actual = html.FormLabelFor(x => x.Relation.NotRequired).ToString();
            String expected =
                "<label for=\"Relation_NotRequired\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormLabelFor_NullableValueType()
        {
            String actual = html.FormLabelFor(x => x.Relation.Date).ToString();
            String expected =
                "<label for=\"Relation_Date\">" +
                    "<span class=\"require\"></span>" +
                "</label>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextBoxFor_Expression()
        {
            String actual = html.FormTextBoxFor(x => x.Number).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Number\" name=\"Number\" type=\"text\" value=\"{0}\" />",
                model.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_EditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_NotEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)

        [Fact]
        public void FormTextBoxFor_FormattedEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, "{0}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_FormattedNotEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_FormattedExpression()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}").ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_MergesClassAttributes()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_OverwrittenAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_OverwrittenReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedNotEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)

        [Fact]
        public void FormTextBoxFor_AttributedFormat_MergesClassAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.Number, "{0:0.00}", new { @class = "test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control test\" id=\"Relation_Number\" name=\"Relation.Number\" type=\"text\" value=\"{0}\" />",
                String.Format("{0:0.00}", model.Relation.Number));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedFormat_OverwritesAutocompleteAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.Relation.NotRequired, "{0}", new { autocomplete = "on" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"on\" class=\"form-control\" id=\"Relation_NotRequired\" name=\"Relation.NotRequired\" type=\"text\" value=\"{0}\" />",
                model.Relation.NotRequired);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedFormat_OverwritesReadOnlyAttribute()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}", new { @readonly = "false" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"false\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedFormat_EditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableTrue, "{0}", new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableTrue\" name=\"EditableTrue\" type=\"text\" value=\"{0}\" />",
                model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextBoxFor_AttributedFormat_NotEditableExpression()
        {
            String actual = html.FormTextBoxFor(x => x.EditableFalse, "{0}", new { data_tab = "Test" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control\" data-tab=\"Test\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)

        [Fact]
        public void FormPasswordFor_Expression()
        {
            String expected = "<input autocomplete=\"off\" class=\"form-control\" id=\"Relation_Required\" name=\"Relation.Required\" type=\"password\" />";
            String actual = html.FormPasswordFor(x => x.Relation.Required).ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormTextAreaFor_Expression()
        {
            String actual = html.FormTextAreaFor(x => x.Number).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"Number\" name=\"Number\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_EditableExpression()
        {
            String actual = html.FormTextAreaFor(x => x.EditableTrue).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableTrue\" name=\"EditableTrue\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_NotEditableExpression()
        {
            String actual = html.FormTextAreaFor(x => x.EditableFalse).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormTextAreaFor_AttributedEditableExpression()
        {
            String actual = html.FormTextAreaFor(x => x.EditableTrue, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control test\" cols=\"20\" id=\"EditableTrue\" name=\"EditableTrue\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableTrue);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_AttributedNotEditableExpression()
        {
            String actual = html.FormTextAreaFor(x => x.EditableFalse, new { @class = "test" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control test\" cols=\"20\" id=\"EditableFalse\" name=\"EditableFalse\" readonly=\"readonly\" rows=\"6\">{0}</textarea>",
                Environment.NewLine + model.EditableFalse);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormTextAreaFor_OverwrittenRows()
        {
            String actual = html.FormTextAreaFor(x => x.NotRequired, new { rows = "12" }).ToString();
            String expected = String.Format(
                "<textarea autocomplete=\"off\" class=\"form-control\" cols=\"20\" id=\"NotRequired\" name=\"NotRequired\" rows=\"12\">{0}</textarea>",
                Environment.NewLine + model.NotRequired);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDatePickerFor_Expression()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDatePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDatePickerFor_AttributedExpression()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDatePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)

        [Fact]
        public void FormDateTimePickerFor_Expression()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd H:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)

        [Fact]
        public void FormDateTimePickerFor_AttributedExpression()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");

            String actual = html.FormDateTimePickerFor(x => x.Relation.Date, new { @readonly = "readonly" }).ToString();
            String expected = String.Format(
                "<input autocomplete=\"off\" class=\"form-control datetimepicker\" id=\"Relation_Date\" name=\"Relation.Date\" readonly=\"readonly\" type=\"text\" value=\"{0}\" />",
                model.Relation.Date.Value.ToString("yyyy/MM/dd H:mm"));

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
