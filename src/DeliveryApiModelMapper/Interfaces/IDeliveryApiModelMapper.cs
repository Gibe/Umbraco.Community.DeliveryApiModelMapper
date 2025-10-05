using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Community.DeliveryApiModelMapper.Interfaces;

public interface IDeliveryApiModelMapper
{
    bool CanMapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures);
    object MapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures);
}
