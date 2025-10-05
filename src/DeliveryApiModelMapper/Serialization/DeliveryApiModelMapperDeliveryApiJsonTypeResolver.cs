using System.Text.Json.Serialization.Metadata;
using Umbraco.Cms.Api.Delivery.Json;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Community.DeliveryApiModelMapper.Services;

namespace Umbraco.Community.DeliveryApiModelMapper.Serialization;

public class DeliveryApiModelMapperDeliveryApiJsonTypeResolver : DeliveryApiJsonTypeResolver
{
	protected override Type[] GetDerivedTypes(JsonTypeInfo jsonTypeInfo)
	{
		if (jsonTypeInfo.Type == typeof(IApiContentResponse))
		{
			return base.GetDerivedTypes(jsonTypeInfo)
				.Concat([typeof(DeliveryApiModelMapperApiContentResponse)])
				.ToArray();
		}

		return base.GetDerivedTypes(jsonTypeInfo);
	}
}
