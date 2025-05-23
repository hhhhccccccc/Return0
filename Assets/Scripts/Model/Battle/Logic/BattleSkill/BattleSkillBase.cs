﻿using cfg;
using Zenject;

public class BattleSkillBase : BattleSkillMoment, IModel
{
    public int SkillID;

    public int Spellcaster;

    public int Hit;

    public BattleSkill Cfg;
    [Inject] private IConfigManager ConfigManager;
    public void Init(int skillID)
    {
        SkillID = skillID;
        Cfg = ConfigManager.GetSkill(skillID);
    }

    public void Release(int spellcaster, int hit)
    {
        Spellcaster = spellcaster;
        Hit = hit;
    }
}
