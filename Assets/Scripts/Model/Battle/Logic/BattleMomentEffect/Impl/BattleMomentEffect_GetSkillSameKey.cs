using System.Linq;
using cfg;

public class BattleMomentEffect_GetSkillSameKey : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var whoGet = GetUnitByParamID(Config.ParamList[0]);
        if (whoGet.Count > 0 && Subject != null && Target != null)
        {
            var subjectSkill = BattleLogicBehaviourManager.GetBattleBehaviour(Subject.EntityID).SkillID;
            var subjectSkillKey = ConfigManager.GetBattleSkillConfig(subjectSkill).NeedKey;
            var targetSkill = BattleLogicBehaviourManager.GetBattleBehaviour(Target.EntityID).SkillID;
            var targetSkillKey = ConfigManager.GetBattleSkillConfig(targetSkill).NeedKey;

            foreach (var keyType in subjectSkillKey.Intersect(targetSkillKey))
            {
                foreach (var target in whoGet)
                {
                    target.ChangeProperty((BattlePropertyType)keyType, 1);
                }
            }
        }
    }
}