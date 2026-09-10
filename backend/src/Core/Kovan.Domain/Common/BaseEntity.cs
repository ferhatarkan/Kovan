using System;

namespace Kovan.Domain.Common
{
    public abstract class BaseEntity : IAuditableEntity
    {
        public Guid Id { get; protected set; }
        public Guid TenantId { get; set; } // Her varlığın hangi kiracıya ait olduğunu belirtir.
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}