using practice.domain.Kernel.Command;
using practice.domain.Kernel.Events;

namespace practice.domain.Kernel.Domain;

public abstract class BaseEntity:IEntity
{

    public Guid Id { get; set; }=Guid.NewGuid();
    public int Status { get; set; } = 1;
    public string TrxDate { get; set; } = DateTime.UtcNow.ToString("yyyy/MM/dd");

    private IEnumerable<INotification> Events => _events.AsReadOnly();

    private List<INotification> _events = new();

    public BaseEntity()
    {

    }

    public void AddEvent(INotification iEvent)
    {
        _events.Add(iEvent);
    } 
}