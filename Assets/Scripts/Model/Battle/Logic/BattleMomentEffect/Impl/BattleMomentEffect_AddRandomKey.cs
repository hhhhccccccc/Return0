using Zenject;

public class BattleMomentEffect_AddRandomKey : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var count = Config.ParamList[1].ToInt();
            var list = Util.GetRandomKey(count);
            foreach (var keyType in list)
            {
                subject.AddKey(keyType, 1);
            }
        }
    }
}