using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    public class TicketOffice
    {
        private readonly ITrainProvider _trainProvider;

        public TicketOffice(ITrainProvider trainProvider)
        {
            _trainProvider = trainProvider;
        }

        public Reservation MakeReservation(ReservationRequest request)
        {
            var train = _trainProvider.GetTrain(request.TrainId);

            if (OverCapcity(request, train))
                return Reservation.Empty;
            if (!train.Coaches.Any(c => c.Available >= request.SeatCount))
                return Reservation.Empty;

            return new Reservation(request.TrainId, Guid.NewGuid().ToString(), new List<Seat>() { new Seat(null, 0)  });
        }

        private static bool OverCapcity(ReservationRequest request, Train train)
        {
            return (double) (train.Booked + request.SeatCount)/train.TotalSeats >= 0.7d;
        }
    }
}
