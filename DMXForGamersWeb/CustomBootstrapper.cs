using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses;
using Nancy.Session;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Web
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        #region Favorite Icon

        private byte[] m_FavIcon;

        protected override byte[] FavIcon
        {
            get { return this.m_FavIcon ?? (this.m_FavIcon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            using (var resourceStream = GetType().Assembly.GetManifestResourceStream("DMXForGamers.Web.favicon.ico"))
            {
                var memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);
                return memoryStream.GetBuffer();
            }
        }

        #endregion

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            ResourceViewLocationProvider.RootNamespaces.Add(Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Views");
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            conventions.StaticContentsConventions.Add(StaticResourceConventionBuilder.AddDirectory("/Contents/Scripts", Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Contents.Scripts"));
            conventions.StaticContentsConventions.Add(StaticResourceConventionBuilder.AddDirectory("/Contents/Bootstrap/css", Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Contents.Bootstrap.css"));
            conventions.StaticContentsConventions.Add(StaticResourceConventionBuilder.AddDirectory("/Contents/Bootstrap/js", Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Contents.Bootstrap.js"));
            conventions.StaticContentsConventions.Add(StaticResourceConventionBuilder.AddDirectory("/Contents/Images", Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Contents.Images"));
            conventions.StaticContentsConventions.Add(StaticResourceConventionBuilder.AddDirectory("/Contents/CSS", Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Contents.CSS"));
            base.ConfigureConventions(conventions);
        }

#if DEBUG

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                return new DiagnosticsConfiguration { Password = @"1234" };
            }
        }

#endif

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
            }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }
    }
}
