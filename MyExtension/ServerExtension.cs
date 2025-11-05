using Autofac;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.WebClient.Extensibility;
using DocsVision.WebClient.Helpers;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle;
using Microsoft.Extensions.DependencyInjection;
using MyExtension.ApplicationBusinessTrip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;

namespace MyExtension
{
    /// <summary>
    /// Задаёт описание расширения для WebClient, которое задано в текущей сборке
    /// </summary>
    public class MyExtension : WebClientExtension
    {
        /// <summary>
        /// Создаёт новый экземпляр <see cref="MyExtension" />
        /// </summary>
        /// <param name="serviceProvider">Сервис-провайдер</param>
        public MyExtension(IServiceProvider serviceProvider)
            : base()
        {
        }

        public override string ExtensionName => "MyExtension";

        public override Version ExtensionVersion => new(1, 0, 0);

        /// <summary>
        /// Регистрация типов в IoC контейнере
        /// </summary>
        /// <param name="containerBuilder"></param>
        public override void InitializeServiceCollection(IServiceCollection services)
        {
            services.AddSingleton<IApplicationBusinessTripService, ApplicationBusinessTripService>();

            services.Decorate<ICardLifeCycleEx>((original, serviceProvider) => {
                var typeId = original.CardTypeId;
                if (typeId == CardDocument.ID)
                {
                    var applicationBusinessTripService = serviceProvider.GetRequiredService<IApplicationBusinessTripService>();
                    return new ApplicationBusinessTripLifeCycle(original, applicationBusinessTripService);
                }
                return original;
            });

        }

        /// <summary>
        /// Gets resource managers for layout extension
        /// </summary>
        /// <returns></returns>
        protected override List<ResourceManager> GetLayoutExtensionResourceManagers()
        {
            return new List<ResourceManager>
            {

            };
        }
    }
}