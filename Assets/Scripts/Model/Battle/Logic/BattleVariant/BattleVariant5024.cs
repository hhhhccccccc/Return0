using System;
using cfg;

public class BattleVariant5024 : BattleVariantBase
{
    //todo 根据目标的卦位至多使本次的行动加快4息
    
    //行动后获得2过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 2, null, BattleMomentType.AfterAction);
    }
}
