namespace practice.domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }=Guid.NewGuid();
    public int Status { get; set; } = 1;
    public string TrxDate { get; set; } = DateTime.UtcNow.ToString("yyyy/MM/DD");
    
}