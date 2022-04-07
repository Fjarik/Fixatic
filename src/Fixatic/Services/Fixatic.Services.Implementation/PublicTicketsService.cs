using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class PublicTicketsService : IPublicTicketsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private PublicTicketsManager? _manager;

		public PublicTicketsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<TicketsService> logger)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
		}
		public async Task<ServiceResponse<List<Ticket>>> GetPublicAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Ticket>>();
			try
			{
				response.Item = await _manager!.GetPublicAsync();
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}
			return response;
		}


		private async Task EnsureManagerAsync()
		{
			if (_manager == null)
			{
				await Task.CompletedTask;
				_manager = new PublicTicketsManager(_logger, _applicationSettings);
			}
		}

	}
}
