using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1037 : BattleSkillBase
{
    //todo 交锋失败则在下一息重复该招式
    
    //敌手因招式效果获得的炁-100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            DoReduceHealQi(otherUnit, BattleMomentType.BeforeClash);
        }
    }

    //刚炁+100，随机获得5个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 100, BattleSource.Skill);
        DoAddRandomKey(Subject, 5, ChangeKeyReason.SkillEffect);
    }
}