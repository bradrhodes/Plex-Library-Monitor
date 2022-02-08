using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Error
    {
        public int code { get; set; }
        public string message { get; set; } = string.Empty;
        public int status { get; set; }
    }
}