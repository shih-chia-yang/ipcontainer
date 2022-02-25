using practice.domain.Kernel.Domain;

namespace practice.domain.AggregateModel.Entities;

public class UserProfile:ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string Organization { get; private set; }
    public string Unit { get; private set; }

    protected UserProfile()
    {

    }

    internal UserProfile(
        string firstName,string lastName,
        string phone,string organization,
        string unit)
    {
        FirstName=firstName;
        LastName = lastName;
        Phone = phone;
        Organization = organization;
        Unit = unit;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
        yield return Phone;
        yield return Organization;
        yield return Unit;
    }
}