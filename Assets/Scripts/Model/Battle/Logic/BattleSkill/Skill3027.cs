using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3027 : BattleSkillBase
{
    //todo 每损失2%体减少该招式1刚炁消耗

    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}