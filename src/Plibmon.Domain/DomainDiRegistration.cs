using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Plibmon.Domain.Plex;
using Refit;

namespace Plibmon.Domain;

public static class DomainDiRegistration
{
   public static void AddPlibmonDomain(this IServiceCollection services)
   {
      services.AddSingleton<IGenerateAuthAppUrl, DefaultAuthAppUrlGenerator>();
      services.AddSingleton<IStorageAdapter, FileStorageAdapter>();
      services.AddSingleton<ITokenService, TokenService>();
      services.AddSingleton<IUserInfoService, UserInfoService>();
      services.AddSingleton<IPlibmonService, PlibmonService>();
      services.AddSingleton<IPlexSdk, PlexSdk>();
      services.AddSingleton<IPinService, PinService>();
      services.AddRefitClient<IPlexApi>().ConfigureHttpClient(httpClient =>
      {
         httpClient.BaseAddress = new Uri("https://plex.tv/api/v2");
         httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      });
   }

   public static void AddPlibmonSampleConfig(this IServiceCollection services)
   {
      var plibmonSettings = new PlibmonSettings
      {
         CacheFile = "data.json",
         CacheFolder = "data",
         ClientId = "e6d24101-d3f0-4bfc-88aa-0cbd71f1a137",
         ClientName = "plibmon"
      };
      var fileStorageSettings = new FileStorageSettings
      {
         FileName = plibmonSettings.CacheFile,
         FolderName = plibmonSettings.CacheFolder
      };
      
      services.AddSingleton(_ => plibmonSettings);
      services.AddSingleton(_ => fileStorageSettings);
   }
   
    
}