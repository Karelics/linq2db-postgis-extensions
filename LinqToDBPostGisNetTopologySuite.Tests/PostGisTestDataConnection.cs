using LinqToDB;
using LinqToDB.Data;

using LinqToDBPostGisNetTopologySuite.Tests.Entities;

namespace LinqToDBPostGisNetTopologySuite.Tests
{
    public class PostGisTestDataConnection : DataConnection
    {
        public PostGisTestDataConnection(string connectionString) :
            base("PostgreSQL", connectionString)
        {

        }

        public ITable<OwmCityEntity> OwmCities { get { return this.GetTable<OwmCityEntity>(); } }

        public ITable<TestGeometryEntity> TestGeometries { get { return this.GetTable<TestGeometryEntity>(); } }

        public ITable<TestGeographyEntity> TestGeographies { get { return this.GetTable<TestGeographyEntity>(); } }
    }
}
