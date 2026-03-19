using DeliveryApiModelMapper.TestSite.Models.ModelsBuilder;
using DeliveryApiModelMapper.TestSite.Services;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;

namespace DeliveryApiModelMapper.TestSite.Models.DeliveryApi.ModelMappers;

public class PageModelMapper : IDeliveryApiModelMapper
{
	public Type SchemaModelType() => typeof(PageModel);

	private readonly IDeliveryApiModelFactory _deliveryApiModelFactory;

	public PageModelMapper(IDeliveryApiModelFactory deliveryApiModelFactory)
	{
		_deliveryApiModelFactory = deliveryApiModelFactory;
	}

	public bool CanMapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
		=> content is Page;

	public object MapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
	{
		var page = _deliveryApiModelFactory.Create<PageModel>(content);

		var pageModel = content as Page;
		if (pageModel != null)
		{
			page.Title = pageModel.Title ?? "";
			page.Text = pageModel.Text?.ToString() ?? "";
			page.Link = _deliveryApiModelFactory.Map(pageModel.Links?.FirstOrDefault());
		}

		return page;
	}
}
