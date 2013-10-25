namespace Example
{
    public class Train
    {
        public int TotalSeats { get; set; }
        public int Booked { get; set; }

        public Train(int totalSeats, int booked)
        {
            TotalSeats = totalSeats;
            Booked = booked;
        }
    }
}