using cfg;
using Zenject;

public class BattleMomentEffect_AddRandomKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var count = Config.ParamList[1].ToInt() * BuffLayerCount;
            foreach (var target in targetList)
            {
                target.AddRandomKey(count, (ChangeKeyReason)Config.ParamList[2].ToInt());
            }
        }
    }
}