using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4034 : BattleSkillBase
{
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var usedList = BattleLogicStateManager.RoundUsedSkillGuid;
        var power = usedList.Count(skillID => BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.PowerKilling);
        var art = usedList.Count(skillID => BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling);
        var tech = usedList.Count(skillID => BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.TechniqueImperialStyle);
        var spell = usedList.Count(skillID => BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.SpellFormula);
        Subject.ChangeProperty(BattlePropertyType.GangQi, (power + tech) * 2, BattleSource.Skill);
        Subject.ChangeProperty(BattlePropertyType.XuanQi, (art + spell) * 2, BattleSource.Skill);
    }
} 