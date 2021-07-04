using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Core.Infra.Data;
using MinhaLoja.Core.Infra.Data.EventStore;
using MinhaLoja.Core.Infra.Identity.Middlewares;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Infra.ServiceBus;
using MinhaLoja.Core.Infra.Services.LogHandler;
using MinhaLoja.Core.Infra.Services.Mail;
using MinhaLoja.Core.Mediator;
using MinhaLoja.Core.Settings;
using MinhaLoja.Infra.Api.Identity.Services;
using MinhaLoja.Infra.Data;
using MinhaLoja.Infra.Data.DataSources;
using MinhaLoja.Infra.Data.DataSources.DatabaseMain;
using MinhaLoja.Infra.Data.DataSources.DatabaseSecondary;
using MinhaLoja.Infra.Data.EventStore;
using MinhaLoja.Infra.Identity.IdentityStorage;
using MinhaLoja.Infra.Identity.Middlewares;
using MinhaLoja.Infra.ServiceBus;
using MinhaLoja.Infra.Services.LogHandler;
using MinhaLoja.Infra.Services.Mail;
using System;

namespace MinhaLoja.Infra.Ioc
{
    public static class Dependencies
    {
        public static void RegisterDependencies(
            this IServiceCollection services,
            GlobalSettings settings)
        {
            services.AddMediatR(AppDomain.CurrentDomain.Load("MinhaLoja.Domain"));
            services.AddTransient<IMediatorHandler, MediatorHandler>();
            services.AddTransient<DependenciesAppService, DependenciesAppService>();
            services.AddTransient<DependenciesEventHandler, DependenciesEventHandler>();

            Infra(services, settings);

            MappingDependenciesRepositories.RegisterMappings(services);
        }
        
        public static void RegisterDependenciesWithIdentity(
            this IServiceCollection services,
            GlobalSettings settings)
        {
            RegisterDependencies(services, settings);

            //Identity
            services.AddDbContextPool<IdentityMinhaLojaContext>(options => options.UseSqlServer(settings.DatabaseConnectionString));
            services.AddSingleton<IAuthenticationMiddleware>(new AuthenticationMiddleware());
            services.AddTransient<ITokenService, TokenService>();
        }


        private static void Infra(
            IServiceCollection services,
            GlobalSettings settings)
        {
            //Data
            services.AddDbContextPool<MinhaLojaContext>(options =>
                options.UseSqlServer(settings.DatabaseConnectionString)
            );
            services.AddScoped<MinhaLojaContextSecondaryDatabase, MinhaLojaContextSecondaryDatabase>();
            services.AddTransient<IApplicationTransaction, ApplicationTransaction>();
            services.AddTransient<DependenciesRepositories, DependenciesRepositories>();
            services.AddTransient<IEventStoreRepository, EventStoreRepository>();

            //ErrorApplication
            services.AddTransient<ILogErrorHandler, LogErrorHandler>();

            //
            services.AddTransient<IServiceBusManagement, ServiceBusManagement>();

            //Services
            services.AddTransient<IMailService, MailService>();
        }
    }
}
