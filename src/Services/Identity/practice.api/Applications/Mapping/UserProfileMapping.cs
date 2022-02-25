namespace practice.api.Applications.Mapping;

public class UserProfileMapping:Profile
{
    public UserProfileMapping()
    {
        // CreateMap<TSource,TDestination>
        CreateMap<User, UserProfileViewModel>()
            .ForMember(dest=>dest.Id,from=>from.MapFrom(u=>u.Id))
            .ForMember(dest => dest.Name, from => from.MapFrom(u => $"{u.FirstName} {u.LastName}"))
            .ForMember(dest => dest.Email, from => from.MapFrom(x => x.Email))
            .ForMember(dest => dest.Phone, from => from.MapFrom(x => x.Phone))
            .ForMember(dest => dest.Unit, from => from.MapFrom(x => $"{x.Organization}{x.Unit}"))
            .ReverseMap();
    }
}