using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Api.Common.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;
using Umbraco.Community.DeliveryApiModelMapper.Models;
using Umbraco.Community.DeliveryApiModelMapper.Serialization;
using Umbraco.Community.DeliveryApiModelMapper.Services;

namespace Umbraco.Community.DeliveryApiModelMapper
{
	internal class Composer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddScoped<IDeliveryApiModelMapperService, ModelMapperService>();

			builder.Services.AddScoped<IApiContentResponseBuilder, ModelMapperApiContentResponseBuilder>();

			builder.Services
					.AddControllers()
					.AddJsonOptions(
						Constants.JsonOptionsNames.DeliveryApi,
						options => options
							.JsonSerializerOptions
							.TypeInfoResolver = new ModelMapperDeliveryApiJsonTypeResolver()
					);

			builder.Services.AddOptions<ModelMapperSettings>()
				.BindConfiguration("Umbraco:CMS:DeliveryApiModelMapper")
				.ValidateOnStart();
		}
	}
}
