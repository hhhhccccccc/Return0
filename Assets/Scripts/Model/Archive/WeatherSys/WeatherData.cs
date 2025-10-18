using cfg;

public class WeatherData : IModel
{
    /// <summary>
    /// 季节ID
    /// </summary>
    public int SeasonID;
    /// <summary>
    /// 天气ID
    /// </summary>
    public int WeatherID;
    /// <summary>
    /// 天气描述
    /// </summary>
    public string WeatherDes;
    /// <summary>
    /// 天气类型
    /// </summary>
    public WeatherType WeatherType;
    /// <summary>
    /// 滤镜
    /// </summary>
    public int Filter;
    /// <summary>
    /// 开始时刻
    /// </summary>
    public int StartMoment;
    /// <summary>
    /// 结束时刻
    /// </summary>
    public int ContinueMoment;
}