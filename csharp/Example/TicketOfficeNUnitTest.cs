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
            var trainProvider = new TrainProviderBuilder().WithCapacity(100).WithBooked(50).build();
            var ticketOffice = new TicketOffice(trainProvider);
            var request = new ReservationRequest("express_2000", 1);

            var result = ticketOffice.MakeReservation(request);
            result.BookingId.Should().NotBeNullOrEmpty();
            result.TrainId.Should().Be("express_2000");
            result.Seats.Should().HaveCount(1);
        }

        [Test]
        public void Test_MakeReservation_Return_Empty_Reservation_When_Train_Is_Over_Capacity()
        {
            var trainProvider = new TrainProviderBuilder().WithCapacity(100).WithBooked(70).build();
            var ticketOffice = new TicketOffice(trainProvider);
            var result = ticketOffice.MakeReservation(new ReservationRequestBuilder().WithTrainId("express_2000").WithSeatCount(1).Build());

            IsEmpty(result);
        }

        [Test]
        public void Test_MakeReservation_Return_Empty_Reservation_When_Train_Will_Be_Over_Capacity()
        {
            var trainProvider = new TrainProviderBuilder().WithCapacity(100).WithBooked(65).build();
            var ticketOffice = new TicketOffice(trainProvider);
            var result = ticketOffice.MakeReservation(new ReservationRequestBuilder().WithTrainId("express_2000").WithSeatCount(7).Build());

            IsEmpty(result);
        }

        private static void IsEmpty(Reservation result)
        {
            result.BookingId.Should().BeEmpty();
            result.Seats.Should().HaveCount(0);
            result.TrainId.Should().BeEmpty();
        }
    }

    public class TrainProviderBuilder
    {
        private int capacity = 100;
        private int booked = 100;

        public TrainProviderBuilder WithCapacity(int capacity)
        {
            this.capacity = capacity;
            return this;
        }

        public TrainProviderBuilder WithBooked(int booked)
        {
            this.booked = booked;
            return this;
        }

        public ITrainProvider build()
        {
            var trainProvider = MockRepository.GenerateMock<ITrainProvider>();

            trainProvider.Stub(x => x.GetTrain(Arg<string>.Is.Anything)).Return(new Train(capacity, booked));

            return trainProvider;
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
