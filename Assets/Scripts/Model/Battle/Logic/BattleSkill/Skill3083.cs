using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3083 : BattleSkillBase
{
    //交锋失败下次释放将转移全部异常状态   至多转移2个异常状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (CheckSkillLastClashState(Subject, SkillID, false))
        {
            DoTransferBuff(Target, Subject, Subject, BuffType.Abnormal, 0, BattleMomentType.ReleaseSkillAction);
        }
        else
        {
            DoTransferBuff(Target, Subject, Subject, BuffType.Abnormal, 2, BattleMomentType.ReleaseSkillAction);
        }
    }
}