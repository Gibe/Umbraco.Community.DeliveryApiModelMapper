using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.DeliveryApiModelMapper
{
    internal class DeliveryApiModelMapperManifestFilter : IManifestFilter
    {
        public void Filter(List<PackageManifest> manifests)
        {
            var assembly = typeof(DeliveryApiModelMapperManifestFilter).Assembly;

            manifests.Add(new PackageManifest
            {
                PackageName = "Delivery API Model Mapper",
                Version = assembly.GetName()?.Version?.ToString(3) ?? "1.0.0",
                AllowPackageTelemetry = true,
                Scripts = new string[] {
                    // List any Script files
                    // Urls should start '/App_Plugins/DeliveryApiModelMapper/' not '/wwwroot/DeliveryApiModelMapper/', e.g.
                    // "/App_Plugins/DeliveryApiModelMapper/Scripts/scripts.js"
                },
                Stylesheets = new string[]
                {
                    // List any Stylesheet files
                    // Urls should start '/App_Plugins/DeliveryApiModelMapper/' not '/wwwroot/DeliveryApiModelMapper/', e.g.
                    // "/App_Plugins/DeliveryApiModelMapper/Styles/styles.css"
                }
            });
        }
    }
}
