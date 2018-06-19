using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MicroAppApi.Settings; // This is my settings namespace

namespace MicroAppApi
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
            services.AddCors();

            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
            //Convert the JSON to a PRogramSettings object.

            //this uses the ioptions DI
            //     System.Diagnostics.Debug.WriteLine(Configuration.GetSection("ProgramSettings")?.Value);
            // services.Configure<ProgramSettings>(Configuration.GetSection("ProgramSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader().AllowAnyMethod()
            );
//https://stackoverflow.com/questions/323397/file-path-as-mvc-route-argument
            //app.UseStaticFiles();
            app.UseMvc(routes =>
                {

//    routes.MapRoute("/api/CordovaApp", "{controller=CordovaApp}/{action=Index}/{*id}");
                    routes.MapRoute(
                       name: "default",
                          template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
