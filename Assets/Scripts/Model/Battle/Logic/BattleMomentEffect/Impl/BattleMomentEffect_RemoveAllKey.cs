using System.Linq;
using cfg;

public class BattleMomentEffect_RemoveAllKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var subject = GetUnitByParamID(unitParamID);
        if (subject != null)
        {
            subject.RemoveAllKey();
        }
    }
}