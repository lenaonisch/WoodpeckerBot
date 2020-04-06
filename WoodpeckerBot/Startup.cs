﻿using System;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Extentions;
using IBWT.Framework.Middleware;
using IBWT.Framework.Scheduler;
using IBWT.Framework.State.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WoodpeckerBot.BackgroundTasks;
using WoodpeckerBot.Configuration;
using WoodpeckerBot.Data.Entities;
using WoodpeckerBot.Data.Repository;
using WoodpeckerBot.Handlers;
using WoodpeckerBot.Services;

namespace WoodpeckerBot
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddConfigurationProvider(Configuration, env);

            services.AddScoped<IDataRepository<Order>, OrderRepository>();
            services.AddScoped<IDataRepository<TGUser>, TGUserReposiroty>();

            if (env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase")));
            }
            else
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RemoteDatabase")));
            }
            // Save history of telegram user movements throw the bots' menus
            services.AddBotStateCache<InMemoryStateProvider>();

            services.AddTelegramBot()
                .AddScoped<Texthandler>()
                .AddScoped<StartCommand>()
                .AddScoped<UpdateLogger>()
                .AddScoped<FindTreeHandler>()
              //  .AddScoped<CountWormsService>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<Menu1QueryHandler>()
                .AddScoped<Menu2QueryHandler>()
                .AddScoped<Menu3QueryHandler>()
                .AddScoped<DefaultHandler>()
                ;

            services.AddScoped<IFindTreeService, FindTreeService>();
            //services.AddScoped<ICountWormsService, CountWormsService>();

            services.AddSingleton<IScheduledTask, NotifyWeatherTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            // app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseMvc();
                app.UseTelegramBotLongPolling(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                // app.UseMvc();
                app.UseTelegramBotWebhook(ConfigureBot());
                app.EnsureWebhookSet();
            }

        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .Use<UpdateLogger>()
                // .Use<CustomUpdateLogger>()
                .UseWhen<UpdateMembersList>(When.MembersChanged)
                .MapWhen(When.State("default"), cmdBranch => cmdBranch
                    .UseWhen(When.NewMessage, msgBranch => msgBranch
                    .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                        .UseWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<StartCommand>("start")
                        )
                        .Use<DefaultHandler>()
                        //.Use<Texthandler>()
                    //.Use<NLP>()
                    )
                    
                    //.UseWhen<StickerHandler>(When.StickerMessage)
                    //.UseWhen<WeatherReporter>(When.LocationMessage)
                )
                
                )
                .MapWhen(When.State("menu1"), defaultBranch => defaultBranch
                    .UseWhen<Menu1QueryHandler>(When.CallbackQuery)
                    .UseWhen<FindTreeHandler>(When.LocationMessage)
                    //.MapWhen<CountWormsCallbackHandler>(CountWormsCallbackHandler.CanHandle)
                )
                .MapWhen(When.State("menu2"), defaultBranch => defaultBranch
                    .UseWhen<Menu2QueryHandler>(When.CallbackQuery)
                )
                .MapWhen(When.State("menu3"), defaultBranch => defaultBranch
                    .UseWhen<Menu3QueryHandler>(When.CallbackQuery)
                )

            // .Use<UnhandledUpdateReporter>()
            ;
        }
    }
}