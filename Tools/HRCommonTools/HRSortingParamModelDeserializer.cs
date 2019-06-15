using HRCommonModels;
using System;
using System.Collections.Generic;

namespace HRCommonTools
{
    public static class HRSortingParamModelDeserializer
    {
        private static readonly char _separator = ';';
        private static readonly String _ASC_KEYWORD = "ASC";
        private static readonly String _DESC_KEYWORD = "DESC";

        /// <summary>
        /// Process SortingParamsQuery as a list of (Fields, order)
        /// 1- Splitting params as array of String (filed), String (order)
        /// 1.1- Check validity : number of elements splitted must be even. Throw ArgumentOutOfRangeException() if condition is not verified.
        /// 1.2- Iterate through splitted elements. 
        ///     1.2.1- Even index are FieldName
        ///     1.2.2- Odd index are ordering action. 
        ///         1.2.2.1- Check keyword is compliant with ordering expected ones
        ///         1.2.2.2- Push into returned list at this point.
        ///2- Return List.
        /// </summary>
        /// <returns>a list of tuple Field / Order. Can throw ArgumentOutOfRangeException if keywords order is not compliant with the expected ones.</returns>
        public static IEnumerable<(String, String)> GetFieldOrders(HRSortingParamModel model)
        {
            List<(String, String)> retour = new List<(String, String)>();
            if (model != null && !String.IsNullOrEmpty( model.SortingParamsQuery))
            {
                //1-
                String[] parsedQuery = model.SortingParamsQuery.Split(_separator);
                //1.1-
                int paramsCount = parsedQuery.Length;
                if (paramsCount % 2 != 0)
                {
                    throw new ArgumentOutOfRangeException("Unexpected number of arguments in sortingParamsQuery");
                }
                else
                {
                    String fieldi = String.Empty;
                    String orderingi;
                    //1.2-
                    for (int i = 0; i < paramsCount; i++)
                    {
                        //1.2.1-
                        if (i == 0 || i % 2 == 0)
                        {
                            fieldi = parsedQuery[i];
                            if (String.IsNullOrEmpty(fieldi))
                            {
                                throw new ArgumentOutOfRangeException("FieldName is null");
                            }
                        }
                        //1.2.2-
                        else
                        {
                            orderingi = parsedQuery[i].ToUpper();
                            //1.2.2.1
                            if (orderingi.Equals(_ASC_KEYWORD) || orderingi.Equals(_DESC_KEYWORD))
                            {
                                //1.2.2.2-
                                retour.Add((fieldi, orderingi));
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException("Ordering command is different of " + _ASC_KEYWORD + " or " + _DESC_KEYWORD);
                            }
                        }
                    }
                }
            }
            //2-
            return retour;
        }

    }
}
