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
    public string  FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Organization { get; set; }
    public string Unit { get; set; }
}