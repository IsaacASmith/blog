using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace web_api
{
    public class Startup
    {
        public const string REQUIRE_DATA_READER_POLICY = "RequireDataReader";
        public const string REQUIRE_DATA_WRITER_POLICY = "RequireDataWriter";

        // These are the names we gave our roles in the Azure AD app registration manifest
        public const string DATA_READER_ROLE = "DEMO.DataReader";
        public const string DATA_WRITER_ROLE = "DEMO.DataWriter";

        public const string TENANT_ID = "my_tenant_id"; // Get this from the API app registration overview blade
        public const string CLIENT_ID = "my_client_id"; // Get this from the API app registration overview blade

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Authority = $"https://login.microsoftonline.com/{TENANT_ID}";
                    options.Audience = CLIENT_ID;
                });

            services.AddAuthorization(options => {
               options.AddPolicy(REQUIRE_DATA_READER_POLICY, policy => policy.RequireRole(DATA_READER_ROLE));
               options.AddPolicy(REQUIRE_DATA_WRITER_POLICY, policy => policy.RequireRole(DATA_WRITER_ROLE));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
