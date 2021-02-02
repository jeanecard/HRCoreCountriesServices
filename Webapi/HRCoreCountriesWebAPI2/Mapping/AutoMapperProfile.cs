using AutoMapper;
using HRBirdsDTOModel;
using HREFBirdRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HREFBirdsModel
{
    /// <summary>
    /// A deplacer dans Projet autre.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Hrsource, HRSourceDTO>()
                .ForMember(dest => dest.idSource, act => act.MapFrom(src => src.IdSource))
                .ForMember(dest => dest.caption, act => act.MapFrom(src => src.Caption));
            CreateMap<HRSourceDTO, Hrsource >()
                .ForMember(dest => dest.IdSource, act => act.MapFrom(src => src.idSource))
                .ForMember(dest => dest.Caption, act => act.MapFrom(src => src.caption));

        }
    }
}
