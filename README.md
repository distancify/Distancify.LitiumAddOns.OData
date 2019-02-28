# Distancify.LitiumAddOns.OData

Litium Add-on help with building simple read-only OData endpoints for Litium data.

Requires Litium 7.

## Getting Started

### Prerequisites

This library aims at extending the e-comemrce platform [Litium](https://www.litium.se/). In order to use and develop the project, you need to fulfill their [development system requirements](https://docs.litium.com/documentation/get-started/system-requirements#DevEnv).

### Install

```
Install-Package Distancify.LitiumAddOns.OData
```

### Configure

First, you need to create a model. In this example, we create a simple model for products:

```csharp
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Litium.FieldFramework;
using Litium.Runtime.AutoMapper;

namespace Distancify.LitiumAddOns.OData.Sample
{
    public class ODataProduct : IAutoMapperConfiguration
    {
        [Key]
        public string ArticleNumber { get; set; }
        public string Name { get; set; }

        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ODataProductModel, ODataProduct>()
               .ForMember(x => x.ArticleNumber, m => m.MapFrom(r => r.Variant.Id))
               .ForMember(x => x.Name, m => m.MapFromField(SystemFieldDefinitionConstants.Name));
        }
    }
}
```

Next, you need to create a builder for your model:

```csharp
using Litium.Runtime.AutoMapper;
using Litium.Runtime.DependencyInjection;

namespace Distancify.LitiumAddOns.OData.Sample
{
    [Service]
    public class ODataProductBuilder : ProductModelBuilder<ODataProduct>
    {
        public override object Build(ODataProductModel product)
        {
            return product.MapTo<ODataProduct>();
        }
    }
}
```

Now you're ready to hook everything up. This is done using a IWebApiSetup service. Here's an example:

```csharp
using Litium.Web.WebApi;
using System.Web.Http;

namespace Distancify.LitiumAddOns.OData.Sample
{
    public class ODataConfig : IWebApiSetup
    {
        private readonly ODataProductBuilder _productBuilder;

        public ODataConfig(ODataProductBuilder productBuilder)
        {
            _productBuilder = productBuilder;
        }

        public void SetupWebApi(HttpConfiguration config)
        {
            config.UseLitiumOData()
                .WithProductModelBuilder(_productBuilder)
                .Create();
        }
    }
}
```

## Publishing

The project is built on AppVeyor and set up to automatically push any release branch to NuGet.org. To create a new release, create a new branch using the following convention: `release/v<Major>.<Minor>`. AppVeyor will automatically append the build number.

## Running the tests

The tests are built using xUnit and does not require any setup in order to run inside Visual Studio's standard test runner.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## Authors

See the list of [contributors](https://github.com/distancify/Distancify.LitiumAddOns.OData/graphs/contributors) who participated in this project.

## License

This project is licensed under the LGPL v3 License - see the [LICENSE](LICENSE) file for details