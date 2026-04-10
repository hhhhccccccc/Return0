using System;
using System.Collections.Generic;
using cfg;

public class BattleVariant5014 : BattleVariantBase
{
    //交锋时减少目标本次杀式行动所能造成的伤害，减少量为本次杀式的威力*力50%
    
    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }

    public override void ReduceDamageInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        var other = GetOtherUnit(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleClash ||
                model.BattleClashType == BattleClashType.DoubleClash)
            {
                var getPropertySourceModel = PM.GetClass<GetPropertySourceModel>();
                getPropertySourceModel.SourceType = GetPropertySourceType.ReceiveSkillDamage;
                getPropertySourceModel.TypeID = other.GetSkill().SkillGuid;
                getPropertySourceModel.AttackerID = other.EntityID;
                getPropertySourceModel.HitID = Subject.EntityID;
                dict.Add(GetSymbol, Subject.GetSkillDamageWelly(SkillDataGetType.WellyRateCurr) * 0.5f * Subject.GetProperty(BattlePropertyType.Power, getPropertySourceModel));
                PM.RecycleClass(getPropertySourceModel);
            }
        }
    }
}
