using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1047 : BattleSkillBase
{
    //todo 交锋失败则在下一息重复该招式
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (ClashState.Contains(false))
        {
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

    //刚炁+100，随机获得5个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 100, BattleSource.Skill);
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }

    //获得1层避殃状态，敌手因招式效果获得的炁-100，
    public override void BeforeClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            DoReduceHealQi(otherUnit, BattleMomentType.BeforeClash);
        }
        
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 1, null, BattleMomentType.BeforeClash);
    }
}