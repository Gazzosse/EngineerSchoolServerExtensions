using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.WebClient.State;
using DocsVision.Layout.WebClient.AdvancedLayouts.ExtendedDataSources;
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
    }
}
