using LinqToDB.Data;
using LinqToDB.Mapping;
using Npgsql;
using Npgsql.TypeMapping;

using NTSG = NetTopologySuite.Geometries.Geometry;

namespace LinqToDBPostGisNetTopologySuite
{
    /// <summary>
    /// Utility class with methods for registering PostGIS types mappings.
    /// </summary>
    public static class PostGisTypesMapper
    {
        /// <summary>
        /// Registers PostGIS types mappings for given <paramref name="dataConnection"/>.
        /// </summary>
        /// <remarks>
        /// Npgsql 7+ has no connection-level type mapping, so composite types (<see cref="ValidDetail"/>)
        /// must also be mapped with <see cref="MapPostGisTypes"/> on the data source or globally.
        /// </remarks>
        /// <param name="dataConnection">Database connection.</param>
        public static void RegisterPostGisMappings(this DataConnection dataConnection)
        {
            dataConnection.MappingSchema.SetConverter<object[], GeometryProcessing.MaximumInscribedCircleResult>(objs => CreateMaximumInscribedCircleResult(objs));
        }

        /// <summary>
        /// Maps PostGIS composite types on given <paramref name="typeMapper"/>, e.g. an <see cref="NpgsqlDataSourceBuilder"/>.
        /// </summary>
        /// <param name="typeMapper">Npgsql type mapper.</param>
        /// <returns>The same <paramref name="typeMapper"/> for chaining.</returns>
        public static INpgsqlTypeMapper MapPostGisTypes(this INpgsqlTypeMapper typeMapper)
        {
            typeMapper.MapComposite<ValidDetail>(ValidDetail.CompositeTypeName);
            return typeMapper;
        }

        /// <summary>
        /// Registers PostGIS types mappings globally for all database connections.
        /// </summary>
        public static void RegisterPostGisMappingsGlobally()
        {
#pragma warning disable CS0618 // global type mapping is obsolete since Npgsql 7 but still supported
            NpgsqlConnection.GlobalTypeMapper.MapPostGisTypes();
#pragma warning restore CS0618

            MappingSchema.Default.SetConverter<object[], GeometryProcessing.MaximumInscribedCircleResult>(objs => CreateMaximumInscribedCircleResult(objs));
        }

        private static GeometryProcessing.MaximumInscribedCircleResult CreateMaximumInscribedCircleResult(object[] objs) =>
            new GeometryProcessing.MaximumInscribedCircleResult
            {
                Center = objs[0] as NTSG,
                Nearest = objs[1] as NTSG,
                Radius = (double)objs[2],
            };
    }
}
