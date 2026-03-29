using cfg;

public class BattleBuff30271 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        base.OnBuffStart();
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        if (hp / maxHp <= Config.ParamEx[0])
        {
            Subject.SetHp(0, Subject.EntityID, BattleSource.Buff);
        }
    }

    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        if (hp / maxHp <= Config.ParamEx[0])
        {
            Subject.SetHp(0, Subject.EntityID, BattleSource.Buff);
        }
    }
}
