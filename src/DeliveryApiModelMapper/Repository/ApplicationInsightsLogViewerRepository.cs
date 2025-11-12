using System.Text.Json;
using Azure.Identity;
using Azure.Monitor.Query.Logs;
using Azure.Monitor.Query.Logs.Models;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Profiling.Internal;
using Umbraco.Cms.Core.Logging.Viewer;
using Umbraco.Cms.Infrastructure.Logging.Serilog;
using Umbraco.Community.DeliveryApiModelMapper.Settings;
using static Umbraco.Cms.Core.Constants.Conventions;

namespace Umbraco.Community.DeliveryApiModelMapper.Repository
{
	public interface ILogViewerRepository
	{
		Task<IEnumerable<RequestEntry>> GetLogsAsync(LogTimePeriod logTimePeriod, string? filterExpression = null);
		Task<IEnumerable<ReportEntry>> GetReportAsync(LogTimePeriod logTimePeriod);
	}


	public class ApplicationInsightsLogViewerRepository : ILogViewerRepository
	{
		private readonly UmbracoFileConfiguration _umbracoFileConfig;
		private readonly ApplicationInsightsSettings _options;

		public ApplicationInsightsLogViewerRepository(IOptions<ApplicationInsightsSettings> options, UmbracoFileConfiguration umbracoFileConfig)
		{
			_umbracoFileConfig = umbracoFileConfig;
			_options = options.Value;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<RequestEntry>> GetLogsAsync(LogTimePeriod logTimePeriod, string? filterExpression = null)
		{
			try
			{
				var table = await GetRemoteLogsAsync(logTimePeriod, filterExpression);

				return table.Rows.Select(x => new RequestEntry
				{
					Id = x.GetString("Id"),
					TimeStamp = x.GetDateTimeOffset("TimeGenerated").GetValueOrDefault(),
					Name = x.GetString("Name"),
					Url = x.GetString("Url"),
					Success = x.GetBoolean("Success") ?? false,
					Duration = x.GetDouble("DurationMs") ?? -1,
					PerformanceBucket = x.GetString("PerformanceBucket"),
					ResultCode = x.GetString("ResultCode"),
					Properties = Properties(x.GetString("Properties"))
				}).ToList();

				static Dictionary<string, string?> Properties(string properties)
				{
					if (string.IsNullOrWhiteSpace(properties))
					{
						return new Dictionary<string, string?>();
					}

					return JsonSerializer.Deserialize<Dictionary<string, string?>>(properties) ?? new Dictionary<string, string?>();
				}

			}
			catch (Exception e)
			{
				Log.Error(e, "Error getting logs from Application Insights");
				return Enumerable.Empty<RequestEntry>();
			}
		}

		public async Task<IEnumerable<ReportEntry>> GetReportAsync(LogTimePeriod logTimePeriod)
		{
			try
			{

				var filterExpression = " | where Properties['IsDeliveryApi'] == 'True' "
					+ " | project"
					+ "			DurationMs, "
					+ "			Url, "
					+ "			IsDeliveryApi = Properties['IsDeliveryApi'], "
					+ "			DeliveryApiQueryType = Properties['DeliveryApiQueryType'], "
					+ "			DeliveryApiStatus = Properties['DeliveryApiStatus'] "
					+ " | summarize"
					+ "			AverageDurationMs = avg(DurationMs), "
					+ "			SuccessCount = countif(DeliveryApiStatus == 'success'), "
					+ "			NotFoundCount = countif(DeliveryApiStatus == 'not-found'), "
					+ "			ErrorCount = countif(DeliveryApiStatus == 'exception'), "
					+ "			UnknownCount = countif(DeliveryApiStatus == 'unknown'), "
					+ "			Count = count() "
					+ "				by Url"
					+ " | order by Count desc";

				var table = await GetRemoteLogsAsync(logTimePeriod, filterExpression);

				return table.Rows.Select(row => new ReportEntry
				{
					PathAndQuery = Url(row),
					AverageDurationMs = row.GetDouble("AverageDurationMs") ?? -1,
					SuccessCount = row.GetInt64("SuccessCount").GetValueOrDefault(),
					NotFoundCount = row.GetInt64("NotFoundCount").GetValueOrDefault(),
					Count = row.GetInt64("Count").GetValueOrDefault(),
					ErrorCount = row.GetInt64("ErrorCount").GetValueOrDefault(),
					UnknownCount = row.GetInt64("UnknownCount").GetValueOrDefault()
				}).ToList();


				string Url(LogsTableRow row)
				{
					var url = row.GetString("Url");
					if (string.IsNullOrWhiteSpace(url))
					{
						return url;
					};

					return url.StartsWith("https://") || url.StartsWith("http://")
						? new Uri(url).PathAndQuery : url;
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Error getting logs from Application Insights");
				return Enumerable.Empty<ReportEntry>();
			}
		}

		private async Task<LogsTable> GetRemoteLogsAsync(LogTimePeriod logTimePeriod, string? query = null)
		{
			var aiQuery = "AppRequests";
			if (query.HasValue())
			{
				aiQuery += $" {query}";
			}

			var client = new LogsQueryClient(new DefaultAzureCredential()); // TODO : proper auth - new ClientSecretCredential(_options.TenantId, _options.ClientId, _options.ClientSecret)); TODO : This isn't getting correct permission
			var result = await client.QueryWorkspaceAsync(_options.WorkspaceId, aiQuery,
				 new LogsQueryTimeRange(logTimePeriod.StartTime, logTimePeriod.EndTime.AddDays(1)));

			return result.Value.Table;
		}
	}

	public class RequestEntry
	{
		public string Id { get; set; } = "";
		public DateTimeOffset TimeStamp { get; set; }
		public string Name { get; set; } = "";
		public string Url { get; set; } = "";

		public bool Success { get; set; }
		public string ResultCode { get; set; } = "";
		public double Duration { get; set; }
		public string PerformanceBucket { get; set; } = "";
		public Dictionary<string, string?> Properties { get; set; } = new Dictionary<string, string?>();
	}

	public class ReportEntry
	{
		public string PathAndQuery { get; set; } = "";
		public double AverageDurationMs { get; set; }
		public long SuccessCount { get; set; }
		public long NotFoundCount { get; set; }
		public long ErrorCount { get; set; }
		public long UnknownCount { get; set; }
		public long Count { get; set; }
	}
}
