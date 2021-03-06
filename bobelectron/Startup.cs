using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bobelectron
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "bobelectron", Version = "v1" });
            })
            .AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllers();
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "wwwroot")),
            });

            ElectronBootstrap();
        }

        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public async void ElectronBootstrap()
        {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Show = false,
                Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets\\icon.ico"),
                WebPreferences = new WebPreferences
                {
                    EnableRemoteModule = true,
                    NodeIntegration = true,
                }
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Bob Calypsos .Net Everywhere");
            browserWindow.RemoveMenu();

            ElectronNET.API.Electron.IpcMain.On("open_window", (args) =>
            {
                var jsonArgs = args as Newtonsoft.Json.Linq.JArray;
                var browserWindow = Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
                {
                    Icon = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets\\icon.ico"),
                    WebPreferences = new WebPreferences
                    {
                        EnableRemoteModule = true,
                        NodeIntegration = true,
                    }
                }, jsonArgs[0].ToString());

                _data.Add(jsonArgs[1].ToString(), jsonArgs[2]);
            });

            ElectronNET.API.Electron.IpcMain.On("opened_window", (args) =>
            {
                var id = args as string;
                if (_data.ContainsKey(id))
                {
                    foreach (var win in Electron.WindowManager.BrowserWindows)
                    {
                        win.RemoveMenu();
                        ElectronNET.API.Electron.IpcMain.Send(win, "send_data", _data[id]);
                    }
                    _data.Remove(id);
                }
            });

            Electron.App.BrowserWindowCreated += App_BrowserWindowCreated;
        }

        private void App_BrowserWindowCreated()
        {
        }

    }
}
