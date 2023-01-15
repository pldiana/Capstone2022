using CryptoDevilAPI.DataAccess;
using CryptoDevilAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoDevilAPI", Version = "v1" });
            });

            IMongoClient client = new MongoClient("mongodb+srv://xxxx:xxxx@cluster0.qa0sl.mongodb.net/CryptoDevil?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("CryptoDevil");
            var userExchangeCollection = database.GetCollection<UserExchange>("UserExchange");
            var exchangeCollection = database.GetCollection<Exchange>("Exchange");
            var candleCollection = database.GetCollection<Candle>("Candles");
            ExchangeDA exchangeDA = new ExchangeDA(exchangeCollection);
            UserExchangeDA userExchangeDA = new UserExchangeDA(userExchangeCollection);
            CandleDataDA candleDataDA = new CandleDataDA(candleCollection);
            services.AddSingleton<IExchangeDA>(exchangeDA);
            services.AddSingleton<IUserExchangeDA>(userExchangeDA);
            services.AddSingleton<ICandleDataDA>(candleDataDA);
            services.AddScoped<IUserExchangeRepository, UserExchangeRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AddExchange", policy =>
                //                  policy.RequireClaim("permissions", "add:exchange"));
                //options.AddPolicy("EditExchange", policy =>
                //                  policy.RequireClaim("permissions", "edit:exchange"));
                ////options.AddPolicy("AddStrategy", policy =>
                ////                  policy.RequireClaim("permissions", "add:strategy"));
                ////options.AddPolicy("EditStrategy", policy =>
                ////                  policy.RequireClaim("permissions", "edit:strategy"));
                //options.AddPolicy("DeleteExchange", policy =>
                //                  policy.RequireClaim("permissions", "delete:exchange"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoDevilAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
