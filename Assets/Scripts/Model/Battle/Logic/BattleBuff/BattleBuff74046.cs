using cfg;

//目标下回合刚炁自然恢复不会低于35，自身下回合玄炁自然恢复不会低于35 target.AddRecoverNaturalData(type, value);
public class BattleBuff74046 : BattleBuffBase
{
    private bool Trigger { get; set; }
    public override void ActionWheelStart()
    {
        Trigger = true;
        base.ActionWheelStart();
    }

    public override void EndAction()
    {
        Trigger = false;
        base.EndAction();
    }

    public override void ChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
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

    public override void Recycle()
    {
        Trigger = false;
        base.Recycle();
    }
}
