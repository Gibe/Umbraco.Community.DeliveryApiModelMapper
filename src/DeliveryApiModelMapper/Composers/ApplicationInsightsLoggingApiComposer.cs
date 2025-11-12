using Umbraco.Community.DeliveryApiModelMapper.Repository;
using Umbraco.Community.DeliveryApiModelMapper.Settings;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiModelMapper.Composers
{
    public class ApplicationInsightsLoggingApiComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<ILogViewerRepository, ApplicationInsightsLogViewerRepository>();
            builder.Services
                .AddOptions<ApplicationInsightsSettings>()
                .BindConfiguration("ApplicationInsightsLogging");
        }
    }
}
