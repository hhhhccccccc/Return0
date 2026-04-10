using System;
using cfg;

public class BattleVariant5023 : BattleVariantBase
{
    //行动决定后获得9+GR*9层护体
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, 9 + 9 * Subject.Gr, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
