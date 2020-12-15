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
        public string Status { get; set; }
        public string MainImage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public TypeTrip Type { get; set; }

        public string Transport { get; set; }

        public List<Place> Places { get; set; }
        public List<Member> Members { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<ImageTrip> Images { get; set; }
    }
}
