using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WoodpeckerBot.Services;

namespace WoodpeckerBot.Handlers
{
    public class CountWormsCallbackHandler : IUpdateHandler
    {
        private readonly ICountWormsService _countWormsService;

        public CountWormsCallbackHandler(ICountWormsService countWormsService)
        {
            _countWormsService = countWormsService;
        }
        public static bool CanHandle(IUpdateContext context) =>
            context.Update.CallbackQuery?.Data?.StartsWith("start/WORMS") == true;

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                context.Update.CallbackQuery.Message.Chat,
                "There are " + _countWormsService.GetWorms() + " worms"
            );
                    
          
            await next(context);
        }
    }
}