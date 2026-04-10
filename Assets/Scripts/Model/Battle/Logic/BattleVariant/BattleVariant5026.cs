using System;
using cfg;

public class BattleVariant5026 : BattleVariantBase
{
    public override float GetWellyRateEx(int skillGuid)
    {
        return -0.1f;
    }

    //随机获得1个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.VariantEffect);
    }
}
