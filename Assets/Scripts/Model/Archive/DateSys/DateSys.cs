using System.Linq;
using cfg;

public class DateSys : SingleArchiveModel
{
    private const int DayMoment = 96;
    public int DateID;
    public int Moment;
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
        Dispatch<MomentChangedEventModel>(null);
    }
    
    public DateConfig GetNowDateConfig()
    {
        return ConfigManager.GetDateConfig(DateID);
    }

    public int GetNowSeason()
    {
        var config = GetNowDateConfig();
        if (config != null)
        {
            return config.Season;
        }

        return 0;
    }

    public int GetMoment()
    {
        return (DateID - 1) * 96 + Moment;
    }
}
