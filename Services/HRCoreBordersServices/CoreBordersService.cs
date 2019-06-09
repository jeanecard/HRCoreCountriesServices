using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IHRCoreBordersRepository _bordersRepository = null;

        public HRCoreBordersService(IHRCoreBordersRepository service)
        {
            _bordersRepository = service;
        }

        /// <summary>
        /// Retrieve a specific Border or all Borders.
        /// 1- Check consistancy
        ///     1.2- Create Task on GetBorders's repository action
        ///     1.3- Return Result action with no more processing
        /// 2- Throw Exception if consistancy is KO.
        /// </summary>
        /// <param name="borderID">a BorderID to retrieve. Can be null to retrieve all.</param>
        /// <returns>The border with ID or all borders. 
        /// Throw MemberAccessException if repository is null.</returns>
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID = null)
        {
            //1-
            if (_bordersRepository != null)
            {
                //1.2-
                Task<IEnumerable<HRBorder>> bordersTask = _bordersRepository.GetBordersAsync(borderID);
                await bordersTask;
                //1.3-
                return bordersTask.Result;
            }
            //2-
            else
            {
                throw new MemberAccessException();
            }
        }
    }
}
