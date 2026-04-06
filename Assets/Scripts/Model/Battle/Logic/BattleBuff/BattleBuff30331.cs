using cfg;

//下一次行动中恢复刚炁时获得等量玄炁，恢复玄炁时获得等量刚炁（玉摄念状态）
public class BattleBuff30331 : BattleBuffBase
{
    private bool Trigger { get; set; }
    protected override void OnSelfActionWheelStart()
    {
        Trigger = true;
    }

    public override void EndAction()
    {
        Trigger = false;
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (Trigger)
        {
            if (propType == BattlePropertyType.GangQi)
            {
                DoChangePropertyAbs(Subject, BattlePropertyType.XuanQi, finalPropValue, BattleSource.Buff);
            }
            
            if (propType == BattlePropertyType.XuanQi)
            {
                DoChangePropertyAbs(Subject, BattlePropertyType.GangQi, finalPropValue, BattleSource.Buff);
            }
        }
    }
    protected override void OnBuffRecycle()
    {
        Trigger = false;
    }
}
