using MauiWeather.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiWeather.MVVM.ViewModels
{
    public class WeatherViewModel
    {
        private HttpClient _httpClient;

        public WeatherViewModel()
        {
            _httpClient = new HttpClient();
        }


        public WeatherData WeatherData { get; set; }

        public ICommand SearchCommand => new Command<string>(async (searchBar) =>
        {
            if (!string.IsNullOrWhiteSpace(searchBar))
            {
                Location location = await GetCoordinatesAsync(searchBar.ToString());
                await GetWeather(location);
            }
        });

        private async Task GetWeather(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=weather_code,temperature_2m_max,temperature_2m_min&hourly=temperature_2m";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using(var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<WeatherData>(responseStream);
                    WeatherData = data;
                }
            }
        }

        private async Task<Location> GetCoordinatesAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);

            Location location = locations?.FirstOrDefault();

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }

            return location;
        }

    }
}
