using System.Linq;
using cfg;
using Zenject;

public class DateSys : SingleArchiveModel
{
    private const int DayMoment = 96;
    public int DateID;
    public int Moment;
    //时辰
    public ChronoType ChronoType { get; private set; }
    public override void Init()
    {
        if (DateID == 0)
        {
            DateID = 1;
            LogManager.Debug($"初始化日期成功");
        }
    }

    public void MomentChanged(int moment)
    {
        Moment += moment;
        while (Moment >= DayMoment)
        {
            DateID++;
            Moment -= DayMoment;
        }
        
        RefreshChrono();
        Dispatch<MomentChangedEventModel>(null);
    }

    /// <summary>
    /// 刷新时辰
    /// </summary>
    private void RefreshChrono()
    {
        var nowSeason = GetNowSeason();
        var seasonConfig = ConfigManager.GetSeasonConfig(nowSeason);
        if (Moment >= seasonConfig.Sunrise && Moment < seasonConfig.Morning)
        {
            ChronoType = ChronoType.Sunrise;
        }
        else if (Moment >= seasonConfig.Morning && Moment < seasonConfig.Sunset)
        {
            ChronoType = ChronoType.Morning;
        }
        else if (Moment >= seasonConfig.Sunset && Moment < seasonConfig.Night)
        {
            ChronoType = ChronoType.Sunset;
        }
        else
        {
            ChronoType = ChronoType.Night;
        }
    }
    
    /// <summary>
    /// 当前日期
    /// </summary>
    /// <returns></returns>
    public DateConfig GetNowDateConfig()
    {
        return ConfigManager.GetDateConfig(DateID);
    }
    /// <summary>
    /// 当前季节
    /// </summary>
    /// <returns></returns>
    public int GetNowSeason()
    {
        var config = GetNowDateConfig();
        if (config != null)
        {
            return config.Season;
        }

        return 0;
    }

    /// <summary>
    /// 从1开始一直加的moment
    /// </summary>
    /// <returns></returns>
    public int GetAllMoment()
    {
        return (DateID - 1) * 96 + Moment;
    }
}
