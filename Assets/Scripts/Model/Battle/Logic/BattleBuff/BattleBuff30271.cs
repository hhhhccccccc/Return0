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
            Subject.SetHp(0, BattleSource.Buff);
        }
    }

    protected override void OnHpChanged()
    {
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        if (hp / maxHp <= Config.ParamEx[0])
        {
            Subject.SetHp(0, BattleSource.Buff);
        }
    }
}
