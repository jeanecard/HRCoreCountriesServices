using HRCommonModels;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestServices.MocksAndStubs
{
    class HRCoreBordersRepositoryStub : IHRCoreBordersRepository
    {
        private List<HRBorder> _borders = new List<HRBorder>();
        private HRBorder _selected = new HRBorder();
        public HRCoreBordersRepositoryStub(List<String> borders, String borderSelectable)
        {
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
        /// <summary>
        /// retrun selected or full list.
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(string borderID = null)
        {
            await Task.Delay(1);
            if (borderID != null)
            {
                List<HRBorder> retour = new List<HRBorder>();
                retour.Add(_selected);
                return retour;
            }
            else
            {
                return _borders;
            }
        }

        public Task<IEnumerable<HRBorder>> GetBordersAsync(HRSortingParamModel orderBy)
        {
            throw new NotImplementedException();
        }

        public bool IsSortable()
        {
            throw new NotImplementedException();
        }
    }
}


