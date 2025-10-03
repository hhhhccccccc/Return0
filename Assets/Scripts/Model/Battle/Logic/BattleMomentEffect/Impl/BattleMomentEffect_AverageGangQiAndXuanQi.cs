using cfg;
using Zenject;

public class BattleMomentEffect_AverageGangQiAndXuanQi : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var gangQi = target.GetProperty(BattlePropertyType.GangQi);
                var xuanQi = target.GetProperty(BattlePropertyType.XuanQi);
                var average = (gangQi + xuanQi) / 2.0f;
                var source = BattleSource.None;
                if (Config.ParamList.Count > 1)
                    source = (BattleSource)Config.ParamList[1].ToInt();
                target.SetProperty(BattlePropertyType.GangQi, average, source);
                target.SetProperty(BattlePropertyType.XuanQi, average, source);
            }
        }
    }
}