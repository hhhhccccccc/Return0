using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3115 : BattleSkillBase
{
    //每有一层迅速状态威力增15的百分比
    public override float GetWellyRateEx(int skillGuid)
    {
        return Subject.GetBuffCountByID(GameConst.Battle.BuffXunSu) * 0.15f;
    }
    
    //玄炁+10，随机消耗1个键
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
        DoRemoveRandomKey(Subject, 1, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
}