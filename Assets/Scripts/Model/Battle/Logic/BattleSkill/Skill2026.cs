using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2026 : BattleSkillBase
{
    //施加3层术衰状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, GameConst.Battle.BuffShuShuai, Subject, 3, null, BattleMomentType.BeforeClash);
            }
        }
    }

    //施加1层僵硬
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffJiangYing, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //若持有键超过3个则随机消耗3个键在下一息重复该行动
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        var keyCount = Subject.GetAllKeyCount();
        var need = Config.ParamEx[0].ToInt();
        if (keyCount >= need)
        {
            Subject.RemoveRandomKey(need);
            return new BattleSkillRepeatData
            {
                SkillID = SkillID,
                VariantID = VariantID,
                TargetID = Target.EntityID,
                MaxRepeatCount = 999999999,
                IfLostChangeToOther = false
            };
        }

        return null;
    }
}