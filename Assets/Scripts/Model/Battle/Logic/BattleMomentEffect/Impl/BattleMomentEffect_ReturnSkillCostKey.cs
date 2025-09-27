using cfg;
using Zenject;

public class BattleMomentEffect_ReturnSkillCostKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var subject = GetUnitByParamID(Config.ParamList[0]);
        if (subject != null)
        {
            var skill = subject.GetSkill();
            if (skill != null)
            {
                var cost = skill.GetKeyCostList;
                foreach (var key in cost)
                {
                    subject.ChangeKey((BattleKeyType)key, 1);
                }
            }
        }
    }
}