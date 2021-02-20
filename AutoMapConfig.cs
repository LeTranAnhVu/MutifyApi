using System.Linq;
using AutoMapper;
using Mutify.Dtos;
using Mutify.Models;

namespace Mutify
{
    public class AutoMapConfig:Profile
    {
        public AutoMapConfig()
        {
            CreateMap<TrackDto, Track>().ForMember(dto => dto.Genres, opt => opt.Ignore());
            CreateMap<TrackDto, Track>().ForMember(dto => dto.BlobFileId, opt => opt.Ignore());
            CreateMap<GenreDto, Genre>().ForMember(dto => dto.Tracks, opt => opt.Ignore());
        }
    }
}