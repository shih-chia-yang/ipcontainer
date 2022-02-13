namespace practice.domain.Entities;

/// <summary>
/// first name
/// last name
/// email
/// phone
/// organization
/// unit
/// </summary>
public class User:BaseEntity
{
    public string  FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; set; }
    public string Organization { get; set; }
    public string Unit { get; set; }

    protected User()
    {
        
    }

    internal User(string firstName, string lastName, string email)
        => (FirstName, LastName, Email) = (firstName, lastName, email);

    public static User CreateNew(string firstName, string lastName, string email)
        => new User(firstName, lastName, email);
}