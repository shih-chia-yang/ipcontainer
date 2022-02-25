namespace practice.api.Applications.ViewModels;

public record UserProfileViewModel
{
    public string Name { get;init; }
    public string Email { get;init; }
    public string Phone { get;init; }
    public string Unit{ get;init; }
}