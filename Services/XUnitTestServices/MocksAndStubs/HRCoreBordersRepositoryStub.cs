using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using HRCoreRepository.Interface;

namespace XUnitTestServices.MocksAndStubs
{
    class HRCoreBordersRepositoryStub : IHRCoreRepository<HRBorder>
    {
        private readonly List<HRBorder> _borders = new List<HRBorder>();
        private readonly HRBorder _selected = new HRBorder();
        private readonly bool _isSortable = true;
        private readonly bool _isPaginable;

        public HRCoreBordersRepositoryStub(List<String> borders, String borderSelectable, bool isSortable = true, bool isPaginable = true)
        {
            _isPaginable = isPaginable;
            _isSortable = isSortable;
            if (borders != null)
            {
                foreach (String iter in borders)
                {
                    _borders.Add(new HRBorder() { FIPS = iter });
                }
            }
            if (!String.IsNullOrEmpty(borderSelectable))
            {
                _selected.FIPS = borderSelectable;

            }
        }

        public Task<HRBorder> GetBorderAsync(string borderID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// retrun selected or full list.
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        public async Task<HRBorder> GetAsync(string borderID)
        {
            await Task.Delay(1);
            if (borderID != null)
            {
                return _selected;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<HRBorder>> GetBordersAsync(HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return _borders;
        }

        public async Task<IEnumerable<HRBorder>> GetFullBordersAsync()
        {
            await Task.Delay(1);
            return _borders;
        }

        public Task<IEnumerable<HRBorder>> GetFullsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>()
            {
                CurrentPage = pageModel.PageNumber,
                PageItems = _borders,
                PageSize = pageModel.PageSize,
                TotalItemsCount = (uint)_borders.Count
            };
            return retour;
        }

        public Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedsAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HRBorder>> GetOrderedBordersAsync(HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return _borders;
        }

        public Task<IEnumerable<HRBorder>> GetOrderedsAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingParameterOutModel<HRBorder>> GetPaginatedBordersAsync(PagingParameterInModel pageModel)
        {
            await Task.Delay(1);
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>()
            {
                CurrentPage = pageModel.PageNumber,
                PageItems = _borders,
                PageSize = pageModel.PageSize,
                TotalItemsCount = (uint)_borders.Count
            };
            return retour;
        }

        public bool IsPaginable()
        {
            return _isPaginable;
        }

        public bool IsSortable()
        {
            return _isSortable;
        }

        public Task<PagingParameterOutModel<HRBorder>> GetPaginatedsAsync(PagingParameterInModel pageModel)
        {
            throw new NotImplementedException();
        }
    }
}


