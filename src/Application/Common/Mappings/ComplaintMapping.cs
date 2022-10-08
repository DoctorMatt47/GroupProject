using AutoMapper;
using GroupProject.Application.Complaints;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Common.Mappings;

public class ComplaintMapping : Profile
{
    public ComplaintMapping()
    {
        CreateMap<Complaint, ComplaintResponse>();
    }
}
