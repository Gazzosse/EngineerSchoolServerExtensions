using DocsVision.Platform.WebClient;
using MyExtension.ApplicationBusinessTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension.ApplicationBusinessTrip
{
    public interface IApplicationBusinessTripService
    {
        OnSecondedEmployeeFieldChangeResponse SetOnChange(SessionContext sessionContext, Guid employeeId);
        SetExpensesResponse CalculateExpenses(SessionContext sessionContext, string cityName, int duration);
        Task<GetTicketsCostsResponse> GetTicketsCosts(SessionContext sessionContext, Guid cityId, DateTime departureDate, DateTime returnDate);
        void InitApplication(SessionContext sessionContext, Guid cardId);
    }
}
