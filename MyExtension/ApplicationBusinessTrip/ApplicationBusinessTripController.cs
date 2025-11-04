using DocsVision.Platform.WebClient.Models.Generic;
using DocsVision.Platform.WebClient;
using Microsoft.AspNetCore.Mvc;
using MyExtension.ApplicationBusinessTrip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocsVision.Platform.WebClient.Models;

namespace MyExtension.ApplicationBusinessTrip
{
    public class ApplicationBusinessTripController
    {
        private readonly ICurrentObjectContextProvider _currentObjectContextProvider;
        private readonly IApplicationBusinessTripService _applicationBusinessTripService;

        public ApplicationBusinessTripController(
            ICurrentObjectContextProvider currentObjectContextProvider,
            IApplicationBusinessTripService applicationBusinessTripService)
        {
            _currentObjectContextProvider = currentObjectContextProvider;
            _applicationBusinessTripService = applicationBusinessTripService;
        }

        [HttpPost]
        public CommonResponse<OnSecondedEmployeeFieldChangeResponse> SetFieldsOnSecondedEmployeeFieldChange([FromBody] Guid request)
        {
            var sessionContext = _currentObjectContextProvider.GetOrCreateCurrentSessionContext();
            var result = _applicationBusinessTripService.SetOnChange(sessionContext, request);
            return CommonResponse.CreateSuccess(result);
        }

        [HttpPost]
        public CommonResponse<SetExpensesResponse> SetExpenses([FromBody] SetExpensesRequest request)
        {
            var sessionContext = _currentObjectContextProvider.GetOrCreateCurrentSessionContext();
            var result = _applicationBusinessTripService.CalculateExpenses(sessionContext, request.CityName, request.Duration);
            return CommonResponse.CreateSuccess(result);
        }

    }
}
