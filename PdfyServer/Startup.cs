using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PdfServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            PdfConverterSetup.Setup(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters();
            services.AddCors(options =>
            {
                // todo: Add limitations.
                options.AddPolicy("wildcard", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        { 
            app.UseCors("wildcard");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
