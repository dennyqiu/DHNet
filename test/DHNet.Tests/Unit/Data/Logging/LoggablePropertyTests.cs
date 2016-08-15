using DHNet.Data.Logging;
using DHNet.Objects;
using DHNet.Tests.Data;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Xunit;

namespace DHNet.Tests.Unit.Data.Logging
{
    public class LoggablePropertyTests
    {
        private DbPropertyEntry textProperty;
        private DbPropertyEntry dateProperty;

        public LoggablePropertyTests()
        {
            using (TestingContext context = new TestingContext())
            {
                Role model = ObjectFactory.CreateRole();

                context.Set<Role>().Add(model);
                context.Entry(model).State = EntityState.Modified;
                textProperty = context.Entry(model).Property(prop => prop.Title);
                dateProperty = context.Entry(model).Property(prop => prop.CreationDate);
            }
        }

        #region LoggableProperty(DbPropertyEntry entry, Object originalValue)

        [Fact]
        public void LoggableProperty_IsNotModified()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = false;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_DifferentValues_IsNotModified()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_SameValues_IsNotModified()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = true;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_DifferentValues_IsModified()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            Assert.True(new LoggableProperty(textProperty, "Original").IsModified);
        }

        #endregion

        #region ToString()

        [Fact]
        public void ToString_Modified_CurrentValueNull()
        {
            textProperty.CurrentValue = null;
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "\"Original\"", "null");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Modified_OriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "null", "\"Current\"");
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Modified_Date()
        {
            dateProperty.CurrentValue = new DateTime(2014, 6, 8, 14, 16, 19);
            dateProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", dateProperty.Name, "\"2010-04-03 18:33:17\"", "\"2014-06-08 14:16:19\"");
            String actual = new LoggableProperty(dateProperty, new DateTime(2010, 4, 3, 18, 33, 17)).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Modified_Json()
        {
            textProperty.CurrentValue = "Current\r\nValue";
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "157.45", "\"Current\\r\\nValue\"");
            String actual = new LoggableProperty(textProperty, 157.45).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_NotModified()
        {
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "\"Original\"");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_NotModified_OriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "null");
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_NotModified_Date()
        {
            dateProperty.CurrentValue = new DateTime(2014, 6, 8, 14, 16, 19);
            dateProperty.IsModified = false;

            String actual = new LoggableProperty(dateProperty, new DateTime(2014, 6, 8, 14, 16, 19)).ToString();
            String expected = String.Format("{0}: {1}", dateProperty.Name, "\"2014-06-08 14:16:19\"");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_NotModified_Json()
        {
            textProperty.CurrentValue = "Current\r\nValue";
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "\"Original\\r\\nValue\"");
            String actual = new LoggableProperty(textProperty, "Original\r\nValue").ToString();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
