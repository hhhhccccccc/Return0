using Zenject;

public class BattleMomentEffect_AddRandomKey : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager;
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var count = Config.ParamList[1].ToInt();
            var list = Util.GetRandomKey(count);
            foreach (var target in targetList)
            {
                foreach (var keyType in list)
                {
                    target.ChangeKey(keyType, 1);
                }
            }
        }
    }
}