using HRCommonModels;
using HRCommonTools;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestHRTools
{
    public class HRSortingParamModelDeserializerTest
    {
        /// <summary>
        /// Null sorting param Model retrun empty List.
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWithNullModelExpectEmptyList()
        {
            IEnumerable<(String, String)> retour = HRSortingParamModelDeserializer.GetFieldOrders(null);
            Assert.NotNull(retour);
            Assert.NotNull(retour.GetEnumerator());
            Assert.False(retour.GetEnumerator().MoveNext());
        }
        /// <summary>
        /// Empty sorting param Model retrun empty List.
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWitSortingParamsQueryEmptyExpectEmptyList()
        {
            IEnumerable<(String, String)> retour = HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = String.Empty });
            Assert.NotNull(retour);
            Assert.NotNull(retour.GetEnumerator());
            Assert.False(retour.GetEnumerator().MoveNext());
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWitSortingParamsQueryWithOrderingLeftExpectArgumentOutOfRangeException()
        {
            String query = "FIELD1;ASC;FIELD2";
            bool catched = false;
            try
            {
                HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            }
            catch(ArgumentOutOfRangeException)
            {
                catched = true;
            }
            catch(Exception)
            {
                //Dummy.
            }
            Assert.True(catched);
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWitSortingParamsQueryWithFieldNameLeftExpectArgumentOutOfRangeException()
        {
            String query = "FIELD1;ASC;;DESC";
            bool catched = false;
            try
            {
                HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            }
            catch (ArgumentOutOfRangeException)
            {
                catched = true;
            }
            catch (Exception)
            {
                //Dummy.
            }
            Assert.True(catched);

        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWitSortingParamsQueryWithWrongOrderingArgumentExpectArgumentOutOfRangeException()
        {
            String query = "FIELD1;ASC;FIELD2;DESK";
            bool catched = false;
            try
            {
                HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            }
            catch (ArgumentOutOfRangeException)
            {
                catched = true;
            }
            catch (Exception)
            {
                //Dummy.
            }
            Assert.True(catched);
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWitSortingParamsQueryWithLowerCaseOrderingArgumentExpectReturnOK()
        {
            String query = "FIELD1;AsC;FIELD2;DeSc";
            IEnumerable<(String, String)> result = HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            List<(String, String)> list = new List<(string, string)>(result);
            Assert.NotNull(list);
            Assert.True(list.Count == 2);
            Assert.True(list[0].Item1 == "FIELD1");
            Assert.True(list[0].Item2 == "ASC");
            Assert.True(list[1].Item1 == "FIELD2");
            Assert.True(list[1].Item2 == "DESC");
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWith3FieldsAnd3OrdersCompliantExpectReturnOK()
        {
            String query = "FIELD1;AsC;FIELD2;DeSc;FIELD3;DESC";
            IEnumerable<(String, String)> result = HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            List<(String, String)> list = new List<(string, string)>(result);
            Assert.NotNull(list);
            Assert.True(list.Count == 3);
            Assert.True(list[0].Item1 == "FIELD1");
            Assert.True(list[0].Item2 == "ASC");
            Assert.True(list[1].Item1 == "FIELD2");
            Assert.True(list[1].Item2 == "DESC");
            Assert.True(list[2].Item1 == "FIELD3");
            Assert.True(list[2].Item2 == "DESC");
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void HRDeserializerGetFieldOrdersWith1FieldAnd1OrderCompliantExpectReturnOK()
        {
            String query = "FIELD1;AsC";
            IEnumerable<(String, String)> result = HRSortingParamModelDeserializer.GetFieldOrders(new HRSortingParamModel() { OrderBy = query });
            List<(String, String)> list = new List<(string, string)>(result);
            Assert.NotNull(list);
            Assert.True(list.Count == 1);
            Assert.True(list[0].Item1 == "FIELD1");
            Assert.True(list[0].Item2 == "ASC");
        }
    }
}
