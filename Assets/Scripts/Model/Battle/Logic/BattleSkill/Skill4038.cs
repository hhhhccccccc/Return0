using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4038 : BattleSkillBase
{
    //消耗对手1个随机的键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoRemoveRandomKey(clashUnit, 1, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
    
    //消耗目标至多7个随机的键，若超过2个则根据消耗数量使其获得至少1至多5个随机的键并施加3层失衡
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var removeList = DoRemoveRandomKey(Target, 7, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
        if (removeList.Count > 2)
        {
            var addCount = removeList.Count - 2;
            DoAddRandomKey(Target, addCount, ChangeKeyReason.SkillEffect);
            DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}