﻿using System.Collections.Generic;

namespace Example
{
    public class Reservation
    {
        public string TrainId { get; private set; }
        public string BookingId { get; private set; }
        public List<Seat> Seats { get; private set; }

        public Reservation(string trainId, string bookingId, List<Seat> seats)
        {
            this.TrainId = trainId;
            this.BookingId = bookingId;
            this.Seats = seats;
        }

        public static readonly Reservation Empty = new Reservation(string.Empty, string.Empty, new List<Seat>());
    }
}
