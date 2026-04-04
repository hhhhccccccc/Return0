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
        var heal = Subject.GetProperty(BattlePropertyType.MaxHp) * GetConfigParamFloat(0);
        Subject.HealHp(heal, BattleSource.HeartMethod);
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, GetConfigParamInt(1));
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, GetConfigParamInt(2));
    }
}