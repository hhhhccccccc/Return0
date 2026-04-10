using System;
using cfg;

public class BattleVariant5010 : BattleVariantBase
{
    //todo 行动所选对手本次行动速低于自身则该对手本次杀式行动目标转来至自身，行动后获得过劲状态
    
    
    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
