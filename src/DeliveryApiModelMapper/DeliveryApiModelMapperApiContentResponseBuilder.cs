using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;

namespace Umbraco.Community.DeliveryApiModelMapper;

public class DeliveryApiModelMapperApiContentResponseBuilder : ApiContentResponseBuilder
{
	private readonly IDeliveryApiModelMapperService _modelMapperService;

	public DeliveryApiModelMapperApiContentResponseBuilder(
			IApiContentNameProvider apiContentNameProvider,
			IApiContentRouteBuilder apiContentRouteBuilder,
			IOutputExpansionStrategyAccessor outputExpansionStrategyAccessor,
			IDeliveryApiModelMapperService modelMapperProvider,
			IVariationContextAccessor variationContextAccessor)
			: base(apiContentNameProvider, apiContentRouteBuilder, outputExpansionStrategyAccessor, variationContextAccessor)
	{
		_modelMapperService = modelMapperProvider;
	}

	protected override IApiContentResponse Create(
			IPublishedContent content,
			string name,
			IApiContentRoute route,
			IDictionary<string, object?> properties)
	{
		var cultures = GetCultures(content);
		var mapper = _modelMapperService.Find(content, name, route, cultures);
		if (mapper != null)
		{
			var model = mapper.MapModel(content, name, route, cultures);
			return _modelMapperService.CreateApiResponse(model, content, name, route, properties, cultures);
		}

		return base.Create(content, name, route, properties);
	}
}
