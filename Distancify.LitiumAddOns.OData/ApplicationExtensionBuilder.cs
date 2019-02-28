using Litium.Application.Bootstrapper.Runtime;
using Litium.Application.Runtime;
using Litium.Runtime;
using Microsoft.AspNet.OData.Batch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Distancify.LitiumAddOns.OData
{
    internal class ApplicationExtensionBuilder : IApplicationExtensionBuilder, IApplicationOptionsBuilderExtension
    {
        /// <summary>
        /// This method adds Microsoft.AspNet.OData assemblies to the list of assemblies that Litium will look into for services.
        /// </summary>
        public void Build(IServiceCollection services, IConfiguration config)
        {
            var context = services.Single(x => x.ServiceType == typeof(ApplicationPluginContext));
            services.Remove(context);

            var current = (ApplicationPluginContext)context.ImplementationInstance;
            var replace = new ApplicationPluginContext();
            var assemblies = current.Assemblies.Concat(new[]
            {
                typeof(DefaultODataBatchHandler).Assembly
            }).ToList().AsReadOnly();

            var types = current.Types.Concat(typeof(DefaultODataBatchHandler).Assembly.GetTypes()).ToList().AsReadOnly();
            var instancableTypes = types.GetTypes(x => !x.GetTypeInfo().IsAbstract).ToList().AsReadOnly();

            typeof(ApplicationPluginContext).GetProperty(nameof(ApplicationPluginContext.Assemblies)).SetValue(replace, assemblies);
            typeof(ApplicationPluginContext).GetProperty(nameof(ApplicationPluginContext.InstancableTypes)).SetValue(replace, instancableTypes);
            typeof(ApplicationPluginContext).GetProperty(nameof(ApplicationPluginContext.Types)).SetValue(replace, types);

            services.AddSingleton(replace);
        }

        public void Configure(ApplicationOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ExtensionsBuilder.Add(this);
        }
    }
}
