namespace DeliveryApiModelMapper.TestSite.Models.DeliveryApi;

public class BaseModel
{
	public string MetaTitle { get; set; } = "";
	public string MetaDescription { get; set; } = "";
	public string MetaImageUrl { get; set; } = "";

	public ICollection<ContentLink> FooterLinks { get; set; } = new List<ContentLink>();
}

public class ContentLink
{
	public string Title { get; set; } = "";
	public string Url { get; set; } = "";
	public string Target { get; set; } = "";
}
