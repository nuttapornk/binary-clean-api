namespace Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    public int Status { get; set; }
    public Guid CreateUser { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? UpdateUser { get; set; }
    public DateTime? UpdateDate { get; set; }
}
