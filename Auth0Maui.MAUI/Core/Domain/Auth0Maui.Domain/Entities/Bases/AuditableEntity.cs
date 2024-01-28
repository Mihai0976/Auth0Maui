
namespace Auth0Maui.Domain.Entities.Bases;


public abstract class AuditableEntity : IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}