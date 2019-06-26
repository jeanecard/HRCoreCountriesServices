using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace XUnitTestServices.MocksAndStubs
{
    class HRCoreBordersRepositoryStub : IHRCoreBordersRepository
    {
        private List<HRBorder> _borders = new List<HRBorder>();
        private HRBorder _selected = new HRBorder();
        private bool _isSortable = true;
        private bool _isPaginable;

        public HRCoreBordersRepositoryStub(List<String> borders, String borderSelectable, bool isSortable = true, bool isPaginable = true)
        {
            _isPaginable = isPaginable;
            _isSortable = isSortable;
            if(borders != null)
            {
                foreach(String iter in borders)
                {
                    _borders.Add(new HRBorder() { FIPS = iter });
                }
            }
            if(!String.IsNullOrEmpty(borderSelectable))
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
        public async Task<HRBorder> GetBordersAsync(string borderID)
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

        public async  Task<IEnumerable<HRBorder>> GetBordersAsync(HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return _borders;
        }

        public async Task<IEnumerable<HRBorder>> GetFullBordersAsync()
        {
            await Task.Delay(1);
            return _borders;
        }

        public async Task<PagingParameterOutModel<HRBorder>> GetOrderedAndPaginatedBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>();
            retour.CurrentPage = pageModel.PageNumber;
            retour.PageItems = _borders;
            retour.PageSize = pageModel.PageSize;
            retour.TotalItemsCount = _borders.Count;
            return retour;
        }

        public async Task<IEnumerable<HRBorder>> GetOrderedBordersAsync(HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            return _borders;
        }

        public async  Task<PagingParameterOutModel<HRBorder>> GetPaginatedBordersAsync(PagingParameterInModel pageModel)
        {
            await Task.Delay(1);
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>();
            retour.CurrentPage = pageModel.PageNumber;
            retour.PageItems = _borders;
            retour.PageSize = pageModel.PageSize;
            retour.TotalItemsCount = _borders.Count;
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
    }
}


