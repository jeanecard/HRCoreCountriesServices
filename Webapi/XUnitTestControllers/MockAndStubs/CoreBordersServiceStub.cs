using HRCommonModel;
using HRCommonModels;
using HRCoreBordersModel;
using HRCoreBordersServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestControllers
{
    internal class CoreBordersServiceStub : ICoreBordersService
    {
        private readonly List<HRBorder> _list = new List<HRBorder>();
        public bool ThrowException = false;
        public async Task<HRBorder> GetBorderAsync(string borderID)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw new Exception("");
            }
            if (borderID != null)
            {
                foreach (HRBorder iterator in _list)
                {
                    if (iterator.FIPS == borderID)
                    {
                        return iterator;
                    }
                }
            }
            return null;
        }

        public bool IsSortable()
        {
            throw new NotImplementedException();
        }

        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw new Exception("");
            }
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>()
            {
                PageItems = _list,
                TotalItemsCount = (uint)_list.Count,
                PageSize = pageModel.PageSize
            };
            return retour;
        }

        public bool IsPaginable()
        {
            throw new NotImplementedException();
        }

        public CoreBordersServiceStub(List<String> bordersID)
        {
            if (bordersID != null)
            {
                foreach (String iterator in bordersID)
                {
                    HRBorder borderi = new HRBorder() { FIPS = iterator };
                    _list.Add(borderi);
                }
            }
        }
    }
}
