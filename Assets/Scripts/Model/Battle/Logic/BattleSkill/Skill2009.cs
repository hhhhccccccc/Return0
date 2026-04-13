using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2009 : BattleSkillBase
{
    //每有一个键该招式威力增加8的百分比
    public override float GetWellyRateEx(int skillGuid)
    {
        return 0.08f * Subject.GetAllKeyCount();
    }
    
    //刚炁+30，消耗全部的键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 30, BattleSource.Skill);
        DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
}