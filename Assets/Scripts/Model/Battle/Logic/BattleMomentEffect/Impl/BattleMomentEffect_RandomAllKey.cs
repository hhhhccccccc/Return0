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
            var delta = Config.ParamList[1].ToInt();
            var count = subject.GetKeyCount() + delta;
            subject.RemoveAllKey();
            var list = Util.GetRandomKey(count);
            foreach (var keyType in list)
            {
                subject.ChangeKey(keyType, 1);
            }
        }
    }
}