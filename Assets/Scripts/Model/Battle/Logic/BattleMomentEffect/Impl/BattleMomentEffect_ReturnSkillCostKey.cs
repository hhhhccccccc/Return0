using cfg;
using Zenject;

public class BattleMomentEffect_ReturnSkillCostKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                
            }
        }
    }
}