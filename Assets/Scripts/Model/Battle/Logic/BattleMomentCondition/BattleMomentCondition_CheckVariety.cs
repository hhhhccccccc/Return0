using cfg;

public class BattleMomentCondition_CheckVariety : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            return target.CheckVariety((HeroVariety)Config.ParamList[1].ToInt());
        }
        
        return false;
    }
}