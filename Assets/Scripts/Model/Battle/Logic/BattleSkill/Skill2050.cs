using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2050 : BattleSkillBase
{
    //招式的玄炁消耗转为当前90%，至多90
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.XuanQi, 0.9f, 90);
    }
    
    //至少造成80%技的伤害时返还消耗的键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var damage = model.GetSelfAttackHpValue(Subject.EntityID);
            if (damage >= Subject.GetProperty(BattlePropertyType.Tech) * 0.8f)
            {
                foreach (var key in TruthCostKey)
                {
                    Subject.AddBattleKey(key, ChangeKeyReason.SkillEffect, ChangeKeyType.Back);
                }
            }
        }
    }
}