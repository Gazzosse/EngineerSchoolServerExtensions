using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.WebClient;
using DocumentFormat.OpenXml.ExtendedProperties;
using MyExtension.ApplicationBusinessTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension.ApplicationBusinessTrip
{
    public class ApplicationBusinessTripService : IApplicationBusinessTripService
    {
        public OnSecondedEmployeeFieldChangeResponse SetOnChange(SessionContext sessionContext, Guid employeeId)
        {

            var staffService = sessionContext.ObjectContext.GetService<IStaffService>();

            //var unit = sessionContext.ObjectContext.GetObject<StaffUnit>(employeeUnitId) ?? throw new ArgumentException("Invalid unit id");

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
    }
}
