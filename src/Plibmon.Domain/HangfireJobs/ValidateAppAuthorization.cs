using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Plibmon.Domain.Events;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain.HangfireJobs;

public class ValidateAppAuthorization 
{
    public static string BackgroundTaskId { get; } = "AppAuthValidator";
    
    private readonly IPinService _pinService;
    private readonly IMediator _mediator;
    private readonly ILogger<ValidateAppAuthorization> _logger;
    private readonly IRecurringJobManager _recurringJobManager;

    public ValidateAppAuthorization(IPinService pinService, IMediator mediator, ILogger<ValidateAppAuthorization> logger, IRecurringJobManager recurringJobManager)
    {
        _pinService = pinService ?? throw new ArgumentNullException(nameof(pinService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _recurringJobManager = recurringJobManager;
    }

    public async Task Validate(string clientId, CancellationToken cancellationToken)
    {
        var authResult = await _pinService.ValidatePin(clientId, cancellationToken).ConfigureAwait(false);

        switch (authResult)
        {
            case PinAuthorizationResponse.Success pinAuth:
                _logger.LogInformation("App is authorized.");
                _logger.LogDebug("App is authorized: {@token}", pinAuth.AuthToken);
                // Publish an event
                await _mediator.Publish(new AppAuthorizationChecked.Authorized(), cancellationToken).ConfigureAwait(false);
                // Kill this job
                _recurringJobManager.RemoveIfExists(BackgroundTaskId);
                break;
            case PinAuthorizationResponse.PinNotYetAuthorized:
                _logger.LogInformation("App is not yet authorized.");
                await _mediator.Publish(new AppAuthorizationChecked.NotAuthorized(), cancellationToken)
                    .ConfigureAwait(false);
                break;
            case PinAuthorizationResponse.PinAuthorizationInvalidOrExpired:
                _logger.LogError("Cannot authorize app. Pin information is invalid or expired.");
                await _mediator.Publish(new AppAuthorizationChecked.PinNotFound(), cancellationToken)
                    .ConfigureAwait(false);
                _recurringJobManager.RemoveIfExists(BackgroundTaskId);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}