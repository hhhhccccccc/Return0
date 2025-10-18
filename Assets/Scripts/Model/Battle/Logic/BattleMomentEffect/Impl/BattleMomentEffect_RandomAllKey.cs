using System.Linq;
using cfg;

public class BattleMomentEffect_RandomAllKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var delta = Config.ParamList[1].ToInt();
                var count = target.GetAllKeyCount() + delta;
                target.RemoveAllKey();
                var list = Util.GetRandomKey(count);
                foreach (var keyType in list)
                {
                    target.ChangeKey(keyType, 1);
                }
            }
        }
    }
}