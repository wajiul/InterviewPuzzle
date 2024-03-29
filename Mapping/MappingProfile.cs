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
            CreateMap<OptionDto, Option>();
            CreateMap<VivaQuestionDto, VivaQuestion>();
            CreateMap<SolutionDto, Solution>();
            CreateMap<CodingQuestionDto, CodingQuestion>();

        }
    }
}
