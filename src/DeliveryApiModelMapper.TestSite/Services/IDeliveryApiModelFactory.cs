using DeliveryApiModelMapper.TestSite.Models.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace DeliveryApiModelMapper.TestSite.Services
{
	public interface IDeliveryApiModelFactory
	{
		TModel Create<TModel>(IPublishedContent content) where TModel : BaseModel, new();
		FooterLink Map(Link link);
	}
}