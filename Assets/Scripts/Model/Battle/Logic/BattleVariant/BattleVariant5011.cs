using System;
using cfg;

public class BattleVariant5011 : BattleVariantBase
{
    //释放成功下个回合玄炁的自然恢复增加3
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 75011, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }


    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
