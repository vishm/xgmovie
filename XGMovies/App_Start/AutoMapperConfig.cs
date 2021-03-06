﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGMovies
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => InitializeMapper(cfg));
        }

        private static void InitializeMapper(IMapperConfiguration cfg)
        {
            // Define two mapping, .reversemap() not working as expected/or not understood by me.
            cfg.CreateMap<Models.NewMovie, XGMoviesBackEnd.Domain.Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TheMovideDbOrgId, opt => opt.Ignore())
                .ForMember(dest => dest.Characters, opt => opt.Ignore());

            // Map Movie.Id <-> Movie.ObjectId
            cfg.CreateMap<Models.Movie, XGMoviesBackEnd.Domain.Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ObjectId))
                .ForMember(dest => dest.TheMovideDbOrgId, opt => opt.MapFrom(src => src.MovieDbId))
                .ForMember(dest => dest.Characters, opt => opt.Ignore());



            cfg.CreateMap<XGMoviesBackEnd.Domain.Movie, Models.Movie>()
                .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MovieDbId, opt => opt.MapFrom(src => src.TheMovideDbOrgId));



            Mapper.AssertConfigurationIsValid();
        }
    }
}
