using AutoMapper;

namespace CheckRegistry.DataAccess.Mappings;

public class DataAccessProfile : Profile
{
    public DataAccessProfile()
    {
        //Registration Result
        CreateMap<Domain.Entities.RegistrationResult, DataAccess.Entities.RegistrationResult>()
            .ReverseMap();
            

        // Check
        CreateMap<Domain.Entities.Check, DataAccess.Entities.Check>()
            .ForMember(d => d.Id, m => m.MapFrom(o => o.Id))
            .ForMember(d => d.IssueDate, m => m.MapFrom(o => o.IssueDate))
            .ForMember(d => d.VersionNumber, m => m.Ignore())
            .ForMember(d => d.Data, m => m.MapFrom(o => o.Data))
            .ForMember(d => d.RegistrationResults, m => m.MapFrom(o => o.RegistrationResults))
            .ReverseMap();
    }
}