using practice.domain.Kernel.Events;

namespace practice.domain.Events;

public class RegisteredDomainEvent:INotification
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public RegisteredDomainEvent(string firstName,string lastName,string email,string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
}