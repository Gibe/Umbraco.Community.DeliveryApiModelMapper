# Delivery Api Model Mapper 

[![Downloads](https://img.shields.io/nuget/dt/Umbraco.Community.DeliveryApiModelMapper?color=cc9900)](https://www.nuget.org/packages/Umbraco.Community.DeliveryApiModelMapper/)
[![NuGet](https://img.shields.io/nuget/vpre/Umbraco.Community.DeliveryApiModelMapper?color=0273B3)](https://www.nuget.org/packages/Umbraco.Community.DeliveryApiModelMapper)
[![GitHub license](https://img.shields.io/github/license/tristanjthompson/Umbraco.Community.DeliveryApiModelMapper?color=8AB803)](../LICENSE)

This package allows you to create custom models which will be output in the Delivery API response as a custom `"model"` property. The mapping is applied whenever a content item is returned by the Delivery API — whether you are fetching by route, by id, or via a content query.

e.g.

Instead of this original Umbraco Delivery API response:
```json
{
	"contentType": "home",
	"name": "Home",
	"createDate": "2025-09-30T17:05:04.6009112",
	"updateDate": "2025-10-05T09:55:35.0150656",
	"route": {
		"path": "/",
		"startItem": {
			"id": "a086db43-c507-4be6-aadf-dc734abdea45",
			"path": "home"
		}
	},
	"id": "a086db43-c507-4be6-aadf-dc734abdea45",
	"properties": {
		"contentTitle": "Welcome to the site",
		"contentText": {
			"markup": "<p>This is some lovely text</p>",
			"blocks": []
		},
		"seoMetaTitle": "Home page | My lovely site",
		"seoMetaDescription": "This is a lovely site, please rank it higher",
		"seoMetaImage": [
			{
				"focalPoint": null,
				"crops": [],
				"id": "4585ad94-0c21-4ae7-a621-504679a449a1",
				"name": "Enceladus PIA08409 Full",
				"mediaType": "Image",
				"url": "/media/skwjkrjb/enceladus_pia08409_full.jpg",
				"extension": "jpg",
				"width": 3380,
				"height": 2211,
				"bytes": 665148,
				"properties": {}
			}
		],
	},
	"cultures": {}
}
```

...you could return this...

```json
{
	"contentType": "home",
	"name": "Home",
	"createDate": "2025-09-30T17:05:04.6009112",
	"updateDate": "2025-10-05T09:55:35.0150656",
	"route": {
		"path": "/",
		"startItem": {
			"id": "a086db43-c507-4be6-aadf-dc734abdea45",
			"path": "home"
		}
	},
	"id": "a086db43-c507-4be6-aadf-dc734abdea45",
	"model": {
		"title": "Welcome to the site",
		"text": "<p>This is some lovely text</p>",
		"metaTitle": "Home page | My lovely site",
		"metaDescription": "This is a lovely site, please rank it higher",
		"metaImageUrl": "/media/skwjkrjb/enceladus_pia08409_full.jpg?width=1200",
		"lastUpdated": "2025-10-05T09:55:35.0150656",
		"url": "/"
	},
	"cultures": {}
}
```

... or even this...

```json
{
	"model": {
		"title": "Welcome to the site",
		"text": "<p>This is some lovely text</p>",
		"metaTitle": "Home page | My lovely site",
		"metaDescription": "This is a lovely site, please rank it higher",
		"metaImageUrl": "/media/skwjkrjb/enceladus_pia08409_full.jpg?width=1200",
		"lastUpdated": "2025-10-05T09:55:35.0150656",
		"url": "/"
	}
}
```

## Installation

Add the package to an existing Umbraco website (v13+) from nuget:

`dotnet add package Umbraco.Community.DeliveryApiModelMapper`

## Mapping
### Defining mappings
You define a mapping by implementing the `IDeliveryApiModelMapper` interface. This can be found in the `Umbraco.Community.DeliveryApiModelMapper.Interfaces` namespace.  Multiple mappings can be defined.

The `IDeliveryApiModelMapper` interface has two mandatory methods to implement:
1. `bool CanMapModel(IPublishedContent content, ...)` - this is used to say whether this mapper should be used for a particular piece of content.
2. `object MapModel(IPublishedContent content, ...)` - this is where you create your custom mapping.

e.g. to create a custom mapping for home page content to a `HomeModel` you would implement the following:
```csharp
public class HomeModelMapper : IDeliveryApiModelMapper
{
	public bool CanMapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
		=> content.ContentType.Alias == "homePage";

	public object MapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
	{
		return new HomeModel
		{
			Title = content.Value<string>("contentTitle"),
			Text = content.Value<string>("contentText")
		};
	}
}
```

### Adding your model to the OpenAPI Swagger spec (optional)

If you would like your custom model to appear in the Delivery API's OpenAPI / Swagger schema — for example, to enable TypeScript type generation — you can implement the following method on your mapper and return the `Type` that you would like included in the OpenAPI Swagger spec:
* `Type? SchemaModelType()`

```csharp
public class HomeModelMapper : IDeliveryApiModelMapper
{
	public Type? SchemaModelType() => typeof(HomeModel);

	public bool CanMapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
		=> content.ContentType.Alias == "homePage";

	public object MapModel(IPublishedContent content, string name, IApiContentRoute route, IDictionary<string, IApiContentRoute> cultures)
	{
		return new HomeModel
		{
			Title = content.Value<string>("contentTitle"),
			Text = content.Value<string>("contentText")
		};
	}
}
```

When the schema type is provided, the package registers a Swashbuckle document filter that adds the model (and any of its nested types) to `#/components/schemas` in the Delivery API spec at `/umbraco/swagger/delivery/swagger.json`. Mappers that do not override `SchemaModelType()` are unaffected.

### Registering mappings
Once you've created your mappings, you need to register them in an `IComposer`.

e.g.
```csharp
builder.Services.AddScoped<IDeliveryApiModelMapper, HomeModelMapper>();
```

If you have more than one mapping, you simply register them all individually.  Note - the order they are registered is the order they are checked.

e.g.
```csharp
builder.Services.AddScoped<IDeliveryApiModelMapper, HomeModelMapper>();
builder.Services.AddScoped<IDeliveryApiModelMapper, AboutUsModelMapper>();
builder.Services.AddScoped<IDeliveryApiModelMapper, ContentPageModelMapper>();
```

### Configuring the output

If no `IDeliveryApiModelMapper` is found for a piece of content being returned by the Delivery API then the original Umbraco Delivery API response is returned without any changes.

However, if a mapper is found, then the response is modified according to the configured "ModelMode".  

#### Model Modes
There are 4 "ModelMode" values that can be configured:

- `Everything` (default) - appends the custom `"model"` property to the original Delivery API response.
- `ExcludeProperties` - removes the original `"properties"` property and swaps it for the custom `"model"` property.
- `ModelOnly` - only outputs the custom `"model"` property in the response.
- `ExcludeModel` - returns the original Delivery API response.

For more details on model modes, see the [Model Modes documentation](./model-modes.md).

#### Choosing a ModelMode

##### Default ModelMode
A default "ModelMode" is set in the `appsettings.json`.  This will apply to all Delivery API requests that have a matching `IDeliveryApiModelMapper` interface implemented.

```json
{
	"Umbraco": {
		"CMS": {
			"DeliveryApiModelMapper": {
				"ModelMode": "Everything"
			}
		}
	}
}
```

#### Overriding the default ModelMode

The default ModelMode can be overridden by adding a `?modelmode=[ModelMode]` query string parameter to your Delivery API call.

e.g. if you have configured the default ModelMode in your `appsettings.json` to `Everything` then...
* ...a call to `/umbraco/delivery/api/v2/content/item` will return `Everything`
* ...a call to `/umbraco/delivery/api/v2/content/item?modelmode=ModelOnly` will return `ModelOnly`

## Contributing

Contributions to this package are most welcome! Please read the [Contributing Guidelines](CONTRIBUTING.md).

## Acknowledgments

[tristanjthompson](https://github.com/tristanjthompson)