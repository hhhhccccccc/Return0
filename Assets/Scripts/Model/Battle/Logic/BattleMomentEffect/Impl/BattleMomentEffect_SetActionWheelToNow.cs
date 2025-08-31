using cfg;
using Zenject;

public class BattleMomentEffect_SetActionWheelToNow : BattleMomentEffect
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            target.SetActionWheelToNow();
            BattleLogicStateManager.CallAddUnitToNowLogicCalculate(target.EntityID);
        }
    }
}