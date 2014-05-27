using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace ModelsOnline
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError += (ctx, ex) =>
            {
                return null;
            };

            base.RequestStartup(requestContainer, pipelines, context);
        }
    }
}