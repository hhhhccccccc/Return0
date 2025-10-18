using cfg;

public class BattleChronoData : IModel
{
    public ChronoType ChronoType;
    public BattleChronoContinueType ContinueType;
    public int Times;
}

public enum BattleChronoContinueType
{
    None = 0,
    ActionWheel = 1,
    Round = 2
}