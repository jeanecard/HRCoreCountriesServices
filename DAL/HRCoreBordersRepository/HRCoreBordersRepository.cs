using Dapper;
using HRConverters;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersRepository
{
    public class CoreBordersRepository : IHRCoreBordersRepository
    {
        private static readonly String _SQLQUERY = " SELECT wkb_geometry, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";
        private static readonly String _SQLQUERYFORDAPPER = " SELECT ST_AsText(wkb_geometry) AS WKT_GEOMETRY, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";
        private readonly IConfiguration _config = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";
        /// <summary>
        /// SQL Query for manual query
        /// </summary>
        public static string SQLQUERY => _SQLQUERY;
        /// <summary>
        /// SQL Query for Dapper
        /// </summary>
        public static string SQLQUERYFORDAPPER => _SQLQUERYFORDAPPER;

        /// <summary>
        /// Dummy default constructor. Private for DI.
        /// </summary>
        private CoreBordersRepository()
        {
            //Dummy.
        }
        /// <summary>
        /// Constructor for DI with Configuration dependency.
        /// </summary>
        /// <param name="config"></param>
        public CoreBordersRepository(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Call the ReaderMethod.
        /// </summary>
        /// <param name="borderID">an optionnal borderID</param>
        /// <returns>A collection with the Border with the borderID querried or all Borders if borderID is not supplied.</returns>
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID = null)
        {
            Task<IEnumerable<HRBorder>> retour = ReadBordersWithDapperAsync(borderID);
            await retour;
            return retour.Result;
        }

        /// <summary>
        /// Mapping from DataBase with Dapper
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        private async Task<IEnumerable<HRBorder>> ReadBordersWithDapperAsync(String borderID = null)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {

                conn.Open();
                try
                {
                    Task<IEnumerable<HRBorder>> retour = conn.QueryAsync<HRBorder>(GetSQLQuery(true, borderID));
                    await retour;
                    return retour.Result;
                }
                catch(Exception)
                {
                    //!log here. 
                    throw;
                }
            }
        }
        /// <summary>
        ///  Mapping from DataBase without Dapper.
        ///  Method first done for testsing purpose.
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        [Obsolete("ReadBordersAsync is deprecated, please use ReadBordersWithDapperAsync instead.")]
        #pragma warning disable IDE0051 // Disable warning unused members as this method is kept for historical reasons
        private async Task<IEnumerable<HRBorder>> ReadBordersAsync(String borderID = null)
        #pragma warning restore IDE0051 // Re-enable previous Warning.
        {
            List<HRBorder> retour = new List<HRBorder>();
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                conn.TypeMapper.UseLegacyPostgis();
                // Retrieve all rows
                string query = GetSQLQuery(false, borderID);
                using (var cmd = new NpgsqlCommand(query, conn))
                using (Task<DbDataReader> readerTask = cmd.ExecuteReaderAsync())
                {
                    await readerTask;
                    NpgsqlDataReader reader = (NpgsqlDataReader)readerTask.Result;
                    PostGisFieldValueGetter readerFacade = new PostGisFieldValueGetter(reader);
                    Task<bool> reading = reader.ReadAsync();
                    await reading;
                    while (reading.Result)
                    {
                        HRBorder modeli = new HRBorder() { WKB_GEOMETRY = HRConverterPostGisToNetTopologySuite.ConvertFrom(readerFacade, 0) };
                        bool columnIsNull = await reader.IsDBNullAsync(1);
                        if (!columnIsNull)
                        {
                            modeli.FIPS = await reader.GetFieldValueAsync<String>(1);
                        }
                        columnIsNull = await reader.IsDBNullAsync(2);
                        if (!columnIsNull)
                        {
                            modeli.ISO2 = await reader.GetFieldValueAsync<String>(2);
                        }
                        columnIsNull = await reader.IsDBNullAsync(3);
                        if (!columnIsNull)
                        {
                            modeli.ISO3 = await reader.GetFieldValueAsync<String>(3);
                        }
                        columnIsNull = await reader.IsDBNullAsync(4);
                        if (!columnIsNull)
                        {
                            modeli.UN = await reader.GetFieldValueAsync<short>(4);
                        }
                        columnIsNull = await reader.IsDBNullAsync(5);
                        if (!columnIsNull)
                        {
                            modeli.NAME = await reader.GetFieldValueAsync<String>(5);
                        }
                        columnIsNull = await reader.IsDBNullAsync(6);
                        if (!columnIsNull)
                        {
                            modeli.AREA = await reader.GetFieldValueAsync<double>(6);
                        }
                        columnIsNull = await reader.IsDBNullAsync(7);
                        if (!columnIsNull)
                        {
                            modeli.POP2005 = await reader.GetFieldValueAsync<int>(7);
                        }
                        modeli.REGION = "Not converted";
                        modeli.SUBREGION = "Not converted";
                        columnIsNull = await reader.IsDBNullAsync(10);
                        if (!columnIsNull)
                        {
                            modeli.LON = await reader.GetFieldValueAsync<double>(10);
                        }
                        columnIsNull = await reader.IsDBNullAsync(11);
                        if (!columnIsNull)
                        {
                            modeli.LAT = await reader.GetFieldValueAsync<double>(11);
                        }
                        retour.Add(modeli);
                        reading = reader.ReadAsync();
                    }
                }
            }
            return retour;
        }
#pragma warning restore 0169

        /// <summary>
        /// Generate SQLQuery with WHERE clause if necessary on borderID.
        /// </summary>
        /// <param name="isforDapper">true if the query has to be run by Dapper.</param>
        /// <param name="borderID">borderID</param>
        /// <returns>SQLQuery to be run</returns>
        public string GetSQLQuery(bool isforDapper, String borderID = null)
        {
            StringBuilder sb = new StringBuilder();
            if(isforDapper)
            {
                sb.Append(SQLQUERYFORDAPPER);
            }
            else
            {
                sb.Append(SQLQUERY);
            }
           
            if (borderID != null)
            {
                sb.Append("WHERE FIPS = '");
                sb.Append(borderID);
                sb.Append("'");
            }
            return sb.ToString(); ;
        }
    }
}
