using cfg;

public class BattleBuff30271 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        if (hp / maxHp <= Config.ParamEx[0])
        {
            DoSetHp(Subject, 0, Subject, BattleSource.Buff);
        }
    }

    protected override void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        if (hp / maxHp <= Config.ParamEx[0])
        {
            DoSetHp(Subject, 0, Subject, BattleSource.Buff);
        }
    }
}
