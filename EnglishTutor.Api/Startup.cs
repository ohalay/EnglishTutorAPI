using System.IO;
using AutoMapper;
using EnglishTutor.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Services;
using EnglishTutor.Common.AppSettings;
using log4net;
using log4net.Config;

namespace EnglishTutor.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            XmlConfigurator.Configure(LogManager.CreateRepository("log4netRepository"), new FileInfo(Path.Combine(env.ContentRootPath, "log4net.xml")));

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(opt => 
            {
                opt.Filters.Add(typeof(TokenAuthorizationFilter));

            }).AddJsonOptions(option => 
            {
                option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddOptions();
            services.Configure<OxforDictionary>(Configuration.GetSection("OxforDictionary"));
            services.Configure<Firebase>(Configuration.GetSection("Firebase"));
            services.Configure<Translate>(Configuration.GetSection("Translate"));
            services.Configure<Account>(Configuration.GetSection("Account"));

            services.AddTransient<IFirebaseService, FirebaseService>();
            services.AddTransient<IOxforDictionaryService, OxforDictionaryService>();
            services.AddTransient<ITranslateService, TranslateService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddSingleton(LogManager.GetLogger("log4netRepository", "logger"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<ErrorHandlingMiddleware>(app.ApplicationServices.GetService<ILog>());
            app.UseMvc();
        }
    }
}
