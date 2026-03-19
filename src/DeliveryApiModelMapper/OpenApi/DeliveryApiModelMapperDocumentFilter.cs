using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Community.DeliveryApiModelMapper.Interfaces;

namespace Umbraco.Community.DeliveryApiModelMapper.OpenApi;

internal class DeliveryApiModelMapperDocumentFilter : IDocumentFilter
{
	private const string DeliveryApiDocumentName = "delivery";

	private readonly IServiceScopeFactory _serviceScopeFactory;

	public DeliveryApiModelMapperDocumentFilter(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		if (!string.Equals(context.DocumentName, DeliveryApiDocumentName, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		using var scope = _serviceScopeFactory.CreateScope();
		var mappers = scope.ServiceProvider.GetServices<IDeliveryApiModelMapper>();

		foreach (var mapper in mappers)
		{
			var schemaModelType = mapper.SchemaModelType();

			if (schemaModelType is not null)
			{
				context.SchemaGenerator.GenerateSchema(schemaModelType, context.SchemaRepository);
			}
		}
	}
}
