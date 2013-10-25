using System.Collections.Generic;
using Example.AcceptanceTests;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace Example
{
    [TestFixture]
    public class TicketOfficeNUnitTest
    {
        
        [Test]
        public void Test_MakeReservation_Returns_Valid_Reservation_When_Train_Has_Capacity()
        {
            var trainProvider = new TrainProviderBuilder()
                .WithCoach(new CoachBuilder().WithCapacity(100).WithBooked(50).Build()).Build();

            var ticketOffice = new TicketOffice(trainProvider);
            var request = new ReservationRequest("express_2000", 1);

            var result = ticketOffice.MakeReservation(request);
            result.BookingId.Should().NotBeNullOrEmpty();
            result.TrainId.Should().Be("express_2000");
            result.Seats.Should().HaveCount(1);
        }

        [Test]
        public void Test_MakeReservation_Return_Empty_When_Train_Is_Over_Capacity()
        {
            var trainProvider = new TrainProviderBuilder()
                .WithCoach(new CoachBuilder().WithCapacity(100).WithBooked(70).Build()).Build();
            var ticketOffice = new TicketOffice(trainProvider);
            var result = ticketOffice.MakeReservation(new ReservationRequestBuilder().WithTrainId("express_2000").WithSeatCount(1).Build());

            IsEmpty(result);
        }

        [Test]
        public void Test_MakeReservation_Return_Empty_When_Train_Will_Be_Over_Capacity()
        {
            var trainProvider = new TrainProviderBuilder()
                .WithCoach(new CoachBuilder().WithCapacity(100).WithBooked(65).Build()).Build();
            var ticketOffice = new TicketOffice(trainProvider);
            var result = ticketOffice.MakeReservation(new ReservationRequestBuilder().WithTrainId("express_2000").WithSeatCount(7).Build());

            IsEmpty(result);
        }

        [Test]
        public void Test_MakeReservation_Return_Empty_When_Booking_Is_Larger_Than_Any_Of_The_Coaches_Capacity()
        {
            var trainProvider = new TrainProviderBuilder()
                .WithCoach(new CoachBuilder().WithBooked(0).WithCapacity(10).Build())
                .WithCoach(new CoachBuilder().WithBooked(0).WithCapacity(10).Build())
                .Build();

            var ticketOffice = new TicketOffice(trainProvider);
            var result = ticketOffice.MakeReservation(new ReservationRequestBuilder().WithTrainId("express_2000").WithSeatCount(11).Build());
            IsEmpty(result);
        }

        private static void IsEmpty(Reservation result)
        {
            result.BookingId.Should().BeEmpty();
            result.Seats.Should().HaveCount(0);
            result.TrainId.Should().BeEmpty();
        }
    }

    public class CoachBuilder
    {
        private int capacity = 100;
        private int booked = 100;

        public CoachBuilder WithCapacity(int capacity)
        {
            this.capacity = capacity;
            return this;
        }

        public CoachBuilder WithBooked(int booked)
        {
            this.booked = booked;
            return this;
        }

        public Coach Build()
        {
            return new Coach(capacity, booked);
        }
    }

    public class Coach
    {
        public int TotalSeats { get; private set; }
        public int Booked { get; private set; }
        public int Available
        {
            get { return TotalSeats - Booked; }
        }

        public Coach(int totalSeats, int booked)
        {
            TotalSeats = totalSeats;
            Booked = booked;
        }
    }

    public class TrainProviderBuilder
    {
        private List<Coach> coaches = new List<Coach>();
        
        public ITrainProvider Build()
        {
            var trainProvider = MockRepository.GenerateMock<ITrainProvider>();

            trainProvider.Stub(x => x.GetTrain(Arg<string>.Is.Anything)).Return(new Train(coaches));

            return trainProvider;
        }

        public TrainProviderBuilder WithCoach(Coach coach)
        {
            this.coaches.Add(coach);
            return this;
        }
    }

    public class ReservationRequestBuilder
    {
        private string trainId = "express_2000";
        private int seatCount = 1;

        public ReservationRequest Build()
        {
            return new ReservationRequest(trainId, seatCount);
        }

        public ReservationRequestBuilder WithTrainId(string trainId)
        {
            this.trainId = trainId;
            return this;
        }

        public ReservationRequestBuilder WithSeatCount(int seatCount)
        {
            this.seatCount = seatCount;
            return this;
        }
    }
}
