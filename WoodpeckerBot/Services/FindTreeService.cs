using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WoodpeckerBot.Services
{
    class FindTreeService : IFindTreeService
    {
        public FindTreeService()
        {
            
        }

        public Location NearestTree(Location location) =>
            new Location()
            {
                Latitude = location.Latitude + 10,//Random.NextFloat(-10, 10),
                Longitude = location.Longitude + 10//Random.NextFloat(-10, 10)
            };
    }
}
