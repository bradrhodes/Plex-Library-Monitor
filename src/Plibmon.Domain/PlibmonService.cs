using Hangfire;
using Microsoft.Extensions.Options;
using Plibmon.Domain.HangfireJobs;
using Plibmon.Shared;

namespace Plibmon.Domain;

class PlibmonService : IPlibmonService
{
    private readonly PlibmonSettings _settings;
    private readonly ITokenService _tokenService;
    private readonly IPinService _pinService;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IClientIdService _clientIdService;

    public PlibmonService(PlibmonSettings settings, ITokenService tokenService, IPinService pinService, 
        IRecurringJobManager recurringJobManager, IClientIdService clientIdService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _pinService = pinService ?? throw new ArgumentNullException(nameof(pinService));
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
        _clientIdService = clientIdService ?? throw new ArgumentNullException(nameof(clientIdService));
    }
    public Task<bool> CanConnectToPlex(CancellationToken cancellationToken) 
        => _tokenService.HaveValidToken(cancellationToken);

    public async Task<PinLinkResult> GetPinLink(CancellationToken cancellationToken)
        => await _pinService.GetPinLink(
            (await _clientIdService.GetClientId(cancellationToken).ConfigureAwait(false)).ClientId, 
            _settings.ClientName, cancellationToken);

    public void PollForPinAuthorization()
    {
        _recurringJobManager.AddOrUpdate<ValidateAppAuthorization>(ValidateAppAuthorization.BackgroundTaskId, 
            task => 
                task.Validate(_clientIdService.GetClientId(CancellationToken.None).GetAwaiter().GetResult().ClientId, 
                    CancellationToken.None), "*/2 * * * * *");
    }
}