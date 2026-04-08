using System;
using cfg;

public class BattleVariant1007 : BattleVariantBase
{
    //行动决定后获得30+GR*3层护体
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, 30 + Subject.Gr * 3, null, BattleMomentType.AfterAction);
    }
}
