using System;
using cfg;

public class BattleVariant5025 : BattleVariantBase
{
    //todo 根据目标的卦位至多使目标本次的行动延迟4息
    
    //行动后获得2过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 2, null, BattleMomentType.AfterAction);
    }
}
