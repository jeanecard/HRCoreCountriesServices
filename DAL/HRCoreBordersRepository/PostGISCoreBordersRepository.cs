using Dapper;
using HRCommon.Interface;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCommonTools.Interface;
using HRCoreBordersModel;
using HRCoreRepository.Interface;
using log4net;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCoreBordersRepository
{
    public class PostGISCoreBordersRepository : IHRCoreRepository<HRBorder>, ISortable, IPaginable
    {
        private readonly ILog _logger = null;
        private readonly IConfiguration _config = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";
        private readonly IHRPaginer<HRBorder> _paginer = null;
        /// <summary>
        /// List of field available for ORDER part of SQL Query. Prefered to Reflection for performance reasons.
        /// </summary>
        private readonly Dictionary<String, int> _whiteListOfAvaialbleFields = new Dictionary<String, int>();

        /// <summary>
        /// SQL Query for manual query
        /// </summary>
        public static string SQLQUERY { get; } = " SELECT wkb_geometry, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";
        /// <summary>
        /// SQL Query for Dapper
        /// </summary>
        public static string SQLQUERYFORDAPPER { get; } = " SELECT ST_AsText(wkb_geometry) AS WKT_GEOMETRY, FIPS, ISO2, ISO3, UN, NAME, AREA, POP2005, REGION, SUBREGION, LON, LAT FROM boundaries ";

        private static readonly String _WKT_GEOMETRY = "WKT_GEOMETRY";
        private static readonly String _FIPS = "FIPS";
        private static readonly String _ISO2 = "ISO2";
        private static readonly String _ISO3 = "ISO3";
        private static readonly String _UN = "UN";
        private static readonly String _NAME = "NAME";
        private static readonly String _AREA = "AREA";
        private static readonly String _POP2005 = "POP2005";
        private static readonly String _REGION = "REGION";
        private static readonly String _SUBREGION = "SUBREGION";
        private static readonly String _LON = "LON";
        private static readonly String _LAT = "LAT";
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
        /// 3- Add logger Service.
        /// </summary>
        /// <param name="config"></param>
        public PostGISCoreBordersRepository(IConfiguration config, 
            IHRPaginer<HRBorder> paginer,
            ILog logger = null)
        {
            //1-
            _config = config;
            _paginer = paginer;
            //2-
            _whiteListOfAvaialbleFields.Add(_WKT_GEOMETRY, 1);
            _whiteListOfAvaialbleFields.Add(_FIPS, 1);
            _whiteListOfAvaialbleFields.Add(_ISO2, 1);
            _whiteListOfAvaialbleFields.Add(_ISO3, 1);
            _whiteListOfAvaialbleFields.Add(_UN, 1);
            _whiteListOfAvaialbleFields.Add(_NAME, 1);
            _whiteListOfAvaialbleFields.Add(_AREA, 1);
            _whiteListOfAvaialbleFields.Add(_POP2005, 1);
            _whiteListOfAvaialbleFields.Add(_REGION, 1);
            _whiteListOfAvaialbleFields.Add(_SUBREGION, 1);
            _whiteListOfAvaialbleFields.Add(_LON, 1);
            _whiteListOfAvaialbleFields.Add(_LAT, 1);
            //3-
            _logger = logger;
        }
        /// <summary>
        /// Call the ReaderMethod.
        /// 1- Get the Result Action
        /// 2- Return the first element or null.
        /// </summary>
        /// <param name="borderID">the borderID to look for.</param>
        /// <returns>A collection with the Border with the borderID querried or all Borders if borderID is not supplied.</returns>
        public async Task<HRBorder> GetAsync(string borderID)
        {
            HRBorder retour = null;
            //1-
            using (Task<IEnumerable<HRBorder>> bordersAction = ReadBordersWithDapperAsync(borderID))
            {
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
            }
            return retour;
        }

        /// <summary>
        /// Mapping from DataBase with Dapper
        /// </summary>
        /// <param name="borderID">a BorderID</param>
        /// <returns>A list with the corresponding HRBorder. Can throw Exception.</returns>
        private async Task<IEnumerable<HRBorder>> ReadBordersWithDapperAsync(String borderID)
        {
            IEnumerable<HRBorder> retour = null;
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRBorder>> retourTask = conn.QueryAsync<HRBorder>(GetSQLQuery(true, borderID, null)))
                    {
                        await retourTask;
                        retour = retourTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    if(_logger != null)
                    {
                        _logger.Error(ex.Message);
                    }
                    throw;
                }
            }
            return retour;
        }

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
                //Cheat Code to avoid SQL injection. Indeed pbm with SQL Command and SQLParameters on GeometryColumn with postgis.
                sb.Append(borderID.Substring(0, 2));
                sb.Append("'");
            }
            if (pageInModel != null && orderBy == null)
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
                                    sb.Append(" ORDER BY ");
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
        /// Get All Border ordered by orderBy
        /// </summary>
        /// <param name="orderBy">orderBy to apply</param>
        /// <returns>All Borders. Can throw Exception.</returns>
        public async Task<IEnumerable<HRBorder>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            IEnumerable<HRBorder> retour = null;
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRBorder>> retourTask = conn.QueryAsync<HRBorder>(GetSQLQuery(true, null, orderBy)))
                    {
                        await retourTask;
                        retour = retourTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.Error(ex.Message);
                    }
                    throw;
                }
            }
            return retour;
        }
        /// <summary>
        /// Call GetOrderedsAsync with null OrderBy
        /// </summary>
        /// <returns>All Border from DB. No Order garanty/</returns>
        public async Task<IEnumerable<HRBorder>> GetFullsAsync()
        {
            IEnumerable<HRBorder> retour = null;
            using (Task<IEnumerable<HRBorder>> retourTask = GetOrderedsAsync(null))
            {
                await retourTask;
                retour = retourTask.Result;
            }
            return retour;
        }
        /// <summary>
        /// Apply query orederd and paginated on PostGIS DB.
        /// </summary>
        /// <param name="pageModel">the PageModel. Can not be null.</param>
        /// <param name="orderBy">The order. Can be null, will be set to default to avoid a fuzzy Pagination.</param>
        /// <returns>The corresponding PagingParameterOutModel</returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            if (pageModel == null)
            {
                throw new ArgumentNullException();
            }
            if (_paginer == null)
            {
                throw new MemberAccessException("Paginer can not be null.");
            }
            PagingParameterOutModel<HRBorder> retour = null;
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            String queryString = GetSQLQuery(true, null, orderBy, pageModel);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    uint totalItemsCount = 0;
                    using (Task<int> totalCountTask = conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM boundaries"))
                    {
                        await totalCountTask;
                        if (totalCountTask.Result >= 0 && totalCountTask.Result < Int32.MaxValue)
                            totalItemsCount = (uint)(totalCountTask.Result);
                    }
                    if (_paginer.IsValid(pageModel, totalItemsCount))
                    {
                        using (Task<IEnumerable<HRBorder>> queryTask = conn.QueryAsync<HRBorder>(queryString))
                        {
                            await queryTask;
                            retour = new PagingParameterOutModel<HRBorder>()
                            {
                                PageItems = queryTask.Result,
                                PageSize = pageModel.PageSize,
                                TotalItemsCount = totalItemsCount,
                                CurrentPage = pageModel.PageNumber
                            };
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Pagination out of existing range.");
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.Error(ex.Message);
                    }
                    throw;
                }
                return retour;
            }
        }
        /// <summary>
        /// Call GetOrderedAndPaginatedsAsync with orederBy set to null.
        /// </summary>
        /// <param name="pageModel">the pageModel.</param>
        /// <returns>The HRBorders corresponding Page</returns>
        public async Task<PagingParameterOutModel<HRBorder>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            PagingParameterOutModel<HRBorder> retour = null;
            using (Task<PagingParameterOutModel<HRBorder>> retourTask = GetOrderedAndPaginatedsAsync(pageModel, null))
            {
                await retourTask;
                retour = retourTask.Result;
            }
            return retour;
        }
    }
}
