using Hangfire;
using Plibmon.Domain.HangfireJobs;
using Plibmon.Shared;

namespace Plibmon.Domain;

class PlibmonService : IPlibmonService
{
    private readonly PlibmonSettings _settings;
    private readonly ITokenService _tokenService;
    private readonly IPinService _pinService;
    private readonly IRecurringJobManager _recurringJobManager;

    public PlibmonService(PlibmonSettings settings, ITokenService tokenService, IPinService pinService, IRecurringJobManager recurringJobManager)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _pinService = pinService ?? throw new ArgumentNullException(nameof(pinService));
        _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
    }
    public async Task<bool> CanConnectToPlex(CancellationToken cancellationToken)
    {
        var plexToken = await _tokenService.GetToken(cancellationToken).ConfigureAwait(false);
        return !string.IsNullOrEmpty(plexToken.Token);
    }

    public Task<PinLinkResult> GetPinLink(CancellationToken cancellationToken)
        => _pinService.GetPinLink(_settings.ClientId, _settings.ClientName, cancellationToken);

    public void PollForPinAuthorization(string pinId, string pinCode, string clientId, CancellationToken cancellationToken)
    {
        _recurringJobManager.AddOrUpdate<ValidateAppAuthorization>(ValidateAppAuthorization.BackgroundTaskId, task => task.Validate(pinId, pinCode, clientId, CancellationToken.None), "*/2 * * * * *");
    }
}