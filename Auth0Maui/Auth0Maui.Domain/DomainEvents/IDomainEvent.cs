using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Domain.DomainEvents
{
    public interface IDomainEvent<out T>
    {
        DateTime OccurredOn { get; }
        T Data { get; }
    }
}
