using AutoMapper;
using DHNet.Data.Core;
using DHNet.Data.Logging;
using DHNet.Objects;
using DHNet.Tests.Data;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;

namespace DHNet.Tests.Unit.Data.Core
{
    public class UnitOfWorkTests : IDisposable
    {
        private TestingContext context;
        private UnitOfWork unitOfWork;
        private IAuditLogger logger;
        private Role model;

        public UnitOfWorkTests()
        {
            context = new TestingContext();
            model = ObjectFactory.CreateRole();
            logger = Substitute.For<IAuditLogger>();
            unitOfWork = new UnitOfWork(context, logger);

            context.DropData();
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
            context.Dispose();
        }

        #region GetAs<TModel, TDestination>(Int32 id)

        [Fact]
        public void GetAs_ReturnsModelAsDestinationModelById()
        {
            context.Set<Role>().Add(model);
            context.SaveChanges();

            RoleView expected = Mapper.Map<RoleView>(context.Set<Role>().AsNoTracking().Single());
            RoleView actual = unitOfWork.GetAs<Role, RoleView>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Get<TModel>(Int32 id)

        [Fact]
        public void Get_ModelById()
        {
            context.Set<Role>().Add(model);
            context.SaveChanges();

            Role expected = context.Set<Role>().AsNoTracking().Single();
            Role actual = unitOfWork.Get<Role>(model.Id);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Get_NotFound_ReturnsNull()
        {
            Assert.Null(unitOfWork.Get<Role>(0));
        }

        #endregion

        #region To<TDestination>(Object source)

        [Fact]
        public void To_ConvertsSourceToDestination()
        {
            RoleView actual = unitOfWork.To<RoleView>(model);
            RoleView expected = Mapper.Map<RoleView>(model);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Select<TModel>()

        [Fact]
        public void Select_FromSet()
        {
            context.Set<Role>().Add(model);
            context.SaveChanges();

            IEnumerable<Role> actual = unitOfWork.Select<Role>();
            IEnumerable<Role> expected = context.Set<Role>();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region InsertRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void InsertRange_AddsModelsToDbSet()
        {
            IEnumerable<Role> models = new[] { ObjectFactory.CreateRole(1), ObjectFactory.CreateRole(2) };
            unitOfWork.InsertRange(models);

            IEnumerator<Role> actual = context.ChangeTracker.Entries<Role>().Select(entry => entry.Entity).GetEnumerator();
            IEnumerator<Role> expected = models.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(EntityState.Added, context.Entry(actual.Current).State);
                Assert.Same(expected.Current, actual.Current);
            }
        }

        #endregion

        #region Insert<TModel>(TModel model)

        [Fact]
        public void Insert_AddsModelToDbSet()
        {
            unitOfWork.Insert(model);

            Object actual = context.ChangeTracker.Entries<Role>().Single().Entity;
            Object expected = model;

            Assert.Equal(EntityState.Added, context.Entry(model).State);
            Assert.Same(expected, actual);
        }

        #endregion

        #region Update(TModel model)

        [Fact]
        public void Update_UpdatesNotAttachedModel()
        {
            model.Title += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<Role> actual = context.Entry(model);
            Role expected = model;

            Assert.Equal(expected.CreationDate, actual.Entity.CreationDate);
            Assert.Equal(expected.Title, actual.Entity.Title);
            Assert.Equal(EntityState.Modified, actual.State);
            Assert.Equal(expected.Id, actual.Entity.Id);
        }

        [Fact]
        public void Update_UpdatesAttachedModel()
        {
            Role attachedModel = ObjectFactory.CreateRole();
            context.Set<Role>().Add(attachedModel);
            model.Title += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<Role> actual = context.Entry(attachedModel);
            Role expected = model;

            Assert.Equal(expected.CreationDate, actual.Entity.CreationDate);
            Assert.Equal(expected.Title, actual.Entity.Title);
            Assert.Equal(EntityState.Modified, actual.State);
            Assert.Equal(expected.Id, actual.Entity.Id);
        }

        [Fact]
        public void Update_DoesNotModifyCreationDate()
        {
            unitOfWork.Update(model);

            Assert.False(context.Entry(model).Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region DeleteRange<TModel>(IEnumerable<TModel> models)

        [Fact]
        public void DeleteRange_Models()
        {
            IEnumerable<Role> models = new[] { ObjectFactory.CreateRole(1), ObjectFactory.CreateRole(2) };
            foreach (Role model in models)
                context.Set<Role>().Add(model);

            context.SaveChanges();

            unitOfWork.DeleteRange(models);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Delete<TModel>(TModel model)

        [Fact]
        public void Delete_Model()
        {
            context.Set<Role>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete(model);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Delete<TModel>(Int32 id)

        [Fact]
        public void Delete_ModelById()
        {
            context.Set<Role>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete<Role>(model.Id);
            unitOfWork.Commit();

            Assert.Empty(context.Set<Role>());
        }

        #endregion

        #region Rollback()

        [Fact]
        public void Rollback_Changes()
        {
            context.Set<Role>().Add(model);

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.Empty(unitOfWork.Select<Role>());
        }

        #endregion

        #region Commit()

        [Fact]
        public void Commit_SavesChanges()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Commit();

            context.Received().SaveChanges();
        }

        [Fact]
        public void Commit_Logs()
        {
            unitOfWork.Commit();

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.Received().Save();
        }

        [Fact]
        public void Commit_Failed_DoesNotSaveLogs()
        {
            unitOfWork.Insert(new Role { Title = new String('X', 513) });
            Exception exception = Record.Exception(() => unitOfWork.Commit());

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.DidNotReceive().Save();
            Assert.NotNull(exception);
        }

        #endregion

        #region Dispose()

        [Fact]
        public void Dispose_Logger()
        {
            unitOfWork.Dispose();

            logger.Received().Dispose();
        }

        [Fact]
        public void Dispose_Context()
        {
            TestingContext context = Substitute.For<TestingContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Dispose();

            context.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
