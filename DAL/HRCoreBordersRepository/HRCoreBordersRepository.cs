using HRConverters;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using NetTopologySuite.Geometries;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace HRCoreBordersRepository
{
    public class CoreBordersRepository : IHRCoreBordersRepository
    {
        /// <summary>
        /// //!TODO use dapper
        /// //!TODO Try Catch
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetBorders(int? borderID = null)
        {
            List<HRBorder> retour = new List<HRBorder>();
            var connString = "host = db.qgiscloud.com; Username = gxxawt_obddnf; Password = 8d2b58e2; Database = gxxawt_obddnf";
            //Host = myserver; Username = mylogin; Password = mypass; Database = mydatabase
            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                conn.TypeMapper.UseLegacyPostgis();
                //conn.TypeMapper.UseLegacyPostgis();
                // Retrieve all rows
                string query = " SELECT wkb_geometry, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries";
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
                        HRBorder modeli = new HRBorder();
                        modeli.BorderGeometry = HRConverterPostGisToNetTopologySuite.ConvertFrom(readerFacade, 0);
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
                        modeli.REGION = "Non convertie (int au lieuy de String)";// await reader.GetFieldValueAsync<String>(8);
                        modeli.SUBREGION = "Non convertie (int au lieuy de String)";// await reader.GetFieldValueAsync<String>(9);
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
    }
}
