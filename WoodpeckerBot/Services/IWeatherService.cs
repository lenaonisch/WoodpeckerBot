using System.Threading.Tasks;

namespace WoodpeckerBot.Services
{
    interface IWeatherService
    {
        Task<CurrentWeather> GetWeatherAsync(float lat, float lon);
    }
}