using Microsoft.AspNetCore.Mvc;

namespace DeliveryApiModelMapper.TestSite.Controllers
{
	[Route("api/logs")]
	public class DeliveryApiLogApiController : Controller
	{
		private readonly Umbraco.Community.DeliveryApiModelMapper.Repository.ILogViewerRepository _logViewerRepository;

		public DeliveryApiLogApiController(Umbraco.Community.DeliveryApiModelMapper.Repository.ILogViewerRepository logViewerRepository)
		{
			_logViewerRepository = logViewerRepository;
		}

		[HttpGet("")]
		public async Task<IActionResult> Index(DateTime? start = null, DateTime? end = null, int? hours = null)
		{
			end = end ?? DateTime.Now;
			start = start ?? end.Value.AddHours(-(hours ?? 1));

			var period = new Umbraco.Cms.Core.Logging.Viewer.LogTimePeriod(start.Value, end.Value);

			var logs = await _logViewerRepository.GetLogsAsync(period, "| where Properties['IsDeliveryApi'] == 'True'");

			return Json(new
			{
				count = logs.Count(),
				start,
				end,
				logs
			});
		}

		[HttpGet("report")]
		public async Task<IActionResult> Report(DateTime? start = null, DateTime? end = null, int? hours = null)
		{
			end = end ?? DateTime.Now;
			start = start ?? end.Value.AddHours(-(hours ?? 1));

			var period = new Umbraco.Cms.Core.Logging.Viewer.LogTimePeriod(start.Value, end.Value);

			var logs = await _logViewerRepository.GetReportAsync(period);

			return Json(new
			{
				count = logs.Count(),
				start,
				end,
				logs
			});
		}
	}
}
