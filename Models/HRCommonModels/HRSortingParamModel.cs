using System;
using System.Collections.Generic;
using System.Text;

namespace HRCommonModels
{
    /// <summary>
    /// Implements a generic ordering of fields.
    /// The following format is expected : 
    /// FieldName1;ASC or DESC;FieldName2;ASC or DESC
    /// All parts are mandatory. ASC and DESC are ket insensitive.
    /// </summary>
    public class HRSortingParamModel
    {
        private String _sortingParamsQuery = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string SortingParamsQuery {
            get => _sortingParamsQuery;
            set => _sortingParamsQuery = value;
        }
    }
}

