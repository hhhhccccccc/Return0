using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff30011 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        if (LayerCount >= GetConfigParamInt(0))
        {
            DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(1), BattleSource.Buff);
            DoClearBuff(Subject, BuffID);
        }
    }
}
