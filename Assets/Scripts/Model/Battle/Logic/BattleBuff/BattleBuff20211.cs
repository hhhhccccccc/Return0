using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20211 : BattleBuffBase
{
    protected override bool OnCheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        var (s, v) = Util.UnCombSkillGuid(skillGuid);
        var skillType = BattleUtil.GetSkillTypeBySkillID(s);
        return skillType != SkillType.SpellFormula;
    }
}
