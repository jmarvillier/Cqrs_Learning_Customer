using CustomerApi.Commands;
using CustomerApi.Events;
using CustomerApi.Models.CosmosDb;
using CustomerApi.Models.Repositories;
using CustomerApi.Models.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace CustomerApi
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
            services.AddDbContext<CustomerSQLiteDatabaseContext>(_ => _.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<AMQPEventPublisher>();
            services.AddScoped<ICommandHandler<Command>, CustomerCommandHandler>();
            services.AddTransient<ICustomerRepository, CustomerSQLiteRepository>();
            services.AddTransient<IDBCustomerRepository<CustomerObject>, CustomerCosmosDbRepository>();
            services.AddSingleton<CustomerMessageListener>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CustomerSQLiteDatabaseContext>();
                context.Database.EnsureCreated();
            }


            new Thread(() =>
            {
                app.ApplicationServices.GetService<CustomerMessageListener>().Start(env.ContentRootPath);
            }).Start();

            app.UseMvc();
        }
    }
}
