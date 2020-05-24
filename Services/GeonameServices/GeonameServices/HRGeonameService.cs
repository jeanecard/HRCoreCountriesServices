using HRGeoLocator.Models;
using HRGeoLocator.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRGeoLocator.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class HRGeonameService : IHRGeonameService
    {
        private readonly static HttpClient client = new HttpClient();
        private readonly static string _geonameServiceURLStart = @"https://secure.geonames.org/searchJSON?q=";
        private readonly static string _geonameServiceURLEnd = @"&maxRows=5&username=";
        private readonly string _geonameUserName = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public HRGeonameService(IConfiguration config)
        {
            if (config != null)
            {
                _geonameUserName = config[GeoNameConstant.USER_NAME_KEY];
            }
        }
        /// <summary>
        /// First version of proxy on geoname secureservice
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<GeoNameRootObject> GetGeonamesAsync(string pattern)
        {
            if (String.IsNullOrEmpty(_geonameUserName))
            {
                throw new ArgumentNullException("username not set in configuration.");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(_geonameServiceURLStart);
            sb.Append(pattern);
            sb.Append(_geonameServiceURLEnd);
            sb.Append(_geonameUserName);

            using (var streamTask = client.GetAsync(sb.ToString()))
            {
                await streamTask;
                if (streamTask.IsCompletedSuccessfully)
                {
                    using (var taskJson = streamTask.Result.Content.ReadAsStringAsync())
                    {
                        await taskJson;
                        var geoname = JsonSerializer.Deserialize<GeoNameRootObject>(taskJson.Result);
                        return geoname;
                    }
                }
                else
                {
                    throw new Exception("Can not get result from : " + _geonameServiceURLStart);
                }
            }
        }
    }
}
