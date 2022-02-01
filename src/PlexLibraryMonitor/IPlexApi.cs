using System.Threading.Tasks;
using PlexLibraryMonitor.Plex.Models;
using Refit;

namespace PlexLibraryMonitor
{
    public interface IPlexApi
    {
        // [Post("pins?X-Plex-Product={clientName}&X-Plex-Client-Identifier={clientId}&strong=true")]
        [Post("pins")]
        Task<PinResponse> GetPin([AliasAs("X-Plex-Product")] string clientName, 
            [AliasAs("X-Plex-Client-Identifier")] string clientId,
            [AliasAs("strong")] bool strong = true);
        
        
    }
    
    
}