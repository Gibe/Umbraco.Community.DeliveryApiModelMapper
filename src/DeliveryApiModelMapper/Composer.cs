using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Common.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;
using Umbraco.Community.DeliveryApiModelMapper.Models;
using Umbraco.Community.DeliveryApiModelMapper.OpenApi;
using Umbraco.Community.DeliveryApiModelMapper.Serialization;
using Umbraco.Community.DeliveryApiModelMapper.Services;

namespace Umbraco.Community.DeliveryApiModelMapper;

internal class Composer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		builder.Services.AddScoped<IDeliveryApiModelMapperService, DeliveryApiModelMapperService>();

		builder.Services.AddScoped<IApiContentResponseBuilder, DeliveryApiModelMapperApiContentResponseBuilder>();

		builder.Services
						.AddControllers()
						.AddJsonOptions(
								Constants.JsonOptionsNames.DeliveryApi,
								options => options
										.JsonSerializerOptions
										.TypeInfoResolver = new DeliveryApiModelMapperDeliveryApiJsonTypeResolver()
						);

		builder.Services.AddOptions<DeliveryApiModelMapperSettings>()
				.BindConfiguration("Umbraco:CMS:DeliveryApiModelMapper")
				.ValidateOnStart();

		builder.Services.Configure<SwaggerGenOptions>(options =>
				options.DocumentFilter<DeliveryApiModelMapperDocumentFilter>());
	}
}
