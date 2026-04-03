using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3063 : BattleSkillBase
{
    //至少造成80%力的伤害时施加2层破绽状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var hpValue = model.GetSelfAttackHpValue(Subject.EntityID);
            var power = Subject.GetProperty(BattlePropertyType.Power);
            if (hpValue >= power * 0.8f)
            {
                DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 2, null, BattleMomentType.ReleaseSkillAction);   
            }
        }
    }

}