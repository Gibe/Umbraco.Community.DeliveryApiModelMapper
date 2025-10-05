using DeliveryApiModelMapper.TestSite.Models.DeliveryApi.ModelMappers;
using DeliveryApiModelMapper.TestSite.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;

namespace DeliveryApiModelMapper.TestSite.App_Start
{
	public class StartupComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddScoped<IDeliveryApiModelFactory, DeliveryApiModelFactory>();

			ComposeDeliveryApiModelMappers(builder);
		}

		private void ComposeDeliveryApiModelMappers(IUmbracoBuilder builder)
		{
			builder.Services.AddScoped<IDeliveryApiModelMapper, HomeModelMapper>();
		}
	}
}
