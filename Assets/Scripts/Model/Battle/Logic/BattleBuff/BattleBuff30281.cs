using cfg;

public class BattleBuff30281 : BattleBuffBase
{
    protected override float OnGetReplaceSkillGangQiCost()
    {
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var delta = maxHp - hp;
        var single = GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        return delta / single;
    }

    protected override void OnEffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (gangQiDelta <= 0)
            return;

        var hpDelta = Subject.GetProperty(BattlePropertyType.MaxHp) - Subject.GetProperty(BattlePropertyType.Hp);
        var single = GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        var replace = hpDelta / single;
        if (replace >= gangQiDelta)
        {
            gangQiDelta = 0;
            DoChangeProperty(Subject, BattlePropertyType.MaxHpInt, -gangQiDelta * single, BattleSource.Buff);
        }
        else
        {
            gangQiDelta -= replace;
            DoChangeProperty(Subject, BattlePropertyType.MaxHpInt, -replace * single, BattleSource.Buff);
        }
    }
}
