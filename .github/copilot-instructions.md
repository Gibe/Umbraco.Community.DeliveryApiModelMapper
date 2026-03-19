# Instructions for Copilot
## Overview
This solution is for a nuget package aimed at Umbraco (see projects for target version).  Its aim is to create a package that allows a developer to create strongly typed models for the Umbraco Content Delivery API.

## Solution structure
This solution contains two projects:
### DeliveryApiModelMapper
This is the package code - it is what gets shipped when published on nuget.  The full name of the package is Umbraco.Community.DeliveryApiModelMapper

### DeliveryApiModelMapper.TestSite
This is a local test Umbraco site that references the DeliveryApiModelMapper and is used purely for testing the DeliveryApiModelMapper by the developer, allowing them to spin the site up and interact with it directly.

## Folder structure
* /docs - this contains markdown files with documentation for users on how to use the package.
* /bruno - contains some sample API calls for use with Bruno in order for developers to test the Delivery API easily using the TestSite.

## Technology Stack
- **.NET Version**: .NET 10.0
- **Core Dependencies**: 
  - Umbraco.Cms.Core v17.0.0+
  - Umbraco.Cms.Api.Delivery v17.0.0+
  - Umbraco.Cms.Api.Common v17.0.0+
- **Language**: C# with nullable reference types enabled

## Key Abstractions
The package uses a **mapper pattern** to enable extensibility:

- **`IDeliveryApiModelMapper`** — Developers implement this interface to create custom mappers for specific content types. Each mapper declares what it can handle via `CanMapModel()` and produces a strongly-typed model via `MapModel()`.
- **`IDeliveryApiModelMapperService`** — The core service that:
  - Discovers registered mappers and finds the appropriate one for a given content item
  - Orchestrates model creation and API response building
  - Applies the configured `ModelMode` to control the response format
- **`DeliveryApiModelMapperApiContentResponseBuilder`** — Extends Umbraco's `ApiContentResponseBuilder` to intercept Delivery API responses and inject mapped models.

Mappers are discovered and registered via dependency injection, allowing developers to add multiple mappers by registering implementations of `IDeliveryApiModelMapper`.

## Model Modes
The package supports four configurable response modes via the `ModelMode` enum:

- **`Everything`** (default) — Outputs the original Delivery API response plus the custom `model` property. Most flexible but returns larger JSON payloads.
- **`ExcludeProperties`** — Removes the original `properties` and replaces it with the custom `model` property, reducing payload size while retaining structured data.
- **`ModelOnly`** — Returns only the custom `model` property, providing minimal JSON without any raw Delivery API properties.
- **`ExcludeModel`** — Returns the original Delivery API response without any custom model (mappers are not invoked).

For detailed examples and use cases, see [model-modes.md](../docs/model-modes.md).

## Documentation
For developer guidance on creating models and mappers, see:
- [model-modes.md](../docs/model-modes.md) — Explains the four response modes with examples
- [README_nuget.md](../docs/README_nuget.md) — Package overview and getting-started guide

## External Documentation
Developers working with this package should be familiar with the Umbraco Content Delivery API and its extension points:

- **[Content Delivery API](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api)** — Overview of the Delivery API, endpoints, query parameters, and core concepts
- **[Custom Property Editors Support](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api/custom-property-editors-support)** — How to customize property editor output in the Delivery API by implementing `IDeliveryApiPropertyValueConverter`
- **[Extension API for Querying](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api/extension-api-for-querying)** — How to extend the API with custom selecting, filtering, and sorting via `ISelectorHandler`, `IFilterHandler`, and `ISortHandler`

## Swagger / Open API
When the test site is running, the Content Delivery API has swagger documentation available at the following paths:
* Human readable Swagger docs: /umbraco/swagger/index.html?urls.primaryName=Umbraco+Delivery+API
* Machine readable API Open API spec: /umbraco/swagger/delivery/swagger.json