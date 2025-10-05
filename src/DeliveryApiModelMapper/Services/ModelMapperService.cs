using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.DeliveryApiModelMapper.Enums;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;
using Umbraco.Community.DeliveryApiModelMapper.Models;

namespace Umbraco.Community.DeliveryApiModelMapper.Services;

public class ModelMapperService : IDeliveryApiModelMapperService
{
	private readonly IEnumerable<IDeliveryApiModelMapper> _mappers;
	private readonly ModelMapperSettings _settings;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ModelMapperService(IEnumerable<IDeliveryApiModelMapper> mappers,
		IOptions<ModelMapperSettings> settings,
		IHttpContextAccessor httpContextAccessor)
	{
		_mappers = mappers.ToArray();
		_settings = settings.Value;
		_httpContextAccessor = httpContextAccessor;
	}

	public IDeliveryApiModelMapper? Find(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
	{
		return _mappers.FirstOrDefault(m => m.CanMapModel(content, name, route, cultures));
	}

	public IApiContentResponse CreateApiResponse(object model, IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, object?> properties, IDictionary<string, IApiContentRoute> cultures)
	{
		switch (GetModelMode())
		{
			case ModelMode.ExcludeModel:
				return new ModelMapperApiContentResponse(content, name, route, properties, cultures);

			case ModelMode.ExcludeProperties:
				return new ModelMapperApiContentResponse(model, content, name, route, default, cultures);

			case ModelMode.ModelOnly:
				return new ModelMapperApiContentResponse(model);

			default:
			case ModelMode.Everything:
				return new ModelMapperApiContentResponse(model, content, name, route, properties, cultures);
		}
	}

	private ModelMode GetModelMode()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext != null)
		{
			var modelOnlyQueryString = httpContext.Request.Query["modelmode"].ToString();
			if (!string.IsNullOrWhiteSpace(modelOnlyQueryString))
			{
				if (Enum.TryParse<ModelMode>(modelOnlyQueryString, ignoreCase: true, out var modelModeOverride))
				{
					return modelModeOverride;
				}
			}
		}

		return _settings.ModelMode;
	}
}
