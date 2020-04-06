using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WoodpeckerBot.Services
{
    public interface IFindTreeService
    {
        Location NearestTree(Location location);
    }
}
