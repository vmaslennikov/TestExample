using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebViewer.Services;

namespace WebViewer.AppConfiguration {
    public static class DependencyInjectionExtensions {
        public static void AddDependencies (this IServiceCollection services, IConfiguration configuration) {
            services.AddTransient<IImageService, ImageService> ();
        }
    }
}