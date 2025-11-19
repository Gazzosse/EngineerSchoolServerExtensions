using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension.ApplicationBusinessTrip.Models
{
    public class GetTicketsCostsResponse
    {
        public List<Flight> Flights { get; set; }
    }

    public class Flight
    {
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public decimal Price { get; set; }

    }
}
