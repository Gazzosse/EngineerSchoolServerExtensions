using DocsVision.Platform.WebClient;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtension.ApplicationBusinessTrip
{
    public class ApplicationBusinessTripLifeCycle : ICardLifeCycleEx
    {
        public ApplicationBusinessTripLifeCycle(ICardLifeCycleEx baseLifeCycle, IApplicationBusinessTripService service)
        {
            this.BaseLifeCycle = baseLifeCycle;
            this.ApplicationBusinessTripService = service;
        }
        protected ICardLifeCycleEx BaseLifeCycle { get; }
        protected IApplicationBusinessTripService ApplicationBusinessTripService { get; }

        public Guid CardTypeId => BaseLifeCycle.CardTypeId;

        public bool CanDelete(SessionContext sessionContext, CardDeleteLifeCycleOptions options, out string message)
            => BaseLifeCycle.CanDelete(sessionContext, options, out message);

        public Guid Create(SessionContext sessionContext, CardCreateLifeCycleOptions options)
        {
            var cardId = BaseLifeCycle.Create(sessionContext, options);
            if (options.CardKindId == new Guid("6cf0521a-a2f2-4595-a65b-3c26f17f60a8"))
            {
                ApplicationBusinessTripService.InitApplication(sessionContext, cardId);
            }
            return cardId;
        }

        public string GetDigest(SessionContext sessionContext, CardDigestLifeCycleOptions options)
        => BaseLifeCycle.GetDigest(sessionContext, options);

        public void OnDelete(SessionContext sessionContext, CardDeleteLifeCycleOptions options)
            => BaseLifeCycle.OnDelete(sessionContext, options);

        public void OnSave(SessionContext sessionContext, CardSaveLifeCycleOptions options)
            => BaseLifeCycle.OnSave(sessionContext, options);

        public bool Validate(SessionContext sessionContext, CardValidateLifeCycleOptions options, out List<ValidationResult> Validate)
            => BaseLifeCycle.Validate(sessionContext, options, out Validate);
    }
}
