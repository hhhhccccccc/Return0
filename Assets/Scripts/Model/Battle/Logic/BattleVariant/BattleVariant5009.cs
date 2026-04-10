using System;
using cfg;

public class BattleVariant5009 : BattleVariantBase
{
    //todo 炁拶类招式施加的状态层数增加50%（至少1）（只作用一个单位，优先行动目标再者自身），行动的消耗不会低于基础消耗，行动后获得过劲状态
    
    
    //行动后获得过劲状态
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffGuoJin, Subject, 1, null, BattleMomentType.AfterAction);
    }
}
