using cfg;

//todo 表现
public class BattleHeartMethod10127 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        Register<UnitDieEventModel>(OnUnitDie);
        base.Init(heartMethodID, subject);
    }

    private void OnUnitDie(UnitDieEventModel model)
    {
        if (model.DieID == Subject.EntityID)
        {
            return;
        }
        var heal = Subject.GetProperty(BattlePropertyType.MaxHp) * GetParamFloat(0);
        Subject.HealHp(heal, BattleSource.HeartMethod);
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10071, Subject, GetParamInt(1));
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10091, Subject, GetParamInt(2));
    }
}