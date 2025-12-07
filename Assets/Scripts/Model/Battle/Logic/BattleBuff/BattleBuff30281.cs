using cfg;

public class BattleBuff30281 : BattleBuffBase
{
    protected override float OnGetReplaceSkillGangQiCost()
    {
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var delta = maxHp - hp;
        var single = Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        return delta / single;
    }

    protected override void OnEffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (gangQiDelta <= 0)
            return;

        var hpDelta = Subject.GetProperty(BattlePropertyType.MaxHp) - Subject.GetProperty(BattlePropertyType.Hp);
        var single = Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        var replace = hpDelta / single;
        if (replace >= gangQiDelta)
        {
            gangQiDelta = 0;
            Subject.ChangeProperty(BattlePropertyType.MaxHpInt, -gangQiDelta * single);
        }
        else
        {
            gangQiDelta -= replace;
            Subject.ChangeProperty(BattlePropertyType.MaxHpInt, -replace * single);
        }
    }
}
