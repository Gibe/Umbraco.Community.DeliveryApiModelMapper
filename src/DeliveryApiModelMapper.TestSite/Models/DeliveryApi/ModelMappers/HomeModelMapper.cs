using DeliveryApiModelMapper.TestSite.Models.DeliveryApi;
using DeliveryApiModelMapper.TestSite.Models.ModelsBuilder;
using DeliveryApiModelMapper.TestSite.Services;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;

namespace DeliveryApiModelMapper.TestSite.Models.DeliveryApi.ModelMappers
{
	public class HomeModelMapper : IDeliveryApiModelMapper
	{
		private readonly IDeliveryApiModelFactory _deliveryApiModelFactory;

		public HomeModelMapper(IDeliveryApiModelFactory deliveryApiModelFactory)
		{
			_deliveryApiModelFactory = deliveryApiModelFactory;
		}

		public bool CanMapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
			=> content is Home;

		public object MapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
		{
			var home = _deliveryApiModelFactory.Create<HomeModel>(content);

			var homeContent = content as Home;
			if (homeContent != null)
			{
				home.Title = homeContent.Title ?? "";
				home.Text = homeContent.Text?.ToString() ?? "";
			}

			return home;
		}
	}
}
