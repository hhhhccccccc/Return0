using cfg;

//目标下回合刚炁自然恢复不会低于35，自身下回合玄炁自然恢复不会低于35 target.AddRecoverNaturalData(type, value);
public class BattleBuff30331 : BattleBuffBase
{
    private bool Trigger { get; set; }
    protected override void OnSelfActionWheelStart()
    {
        Trigger = true;
        base.OnSelfActionWheelStart();
    }

    public override void EndAction()
    {
        Trigger = false;
        base.EndAction();
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (Trigger)
        {
            if (propType == BattlePropertyType.GangQi)
            {
                Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, finalPropValue);
            }
            
            if (propType == BattlePropertyType.XuanQi)
            {
                Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, finalPropValue);
            }
        }
    }
    protected override void OnBuffRecycle()
    {
        Trigger = false;
    }
}
