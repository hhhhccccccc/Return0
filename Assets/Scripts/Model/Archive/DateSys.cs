using System.Linq;
using cfg;

public class DateSys : SingleArchiveModel
{
    public int Year;
    public int Month;
    public int Day;
    protected override void InitPlayerData()
    {
        var config = ConfigManager.GetDateConfigMap().First().Value;
        Year = config.Year;
        Month = config.Month;
        Day = config.Day;
        LogManager.Debug($"Year : {Year}, Month : {Month}, Day : {Day}");
    }

    public DateConfig GetNowDateConfig()
    {
        return ConfigManager.GetDateConfig(Year, Month, Day);
    }
}
