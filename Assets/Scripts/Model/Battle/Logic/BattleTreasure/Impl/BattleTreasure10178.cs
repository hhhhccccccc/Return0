using cfg;

public class BattleTreasure10178 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffKuYin, Subject, GetConfigParamInt(1), null, BattleMomentType.RoundStart);
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        var skill = Subject.GetSkill();
        var target = skill.Target;
        DoAddBuff(target, GameConst.Battle.BuffKuYin, Subject, GetConfigParamInt(1), null, BattleMomentType.DoDesitionAction);
    }
}


