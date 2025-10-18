using System;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeChrono : BattleMomentEffect
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    
    protected override void OnEffect()
    {
        var chronoType = (ChronoType)(Config.ParamList[0].ToInt());
        var continueType = (BattleChronoContinueType)(Config.ParamList[1].ToInt());    
        var times = Config.ParamList[2].ToInt();    
        BattleLogicStateManager.ChangeChrono(chronoType, continueType, times);
    }
}