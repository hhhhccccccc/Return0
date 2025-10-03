using cfg;
using Zenject;

public class BattleMomentEffect_SetActionWheelToNow : BattleMomentEffect
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.SetActionWheelToNow();
                BattleLogicStateManager.CallAddUnitToNowLogicCalculate(target.EntityID);
            }
        }
    }
}