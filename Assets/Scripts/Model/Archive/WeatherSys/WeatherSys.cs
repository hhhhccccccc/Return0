using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class WeatherSys : SingleArchiveModel
{
    [Inject] private SceneSys SceneSys;
    [Inject] private DateSys DateSys;
    
    /// <summary>
    /// key : zoneID
    /// </summary>
    private Dictionary<int, WeatherData> WeatherDataMap = new();

    public WeatherData GetWeatherData(int zoneID) => WeatherDataMap.TryGetValue(zoneID, out var data) ? data : null;
    public override void Init()
    {
        base.Init();
        Register<ZoneChangedEventModel>(OnZoneChanged);
        Register<SeasonChangedEventModel>(OnSeasonChanged);
        Register<MomentChangedEventModel>(OnMomentChanged2nd);
    }

    private void OnMomentChanged2nd(MomentChangedEventModel model)
    {
        if (!WeatherDataMap.TryGetValue(SceneSys.ZoneID, out var data))
        {
            RefreshWeather();
        }
        else if ((data.ContinueMoment > 0 && DateSys.GetMoment() >= data.StartMoment + data.ContinueMoment) || data.SeasonID != DateSys.GetNowSeason())
        {
            RefreshWeather();
        }
    }

    private void OnZoneChanged(ZoneChangedEventModel model)
    {
        if (!WeatherDataMap.TryGetValue(SceneSys.ZoneID, out var data))
        {
            RefreshWeather();
        }
        else if ((data.ContinueMoment > 0 && DateSys.GetMoment() >= data.StartMoment + data.ContinueMoment)|| data.SeasonID != DateSys.GetNowSeason())
        {
            RefreshWeather();
        }
    }
    
    private void OnSeasonChanged(SeasonChangedEventModel model)
    {
        RefreshWeather();
    }

    private void RefreshWeather()
    {
        var zoneID = SceneSys.ZoneID;
        var zoneConfig = ConfigManager.GetZoneConfig(zoneID);
        var weatherGroupID = zoneConfig.WeatherGroupID;
        var weatherGroupConfig = ConfigManager.GetWeatherGroupConfig(weatherGroupID);
        var seasonID = DateSys.GetNowSeason();
        var poolData = weatherGroupConfig.WeatherPool.FirstOrDefault(data => data.SeasonID == seasonID);
        if (poolData == null)
        {
            var weatherID = weatherGroupConfig.Default;
            var weatherConfig = ConfigManager.GetWeatherConfig(weatherID);
            SetWeather(zoneID, weatherConfig, 0);
        }
        else
        {
            var poolID = poolData.WeatherPoolID;
            var poolConfig = ConfigManager.GetWeatherPoolConfig(poolID);
            var poolList = poolConfig.WeatherPoolData.ToList();
            var resultData = Util.GetRandom(poolList, poolList.Select(p => p.Weight).ToList());
            var weatherID = resultData.WeatherID;
            var weatherConfig = ConfigManager.GetWeatherConfig(weatherID);
            if (weatherConfig == null)
            {
                Error("天气随机错误");
                return;
            }

            var continueMoment = Util.GetRandom(resultData.MinContinue, resultData.MaxContinue);
            SetWeather(zoneID, weatherConfig, continueMoment);
        }
    }

    private void SetWeather(int zoneID, WeatherConfig config, int continueMoment)
    {
        var model = PoolManager.GetClass<WeatherChangedEventModel>();
        if (!WeatherDataMap.TryGetValue(zoneID, out var weatherData))
        {
            weatherData = new WeatherData();
            WeatherDataMap.TryAdd(zoneID, weatherData);
            model.OldWeatherID = 0;
            model.OldWeatherType = 0;
            model.OldWeatherDes = string.Empty;
            model.OldWeatherID = 0;
        }
        else
        {
            model.OldWeatherID = weatherData.WeatherID;
            model.OldWeatherType = weatherData.WeatherType;
            model.OldWeatherDes = weatherData.WeatherDes;
            model.OldWeatherID = weatherData.Filter;
        }

        weatherData.SeasonID = DateSys.GetNowSeason();
        weatherData.WeatherID = config.ID;
        weatherData.WeatherDes = config.Des;
        weatherData.WeatherType = config.WeatherType;
        weatherData.Filter = config.Filter;
        weatherData.StartMoment = DateSys.GetMoment();
        weatherData.ContinueMoment = continueMoment;
        
        model.NewWeatherID = config.ID;
        model.NewWeatherType = config.WeatherType;
        model.NewWeatherDes = config.Des;
        model.NewWeatherID = config.Filter;
        Dispatch(model);
        PoolManager.RecycleClass(model);
    }
}


