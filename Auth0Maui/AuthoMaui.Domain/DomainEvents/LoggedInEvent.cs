using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.DomainEvents
{
     public class LoggedInEvent<T>: IDomainEvent<T>
    {
        public LoggedInEvent(T data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            OccurredOn = DateTime.UtcNow;
        }

        public T Data { get; }
        public DateTime OccurredOn { get; }
    }
}
