using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Complaints;

public class ComplaintMapping : Profile
{
    public ComplaintMapping()
    {
        CreateMap<Complaint, ComplaintResponse>();
    }
}
