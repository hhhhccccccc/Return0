using System;
using cfg;
using Zenject;

public class BattleBuff30421 : BattleBuffBase
{
    public override bool CheckDontBeCounter(MomentParamModel paramModel)
    {
        DoReduceBuffLayerCount(Subject, BuffID, 1);
        return true;
    }
}
