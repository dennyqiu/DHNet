using DHNet.Data.Logging;
using DHNet.Objects;
using DHNet.Tests.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Xunit;

namespace DHNet.Tests.Unit.Data.Logging
{
    public class LoggableEntityTests : IDisposable
    {
        private DbEntityEntry<BaseModel> entry;
        private TestingContext context;
        private Role model;

        public LoggableEntityTests()
        {
            using (context = new TestingContext())
            {
                context.DropData();
                SetUpData();
            }

            context = new TestingContext();
            model = context.Set<Role>().Single();
            entry = context.Entry<BaseModel>(model);
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region LoggableEntity(DbEntityEntry<BaseModel> entry)

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAddedEntity()
        {
            entry.State = EntityState.Added;

            AsssertProperties(entry.CurrentValues);
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForModifiedEntity()
        {
            String title = model.Title;
            entry.State = EntityState.Modified;
            entry.CurrentValues["Title"] = "Role";
            entry.OriginalValues["Title"] = "Role";

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Title"), title) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForAttachedEntity()
        {
            context.Dispose();
            String title = model.Title;
            context = new TestingContext();
            context.Set<Role>().Attach(model);

            entry = context.Entry<BaseModel>(model);
            entry.OriginalValues["Title"] = "Role";
            entry.CurrentValues["Title"] = "Role";
            entry.State = EntityState.Modified;

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Title"), title) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Fact]
        public void LoggableEntity_CreatesPropertiesForDeletedEntity()
        {
            entry.State = EntityState.Deleted;

            AsssertProperties(entry.GetDatabaseValues());
        }

        [Fact]
        public void LoggableEntity_SetsAction()
        {
            entry.State = EntityState.Deleted;

            String actual = new LoggableEntity(entry).Action;
            String expected = entry.State.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityTypeNameFromProxy()
        {
            String actual = new LoggableEntity(entry).Name;
            String expected = typeof(Role).Name;

            Assert.Equal("System.Data.Entity.DynamicProxies", entry.Entity.GetType().Namespace);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityTypeName()
        {
            entry = context.Entry<BaseModel>(context.Set<Role>().Add(new Role()));

            String actual = new LoggableEntity(entry).Name;
            String expected = typeof(Role).Name;

            Assert.NotEqual("System.Data.Entity.DynamicProxies", entry.Entity.GetType().Namespace);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoggableEntity_SetsEntityId()
        {
            Int32 actual = new LoggableEntity(entry).Id();
            Int32 expected = model.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region ToString()

        [Fact]
        public void ToString_FormsEntityChanges()
        {
            StringBuilder changes = new StringBuilder();
            LoggableEntity loggableEntity = new LoggableEntity(entry);
            foreach (LoggableProperty property in loggableEntity.Properties)
                changes.AppendFormat("{0}{1}", property, Environment.NewLine);

            String actual = loggableEntity.ToString();
            String expected = changes.ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            context.Set<Role>().Add(ObjectFactory.CreateRole());
            context.SaveChanges();
        }

        private void AsssertProperties(DbPropertyValues originalValues)
        {
            IEnumerable<String> properties = originalValues.PropertyNames;

            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();
            IEnumerator<LoggableProperty> expected = properties.Where(property => property != "Id")
                .Select(name => new LoggableProperty(entry.Property(name), originalValues[name])).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.IsModified, actual.Current.IsModified);
                Assert.Equal(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        #endregion
    }
}
