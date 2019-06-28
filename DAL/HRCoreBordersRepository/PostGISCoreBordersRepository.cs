using Dapper;
using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRConverters;
using HRCoreBordersModel;
using HRCoreRepository.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersRepository
{
    public class PostGISCoreBordersRepository : IHRCoreRepository<HRBorder>, ISortable, IPaginable
    {
        private static readonly String _SQLQUERY = " SELECT wkb_geometry, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";
        private static readonly String _SQLQUERYFORDAPPER = " SELECT ST_AsText(wkb_geometry) AS WKT_GEOMETRY, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";
        private readonly IConfiguration _config = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";
        /// <summary>
        /// List of field available for ORDER part of SQL Query. Prefered to Reflection for performance reasons.
        /// </summary>
        private readonly Dictionary<String, int> _whiteListOfAvaialbleFields = new Dictionary<String, int>();

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
        private PostGISCoreBordersRepository()
        {
            //Dummy.
        }
        /// <summary>
        /// Constructor for DI with Configuration dependency.
        /// 1- Set Config.
        /// 2- Add field avalaible for ORDER QUERY.
        /// </summary>
        /// <param name="config"></param>
        public PostGISCoreBordersRepository(IConfiguration config)
        {
            //1-
            _config = config;
            //2-
            _whiteListOfAvaialbleFields.Add("WKT_GEOMETRY", 1);
            _whiteListOfAvaialbleFields.Add("FIPS", 1);
            _whiteListOfAvaialbleFields.Add("ISO2", 1);
            _whiteListOfAvaialbleFields.Add("ISO3", 1);
            _whiteListOfAvaialbleFields.Add("UN", 1);
            _whiteListOfAvaialbleFields.Add("NAME", 1);
            _whiteListOfAvaialbleFields.Add("AREA", 1);
            _whiteListOfAvaialbleFields.Add("POP2005", 1);
            _whiteListOfAvaialbleFields.Add("REGION", 1);
            _whiteListOfAvaialbleFields.Add("SUBREGION", 1);
            _whiteListOfAvaialbleFields.Add("LON", 1);
            _whiteListOfAvaialbleFields.Add("LAT", 1);
        }
        /// <summary>
        /// Call the ReaderMethod.
        /// 1- Get the Result Action
        /// 2- Return the first element or null.
        /// TODO UT this please !!
        /// </summary>
        /// <param name="borderID">the borderID to look for.</param>
        /// <returns>A collection with the Border with the borderID querried or all Borders if borderID is not supplied.</returns>
        public async Task<HRBorder> GetAsync(string borderID) 
        {
            HRBorder retour = null;
            //1-
            Task<IEnumerable<HRBorder>> bordersAction = ReadBordersWithDapperAsync(borderID);
            await bordersAction;
            if (bordersAction.Result != null)
            {
                List<HRBorder> borderList = bordersAction.Result.ToList<HRBorder>();
                //2-
                if (borderList != null && borderList.Count > 0)
                {
                    retour = borderList.First<HRBorder>();
                }
            }
            return retour;
        }

        /// <summary>
        /// Mapping from DataBase with Dapper
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        private async Task<IEnumerable<HRBorder>> ReadBordersWithDapperAsync(String borderID)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {

                conn.Open();
                try
                {
                    Task<IEnumerable<HRBorder>> retour = conn.QueryAsync<HRBorder>(GetSQLQuery(true, borderID, null));
                    await retour;
                    return retour.Result;
                }
                catch (Exception)
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
                    //!TODO dispose ...
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
        /// Generate SQLQuery with WHERE clause if necessary on borderID and create ORder BY Clause.
        /// Where clause is protected with Parameter to avoid SQL Injection
        /// Order By clause is checked against a White List of field available.
        /// </summary>
        /// <param name="isforDapper">true if the query has to be run by Dapper.</param>
        /// <param name="borderID">borderID</param>
        /// <returns>SQLQuery to be run or Exception if could not convert to a propoer SQL query (including revention agains t SQL Injection)</returns>
        public string GetSQLQuery(bool isforDapper, String borderID = null, HRSortingParamModel orderBy = null, PagingParameterInModel pageInModel = null)
        {
            StringBuilder sb = new StringBuilder();
            if (isforDapper)
            {
                sb.Append(SQLQUERYFORDAPPER);
            }
            else
            {
                sb.Append(SQLQUERY);
            }
            
            if (!String.IsNullOrEmpty(borderID)
                && borderID.Length == 2)
            {
                sb.Append("WHERE FIPS = '");
                //Cheat Code to avoid SQL injection. Indeed pbm with SQL Command and SQLPArameters on GeometryColumn with postgis.
                sb.Append(borderID.Substring(0, 2));
                sb.Append("'");

            }
            if(pageInModel != null && orderBy == null)
            {
                //Set default order by as pagination required order by 
                orderBy = new HRSortingParamModel() { OrderBy = "FIPS;ASC" };
            }
            if (orderBy != null)
            {
                IEnumerable<(String, String)> orders = HRSortingParamModelDeserializer.GetFieldOrders(orderBy);
                if (orders != null)
                {
                    List<(String, String)> ordersList = orders.ToList();
                    if (ordersList != null)
                    {
                        int itemCount = ordersList.Count;
                        for (int i = 0; i < itemCount; i++)
                        {
                            String fieldNamei = ordersList[i].Item1;
                            if (!String.IsNullOrEmpty(fieldNamei)
                                && _whiteListOfAvaialbleFields != null
                                && _whiteListOfAvaialbleFields.ContainsKey(fieldNamei.ToUpper()))
                            {
                                if (i == 0)
                                {
                                    sb.Append("ORDER BY ");
                                }
                                sb.Append(fieldNamei);
                                sb.Append(" ");
                                sb.Append(ordersList[i].Item2);
                                sb.Append(" ");
                            }
                            else
                            {
                                throw new InvalidOperationException("Field unknow to sort on : " + fieldNamei);
                            }
                        }
                    }
                }
                if (pageInModel != null)
                {
                    sb.Append(" LIMIT ");
                    if (pageInModel.PageSize > 0)
                    {
                        sb.Append(pageInModel.PageSize.ToString());
                    }
                    else
                    {
                        sb.Append("ALL");
                    }
                    sb.Append(" OFFSET ");
                    sb.Append(pageInModel.PageNumber * pageInModel.PageSize);
                    sb.Append(" ");

                }
            }
           
            return sb.ToString();
        }
        /// <summary>
        /// Return true as PostGIS/Postgres are sortable.
        /// </summary>
        /// <returns>true</returns>
        public bool IsSortable()
        {
            return true;
        }
        /// <summary>
        /// Return true as PostGis can Paginate.
        /// </summary>
        /// <returns>Always true</returns>
        public bool IsPaginable()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    Task<IEnumerable<HRBorder>> retour = conn.QueryAsync<HRBorder>(GetSQLQuery(true, null, orderBy));
                    await retour;
                    return retour.Result;
                }
                catch (Exception)
                {
                    //!log here. 
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetFullsAsync()
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    Task<IEnumerable<HRBorder>> retour = conn.QueryAsync<HRBorder>(GetSQLQuery(true, null, null));
                    await retour;
                    return retour.Result;
                }
                catch (Exception)
                {
                    //!log here. 
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            String queryString = GetSQLQuery(true, null, orderBy, pageModel);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    Task<int> totalCountTask = conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM boundaries");
                    await totalCountTask;
                    int totalItemsCount = totalCountTask.Result;
                    //!Add a tu on this part
                    if (pageModel.PageNumber * pageModel.PageSize <= totalItemsCount)
                    {
                        Task<IEnumerable<HRBorder>> queryTask = conn.QueryAsync<HRBorder>(queryString);
                        await queryTask;
                        PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>()
                        {
                            PageItems = queryTask.Result,
                            PageSize = pageModel.PageSize,
                            TotalItemsCount = totalItemsCount,
                            CurrentPage = pageModel.PageNumber
                        };
                        return retour;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Pagination out of existing range.");
                    }
                }
                catch (Exception)
                {
                    //!log here. 
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async  Task<PagingParameterOutModel<HRBorder>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            //!TODO copier coller !!
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            String queryString = GetSQLQuery(true, null, null, pageModel);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    Task<int> totalCountTask = conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM boundaries");
                    await totalCountTask;
                    int totalItemsCount = totalCountTask.Result;
                    //!Add a tu on this part
                    if (pageModel.PageNumber * pageModel.PageSize <= totalItemsCount)
                    {
                        Task<IEnumerable<HRBorder>> queryTask = conn.QueryAsync<HRBorder>(queryString);
                        await queryTask;
                        PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>() {
                            PageItems = queryTask.Result,
                        PageSize = pageModel.PageSize,
                        TotalItemsCount = totalItemsCount,
                        CurrentPage = pageModel.PageNumber
                    };
                        return retour;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Pagination out of existing range.");
                    }
                }
                catch (Exception)
                {
                    //!log here. 
                    throw;
                }
            }
        }
    }
}
