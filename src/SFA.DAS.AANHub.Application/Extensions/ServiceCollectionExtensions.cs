using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        }
    }
}
