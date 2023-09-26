namespace WeatherForecastConsole.Domain;

public class WeatherForecast // representera en datatstruktur
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}
