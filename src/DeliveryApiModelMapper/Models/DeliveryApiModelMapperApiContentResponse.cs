using System.Text.Json.Serialization;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Community.DeliveryApiModelMapper.Services;

public class DeliveryApiModelMapperApiContentResponse : ApiContentResponse, IApiContentResponse
{
	[JsonPropertyOrder(-100)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public string ContentType => base.ContentType;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public string Name => base.Name;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public DateTime CreateDate => base.CreateDate;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public DateTime UpdateDate => base.UpdateDate;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public IApiContentRoute Route => base.Route;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public Guid Id => base.Id;

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public object Model { get; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public new IDictionary<string, object?> Properties => base.Properties;

	[JsonPropertyOrder(100)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public IDictionary<string, IApiContentRoute> Cultures => base.Cultures;

	public DeliveryApiModelMapperApiContentResponse(
		object model)
		: base(default, default, default, default, default, default, default, default)
	{
		Model = model;
	}

	public DeliveryApiModelMapperApiContentResponse(
		IPublishedContent content,
		string name,
		IApiContentRoute route,
		IDictionary<string, object?> properties,
		IDictionary<string, IApiContentRoute> cultures)
		: base(content.Key, name, content.ContentType.Alias, content.CreateDate, content.UpdateDate, route, properties, cultures)
	{
		Model = default;
	}

	public DeliveryApiModelMapperApiContentResponse(
		object model,
		IPublishedContent content,
		string name,
		IApiContentRoute route,
		IDictionary<string, object?> properties,
		IDictionary<string, IApiContentRoute> cultures)
		: base(content.Key, name, content.ContentType.Alias, content.CreateDate, content.UpdateDate, route, properties, cultures)
	{
		Model = model;
	}
}
