﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Web
{
    public class SelfHost : IDisposable
    {
        private bool disposedValue;

        private WebApplication _webApp = null;

        public async Task Start(int port)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                ApplicationName = "DMXForGamers.Web"
            });

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenLocalhost(port);
            });

            builder.Services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
#if DEBUG
                configure.SetMinimumLevel(LogLevel.Trace);
#endif
            });

            builder.Services.AddRazorPages();

            builder.Services.AddControllers();

            _webApp = builder.Build();

            _webApp.UseExceptionHandler("/Error");

            _webApp.UseStaticFiles();

            _webApp.UseRouting();

            _webApp.UseAuthorization();

            _webApp.MapRazorPages();

            _webApp.MapControllers();

            await _webApp.StartAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_webApp != null)
                    {
                        _webApp.StopAsync().Wait();
                        _webApp = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
