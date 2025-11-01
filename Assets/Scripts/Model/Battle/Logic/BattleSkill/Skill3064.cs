using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3064 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (ClashState.Contains(true))
        {
            var enemies = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
            var randomTarget = Util.GetRandom(enemies);
            return new BattleSkillRepeatData
            {
                SkillID = GetSkillID(),
                TargetID = randomTarget.EntityID,
                MaxRepeatCount = 999999999,
                IfLostChangeToOther = false
            };
        }

        return null;
    }
}