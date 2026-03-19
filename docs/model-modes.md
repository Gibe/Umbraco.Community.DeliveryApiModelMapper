# Model Modes
There are 4 "ModelMode" values that can be configured:

- `Everything` (default) - appends the custom `"model"` property to the original Delivery API response.
- `ExcludeProperties` - removes the original `"properties"` property and swaps it for the custom `"model"` property.
- `ModelOnly` - only outputs the custom `"model"` property in the response.
- `ExcludeModel` - returns the original Delivery API response.


## ModelMode: `Everything`
This is the default model mode.  This will output the original Delivery API response (including the original `"properties"` property), but also include the custom `"model"` property.

This is the most flexible mode as it allows access to the original properties, as well as the custom model.  The disadvantage of this is that the returned JSON could be quite large.

e.g.
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
		"url": "/",
	},
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

## ModelMode: `ExcludeProperties`
The `"properties"` in the JSON output will get swapped out for the custom `"model"` property.

This allows you to see the original Umbraco metadata along with the custom model whilst reducing the response size by excluding the original `"properties"`.

e.g.
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

## ModelMode: `ModelOnly`
Only the custom `"model"` property will be output.  

This may be useful in scenarios where you only want your custom values and would like to keep the returned JSON as lightweight as possible.

e.g.
```json
{
	"model": {
		"title": "Welcome to the site",
		"text": "<p>This is some lovely text</p>",
		"metaTitle": "Home page | My lovely site",
		"metaDescription": "This is a lovely site, please rank it higher",
		"metaImageUrl": "/media/skwjkrjb/enceladus_pia08409_full.jpg?width=1200",
		"lastUpdated": "2025-10-05T09:55:35.0150656",
		"url": "/",
	}
}
```

## ModelMode: `ExcludeModel`
This will exclude the custom `"model"` property and output the original Delivery API JSON with the  `"properties"` in the JSON output as usual.

e.g.
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
		]
	},
	"cultures": {}
}
```