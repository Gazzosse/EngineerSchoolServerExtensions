using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WebClient.State;
using DocsVision.Layout.WebClient.AdvancedLayouts.ExtendedDataSources;
using DocsVision.Platform.WebClient;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using MyExtension.ApplicationBusinessTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Document = DocsVision.BackOffice.ObjectModel.Document;

namespace MyExtension.ApplicationBusinessTrip
{
    public class ApplicationBusinessTripService : IApplicationBusinessTripService
    {
        public OnSecondedEmployeeFieldChangeResponse SetOnChange(SessionContext sessionContext, Guid employeeId)
        {

            var staffService = sessionContext.ObjectContext.GetService<IStaffService>();

            var secondedEmployee = staffService.Get(employeeId) ?? throw new ArgumentException($"Invalid employee id {employeeId}");

            var manager = staffService.GetEmployeeManager(secondedEmployee);

            var managerId = manager.GetObjectId();
            var workPhoneNumber = manager.Phone;

            return new OnSecondedEmployeeFieldChangeResponse
            {
                ManagerId = managerId,
                WorkPhoneNumber = workPhoneNumber
            };
        }

        public SetExpensesResponse CalculateExpenses(SessionContext sessionContext, string cityName, int duration)
        {
            var baseUniversalService = sessionContext.ObjectContext.GetService<IBaseUniversalService>();

            var cityType = baseUniversalService.FindItemTypeWithSameName("Города", null);

            var cityItem = baseUniversalService.FindItemWithSameName(cityName, cityType);

            var expenses = (decimal)cityItem.ItemCard.MainInfo["DailyExpenses"] * duration;

            return new SetExpensesResponse
            {
                Expenses = expenses
            };
        }

        public void InitApplication(SessionContext sessionContext, Guid cardId)
        {   
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId);
            var staffService = sessionContext.ObjectContext.GetService<IStaffService>();

            var currentEmployee = staffService.GetCurrentEmployee();
            var manager = staffService.GetEmployeeManager(currentEmployee);

            card.MainInfo["SecondedEmployee"] = currentEmployee.GetObjectId();
            card.MainInfo["Manager"] = manager.GetObjectId();
            card.MainInfo["WorkPhoneNumber"] = manager.Phone;

            var secretaryGroup = staffService.FindGroupByName(null, "Секретарь");
            var availableSecretary = staffService.GetGroupEmployees(secretaryGroup, true, true, true).FirstOrDefault();

            var arrangers = (IList<BaseCardSectionRow>)card.GetSection(new Guid("D8A59AFE-3118-4AEB-9419-59DE69D4B622"));
            var arrangerRow = new BaseCardSectionRow();
            arrangerRow["Arranger"] = availableSecretary?.GetObjectId();
            arrangers.Add(arrangerRow);

            var approvers = (IList<BaseCardSectionRow>)card.GetSection(CardDocument.Approvers.ID);
            var approverRow = new BaseCardSectionRow();
            approverRow["Approver"] = currentEmployee.GetObjectId() == manager.GetObjectId()
                ? staffService.GetUnits(null, true).First().Manager.GetObjectId()
                : manager.GetObjectId();
            approvers.Add(approverRow);

            sessionContext.ObjectContext.SaveObject(card);
        }

        public async Task<GetTicketsCostsResponse> GetTicketsCosts(SessionContext sessionContext, Guid cityId, DateTime departureDate, DateTime returnDate)
        {
            var baseUniversalService = sessionContext.ObjectContext.GetService<IBaseUniversalService>();

            var cityType = baseUniversalService.FindItemTypeWithSameName("Города", null);

            var cityItem = cityType.Items.FirstOrDefault(x => x.GetObjectId() == cityId) 
                ?? throw new Exception($"Не найден город с ID {cityId}");

            var airportCode = (string)cityItem.ItemCard.MainInfo["AirportCode"];

            if (airportCode == "LED")
            {
                var responseFlight = new Flight()
                {
                    Airline = "00",
                    FlightNumber = "000",
                    Price = 0
                };
                return new GetTicketsCostsResponse
                {
                    Flights = new()
                    {
                        responseFlight
                    }
                };
            }

            string apiToken = "b165d8c4be5500d4da61df5067fd34ad";
            // Формируем URL для API запроса
            string apiUrl = BuildApiUrl(apiToken, airportCode, departureDate, returnDate);

            // Выполняем запрос к API
            HttpResponseMessage response = await new HttpClient().GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка API: {response.StatusCode}");
            }

            // Читаем и десериализуем ответ
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

            if (apiResponse is null || !apiResponse.success)
            {
                throw new Exception("API вернуло ошибку");
            }

            // Преобразуем данные и сортируем по стоимости
            var flights = ProcessFlights(apiResponse);

            return new GetTicketsCostsResponse
            {
                Flights = flights
            };
        }

        private string BuildApiUrl(string apiToken, string destination, DateTime departureDate, DateTime returnDate)
        {
            return $"https://api.travelpayouts.com/aviasales/v3/prices_for_dates?" +
                   $"origin=LED" +
                   $"&destination={destination.ToUpper()}" +
                   $"&departure_at={departureDate:yyyy-MM-dd}" +
                   $"&return_at={returnDate:yyyy-MM-dd}" +
                   $"&unique=false" +
                   $"&sorting=price" +
                   $"&direct=true" +
                   $"&currency=rub" +
                   $"&limit=10" +
                   $"&page=1" +
                   $"&one_way=false" +
                   $"&token={apiToken}";
        }

        private List<Flight> ProcessFlights(ApiResponse apiResponse)
        {
            var flights = new List<Flight>();

            if (apiResponse.data is null)
                return flights;

            foreach (var flightData in apiResponse.data)
            {
                var flight = new Flight
                {
                    Airline = flightData.airline,
                    FlightNumber = flightData.flight_number,
                    Price = flightData.price
                };
                flights.Add(flight);
            }

            // Сортируем по возрастанию стоимости
            return flights.OrderBy(x => x.Price).ToList();
        }
    }

    // Вспомогательные классы для десериализации
    internal class ApiResponse
    {
        public bool success { get; set; }
        public List<FlightData>? data { get; set; }
        public string? currency { get; set; }
    }

    // Snake case для избежания ошибок при десериализации
    internal class FlightData
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public string origin_airport { get; set; }
        public string destination_airport { get; set; }
        public decimal price { get; set; }
        public string airline { get; set; }
        public string flight_number { get; set; }
        public DateTime departure_at { get; set; }
        public DateTime return_at { get; set; }
        public int transfers { get; set; }
        public int return_transfers { get; set; }
        public int duration { get; set; }
        public int duration_to { get; set; }
        public int duration_back { get; set; }
        public string link { get; set; }
    }
}
