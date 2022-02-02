﻿namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract class ValidateTokenResponse
    {
        public class ValidToken : ValidateTokenResponse
        {
            public PlexUserInfo PlexUserInfo { get; set; }
        }
        
        public class InvalidToken : ValidateTokenResponse{}
    }
}