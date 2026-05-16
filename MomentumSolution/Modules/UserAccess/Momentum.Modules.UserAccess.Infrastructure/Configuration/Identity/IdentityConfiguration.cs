using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Momentum.Modules.UserAccess.Application.Contracts;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Identity;

public static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentityService(this IServiceCollection services)
    {
        services
            .AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<UserAccessContext>();
            })
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("connect/token");

                options.RegisterScopes(
                    "all",
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    CustomClaimTypes.Roles);

                options.AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .AllowClientCredentialsFlow();

                options.AcceptAnonymousClients();

                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        return services;
    }

    public static IApplicationBuilder AddIdentityService(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
