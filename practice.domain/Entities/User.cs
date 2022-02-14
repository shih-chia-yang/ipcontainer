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
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; } = string.Empty;
    public string Organization { get; private set; } = string.Empty;
    public string Unit { get; private set; } = string.Empty;

    protected User()
    {
        
    }

    internal User(string firstName, string lastName, string email)
        => (FirstName, LastName, Email) = (firstName, lastName, email);

    public static User CreateNew(string firstName, string lastName, string email)
        => new User(firstName, lastName, email);

    public static User NoUser() => new User();

    public bool IsEmpty()
        => string.IsNullOrEmpty(FirstName) &&
        string.IsNullOrEmpty(LastName) &&
        string.IsNullOrEmpty(Email) &&
        string.IsNullOrEmpty(Phone) &&
        string.IsNullOrEmpty(Organization) &&
        string.IsNullOrEmpty(Unit);

    public void UpdateEmail(string email)
        =>Email = email;

    public void UpdatePhone(string phone)
        =>Phone = phone;

    public void UpdateOrganization(string organization)
        => Organization = organization;
    public void UpdateUnit(string unit)
        => Unit = unit;
}