using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WoodpeckerBot.Services
{
    public class CountWormsService : ICountWormsService
    {
        public int GetWorms()
        {
            return Random.Next();
        }
    }
}
