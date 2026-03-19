namespace DeliveryApiModelMapper.TestSite.Models.DeliveryApi;

public class BaseModel
{
	public string MetaTitle { get; set; } = "";
	public string MetaDescription { get; set; } = "";
	public string MetaImageUrl { get; set; } = "";

	public ICollection<FooterLink> FooterLinks { get; set; } = new List<FooterLink>();
}

public class FooterLink
{
	public string Title { get; set; } = "";
	public string Url { get; set; } = "";
	public string Target { get; set; } = "";
}
