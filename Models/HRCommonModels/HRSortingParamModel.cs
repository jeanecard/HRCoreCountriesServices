﻿using System;

namespace HRCommonModels
{
    /// <summary>
    /// Implements a generic ordering of fields.
    /// The following format is expected : 
    /// FieldName1;ASC or DESC;FieldName2;ASC or DESC
    /// All parts are mandatory. ASC and DESC are key insensitive.
    /// </summary>
    public class HRSortingParamModel
    {
        private String _sortingParamsQuery = String.Empty;

        /// <summary>
        /// The orderby Query. Expected : "Field1;DESC;Field2;ASC". No defauld value allowed for ASC or DESC. These values must be explicitly specified.
        /// </summary>
        public string OrderBy
        {
            get => _sortingParamsQuery;
            set => _sortingParamsQuery = value;
        }

        /// <summary>
        /// Check if object is not on default state.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialised()
        {
            return !String.IsNullOrEmpty(_sortingParamsQuery);
        }
    }
}

