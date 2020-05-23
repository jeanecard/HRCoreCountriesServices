using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using HRCommonTools.Interface;
using log4net;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Logging;

namespace HRBirdsRepository
{
    public class HRBirdRepository : IHRBirdRepository
    {

        private readonly ILogger<HRBirdRepository> _logger = null;
        private readonly IConfiguration _config = null;
        private static readonly String _DBUSER = "HRCountries:Username";
        private static readonly String _DBPASSWORD = "HRCountries:Password";
        private static readonly String CONNECTION_STRING_KEY = "BordersConnection";

        public static string SQLQUERY { get; } = " SELECT sumup, name, language, scientificName, mainPicture, mainSound FROM public.\"V_MAIN_RECORDS\" WHERE language = '{0}' ";
        public static string SQLQUERY_COUNT { get; } = " SELECT COUNT(*) FROM public.\"V_MAIN_RECORDS\" WHERE language = '{0}' ";

        private HRBirdRepository()
        {
            //Dummy for DI
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="conf"></param>
        public HRBirdRepository(ILogger<HRBirdRepository> logger, IConfiguration conf)
        {
            _logger = logger;
            _config = conf;
        }
        /// <summary>
        /// 1- Check for consistency.
        /// </summary>
        /// <param name="query">Can not be null</param>
        /// <param name="pageModel">Can not be null</param>
        /// <param name="orderBy">Can be null</param>
        /// <returns></returns>
        public async Task<PagingParameterOutModel<HRBirdMainOutput>> GetMainRecordsAsync(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            //1-
            if(query == null || pageModel == null)
            {
                throw new ArgumentNullException("At least one of arguments is null");
            }
            //2-
            PagingParameterOutModel<HRBirdMainOutput> retour = new PagingParameterOutModel<HRBirdMainOutput>();
            using (Task<IEnumerable<HRBirdMainOutput>> mainRecordsAction = GetMainRecordsWithDapperAsync(query, pageModel, orderBy))
            {
                await mainRecordsAction;
                if (mainRecordsAction.IsCompleted)
                {
                    retour.CurrentPage = pageModel.PageNumber;
                    retour.PageSize = pageModel.PageSize;
                    retour.PageItems = mainRecordsAction.Result;
                    using (var countTask = GetMainRecordsCountWithDapperAsync(query, pageModel, orderBy))
                    {
                        await countTask;
                        if (countTask.IsCompleted)
                        {
                            retour.TotalItemsCount = countTask.Result;
                        }
                    }
                }
                else
                {
                    throw new Exception("Something get wrong in PostGisrepository : GetAsync(IEnumerable<string> borderIDs)");
                }
            }
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private async Task<IEnumerable<HRBirdMainOutput>> GetMainRecordsWithDapperAsync(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {
                    using (Task<IEnumerable<HRBirdMainOutput>> retourTask = conn.QueryAsync<HRBirdMainOutput>(GetSQLQueryMainRecords(query, pageModel, orderBy)))
                    {
                        await retourTask;
                        if (retourTask.IsCompleted)
                        {
                            return retourTask.Result;
                        }
                        else
                        {
                            throw new Exception("QueryAsync : Can not complete Task.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.Message);
                    }
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private async Task<uint> GetMainRecordsCountWithDapperAsync(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            String cxString = _config.GetConnectionString(CONNECTION_STRING_KEY);
            cxString = String.Format(cxString, _config[_DBUSER], _config[_DBPASSWORD]);
            using (var conn = new NpgsqlConnection(cxString))
            {
                conn.Open();
                try
                {

                    using (Task<int> taskCount = conn.ExecuteScalarAsync<int>(GetSQLQueryMainRecordsCount(query, pageModel, orderBy)))
                    {
                        await taskCount;
                        if (taskCount.IsCompleted)
                        {
                            return (uint)(taskCount.Result);
                        }
                        else
                        {
                            throw new Exception("ExecuteScalarAsync : Can not complete Task.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                    {
                        _logger.LogError(ex.Message);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private String GetSQLQueryMainRecords(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            if(query == null || String.IsNullOrEmpty(query.Lang_Iso_Code))
            {
                throw new ArgumentNullException("Lang_Iso_Code not set.");
            }
            return String.Format(SQLQUERY, query.Lang_Iso_Code.ToLower());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        private String GetSQLQueryMainRecordsCount(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            if (query == null || String.IsNullOrEmpty(query.Lang_Iso_Code))
            {
                throw new ArgumentNullException("Lang_Iso_Code not set.");
            }
            return String.Format(SQLQUERY_COUNT, query.Lang_Iso_Code.ToLower());
        }

    }
}
