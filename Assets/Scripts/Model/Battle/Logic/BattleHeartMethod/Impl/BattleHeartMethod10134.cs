using cfg;

public class BattleHeartMethod10134 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitDieEventModel>(OnUnitDie);
    }

    private void OnUnitDie(UnitDieEventModel model)
    {
        var target = BattleManager.GetUnit(model.DieID);
        var count = target.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        var heal = (GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr) * (1 + count * GetConfigParamFloat(2));
        DoHealHp(Subject, heal, BattleSource.HeartMethod);
    }
}