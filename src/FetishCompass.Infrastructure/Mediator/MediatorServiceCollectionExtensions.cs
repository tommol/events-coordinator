using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Application.Queries;
using FetishCompass.Shared.Application.Common;

namespace FetishCompass.Infrastructure.Mediator
{
    public static class MediatorServiceCollectionExtensions
    {
        /// <summary>
        /// Rejestruje mediator oraz wszystkie handlery z podanych asemblerów.
        /// </summary>
        public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Rejestracja Mediatora i aliasów interfejsów.
            // Rejestrujemy implementację jako IMediator, a następnie mapujemy ICommandBus/IQueryBus do tej samej instancji.
            services.AddSingleton<IMediator, Mediator>();
            services.AddSingleton<ICommandBus>(sp => sp.GetRequiredService<IMediator>());
            services.AddSingleton<IQueryBus>(sp => sp.GetRequiredService<IMediator>());

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            var types = assemblies
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.ImplementedInterfaces;

                foreach (var iface in interfaces)
                {
                    if (iface.IsGenericType)
                    {
                        var def = iface.GetGenericTypeDefinition();

                        if (def == typeof(ICommandHandler<>))
                        {
                            services.AddTransient(iface, type.AsType());
                        }
                        else if (def == typeof(ICommandHandler<,>))
                        {
                            services.AddTransient(iface, type.AsType());
                        }
                        else if (def == typeof(IQueryHandler<,>))
                        {
                            services.AddTransient(iface, type.AsType());
                        }
                    }
                }
            }

            return services;
        }
    }
}
