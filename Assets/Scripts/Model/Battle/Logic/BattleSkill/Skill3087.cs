using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3087 : BattleSkillBase
{
    //自身异常状态未超过2个则施加1层晕眩状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (CheckBuffTypeCount(Subject, BuffType.Abnormal, 2, DataRelation.XiaoYuDengYu))
        {
            DoAddBuff(Target, GameConst.Battle.BuffXuanYun, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}