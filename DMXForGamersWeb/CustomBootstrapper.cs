using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
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
        private ThreadedProcessingQueue<NewClientArgs> m_newClientQueue;

        private Func<NewClientArgs, bool> m_newClientCallback { get; set; }

        private Dictionary<string, NancyClient> m_Clients = new Dictionary<string, NancyClient>();

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

        private CustomBootstrapper()
        {
            // Do NOT use this constructor
        }

        public CustomBootstrapper(Func<NewClientArgs, bool> newClientCallback)
        {
            m_newClientCallback = newClientCallback;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            if (ResourceViewLocationProvider.RootNamespaces.ContainsKey(Assembly.GetAssembly(typeof(MainModule))) == false)
            {
                ResourceViewLocationProvider.RootNamespaces.Add(Assembly.GetAssembly(typeof(MainModule)), "DMXForGamers.Web.Views");
            }
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            CustomStatusCode.AddCode(403);

            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);

            if (m_newClientCallback != null)
            {
                m_newClientQueue = new ThreadedProcessingQueue<NewClientArgs>(x =>
                {
                    if (m_newClientCallback(x) == true)
                    {
                        m_Clients[x.ClientAddress].IsAllowed = true;
                    }
                    else
                    {
                        m_Clients[x.ClientAddress].IsAllowed = false;
                    }
                });
                m_newClientQueue.Start();
            }

            pipelines.BeforeRequest += ProcessBeforeRequest;
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


        public override void Configure(INancyEnvironment environment)
        {
#if DEBUG
            environment.Diagnostics(true, "1234");
#endif
            base.Configure(environment);
        }

        // TO REMOVE
        //protected override NancyInternalConfiguration InternalConfiguration
        //{
        //    get
        //    {
        //        return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
        //    }
        //}

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }

        // PEV - 4/6/2018 - This is needed because Dispose in the base class is NOT virutal in version 1.4.4
        public void Cleanup()
        {
            if (m_newClientQueue != null)
            {
                m_newClientQueue.Dispose();
                m_newClientQueue = null;
            }
        }

        #region Before/After Request Processing

        Response ProcessBeforeRequest(NancyContext ctx)
        {
            var hostAddress = ctx.Request.UserHostAddress;

            if (hostAddress != "::1")
            {
                NancyClient client;
                if (m_Clients.TryGetValue(hostAddress, out client) == true)
                {
                    if (client.IsAllowed.HasValue == false)
                    {
                        return new Response()
                        {
                            StatusCode = HttpStatusCode.Forbidden
                        };
                    }
                    else
                    {
                        if (client.IsAllowed.Value == true)
                        {
                            return null;
                        }
                        else
                        {
                            return new Response()
                            {
                                StatusCode = HttpStatusCode.NotFound
                            };
                        }
                    }
                }
                else
                {
                    if (m_Clients != null)
                    {
                        m_Clients.Add(hostAddress, new NancyClient() { Address = hostAddress });

                        m_newClientQueue.AddToQueue(new NewClientArgs(hostAddress));
                    }

                    return new Response()
                    {
                        StatusCode = HttpStatusCode.Forbidden
                    };
                }
            }

            return null;
        }

        #endregion
    }
}
