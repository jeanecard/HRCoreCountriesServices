using HRCommonModel;
using HRCommonModels;
using HRCommonTools;
using HRCommonTools.Interace;
using HRCoreBordersModel;
using HRCoreBordersRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HRCoreBordersServices
{
    public class HRCoreBordersService : ICoreBordersService
    {
        private readonly IHRCoreBordersRepository _bordersRepository = null;
        //!TODO !!
        private readonly IHRPaginer<HRBorder> _paginer = null;
        private readonly static ushort _maxPageSize = 50;
        public HRCoreBordersService(IHRCoreBordersRepository service)
        {
            _bordersRepository = service;
            //!TODO
            _paginer = new HRPaginer<HRBorder>();
        }

        public bool IsSortable()
        {
            if(_bordersRepository != null)
            {
                return _bordersRepository.IsSortable();
            }
            else
            {
                throw new MemberAccessException();
            }
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
        public async Task<IEnumerable<HRBorder>> GetBordersAsync(String borderID)
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

        public async Task<PagingParameterOutModel<HRBorder>> GetBordersAsync(PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            //1-
            if (_bordersRepository != null)
            {
                //1.2-
                Task<IEnumerable<HRBorder>> bordersTask = _bordersRepository.GetBordersAsync(orderBy);
                await bordersTask;
                PagingParameterOutModel<HRBorder> retour = null;
                //1.3-
                if (orderBy != null)
                {
                    var movies = bordersTask.Result.OrderBy(c => c.ISO2).ThenBy(n => n.ISO3);
                }
                if(pageModel != null)
                {
                    if (!_paginer.IsValid(pageModel, bordersTask.Result))
                    {
                        retour = null;
                    }
                    else
                    {
                        retour = _paginer.GetPagination(pageModel, bordersTask.Result, _maxPageSize);
                        return retour;
                    }
                }
                return retour;
            }
            //2-
            else
            {
                throw new MemberAccessException();
            }
        }

    }
}
