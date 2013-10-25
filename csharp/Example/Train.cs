using System.Collections.Generic;
using System.Linq;

namespace Example
{
    public class Train
    {
        private readonly List<Coach> coaches;

        public IEnumerable<Coach> Coaches
        {
            get { return coaches; }
        }

        public int TotalSeats
        {
            get { return coaches.Sum(c => c.TotalSeats); }
        }

        public int Booked
        {
            get { return coaches.Sum(c => c.Booked); }
        }

        public Train(List<Coach> coaches)
        {
            this.coaches = coaches;
        }
    }
}