using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiProdutos", Version = "v1" });
            });

            services.AddHangfire(op =>
            {
                //Criar uma base zerada no sql server e depois exeutar aplicação
                //ela ira criar as tabelas sozinho
                //op.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                //op.UseSimpleAssemblyNameTypeSerializer();
                //op.UseRecommendedSerializerSettings();
                //op.UseSqlServerStorage(@"Data Source=.\SQL2014;Initial Catalog=HangFire;Persist Security Info=True;User ID=sa;PWD=;", new SqlServerStorageOptions
                //{
                //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                //    QueuePollInterval = TimeSpan.Zero,
                //    UseRecommendedIsolationLevel = true,
                //    DisableGlobalLocks = true
                //});
                op.UseMemoryStorage();
            });

            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireDashboard();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProdutos V1"));
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //BackgroundJob.Enqueue(() => MeuPrimeiroJobfireandforget());

            //RecurringJob.AddOrUpdate(() => Console.WriteLine("RecurringJob - AddOrUpdate"), Cron.Hourly);

            //BackgroundJob.Schedule(() => Console.WriteLine("BackgroundJob - Schedule"), TimeSpan.FromDays(2));

            //string jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Job PAI"));
            //BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Job FILHA"));
        }

        //public async Task MeuPrimeiroJobfireandforget()
        //{
        //    await Task.Run(() =>
        //    {
        //        throw new Exception("Teste exception");
        //        //Console.WriteLine("Bem vindo ao hangfire");
        //    });
        //}
    }
}
