using cfg;
using Zenject;

public class BattleMomentEffect_AverageGangQiAndXuanQi : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var gangQi = subject.GetProperty(BattlePropertyType.GangQi);
            var xuanQi = subject.GetProperty(BattlePropertyType.XuanQi);
            var average = (gangQi + xuanQi) / 2.0f;
            var source = BattleSource.None;
            if (Config.ParamList.Count > 1)
                source = (BattleSource)Config.ParamList[1].ToInt();
            subject.SetProperty(BattlePropertyType.GangQi, average, source);
            subject.SetProperty(BattlePropertyType.XuanQi, average, source);
        }
    }
}