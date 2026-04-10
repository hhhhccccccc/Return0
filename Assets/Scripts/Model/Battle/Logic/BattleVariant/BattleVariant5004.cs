using System;
using cfg;

public class BattleVariant5004 : BattleVariantBase
{
    //本次的行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (model != null)
        {
            if (model.SourceType == GetPropertySourceType.GetSkillCostView || model.SourceType == GetPropertySourceType.GetSkillCostCheck || model.SourceType == GetPropertySourceType.GetSkillCostLogic)
            {
                if (propertyType == BattlePropertyType.GangQi)
                {
                    var gangQiCost = Skill.GetGangQiCost();
                    return Math.Min(gangQiCost * 0.2f, 10);
                }
                
                if (propertyType == BattlePropertyType.XuanQi)
                {
                    var xuanQiCost = Skill.GetXuanQiCost();
                    return Math.Min(xuanQiCost * 0.2f, 10);
                }
                
            }
        }

        return 0;
    }

    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
