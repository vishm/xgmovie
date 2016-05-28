using AutoMapper;
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

            // Map Movie.Id <-> Movie.ObjectId
            cfg.CreateMap<Models.Movie, XGMoviesBackEnd.Domain.Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ObjectId));
                       
            cfg.CreateMap<XGMoviesBackEnd.Domain.Movie, Models.Movie>()
                .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.Id));
            


            Mapper.AssertConfigurationIsValid();
        }
    }
}
