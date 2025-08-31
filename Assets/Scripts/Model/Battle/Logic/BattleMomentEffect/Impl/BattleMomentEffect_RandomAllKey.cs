using System.Linq;
using cfg;

public class BattleMomentEffect_RandomAllKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var subject = GetUnitByParamID(unitParamID);
        if (subject != null)
        {
            var count = subject.GetKeyCount();
            var list = Util.GetRandomKey(count);
            foreach (var keyType in list)
            {
                subject.ChangeKey(keyType, 1);
            }
        }
    }
}