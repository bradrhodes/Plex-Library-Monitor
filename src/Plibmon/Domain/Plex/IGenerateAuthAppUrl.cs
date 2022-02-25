namespace Plibmon.Domain.Plex
{
    public interface IGenerateAuthAppUrl
    {
        // https://app.plex.tv/auth/#?clientID=e6d24101-d3f0-4bfc-88aa-0cbd71f1a137&code=1d6p3ri7ed77233n2j7ukceri&context%5Bdevice%5D%5Bproduct%5D=plibmon
        string GenerateUrl(string clientId, string pinCode, string clientName);
    }
    
    public class DefaultAuthAppUrlGenerator : IGenerateAuthAppUrl
    {
        // This is pretty ugly but it was just quick and dirty
        // The nested objects (context%5Bdevice%5D%5Bproduct%5D) I didn't know how to deal with in a better way
        public string GenerateUrl(string clientId, string pinCode, string clientName) => 
             $"https://app.plex.tv/auth/#?clientID={clientId}&code={pinCode}&context[device][product]={clientName}";
    }
}