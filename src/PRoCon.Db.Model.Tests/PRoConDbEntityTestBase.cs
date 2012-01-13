using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Tests.Domain
{
    using NUnit.Framework;

    [TestFixture]
    public abstract class PRoConDbEntityTestBase<T> : PRoConDbTestBase
    {
        /// <summary>
        /// Creates an Entity for Basic Test
        /// </summary>
        /// <returns></returns>
        public abstract T CreateEntity ();

        /// <summary>
        /// Checks the Entity for correct data
        /// </summary>
        /// <param name="entity"></param>
        public abstract void AssertEntity (T entity);

        /// <summary>
        /// Should Change the Entity
        /// </summary>
        /// <param name="entity"></param>
        public abstract void UpdateEntity (T entity);

        [Test]
        public void GenericMappingTest()
        {
            var entity = this.CreateEntity();
            var session = this.Provider.Session;
            var id = session.Save(entity);
            session.Flush();
            session.Evict(entity);

            session = this.Provider.Session;
            entity = session.Get<T>(id);
            this.AssertEntity(entity);

            this.UpdateEntity(entity);
            id = session.Save(entity);
            session.Flush();
            session.Evict(entity);

            session = this.Provider.Session;
            entity = session.Get<T>(id);
            this.AssertUpdatedEntity(entity);

            session = this.Provider.Session;
            session.Delete(entity);
            session.Flush();
            session.Evict(entity);

            session = this.Provider.Session;
            entity = session.Get<T>(id);
            Assert.IsNull(entity);
        }

        protected abstract void AssertUpdatedEntity (T entity);
    }
}
