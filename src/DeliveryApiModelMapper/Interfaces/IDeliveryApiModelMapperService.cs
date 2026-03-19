using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Community.DeliveryApiModelMapper.Interfaces;

public interface IDeliveryApiModelMapperService
{
	IDeliveryApiModelMapper? Find(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures);
	IApiContentResponse CreateApiResponse(object model, IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, object?> properties, IDictionary<string, IApiContentRoute> cultures);
}
