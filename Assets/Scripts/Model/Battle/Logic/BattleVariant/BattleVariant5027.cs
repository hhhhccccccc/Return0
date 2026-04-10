using System;
using cfg;

public class BattleVariant5027 : BattleVariantBase
{
    //本次的行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 1);
    }

    //行动后获得1层缓速状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuanSu, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
