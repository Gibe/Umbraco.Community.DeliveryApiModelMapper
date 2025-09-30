using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Community.DeliveryApiModelMapper
{
    internal class DeliveryApiModelMapperComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<DeliveryApiModelMapperManifestFilter>();
        }
    }
}
