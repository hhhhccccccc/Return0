using cfg;

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
        DoHealHp(Subject, heal, BattleSource.HeartMethod);
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, GetConfigParamInt(1), null, BattleMomentType.None);
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, GetConfigParamInt(2), null, BattleMomentType.None);
    }
}