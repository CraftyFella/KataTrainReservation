using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;

namespace Example.AcceptanceTests
{
    [TestFixture]
    public class TrainReservationTests
    {
        [Explicit]
        public void Test_Reserve_Seats_Via_Post()
        {
            const string url = "http://127.0.0.1:8083";
            var client = new RestClient(url);
            var request = new RestRequest("reserve", Method.POST);
            request.AddParameter("train_id", "express_2000");
            request.AddParameter("seat_count", 4);

            var restResponse = client.Execute(request);

            var result = JsonConvert.DeserializeObject<ReservationResult>(restResponse.Content);

            Assert.That(result.TrainId, Is.EqualTo("express_2000"));
            Assert.That(result.Seats.Count(), Is.EqualTo(4));
            Assert.That(result.Seats.First(), Is.EqualTo("1A"));
            Assert.That(result.BookingReference, Is.EqualTo("75bcd15"));
        }
    }

    public class ReservationResult
    {
        [JsonProperty(PropertyName = "train_id")]
        public string TrainId { get; set; }
        public IEnumerable<string> Seats { get; set; }
        [JsonProperty(PropertyName = "booking_reference")]
        public string BookingReference { get; set; }
    }
}