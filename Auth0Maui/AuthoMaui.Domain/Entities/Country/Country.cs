using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Entities.Country
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DialCode { get; set; }
        public string IsoCode { get; set; }
        public string Flag { get; set; }
    }
}
