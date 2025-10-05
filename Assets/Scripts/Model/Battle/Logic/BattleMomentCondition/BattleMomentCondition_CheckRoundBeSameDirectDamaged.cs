using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckRoundBeSameDirectDamaged : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        var target = GetUnitByParamID(Config.ParamList[1]);
        if (subject != null && target != null)
        { 
            var state = Config.ParamList[2].ToInt() == 1;
            if (state && subject.CheckRoundBeSameDirectDamaged(target.EntityID))
            {
                return true;
            }

            if (!state && !subject.CheckRoundBeSameDirectDamaged(target.EntityID))
            {
                return true;
            }
        }

        return false;
    }
}