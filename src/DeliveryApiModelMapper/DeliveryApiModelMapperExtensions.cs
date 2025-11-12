using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NPoco.Expressions;
using OpenTelemetry.Trace;

namespace Umbraco.Community.DeliveryApiModelMapper
{
	public static class DeliveryApiModelMapperExtensions
	{
		public static IServiceCollection AddDeliveryApiModelMapper(this IServiceCollection services)
		{
			services.ConfigureOpenTelemetryTracerProvider((x, builder) =>
			{
				builder.AddProcessor(new EnrichingActivityProcessor(x.GetService<IHttpContextAccessor>()));
			});


			return services;
		}
	}
}

