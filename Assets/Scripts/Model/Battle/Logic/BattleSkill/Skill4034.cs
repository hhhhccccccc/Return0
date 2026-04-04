using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4034 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var power = GetSkillUseCount(SkillType.PowerKilling);
        var art = GetSkillUseCount(SkillType.ArtKilling);
        var tech = GetSkillUseCount(SkillType.TechniqueImperialStyle);
        var spell = GetSkillUseCount(SkillType.SpellFormula);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, (power + tech) * 2, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, (art + spell) * 2, BattleSource.Skill);
    }

    private int GetSkillUseCount(SkillType skillType)
    {
        return BattleLogicStateManager.RoundUsedSkillGuid.Count(skillGuid =>
        {
            var (s, v) = Util.UnCombSkillGuid(skillGuid);
            return BattleUtil.GetSkillTypeBySkillID(s) == skillType;
        });
    }
} 