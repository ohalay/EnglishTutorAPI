using AutoMapper;
using EnglishTutor.Api.Models;
using EnglishTutor.Common.Dto;

namespace EnglishTutor.Api.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<SettingsModel, Settings>().ReverseMap();

            CreateMap<Word, WordModel>()
                .ForMember(dest => dest.Translation, option => option.Ignore());

            CreateMap<LanguageModel, LanguageModel>().ReverseMap();
        }
    }
}