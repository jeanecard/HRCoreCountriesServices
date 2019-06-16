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
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(string borderID = null)
        {
            await Task.Delay(1);
            if (ThrowException)
            {
                throw new Exception("");
            }
            if (borderID != null)
            {
                List<HRBorder> retour = new List<HRBorder>();
                foreach (HRBorder iterator in _list)
                {
                    if (iterator.FIPS == borderID)
                    {
                        retour.Add(iterator);
                        break;
                    }
                }
                return retour;
            }
            else
            {
                return _list;
            }
        }

        public bool IsSortable()
        {
            throw new NotImplementedException();
        }

        public Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public CoreBordersServiceStub(List<String> bordersID)
        {
            if (bordersID != null)
            {
                foreach (String iterator in bordersID)
                {
                    HRBorder borderi = new HRBorder();
                    borderi.FIPS = iterator;
                    _list.Add(borderi);
                }
            }
        }
    }
}
