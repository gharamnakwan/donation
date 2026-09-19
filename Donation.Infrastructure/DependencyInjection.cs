using Microsoft.Extensions.DependencyInjection;
using Donation.Application.Abstractions.Services;
using Donation.Infrastructure.Services;

namespace Donation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    { 
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}