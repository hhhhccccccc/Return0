using cfg;
using Zenject;

public class BattleMomentEffect_SetMinRecoverQiNatural : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var type = Config.ParamList[1].ToInt();
            var value = Config.ParamList[2];
            foreach (var target in targetList)
            {
                target.AddMinRecoverNaturalData(type, value);
            }
        }
    }
}