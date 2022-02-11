using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Plibmon.Domain.Persistance;
using Plibmon.Domain.Plex;
using Refit;

namespace Plibmon.Domain;

public static class DomainDiRegistration
{
   public static void AddPlibmonDomain(this IServiceCollection services, WebApplicationBuilder builder)
   {
      // Note: If you need to override the values in the appSettings.json file, they can be overridden via
      // Environment vars.  Naming uses double __.  Ex. To override the ClientId, create and set an
      // Environment var named 'Plibmon__ClientId'.
      services.AddSingleton<PlibmonSettings>(_ =>
         builder.Configuration.GetSection(PlibmonSettings.ConfigSectionName).Get<PlibmonSettings>());
      services.AddSingleton<FileStorageSettings>(provider =>
      {
         var settings = provider.GetRequiredService<PlibmonSettings>();

         return new FileStorageSettings
         {
            FileName = settings.CacheFile,
            FolderName = settings.CacheFolder
         };
      });
      
      services.AddSingleton<IGenerateAuthAppUrl, DefaultAuthAppUrlGenerator>();
      services.AddSingleton<IStorageAdapter, FileStorageAdapter>();
      services.AddSingleton<ITokenService, TokenService>();
      services.AddSingleton<IUserInfoService, UserInfoService>();
      services.AddSingleton<IPlibmonService, PlibmonService>();
      services.AddSingleton<IPlexSdk, PlexSdk>();
      services.AddSingleton<IPinService, PinService>();
      services.AddSingleton<IClientIdService, ClientIdService>();
      services.AddRefitClient<IPlexApi>().ConfigureHttpClient((provider, httpClient) =>
      {
         var settings = provider.GetRequiredService<PlibmonSettings>();

         httpClient.BaseAddress = new Uri(settings.PlexApiBaseAddress);
         httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      });
   }
}

public class SettingsNotLoadedException : Exception{}