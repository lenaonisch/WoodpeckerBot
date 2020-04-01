using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            Message msg = context.Update.Message;
            Location location = msg.Location;

            var treeLocation = _findTreeService.NearestTree(location);

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                $"Nearest tree is *{treeLocation.Longitude}; {treeLocation.Latitude}* ",
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId
            );
        }
    }
}
