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
        var heal = (GetParamFloat(0) + GetParamFloat(1) * Subject.Gr) * (1 + count * GetParamFloat(2));
        var finalValue = Subject.HealHp(heal, BattleSource.HeartMethod);
        EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeHp, finalValue);
    }
}