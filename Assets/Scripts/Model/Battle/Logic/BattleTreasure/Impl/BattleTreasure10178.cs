using cfg;

//todo 表现
public class BattleTreasure10178 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffKuYin, Subject, GetParamInt(1));
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        var skill = Subject.GetSkill();
        var target = skill.Target;
        BattleBuffManager.AddBuff(target, GameConst.Battle.BuffKuYin, Subject, GetParamInt(1));
    }
}


