using HRCommon.Interface;
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
        public bool IsSortable = true;
        public bool IsPaginable = true;
        private Exception _exception = null;

        public Exception ExceptionToThrow { get
            {
                if (_exception == null)
                {
                    _exception = new Exception();
                }
                return _exception;
            }
            internal set
            {
                _exception = value;
            } }

        public async Task<HRBorder> GetBorderAsync(string borderID)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw ExceptionToThrow;
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


        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw ExceptionToThrow;
            }
            PagingParameterOutModel<HRBorder> retour = new PagingParameterOutModel<HRBorder>()
            {
                PageItems = _list,
                TotalItemsCount = (uint)_list.Count,
                PageSize = pageModel.PageSize
            };
            return retour;
        }

        bool ISortable.IsSortable()
        {
            return IsSortable;
        }

        bool IPaginable.IsPaginable()
        {
            return IsPaginable;
        }

        public Task<IEnumerable<string>> GetContinentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetContinentByIDAsync(string id)
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
