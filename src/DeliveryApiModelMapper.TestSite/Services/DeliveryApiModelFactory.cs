using DeliveryApiModelMapper.TestSite.Models.DeliveryApi;
using DeliveryApiModelMapper.TestSite.Models.ModelsBuilder;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace DeliveryApiModelMapper.TestSite.Services;

public class DeliveryApiModelFactory : IDeliveryApiModelFactory
{
	public TModel Create<TModel>(IPublishedContent content) where TModel : BaseModel, new()
	{
		var model = new TModel();

		var home = content.AncestorOrSelf<Home>();

		if (home != null)
		{
			model.MetaTitle = home.MetaTitle ?? "";
			model.MetaDescription = home.MetaDescription ?? "";
			model.MetaImageUrl = home.MetaImage?.Url() ?? "";

			if (home?.FooterLinks != null)
			{
				model.FooterLinks = home.FooterLinks.Select(Map).ToList();
			}
		}

		return model;
	}

	public ContentLink Map(global::Umbraco.Cms.Core.Models.Link? link)
	{
		if (link == null)
		{
			return new ContentLink
			{
				Target = "",
				Title = "",
				Url = ""
			};
		}

		return new ContentLink
		{
			Target = link.Target ?? "",
			Title = link.Name ?? "",
			Url = link.Url ?? ""
		};
	}
}
