using System;
using cfg;

public class BattleVariant5028 : BattleVariantBase
{
    //招式的威力增加5的百分比，
    public override float GetWellyRateEx(int skillGuid)
    {
        return 0.05f;
    }

    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
