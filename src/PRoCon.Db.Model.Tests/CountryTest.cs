namespace PRoCon.Db.Tests.Domain
{
    using System;
    using Db.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class CountryTest : PRoConDbEntityTestBase<Country>
    {
        public override Country CreateEntity()
        {
            return new Country() {Name = "Germany"};
        }

        public override void AssertEntity(Country entity)
        {
            Assert.AreEqual("Germany", entity.Name);
        }

        public override void UpdateEntity(Country entity)
        {
            entity.Name = "USA";
        }

        protected override void AssertUpdatedEntity(Country entity)
        {
            Assert.AreEqual("USA", entity.Name);
        }
    }
}