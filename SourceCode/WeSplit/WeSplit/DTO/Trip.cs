using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.DTO
{
    class Trip
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TypeTrip Type { get; set; }

        public Transport Transport { get; set; }

        public List<Place> Places{ get; set; }
        public List<Member> Members{ get; set; }
    }
}
