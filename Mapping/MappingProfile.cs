using AutoMapper;
using InterviewPuzzle.Controllers.DTO;
using InterviewPuzzle.Data_Access.Model;

namespace InterviewPuzzle.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<McqDto, MCQ>();
            CreateMap<MCQ, McqDto>();
            CreateMap<OptionDto, Option>();
            CreateMap<Option, OptionDto>();
            CreateMap<VivaQuestionDto, VivaQuestion>();
            CreateMap<VivaQuestion, VivaQuestionDto>();
            CreateMap<SolutionDto, Solution>();
            CreateMap<CodingQuestionDto, CodingQuestion>();
            CreateMap<CodingQuestion, CodingQuestionDto>();
        }
    }
}
