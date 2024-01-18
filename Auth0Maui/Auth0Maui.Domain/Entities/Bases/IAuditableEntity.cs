using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Domain.Entities.Bases
{
    public interface IAuditableEntity
    {
        Guid Id { get; set; }
        DateTime CreatedDate { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        Guid? LastModifiedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}
