using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2073 : BattleSkillBase
{
    //单方面攻击施加3层赤沸状态，若目标本回合还未行动过则额外施加2层赤沸状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleAction)
            {
                var count = 3;
                if (Target.RoundAlreadyActionTimes == 0)
                {
                    count += 2;
                }
                
                DoAddBuff(Target, GameConst.Battle.BuffChiFei, Subject, count, null, BattleMomentType.ReleaseSkillAction);
            }
        }
    }
}