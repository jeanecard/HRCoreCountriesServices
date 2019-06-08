using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IHRCoreBordersRepository _bordersRepository = null;

        public HRCoreBordersService(IHRCoreBordersRepository service )
        {
            _bordersRepository = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="borderID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID = null)
        {
            if(_bordersRepository != null)
            {
                Task<IEnumerable<HRBorder>> bordersTask = _bordersRepository.GetBordersAsync(borderID);
                await bordersTask;
                return bordersTask.Result;
            }
            else
            {
                throw new MemberAccessException();
            }
        }
    }
}
