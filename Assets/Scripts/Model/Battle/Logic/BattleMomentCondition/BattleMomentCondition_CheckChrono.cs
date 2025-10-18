using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentCondition_CheckChrono : BattleMomentCondition
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    protected override bool OnCondition()
    {
        var check = Config.ParamList[0].ToInt();
        var state = Config.ParamList[1].ToInt() == 1;
        switch (check)
        {
            case 1:
                if (state && BattleLogicStateManager.BattleChronoType == ChronoType.Sunrise)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Sunrise)
                {
                    return true;
                }
                break;
            case 2:
                if (state && BattleLogicStateManager.BattleChronoType == ChronoType.Morning)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Morning)
                {
                    return true;
                }
                break;
            case 3:
                if (state && BattleLogicStateManager.BattleChronoType == ChronoType.Sunset)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Sunset)
                {
                    return true;
                }
                break;
            case 4:
                if (state && BattleLogicStateManager.BattleChronoType == ChronoType.Night)
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Night)
                {
                    return true;
                }
                break;
            case 5:
                if (state && (BattleLogicStateManager.BattleChronoType == ChronoType.Sunrise || BattleLogicStateManager.BattleChronoType == ChronoType.Morning))
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Sunrise && BattleLogicStateManager.BattleChronoType != ChronoType.Morning)
                {
                    return true;
                }
                break;
            case 6:
                if (state && (BattleLogicStateManager.BattleChronoType == ChronoType.Sunset || BattleLogicStateManager.BattleChronoType == ChronoType.Night))
                {
                    return true;
                }
                
                if (!state && BattleLogicStateManager.BattleChronoType != ChronoType.Sunset && BattleLogicStateManager.BattleChronoType != ChronoType.Night)
                {
                    return true;
                }
                break;
        }
        
        
        return false;
    }
}