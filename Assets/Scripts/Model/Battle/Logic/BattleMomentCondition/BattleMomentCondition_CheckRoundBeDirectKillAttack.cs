using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckRoundBeDirectKillAttack : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        var targetID = Config.ParamList[1].ToInt();
        var state = Config.ParamList[2].ToInt() == 1;
        if (targetID == 0)
        {
            if (state && subject.CheckRoundBeDirectKillAttack(0))
            {
                return true;
            }

            if (!state && !subject.CheckRoundBeDirectKillAttack(0))
            {
                return true;
            }

            return false;
        }
        else
        {
            var target = GetUnitByParamID(targetID);
            if (subject != null && target != null)
            { 
                if (state && subject.CheckRoundBeDirectKillAttack(target.EntityID))
                {
                    return true;
                }

                if (!state && !subject.CheckRoundBeDirectKillAttack(target.EntityID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}