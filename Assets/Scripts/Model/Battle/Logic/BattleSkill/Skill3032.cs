using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3032 : BattleSkillBase
{
    //招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.5f, 50);
    }

    //至少造成55%力的伤害时施加2层武衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var power = Subject.GetProperty(BattlePropertyType.Power);
        if (paramModel is DamageParamModel model)
        {
            var hpValue = model.GetSelfAttackHpValue(Subject.EntityID);
            if (hpValue >= power * 0.55f)
            {
                DoAddBuff(Target, GameConst.Battle.BuffWuShuai, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }
}