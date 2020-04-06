using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WoodpeckerBot.Services;

namespace WoodpeckerBot.Handlers
{
    class FindTreeHandler : IUpdateHandler
    {
        private readonly IFindTreeService _findTreeService;

        public FindTreeHandler(IFindTreeService findTreeService)
        {
            _findTreeService = findTreeService;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message;
            Location location = msg.Location;

            var treeLocation = _findTreeService.NearestTree(location);
            
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                $"Nearest tree is *{treeLocation.Longitude}; {treeLocation.Latitude}* ",
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                replyMarkup: new ReplyKeyboardRemove(),
                /*new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("back", "back::")
                    }
                ),*/
                cancellationToken: cancellationToken
            );

            //await context.Bot.Client
            //    new SendMessageRequest(
            //        context.Update.CallbackQuery.Message.Chat,
            //        context.Update.CallbackQuery.Message.MessageId,
                    
            //        )
            //    );
        }
    }
}
