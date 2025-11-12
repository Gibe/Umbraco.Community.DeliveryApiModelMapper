using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;

namespace Umbraco.Community.DeliveryApiModelMapper
{
	public class EnrichingActivityProcessor : BaseProcessor<Activity>
	{
		public EnrichingActivityProcessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override void OnEnd(Activity data)
		{
			if (data.Kind != ActivityKind.Server)
			{
				return;
			}

			var tags = data.TagObjects.ToDictionary(x => x.Key, x => x.Value?.ToString());

			var path = Tag("url.path");
			var statusCode = int.TryParse(Tag("http.response.status_code"), out var status) ? status : 0;
			var status2 = _httpContextAccessor.HttpContext!.Response.StatusCode;

			if (!path.StartsWith(DeliveryApiBasePath, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			data.AddTag("IsDeliveryApi", true);
			data.AddTag("DeliveryApiQueryType", QueryType(path));
			data.AddTag("DeliveryApiStatus", StatusCodeDescription(statusCode));

			string Tag(string key)
			{
				if (!tags.ContainsKey(key))
				{
					return "";
				}

				return tags[key];
			}
		}

		private string QueryType(string path)
		{
			var queryType = "search";
			if (path.StartsWith(DeliveryApiItemPath, StringComparison.OrdinalIgnoreCase))
			{
				queryType = "item";
			}

			return queryType;
		}

		private string StatusCodeDescription(int statusCode)
		{
			switch (statusCode)
			{
				case 200:
					return "success";
				case 404:
					return "not-found";
				default:
					return statusCode >= 500 && statusCode < 600
						? "exception"
						: ((HttpStatusCode)statusCode).ToString();
			}
		}

		private const string DeliveryApiBasePath = "/umbraco/delivery/api/v2/";
		private const string DeliveryApiItemPath = DeliveryApiBasePath + "item";
		private readonly IHttpContextAccessor _httpContextAccessor;
	}
}

