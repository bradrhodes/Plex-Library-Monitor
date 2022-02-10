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
      services.AddRefitClient<IPlexApi>().ConfigureHttpClient((provider, httpClient) =>
      {
         var settings = provider.GetRequiredService<PlibmonSettings>();

         httpClient.BaseAddress = new Uri(settings.PlexApiBaseAddress);
         httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      });
   }
}

public class SettingsNotLoadedException : Exception{}