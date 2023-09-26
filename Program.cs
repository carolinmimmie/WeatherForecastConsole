using System.Text.Json;
using WeatherForecastConsole.Domain;
using static System.Console;

namespace WeatherForecastConsole;

class Program
{
    // Httpclient finns tillgänglig, behöver inte installeras via NuGet
    //Den gör HTTP anrop över nätverket
    //För att göra HHTP.anrop ( text GET) behöver vi använda ett bibliotek
    //som kan göra detta. såsom HttpClient. Använder detta i våran metod GetForCast()

    static readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:8000/")//adressen
    };
    static void Main()
    {
        Title = "Weather Forecast Console";

        while (true)
        {

            //CursorVisible = false;

            WriteLine("1. Lista väderprognoser");


            var keyPressed = ReadKey(true);

            Clear();

            switch (keyPressed.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:

                    ListWeatherForecastView();

                    break;
            }

            Clear();

        }
    }

    private static void ListWeatherForecastView()
    {
        // 1 - Hämta forecasts från backend (web api) (alltså skicka en HHTP GET förfrågan)
        var forecasts = GetForecasts();

        // 2 - Skriv ut en "tabell", på samma sätt som vi precis gjorde i vår web-applikation

        Write($"{"Date",-16}");
        Write($"{"Summary",-16}");
        WriteLine("Temperature (C)");

        foreach (var forecast in forecasts)
        {
            Write($"{forecast.Date,-16}");
            Write($"{forecast.Summary,-16}");
            WriteLine(forecast.TemperatureC);
        }

        // 3 - Vänta på att användaren trycker på escape, återvänd då till huvudmenyn
        WaitUntilKeyPressed(ConsoleKey.Escape);

    }

    //vi vill Returnera IEnumerable <WeatherForeCast>
    // IEnumarable inte är en datatyp i sig själv utan snarare
    // ett gränssnitt som används för att hantera upprepning över samlingar av data.

    private static IEnumerable<WeatherForecast> GetForecasts()
    {
        // 1 - Skicka ett HTTP GET-anrop till backend (web api)
        // HTTP finns det olika metoder (GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS, TRACE, ...)

        // https://localhost:8000/ + weatherforecast
        // Skickar en HTTP GET till https://localhost:8000/weatherforecast
        var response = httpClient.GetAsync("weatherforecast").Result;

        // 2 - Läs ut JSON som vi fått tillbaka
        var json = response.Content.ReadAsStringAsync().Result;

        // 3 - Deserialisera JSON till ett objekt (IEnumerable<WeatherForecast>)
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var forecasts = JsonSerializer
            .Deserialize<IEnumerable<WeatherForecast>>(json, serializeOptions)
            ?? new List<WeatherForecast>();

        // 4 - Returnera resultatet (IEnumerable<WeatherForecast>)

        return forecasts;
    }



    private static void WaitUntilKeyPressed(ConsoleKey key)
    {
        while (ReadKey(true).Key != key) ;
    }



}

