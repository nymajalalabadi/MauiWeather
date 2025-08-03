using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiWeather.MVVM.ViewModels
{
    public class WeatherViewModel
    {
        public ICommand SearchCommand => new Command<string>(async (searchBar) =>
        {
            if (!string.IsNullOrWhiteSpace(searchBar))
            {
                Location location = await GetCoordinatesAsync(searchBar.ToString());
                
            }
        });
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
