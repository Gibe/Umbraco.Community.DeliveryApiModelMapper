namespace DeliveryApiModelMapper.TestSite.Models.DeliveryApi;

public class PageModel : BaseModel
{
	public string Title { get; set; }
	public string Text { get; set; }

	public ContentLink Link { get; set; }
}
