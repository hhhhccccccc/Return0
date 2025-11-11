using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill3064 : BattleSkillBase
{
    [Inject] private BattleManager BattleManager { get; set; }
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        if (ClashState.Contains(true))
        {
            var enemies = BattleManager.GetAllOpponentUnit(Subject.EntityID, true).Where(enemy => Subject.CheckSkillCanDoDesition_Logic(SkillGuid, enemy)).ToList();
            if (enemies.Count > 0)
            {
                var randomTarget = Util.GetRandom(enemies);
                return new BattleSkillRepeatData
                {
                    SkillID = SkillID,
                    TargetID = randomTarget.EntityID,
                    MaxRepeatCount = 999999999,
                    IfLostChangeToOther = false
                };
            }
        }

        return null;
    }
}