using HRBordersAndCountriesWebAPI2.Utils;
using HRCoreCountriesServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestControllers
{
    public class HRContinentForkerTest
    {
        [Fact]
        public void HRContinentController_On_Get_With_Null_Service_Return_HTTP500()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            (int, IEnumerable<String>) result = forfker.Get(null);
            Assert.Equal(result.Item1, StatusCodes.Status500InternalServerError);
            Assert.Null(result.Item2);
        }
        [Fact]
        public void HRContinentController_On_Get_With_Service_Exception_Return_HTTP500()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null);
            service.ThrowException = true;
            (int, IEnumerable<String>) result = forfker.Get(service);
            Assert.Equal(result.Item1, StatusCodes.Status500InternalServerError);
            Assert.Null(result.Item2);
        }
        [Fact]
        public void HRContinentController_On_Get_In_Consistant_Context_Return_HTTP200()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(new List<String>() { "100", "200"});
            service.ThrowException = false;
            (int, IEnumerable<String>) result = forfker.Get(service);
            Assert.Equal(result.Item1, StatusCodes.Status200OK);
            Assert.Contains("100", result.Item2);
            Assert.Contains("200", result.Item2);
        }

        [Fact]
        public void HRContinentController_On_GetID_With_Null_Service_Return_HTTP500()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            (int, String) result = forfker.Get(String.Empty, null);
            Assert.Equal(result.Item1, StatusCodes.Status500InternalServerError);
            Assert.Null(result.Item2);
        }
        [Fact]
        public void HRContinentController_On_GetID_With_Service_Exception_Return_HTTP500()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(null);
            service.ThrowException = true;
            (int, String) result = forfker.Get(String.Empty, service);
            Assert.Equal(result.Item1, StatusCodes.Status500InternalServerError);
            Assert.Null(result.Item2);
        }
        [Fact]
        public void HRContinentController_On_GetID_With_Existing_ID_Return_HTTP200()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(new List<string>() { "200"});
            service.ThrowException = false;
            (int, String) result = forfker.Get("200", service);
            Assert.Equal(result.Item1, StatusCodes.Status200OK);
            Assert.Equal("200", result.Item2);
        }
        [Fact]
        public void HRContinentController_On_GetID_With_UnExisting_ID_Return_HTTP404()
        {
            HRContinentControllerForker forfker = new HRContinentControllerForker();
            CoreCountriesServiceStub service = new CoreCountriesServiceStub(new List<string>() { "200" });
            service.ThrowException = false;
            (int, String) result = forfker.Get("HR", service);
            Assert.Equal(result.Item1, StatusCodes.Status404NotFound);
            Assert.Equal(String.Empty, result.Item2);
        }
    }
}
