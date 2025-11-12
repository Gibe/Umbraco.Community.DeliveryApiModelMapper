using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Umbraco.Community.DeliveryApiModelMapper
{
	public static class DeliveryApiModelMapperExtensions
	{
		public static IServiceCollection AddDeliveryApiModelMapper(this IServiceCollection services)
		{
			services.ConfigureOpenTelemetryTracerProvider((x, builder) =>
			{
				builder.AddProcessor(new EnrichingActivityProcessor());
			});


			return services;
		}
	}

	public class EnrichingActivityProcessor : BaseProcessor<Activity>
	{
		public override void OnEnd(Activity data)
		{
			if (data.Kind != ActivityKind.Server)
			{
				return;
			}

			var tags = data.Tags.ToDictionary(x => x.Key, x => x.Value);
			var path = tags["url.path"];

			if (!path.StartsWith("/umbraco/delivery/api/v2/", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			data.AddTag("IsDeliveryApi", true);
		}
	}
}

