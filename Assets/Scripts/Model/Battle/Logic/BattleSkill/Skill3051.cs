using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3051 : BattleSkillBase
{
    //施加1层缓速获得2层力衰和1层刚聚
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffHuanSu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffLiShuai, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffGangJu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (paramModel is { } model)
        {
            var propertyValue = Subject.GetProperty(BattlePropertyType.Power);
            var checkValue = 0.5f * propertyValue;
            if (BattleUtil.CompareValue(model.GetSelfAttackHpValue(Subject.EntityID), checkValue, DataRelation.DaYuDengYu))
            {
                return new BattleSkillRepeatData
                {
                    SkillID = SkillID,
                    TargetID = Target.EntityID,
                    MaxRepeatCount = 2,
                    IfLostChangeToOther = false
                };
            }
        }
        
        return null;
    }
}