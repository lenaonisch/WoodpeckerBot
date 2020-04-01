using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WoodpeckerBot.Handlers;
using WoodpeckerBot.Options;
using WoodpeckerBot.Services;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace WoodpeckerBot
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EchoBot>()
                .Configure<BotOptions<EchoBot>>(Configuration.GetSection("EchoBot"))
                .Configure<CustomBotOptions<EchoBot>>(Configuration.GetSection("EchoBot"))
                .AddScoped<TextEchoer>()
                .AddScoped<PingCommand>()
                .AddScoped<StartCommand>()
                .AddScoped<WebhookLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<FindTreeHandler>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<CallbackQueryHandler>()
            ;
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IFindTreeService, FindTreeService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EchoBot>(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                // use Telegram bot webhook middleware in higher environments
                app.UseTelegramBotWebhook<EchoBot>(ConfigureBot());
                // and make sure webhook is enabled
                app.EnsureWebhookSet<EchoBot>();
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private IBotBuilder ConfigureBot()
        {
            Telegram.Bot.Framework.Abstractions.IBot t;
            return new BotBuilder()
                .Use<ExceptionHandler>()

                // .Use<CustomUpdateLogger>()
                .UseWhen<WebhookLogger>(When.Webhook)

                .UseWhen<UpdateMembersList>(When.MembersChanged)
                
                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                        .Use<TextEchoer>()
                        .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<StartCommand>("start")
                        )
                    //.Use<NLP>()
                    )
                    .MapWhen<FindTreeHandler>(When.LocationMessage)
                )

                .MapWhen<CallbackQueryHandler>(When.CallbackQuery)

                // .Use<UnhandledUpdateReporter>()
                ;
        }
    }
}
