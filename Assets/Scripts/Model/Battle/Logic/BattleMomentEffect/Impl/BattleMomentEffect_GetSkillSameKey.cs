using System.Linq;
using cfg;

public class BattleMomentEffect_GetSkillSameKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var whoGet = GetUnitByParamID(unitParamID);
        if (whoGet != null && Subject != null && Target != null)
        {
            var subjectSkill = BattleLogicBehaviourManager.GetBattleBehaviour(Subject.EntityID).SkillID;
            var subjectSkillKey = ConfigManager.GetBattleSkill(subjectSkill).NeedKey;
            var targetSkill = BattleLogicBehaviourManager.GetBattleBehaviour(Target.EntityID).SkillID;
            var targetSkillKey = ConfigManager.GetBattleSkill(targetSkill).NeedKey;

            foreach (var keyType in subjectSkillKey.Intersect(targetSkillKey))
            {
                whoGet.ChangeProperty((BattlePropertyType)keyType, 1);
            }
        }
    }
}