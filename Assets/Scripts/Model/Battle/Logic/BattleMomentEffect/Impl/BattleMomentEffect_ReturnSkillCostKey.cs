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
                var skill = target.GetSkill();
                if (skill != null)
                {
                    var cost = skill.GetKeyCostList;
                    foreach (var key in cost)
                    {
                        target.ChangeKey((BattleKeyType)key, 1);
                    }
                }
            }
        }
    }
}