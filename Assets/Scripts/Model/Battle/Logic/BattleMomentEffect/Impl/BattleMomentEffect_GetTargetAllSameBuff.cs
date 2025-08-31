using Zenject;

public class BattleMomentEffect_GetTargetAllSameBuff : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        var target = GetUnitByParamID(Config.ParamList[1]);
        if (subject != null && target != null)
        {
            //todo 获得目标相同的全部增益状态和异常状态
        }
    }
}