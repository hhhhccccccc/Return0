using cfg;

public class BattleWeatherData : IModel
{
    public BattleWeatherType WeatherType;
    public BattleWeatherContinueType ContinueType;
    public int Times;
}

public enum BattleWeatherContinueType
{
    None = 0,
    ActionWheel = 1,
    Round = 2
}