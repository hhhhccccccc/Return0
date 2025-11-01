using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3101 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (Subject.HasBuffType(BuffType.Gain))
        {
            var enemies = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
            var randomTarget = Util.GetRandom(enemies);
            return new BattleSkillRepeatData
            {
                SkillID = GetSkillID(),
                TargetID = randomTarget.EntityID,
                MaxRepeatCount = 1,
                IfLostChangeToOther = false
            };
        }

        return null;
    }
}